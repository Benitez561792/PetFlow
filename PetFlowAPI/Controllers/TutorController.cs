using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetFlowAPI.Data;
using PetFlowAPI.DTOs;
using PetFlowAPI.Models;

namespace PetFlowAPI.Controllers
{
    [ApiController]
    [Route("tutors")]
    public class TutorController : ControllerBase
    {
        private readonly PetFlowContext _context;
        private readonly IMapper _mapper;

        public TutorController(PetFlowContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<TutorResponseDTO>> Create(TutorRequestDTO request)
        {
            var tutor = _mapper.Map<Tutor>(request);
            // Em um cenário real, o password seria hasheado aqui
            tutor.PasswordHash = request.Password; 
            
            _context.Tutors.Add(tutor);
            await _context.SaveChangesAsync();

            var response = _mapper.Map<TutorResponseDTO>(tutor);
            return CreatedAtAction(nameof(GetById), new { id = tutor.Id }, response);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TutorResponseDTO>>> GetAll(
            [FromQuery] string? name, 
            [FromQuery] int page = 0, 
            [FromQuery] int size = 10)
        {
            var query = _context.Tutors.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(t => t.Name.Contains(name));
            }

            var tutors = await query
                .Skip(page * size)
                .Take(size)
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<TutorResponseDTO>>(tutors));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TutorResponseDTO>> GetById(long id)
        {
            var tutor = await _context.Tutors.FindAsync(id);

            if (tutor == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TutorResponseDTO>(tutor));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, TutorRequestDTO request)
        {
            var tutor = await _context.Tutors.FindAsync(id);

            if (tutor == null)
            {
                return NotFound();
            }

            _mapper.Map(request, tutor);
            tutor.PasswordHash = request.Password;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TutorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(_mapper.Map<TutorResponseDTO>(tutor));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var tutor = await _context.Tutors.FindAsync(id);
            if (tutor == null)
                return NotFound();

            // Remove todos os registros filhos antes de remover o tutor
            // para evitar violação de FK no Oracle (independe de quem criou)

            // 1. Resgates do tutor
            var redeems = await _context.Redeems
                .Where(r => r.TutorId == id).ToListAsync();
            _context.Redeems.RemoveRange(redeems);

            // 2. Pontos de recompensa do tutor
            var rewardPoints = await _context.RewardPoints
                .Where(rp => rp.TutorId == id).ToListAsync();
            _context.RewardPoints.RemoveRange(rewardPoints);

            // 3. Endereços do tutor
            var addresses = await _context.Addresses
                .Where(a => a.TutorId == id).ToListAsync();
            _context.Addresses.RemoveRange(addresses);

            // 4. Pets e seus filhos (health_event, subscription, risk_score)
            var pets = await _context.Pets
                .Where(p => p.TutorId == id).ToListAsync();

            foreach (var pet in pets)
            {
                var healthEvents = await _context.HealthEvents
                    .Where(h => h.PetId == pet.Id).ToListAsync();
                _context.HealthEvents.RemoveRange(healthEvents);

                var subscriptions = await _context.Subscriptions
                    .Where(s => s.PetId == pet.Id).ToListAsync();
                _context.Subscriptions.RemoveRange(subscriptions);

                var riskScores = await _context.RiskScores
                    .Where(rs => rs.PetId == pet.Id).ToListAsync();
                _context.RiskScores.RemoveRange(riskScores);
            }
            _context.Pets.RemoveRange(pets);

            // 5. Tutor
            _context.Tutors.Remove(tutor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TutorExists(long id)
        {
            return _context.Tutors.Any(e => e.Id == id);
        }
    }
}

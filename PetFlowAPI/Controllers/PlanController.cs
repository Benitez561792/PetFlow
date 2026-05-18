using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetFlowAPI.Data;
using PetFlowAPI.DTOs;
using PetFlowAPI.Models;

namespace PetFlowAPI.Controllers
{
    [ApiController]
    [Route("plans")]
    public class PlanController : ControllerBase
    {
        private readonly PetFlowContext _context;
        private readonly IMapper _mapper;

        public PlanController(PetFlowContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<PlanResponseDTO>> Create(PlanRequestDTO request)
        {
            var plan = _mapper.Map<Plan>(request);
            _context.Plans.Add(plan);
            await _context.SaveChangesAsync();

            var response = _mapper.Map<PlanResponseDTO>(plan);
            return CreatedAtAction(nameof(GetById), new { id = plan.Id }, response);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlanResponseDTO>>> GetAll(
            [FromQuery] string? name, 
            [FromQuery] int page = 0, 
            [FromQuery] int size = 10)
        {
            var query = _context.Plans.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => p.Name.Contains(name));
            }

            var plans = await query
                .Skip(page * size)
                .Take(size)
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<PlanResponseDTO>>(plans));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlanResponseDTO>> GetById(long id)
        {
            var plan = await _context.Plans.FindAsync(id);

            if (plan == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PlanResponseDTO>(plan));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, PlanRequestDTO request)
        {
            var plan = await _context.Plans.FindAsync(id);

            if (plan == null)
            {
                return NotFound();
            }

            _mapper.Map(request, plan);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlanExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(_mapper.Map<PlanResponseDTO>(plan));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var plan = await _context.Plans.FindAsync(id);
            if (plan == null)
                return NotFound();

            // Remove assinaturas vinculadas ao plano antes de removê-lo
            var subscriptions = await _context.Subscriptions
                .Where(s => s.PlanId == id).ToListAsync();
            _context.Subscriptions.RemoveRange(subscriptions);

            _context.Plans.Remove(plan);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlanExists(long id)
        {
            return _context.Plans.Any(e => e.Id == id);
        }
    }
}

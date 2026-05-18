using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetFlowAPI.Data;
using PetFlowAPI.DTOs;
using PetFlowAPI.Models;

namespace PetFlowAPI.Controllers
{
    [ApiController]
    [Route("clinics")]
    public class ClinicController : ControllerBase
    {
        private readonly PetFlowContext _context;
        private readonly IMapper _mapper;

        public ClinicController(PetFlowContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>Cria uma nova clínica.</summary>
        [HttpPost]
        [ProducesResponseType(typeof(ClinicResponseDTO), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ClinicResponseDTO>> Create(ClinicRequestDTO request)
        {
            var clinic = _mapper.Map<Clinic>(request);
            _context.Clinics.Add(clinic);
            await _context.SaveChangesAsync();

            var response = _mapper.Map<ClinicResponseDTO>(clinic);
            return CreatedAtAction(nameof(GetById), new { id = clinic.Id }, response);
        }

        /// <summary>Lista todas as clínicas com paginação e filtro por nome.</summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ClinicResponseDTO>), 200)]
        public async Task<ActionResult<IEnumerable<ClinicResponseDTO>>> GetAll(
            [FromQuery] string? name,
            [FromQuery] int page = 0,
            [FromQuery] int size = 10)
        {
            var query = _context.Clinics.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(c => c.Name.Contains(name));

            var clinics = await query
                .OrderBy(c => c.Id)
                .Skip(page * size)
                .Take(size)
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<ClinicResponseDTO>>(clinics));
        }

        /// <summary>Busca uma clínica pelo ID.</summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ClinicResponseDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ClinicResponseDTO>> GetById(long id)
        {
            var clinic = await _context.Clinics.FindAsync(id);

            if (clinic == null)
                return NotFound(new { message = $"Clínica com ID {id} não encontrada." });

            return Ok(_mapper.Map<ClinicResponseDTO>(clinic));
        }

        /// <summary>Atualiza os dados de uma clínica.</summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ClinicResponseDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(long id, ClinicRequestDTO request)
        {
            var clinic = await _context.Clinics.FindAsync(id);

            if (clinic == null)
                return NotFound(new { message = $"Clínica com ID {id} não encontrada." });

            _mapper.Map(request, clinic);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClinicExists(id))
                    return NotFound(new { message = $"Clínica com ID {id} não encontrada." });
                throw;
            }

            return Ok(_mapper.Map<ClinicResponseDTO>(clinic));
        }

        /// <summary>
        /// Remove uma clínica e todos os seus registros dependentes em cascata.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(long id)
        {
            // Busca a clínica (sem Include para evitar erro de IQueryable)
            var clinic = await _context.Clinics.FindAsync(id);

            if (clinic == null)
                return NotFound(new { message = $"Clínica com ID {id} não encontrada." });

            // 1. Planos desta clínica
            var plans = await _context.Plans
                .Where(p => p.ClinicId == id)
                .ToListAsync();

            foreach (var plan in plans)
            {
                // 2. Assinaturas vinculadas ao plano
                var subscriptions = await _context.Subscriptions
                    .Where(s => s.PlanId == plan.Id)
                    .ToListAsync();
                _context.Subscriptions.RemoveRange(subscriptions);
            }

            // 3. Eventos de saúde vinculados à clínica
            var healthEvents = await _context.HealthEvents
                .Where(h => h.ClinicId == id)
                .ToListAsync();
            _context.HealthEvents.RemoveRange(healthEvents);

            // 4. Descontos de parceiros vinculados à clínica
            var partnerDiscounts = await _context.PartnerDiscounts
                .Where(pd => pd.ClinicId == id)
                .ToListAsync();

            foreach (var pd in partnerDiscounts)
            {
                // 5. Templates de cupom vinculados ao desconto
                var templates = await _context.CouponTemplates
                    .Where(ct => ct.PartnerDiscountId == pd.Id)
                    .ToListAsync();

                foreach (var template in templates)
                {
                    // 6. Cupons vinculados ao template
                    var coupons = await _context.Coupons
                        .Where(c => c.TemplateId == template.Id)
                        .ToListAsync();

                    foreach (var coupon in coupons)
                    {
                        // 7. Resgates vinculados ao cupom
                        var redeems = await _context.Redeems
                            .Where(r => r.CouponId == coupon.Id)
                            .ToListAsync();
                        _context.Redeems.RemoveRange(redeems);
                    }

                    _context.Coupons.RemoveRange(coupons);
                }

                _context.CouponTemplates.RemoveRange(templates);
            }

            _context.PartnerDiscounts.RemoveRange(partnerDiscounts);

            // 8. Planos
            _context.Plans.RemoveRange(plans);

            // 9. Clínica
            _context.Clinics.Remove(clinic);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClinicExists(long id)
        {
            return _context.Clinics.Any(e => e.Id == id);
        }
    }
}

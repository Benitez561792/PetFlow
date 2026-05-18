using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetFlowAPI.Data;
using PetFlowAPI.DTOs;
using PetFlowAPI.Models;
using PetFlowAPI.Enums;

namespace PetFlowAPI.Controllers
{
    [ApiController]
    [Route("coupons")]
    public class CouponController : ControllerBase
    {
        private readonly PetFlowContext _context;
        private readonly IMapper _mapper;

        public CouponController(PetFlowContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<CouponResponseDTO>> Create(CouponRequestDTO request)
        {
            // Valida que a data de expiração, se informada, não está no passado
            if (request.ExpirationDate.HasValue && request.ExpirationDate.Value.Date < DateTime.Today)
                return BadRequest(new
                {
                    campo = "expirationDate",
                    mensagem = "A data de expiração não pode ser uma data no passado."
                });

            var coupon = _mapper.Map<Coupon>(request);
            _context.Coupons.Add(coupon);
            await _context.SaveChangesAsync();

            var response = _mapper.Map<CouponResponseDTO>(coupon);
            return CreatedAtAction(nameof(GetById), new { id = coupon.Id }, response);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CouponResponseDTO>>> GetAll(
            [FromQuery] string? code, 
            [FromQuery] int page = 0, 
            [FromQuery] int size = 10)
        {
            var query = _context.Coupons.AsQueryable();

            if (!string.IsNullOrEmpty(code))
            {
                query = query.Where(c => c.Code.Contains(code));
            }

            var coupons = await query
                .Skip(page * size)
                .Take(size)
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<CouponResponseDTO>>(coupons));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CouponResponseDTO>> GetById(long id)
        {
            var coupon = await _context.Coupons.FindAsync(id);

            if (coupon == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CouponResponseDTO>(coupon));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, CouponRequestDTO request)
        {
            // Valida data de expiração também no update
            if (request.ExpirationDate.HasValue && request.ExpirationDate.Value.Date < DateTime.Today)
                return BadRequest(new
                {
                    campo = "expirationDate",
                    mensagem = "A data de expiração não pode ser uma data no passado."
                });

            var coupon = await _context.Coupons.FindAsync(id);

            if (coupon == null)
            {
                return NotFound();
            }

            _mapper.Map(request, coupon);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CouponExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(_mapper.Map<CouponResponseDTO>(coupon));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var coupon = await _context.Coupons.FindAsync(id);
            if (coupon == null)
                return NotFound();

            // Remove resgates vinculados ao cupom antes de removê-lo
            var redeems = await _context.Redeems
                .Where(r => r.CouponId == id).ToListAsync();
            _context.Redeems.RemoveRange(redeems);

            _context.Coupons.Remove(coupon);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(long id, [FromQuery] string status)
        {
            var coupon = await _context.Coupons.FindAsync(id);

            if (coupon == null)
            {
                return NotFound();
            }

            if (Enum.TryParse<CouponStatus>(status, true, out var newStatus))
            {
                coupon.Status = newStatus;
                await _context.SaveChangesAsync();
                return Ok(_mapper.Map<CouponResponseDTO>(coupon));
            }

            return BadRequest("Status inválido");
        }

        private bool CouponExists(long id)
        {
            return _context.Coupons.Any(e => e.Id == id);
        }
    }
}

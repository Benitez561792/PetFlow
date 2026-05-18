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
    [Route("subscriptions")]
    public class SubscriptionController : ControllerBase
    {
        private readonly PetFlowContext _context;
        private readonly IMapper _mapper;

        public SubscriptionController(PetFlowContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<SubscriptionResponseDTO>> Create(SubscriptionRequestDTO request)
        {
            var subscription = _mapper.Map<Subscription>(request);
            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();

            var response = _mapper.Map<SubscriptionResponseDTO>(subscription);
            return CreatedAtAction(nameof(GetById), new { id = subscription.Id }, response);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubscriptionResponseDTO>>> GetAll(
            [FromQuery] long? petId,
            [FromQuery] string? status,
            [FromQuery] int page = 0, 
            [FromQuery] int size = 10,
            [FromQuery] string? sortBy = "createdAt",
            [FromQuery] string? direction = "desc")
        {
            var query = _context.Subscriptions.AsQueryable();

            if (petId.HasValue)
            {
                query = query.Where(s => s.PetId == petId);
            }

            if (!string.IsNullOrEmpty(status))
            {
                if (Enum.TryParse<SubscriptionStatus>(status, true, out var statusEnum))
                {
                    query = query.Where(s => s.Status == statusEnum);
                }
            }

            var subscriptions = await query
                .Skip(page * size)
                .Take(size)
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<SubscriptionResponseDTO>>(subscriptions));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SubscriptionResponseDTO>> GetById(long id)
        {
            var subscription = await _context.Subscriptions.FindAsync(id);

            if (subscription == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<SubscriptionResponseDTO>(subscription));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, SubscriptionRequestDTO request)
        {
            var subscription = await _context.Subscriptions.FindAsync(id);

            if (subscription == null)
            {
                return NotFound();
            }

            _mapper.Map(request, subscription);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubscriptionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(_mapper.Map<SubscriptionResponseDTO>(subscription));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var subscription = await _context.Subscriptions.FindAsync(id);
            if (subscription == null)
            {
                return NotFound();
            }

            _context.Subscriptions.Remove(subscription);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(long id, [FromQuery] string status)
        {
            var subscription = await _context.Subscriptions.FindAsync(id);

            if (subscription == null)
            {
                return NotFound();
            }

            if (Enum.TryParse<SubscriptionStatus>(status, true, out var newStatus))
            {
                subscription.Status = newStatus;
                await _context.SaveChangesAsync();
                return Ok(_mapper.Map<SubscriptionResponseDTO>(subscription));
            }

            return BadRequest("Status inválido");
        }

        private bool SubscriptionExists(long id)
        {
            return _context.Subscriptions.Any(e => e.Id == id);
        }
    }
}

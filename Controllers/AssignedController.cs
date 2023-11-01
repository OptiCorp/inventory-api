using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace inventory_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignedController : ControllerBase
    {
        private readonly TodoContext _context;

        public AssignedController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/Assigned
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Assigned>>> GetAssigned()
        {
          if (_context.Assigned == null)
          {
              return NotFound();
          }
            return await _context.Assigned.ToListAsync();
        }

        // GET: api/Assigned/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Assigned>> GetAssigned(string id)
        {
          if (_context.Assigned == null)
          {
              return NotFound();
          }
            var assigned = await _context.Assigned.FindAsync(id);

            if (assigned == null)
            {
                return NotFound();
            }

            return assigned;
        }

        // PUT: api/Assigned/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAssigned(string id, Assigned assigned)
        {
            if (id != assigned.Id)
            {
                return BadRequest();
            }

            _context.Entry(assigned).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssignedExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Assigned
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Assigned>> PostAssigned(Assigned assigned)
        {
          if (_context.Assigned == null)
          {
              return Problem("Entity set 'TodoContext.Assigned'  is null.");
          }
            _context.Assigned.Add(assigned);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AssignedExists(assigned.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAssigned", new { id = assigned.Id }, assigned);
        }

        // DELETE: api/Assigned/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssigned(string id)
        {
            if (_context.Assigned == null)
            {
                return NotFound();
            }
            var assigned = await _context.Assigned.FindAsync(id);
            if (assigned == null)
            {
                return NotFound();
            }

            _context.Assigned.Remove(assigned);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AssignedExists(string id)
        {
            return (_context.Assigned?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

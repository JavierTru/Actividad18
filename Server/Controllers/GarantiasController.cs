using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Actividad17.Server.Contexto;
using Actividad17.Shared.Models;

namespace Actividad17.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GarantiasController : ControllerBase
    {
        private readonly ContextoLimpieza _context;

        public GarantiasController(ContextoLimpieza context)
        {
            _context = context;
        }

        // GET: api/Garantias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Garantias>>> GetGarantias()
        {
          if (_context.Garantias == null)
          {
              return NotFound();
          }
            var servicios = await _context.Garantias.Include(s => s.Servicios).ToListAsync();
            return servicios;
        }

        // GET: api/Garantias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Garantias>> GetGarantias(int id)
        {
          if (_context.Garantias == null)
          {
              return NotFound();
          }

            var garantias = await _context.Garantias.Include(s => s.Servicios).FirstOrDefaultAsync(s => s.Id == id);

            if (garantias == null)
            {
                return NotFound();
            }


            return garantias;
        }

        // PUT: api/Garantias/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGarantias(int id, Garantias garantias)
        {
            if (id != garantias.Id)
            {
                return BadRequest();
            }

            _context.Entry(garantias).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GarantiasExists(id))
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

        // POST: api/Garantias
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Garantias>> PostGarantias(Garantias garantias)
        {
          if (_context.Garantias == null)
          {
              return Problem("Entity set 'ContextoLimpieza.Garantias'  is null.");
          }
            _context.Garantias.Add(garantias);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGarantias", new { id = garantias.Id }, garantias);
        }

        // DELETE: api/Garantias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGarantias(int id)
        {
            if (_context.Garantias == null)
            {
                return NotFound();
            }
            var garantias = await _context.Garantias.FindAsync(id);
            if (garantias == null)
            {
                return NotFound();
            }

            _context.Garantias.Remove(garantias);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GarantiasExists(int id)
        {
            return (_context.Garantias?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

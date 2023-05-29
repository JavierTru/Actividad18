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
    public class ServiciosController : ControllerBase
    {
        private readonly ContextoLimpieza _context;

        public ServiciosController(ContextoLimpieza context)
        {
            _context = context;
        }

        // GET: api/Servicios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Servicios>>> GetServicios()
        {
            if (_context.Servicios == null)
            {
                return NotFound();
            }

            var servicios = await _context.Servicios.Include(s => s.Garantias).ToListAsync();
            return servicios;
        }

        // GET: api/Servicios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Servicios>> GetServicios(int id)
        {
            if (_context.Servicios == null)
            {
                return NotFound();
            }

            var servicios = await _context.Servicios.Include(s => s.Garantias).FirstOrDefaultAsync(s => s.Id == id);

            if (servicios == null)
            {
                return NotFound();
            }

            return servicios;
        }

        // POST: api/Servicios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Servicios>> PostServicios(Servicios servicios)
        {
            if (_context.Servicios == null)
            {
                return Problem("Entity set 'ContextoLimpieza.Servicios' is null.");
            }

            if (servicios.Garantias != null && servicios.Garantias.Count > 1)
            {
                return BadRequest("Solo se puede vincular una garantía a un servicio.");
            }

            if (servicios.ClientesId != null)
            {
                var cliente = await _context.Clientes.FindAsync(servicios.ClientesId);
                if (cliente != null)
                {
                    servicios.Nombre = cliente.Nombre;
                }
            }

            _context.Servicios.Add(servicios);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetServicios", new { id = servicios.Id }, servicios);
        }

        // PUT: api/Servicios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServicios(int id, Servicios servicios)
        {
            if (id != servicios.Id)
            {
                return BadRequest();
            }

            _context.Entry(servicios).State = EntityState.Modified;

            if (servicios.ClientesId != null)
            {
                var cliente = await _context.Clientes.FindAsync(servicios.ClientesId);
                if (cliente != null)
                {
                    servicios.Nombre = cliente.Nombre;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiciosExists(id))
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


        // DELETE: api/Servicios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServicios(int id)
        {
            if (_context.Servicios == null)
            {
                return NotFound();
            }

            var servicios = await _context.Servicios.Include(s => s.Garantias).FirstOrDefaultAsync(s => s.Id == id);
            if (servicios == null)
            {
                return NotFound();
            }

            // Desvincular el servicio del cliente
            if (servicios.Clientes != null)
            {
                servicios.Clientes.Servicios.Remove(servicios);
            }

            // Eliminar las garantías vinculadas al servicio
            if (servicios.Garantias != null)
            {
                _context.Garantias.RemoveRange(servicios.Garantias);
            }

            _context.Servicios.Remove(servicios);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServiciosExists(int id)
        {
            return (_context.Servicios?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

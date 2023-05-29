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
    public class ClientesController : ControllerBase
    {
        private readonly ContextoLimpieza _context;

        public ClientesController(ContextoLimpieza context)
        {
            _context = context;
        }

        // GET: api/Clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Clientes>>> GetClientes()
        {
            if (_context.Clientes == null)
            {
                return NotFound();
            }

            // Incluir los servicios vinculados y las garantías vinculadas a esos servicios
            var clientes = await _context.Clientes.Include(c => c.Servicios).ThenInclude(t => t.Garantias).ToListAsync();

            return clientes;
        }

        // GET: api/Clientes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Clientes>> GetClientes(int id)
        {
            if (_context.Clientes == null)
            {
                return NotFound();
            }

            var clientes = await _context.Clientes.FindAsync(id);

            if (clientes == null)
            {
                return NotFound();
            }

            return clientes;
        }

        // PUT: api/Clientes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClientes(int id, Clientes clientes)
        {
            if (id != clientes.Id)
            {
                return BadRequest();
            }

            _context.Entry(clientes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientesExists(id))
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

        // POST: api/Clientes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Clientes>> PostClientes(Clientes clientes)
        {
            if (_context.Clientes == null)
            {
                return Problem("Entity set 'ContextoLimpieza.Clientes' is null.");
            }

            _context.Clientes.Add(clientes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClientes", new { id = clientes.Id }, clientes);
        }

        // DELETE: api/Clientes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClientes(int id)
        {
            if (_context.Clientes == null)
            {
                return NotFound();
            }

            var clientes = await _context.Clientes.Include(c => c.Servicios).ThenInclude(t => t.Garantias).FirstOrDefaultAsync(c => c.Id == id);
            if (clientes == null)
            {
                return NotFound();
            }

            // Eliminar los servicios vinculados y las garantías vinculadas a esos servicios
            if (clientes.Servicios != null)
            {
                foreach (var servicio in clientes.Servicios)
                {
                    if (servicio.Garantias != null)
                    {
                        _context.Garantias.RemoveRange(servicio.Garantias);
                    }
                }

                _context.Servicios.RemoveRange(clientes.Servicios);
            }

            _context.Clientes.Remove(clientes);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClientesExists(int id)
        {
            return (_context.Clientes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

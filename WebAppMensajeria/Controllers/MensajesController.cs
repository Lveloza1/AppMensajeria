using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppMensajeria.Contexts;
using AppMensajeria.Models;

namespace WebAppMensajeria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MensajesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MensajesController()
        {
            _context =new AppDbContext();
            _context.Database.EnsureCreated();
        }

        // GET: api/Mensajes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mensaje>>> GetMensajes()
        {
            return await _context.Mensajes.Include(x=>x.UsuarioChat).ToListAsync();
        }

        // PUT: api/Mensajes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMensaje(int id, Mensaje mensaje)
        {
            if (id != mensaje.MensajeID)
            {
                return BadRequest();
            }

            _context.Entry(mensaje).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MensajeExists(id))
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

        // POST: api/Mensajes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Mensaje>> PostMensaje(Mensaje mensaje)
        {
            _context.Mensajes.Add(mensaje);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMensaje", new { id = mensaje.MensajeID }, mensaje);
        }

        // DELETE: api/Mensajes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Mensaje>> DeleteMensaje(int id)
        {
            var mensaje = await _context.Mensajes.FindAsync(id);
            if (mensaje == null)
            {
                return NotFound();
            }

            _context.Mensajes.Remove(mensaje);
            await _context.SaveChangesAsync();

            return mensaje;
        }

        private bool MensajeExists(int id)
        {
            return _context.Mensajes.Any(e => e.MensajeID == id);
        }
    }
}

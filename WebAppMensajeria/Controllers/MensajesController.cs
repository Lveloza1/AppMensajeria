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
            return await _context.Mensajes.Include(x=>x.UsuarioChat).Include(x => x.UsuarioChat.Usuario).Include(x => x.UsuarioChat.Chat).ToListAsync();
        }

        // GET: api/Mensaje/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Mensaje>>> GetMensaje(int id)
        {
            var Mensaje = await _context.Mensajes.Include(x=>x.UsuarioChat).Include(x => x.UsuarioChat.Usuario).Include(x=>x.UsuarioChat.Chat).Where(x => x.UsuarioChat.ChatID== id).ToListAsync();

            if (Mensaje == null)
            {
                return NotFound();
            }

            return Mensaje;
        }

        // POST: api/Mensajes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Mensaje>> PostMensaje(Mensaje mensaje)
        {
            _context.Mensajes.Add(mensaje);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteUsuario()
        {
            var mensajes = await _context.Mensajes.ToListAsync();
            foreach (var x in mensajes)
            {
                _context.Mensajes.Remove(x);

            }
            await _context.SaveChangesAsync();
            return Ok();
        }


    }
}

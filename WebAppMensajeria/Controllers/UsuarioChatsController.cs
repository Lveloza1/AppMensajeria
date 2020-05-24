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
    public class UsuarioChatsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuarioChatsController()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated();
        }

        // GET: api/UsuarioChats
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioChat>>> GetUsuarioChats()
        {
            return await _context.UsuarioChats.ToListAsync();
        }

        // GET: api/UsuarioChats/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioChat>> GetUsuarioChat(int id)
        {
            var usuarioChat = await _context.UsuarioChats.FindAsync(id);

            if (usuarioChat == null)
            {
                return NotFound();
            }

            return usuarioChat;
        }

        // PUT: api/UsuarioChats/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuarioChat(int id, UsuarioChat usuarioChat)
        {
            if (id != usuarioChat.UsuarioChatID)
            {
                return BadRequest();
            }

            _context.Entry(usuarioChat).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioChatExists(id))
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

        // POST: api/UsuarioChats
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<UsuarioChat>> PostUsuarioChat(UsuarioChat usuarioChat)
        {
            _context.UsuarioChats.Add(usuarioChat);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuarioChat", new { id = usuarioChat.UsuarioChatID }, usuarioChat);
        }

        // DELETE: api/UsuarioChats/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UsuarioChat>> DeleteUsuarioChat(int id)
        {
            var usuarioChat = await _context.UsuarioChats.FindAsync(id);
            if (usuarioChat == null)
            {
                return NotFound();
            }

            _context.UsuarioChats.Remove(usuarioChat);
            await _context.SaveChangesAsync();

            return usuarioChat;
        }

        private bool UsuarioChatExists(int id)
        {
            return _context.UsuarioChats.Any(e => e.UsuarioChatID == id);
        }
    }
}

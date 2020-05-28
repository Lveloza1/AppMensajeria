using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppMensajeria.Contexts;
using AppMensajeria.Models;
using System.Runtime.InteropServices.WindowsRuntime;

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
            return await _context.UsuarioChats.Include(x => x.Usuario).Include(x => x.Chat).ToListAsync();
        }

        // GET: api/UsuarioChats
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<UsuarioChat>>> GetUsuarioChatbyid(int id)
        {
            return await _context.UsuarioChats.Include(x => x.Usuario).Include(x => x.Chat).Where(x => x.UsuarioID == id).ToListAsync();
        }
        // GET: api/UsuarioChats
        [HttpGet("Chat/{id}")]
        public async Task<ActionResult<IEnumerable<UsuarioChat>>> GetUsuarioChatbychat(int id)
        {
            return await _context.UsuarioChats.Include(x => x.Usuario).Include(x => x.Chat).Where(x => x.ChatID == id).ToListAsync();
        }
        [HttpGet("ChatGrupo/{id}")]
        public async Task<ActionResult<IEnumerable<UsuarioChat>>> GruposDelUsuario(int id)
        {
            return await _context.UsuarioChats.Include(x => x.Usuario).Include(x => x.Chat).Where(x=>x.Chat.Tipo==true).Where(x => x.UsuarioID == id).ToListAsync();
        }

        // POST: api/UsuarioChats
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<UsuarioChat>> PostUsuarioChat(UsuarioChat usuarioChat)
        {
            var Validarusuario = await _context.UsuarioChats.Where(x=>x.ChatID==usuarioChat.ChatID).Where(x=>x.UsuarioID==usuarioChat.UsuarioID).FirstOrDefaultAsync();
            if (Validarusuario == null)
            {
                _context.UsuarioChats.Add(usuarioChat);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else {
                return NotFound();
            }
            
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteUsuario()
        {
            var usuarios = await _context.UsuarioChats.ToListAsync();
            foreach (var x in usuarios)
            {
                _context.UsuarioChats.Remove(x);

            }
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}

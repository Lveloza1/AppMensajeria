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

    }
}

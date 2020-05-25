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
    public class ChatsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ChatsController()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated();
        }

        // GET: api/Chats
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Chat>>> GetChats()
        {
            return await _context.Chats.ToListAsync();
        }

        // GET: api/Chats/5
        [HttpGet("{true}")]
        public async Task<ActionResult<IEnumerable<Chat>>> ListaGrupos()
        {
            var chat = await _context.Chats.Where(x=> x.Tipo==true).ToListAsync();

            if (chat == null)
            {
                return NotFound();
            }

            return chat;
        }

        // GET: api/Chats/5
        [HttpGet("{false}")]
        public async Task<ActionResult<IEnumerable<Chat>>> ListaPrivado()
        {
            var chat = await _context.Chats.Where(x => x.Tipo == false).ToListAsync();

            if (chat == null)
            {
                return NotFound();
            }

            return chat;
        }
        // POST: api/Chats
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Chat>> PostChat(Chat chat)
        {
            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChat", new { id = chat.ChatID }, chat);
        }
    }
}

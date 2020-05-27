using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppMensajeria.Contexts;
using AppMensajeria.Models;

namespace WebAppMensajeria.Contexts
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated();
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }


        [HttpGet("{telefono}")]
        public async Task<ActionResult<Usuario>> GetUsuarioChatbyid(string telefono)
        {
            return await _context.Usuarios.Where(x => x.Telefono == telefono).FirstOrDefaultAsync();
        }
        // POST: api/Usuarios
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Usuario>> Post(Usuario usuario)
        {
            var UsuariosAntiguos = await _context.Usuarios.ToListAsync();
            var Validarusuario = await _context.Usuarios.Where(x => x.Telefono == usuario.Telefono).FirstOrDefaultAsync();
            try
            {
                if (Validarusuario == null)
                {
                    _context.Usuarios.Add(usuario);
                    foreach (var x in UsuariosAntiguos)
                    {
                        Chat chat = new Chat
                        {
                          Nombre = x.Nombre + " - " + usuario.Nombre,
                          Tipo = false
                        };
                        _context.Chats.Add(chat);
                        UsuarioChat usuarioChat = new UsuarioChat
                        {
                            UsuarioID = x.UsuarioID,
                            Usuario=x,
                            ChatID = chat.ChatID,
                            Chat=chat
                        };
                        _context.UsuarioChats.Add(usuarioChat);
                        UsuarioChat usuarioChat2 = new UsuarioChat
                        {                            
                            UsuarioID = usuario.UsuarioID,
                            Usuario=usuario,
                            ChatID = chat.ChatID,
                            Chat = chat
                        };
                        _context.UsuarioChats.Add(usuarioChat2);
                    }
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUsuario(int id)
        {
            var Validarusuario = await _context.Usuarios.FindAsync(id);
            if (Validarusuario != null)
            {
                _context.Usuarios.Remove(Validarusuario);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return NotFound();
            }

        }
    }
}

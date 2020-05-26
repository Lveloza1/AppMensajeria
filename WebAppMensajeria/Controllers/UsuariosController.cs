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
            _context =new AppDbContext();
            _context.Database.EnsureCreated();
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        // POST: api/Usuarios
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            var Validarusuario = await _context.Usuarios.Where(x=>x.Telefono==usuario.Telefono).FirstOrDefaultAsync();
            if (Validarusuario == null)
            {
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetUsuario", new { id = usuario.UsuarioID }, usuario);
            }
            else {
                return NotFound();
            }
            
        }
    }
}

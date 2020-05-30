using AppMensajeria.Contexts;
using AppMensajeria.Models;
using System;
using System.Linq;

namespace AppMensajeria.Services
{
    public class PerfilService
    {
        private readonly AppDbContext _context;

        public PerfilService()
        {
            _context = App.GetContext();
        }
        public Perfil ObtenerPerfil()
        {
            return _context.Perfiles.FirstOrDefault();
        }
        public void CrearPerfil(Usuario usuario)
        {
            try
            {
                Perfil perfil = new Perfil
                {
                    MiUsuarioID = usuario.UsuarioID,
                    MiImagen = usuario.Imagen,
                    MiNombre = usuario.Nombre,
                    MiTelefono = usuario.Telefono,
                    MiBiografia = usuario.Biografia
                };
                _context.Perfiles.Add(perfil);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void BorrarPerfil(Perfil perfil)
        {
            _context.Perfiles.Remove(perfil);
            _context.SaveChanges();
        }

    }
}
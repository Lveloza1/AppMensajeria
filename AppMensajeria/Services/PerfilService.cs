using AppMensajeria.Contexts;
using AppMensajeria.Models;
using System;
using System.Collections.ObjectModel;
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
            return _context.Perfiles.Where(e=> e.Mi_UsuarioID != null ).FirstOrDefault();;
        }
        public void CrearPerfil(Usuario usuario)
        {
            try
            {
                Perfil perfil = new Perfil
                {
                    Mi_UsuarioID = usuario.UsuarioID,
                    Mi_Imagen = usuario.Imagen,
                    Mi_Nombre = usuario.Nombre,
                    Mi_Telefono = usuario.Telefono
                };
                _context.Perfiles.Add(perfil);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
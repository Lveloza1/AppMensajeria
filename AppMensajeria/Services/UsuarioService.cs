using AppMensajeria.Contexts;
using AppMensajeria.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace AppMensajeria.Services
{
    public class UsuarioService
    {
        private readonly AppDbContext _context;

        public UsuarioService()
        {
            _context = App.GetContext();
        }
        public ObservableCollection<Usuario> ObtenerUsuarios()
        {
            return _context.Usuarios.ToObservableCollection();
        }
        public void CrearUsuario(Usuario usuario)
        {
            try
            {
                _context.Usuarios.Add(usuario);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Usuario ObtenerUsusarioPorTelefono(int telefono)
        {
            ObservableCollection<Usuario> usuarios = ObtenerUsuarios();
            Usuario usuario = _context.Usuarios.Where(e => e.Telefono == telefono).FirstOrDefault();
            return usuario;

        }

    }
}

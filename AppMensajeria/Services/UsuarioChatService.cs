using AppMensajeria.Contexts;
using AppMensajeria.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace AppMensajeria.Services
{
    public class UsuarioChatService
    {
        private readonly AppDbContext _context;

        public UsuarioChatService()
        {
            _context = App.GetContext();
        }
        public ObservableCollection<UsuarioChat> ObtenerUsuarioChats()
        {
            return _context.UsuarioChats.ToObservableCollection();
        }
        public void CrearUsuarioChat(UsuarioChat UsuarioChat)
        {
            try
            {
                _context.UsuarioChats.Add(UsuarioChat);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public ObservableCollection<UsuarioChat> ObtenerMisChats(int UsuaioID)
        {
            ObservableCollection<UsuarioChat> UsuarioChats = ObtenerUsuarioChats().Where(e => e.UsuarioID == UsuaioID).ToObservableCollection();
            return UsuarioChats;
        }

    }
}

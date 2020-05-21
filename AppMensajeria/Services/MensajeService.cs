using AppMensajeria.Contexts;
using AppMensajeria.Models;
using System;
using System.Collections.ObjectModel;

namespace AppMensajeria.Services
{
    public class MensajeService
    {
        private readonly AppDbContext _context;

        public MensajeService()
        {
            _context = App.GetContext();
        }
        public ObservableCollection<Mensaje> ObtenerMensajes()
        {
            return _context.Mensajes.ToObservableCollection();
        }
        public void CrearChat(Mensaje mensaje)
        {
            try
            {
                _context.Mensajes.Add(mensaje);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}

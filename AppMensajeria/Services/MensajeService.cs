using AppMensajeria.Contexts;
using AppMensajeria.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace AppMensajeria.Services
{
    public class MensajeService
    {
        private readonly AppDbContext _context;
        private const string MensajeAPI = "https://appmensajeria.azurewebsites.net/api/Mensajes";

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

        //Obtener Mensajes del Chat
        public async Task<ObservableCollection<Mensaje>> ObtenerMensajesDelChatApi(string id)
        {
            List<Mensaje> Mensajes = new List<Mensaje>();
            try
            {
                Uri uri = new Uri(string.Format(MensajeAPI + "/" + id, string.Empty));
                HttpClient client = new HttpClient();
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var contenido = await response.Content.ReadAsStringAsync();
                    Mensajes = JsonConvert.DeserializeObject<List<Mensaje>>(contenido);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Mensajes.ToObservableCollection();
        }

    }
}

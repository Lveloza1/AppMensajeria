using AppMensajeria.Contexts;
using AppMensajeria.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AppMensajeria.Services
{
    public class MensajeService
    {
        private readonly AppDbContext _context;
        private const string MensajeAPI = "https://grupo5.azurewebsites.net/api/Mensajes";

        public MensajeService()
        {
            _context = App.GetContext();
        }
        public ObservableCollection<Mensaje> ObtenerMensajes()
        {
            return _context.Mensajes.ToObservableCollection();
        }
        public void CrearMensaje(Mensaje mensaje)
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
        public async Task<ObservableCollection<Mensaje>> ObtenerMensajesDelChatApi(int id)
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
            return Mensajes.OrderBy(x => x.InfoDate).ToObservableCollection();
        }

        public async Task<ObservableCollection<Mensaje>> ObtenerMensajesDelUsuarioApi(int id)
        {
            List<Mensaje> Mensajes = new List<Mensaje>();
            try
            {
                Uri uri = new Uri(string.Format(MensajeAPI + "/Usuario/" + id, string.Empty));
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

        //Enviar mensaje
        public async Task EnviarMensajeApi(Mensaje mensaje)
        {
            try
            {
                var uri = new Uri(string.Format(MensajeAPI, string.Empty));
                var client = new HttpClient();
                var json = JsonConvert.SerializeObject(mensaje);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, content);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.StatusCode.ToString());
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}

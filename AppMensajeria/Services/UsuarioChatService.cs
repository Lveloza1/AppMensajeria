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
    public class UsuarioChatService
    {
        private readonly AppDbContext _context;
        private const string UsuarioChatAPI = "https://appmensajeria.azurewebsites.net/api/UsuarioChats";

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
        //Agregar usuario a Chat (Grupos)
        public async Task AgregarUsiarioAChatApi(UsuarioChat usiarioChat)
        {
            try
            {
                var uri = new Uri(string.Format(UsuarioChatAPI, string.Empty));
                var client = new HttpClient();
                var json = JsonConvert.SerializeObject(usiarioChat);
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
        //Obtener Chat del usuario
        public async Task<ObservableCollection<UsuarioChat>> ObtenerChatDelUsuarioApi(int id)
        {
            List<UsuarioChat> usuariochats = new List<UsuarioChat>();
            try
            {
                Uri uri = new Uri(string.Format(UsuarioChatAPI+"/"+id, string.Empty));
                HttpClient client = new HttpClient();
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var contenido = await response.Content.ReadAsStringAsync();
                    usuariochats = JsonConvert.DeserializeObject<List<UsuarioChat>>(contenido);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return usuariochats.ToObservableCollection();
        }
        //Obtener Grupos del usuario 
        public async Task<ObservableCollection<UsuarioChat>> ObtenerGruposDelUsuarioApi(int id)
        {
            List<UsuarioChat> usuariochats = new List<UsuarioChat>();
            try
            {
                Uri uri = new Uri(string.Format(UsuarioChatAPI + "/ChatGrupo/" + id, string.Empty));
                HttpClient client = new HttpClient();
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var contenido = await response.Content.ReadAsStringAsync();
                    usuariochats = JsonConvert.DeserializeObject<List<UsuarioChat>>(contenido);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return usuariochats.ToObservableCollection();
        }
        //Obtener Tabla UsuarioChat
        public async Task<ObservableCollection<UsuarioChat>> ObtenerTablaUsuarioChatApi()
        {
            List<UsuarioChat> usuariochats = new List<UsuarioChat>();
            try
            {
                Uri uri = new Uri(string.Format(UsuarioChatAPI, string.Empty));
                HttpClient client = new HttpClient();
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var contenido = await response.Content.ReadAsStringAsync();
                    usuariochats = JsonConvert.DeserializeObject<List<UsuarioChat>>(contenido);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return usuariochats.ToObservableCollection();
        }

        public ObservableCollection<UsuarioChat> ObtenerMisChats(int UsuaioID)
        {
            ObservableCollection<UsuarioChat> UsuarioChats = ObtenerUsuarioChats().Where(e => e.UsuarioID == UsuaioID).ToObservableCollection();
            return UsuarioChats;
        }

    }
}

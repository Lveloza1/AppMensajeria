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
    public class ChatService
    {
        private readonly AppDbContext _context;
        private const string ChatAPI = "https://appmensajeria.azurewebsites.net/api/Chats";

        public ChatService()
        {
            _context = App.GetContext();
        }
        public ObservableCollection<Chat> ObtenerChats()
        {
            return _context.Chats.ToObservableCollection();
        }
        public void CrearChat(Chat chat)
        {
            try
            {
                _context.Chats.Add(chat);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public ObservableCollection<Chat> ObtenerChatsPrivados()
        {
            ObservableCollection<Chat> Chats = ObtenerChats().Where(e => e.Tipo == false).ToObservableCollection();
            return Chats;
        }
        public ObservableCollection<Chat> ObtenerChatsGrupo()
        {
            ObservableCollection<Chat> Chats = ObtenerChats().Where(e => e.Tipo == true).ToObservableCollection();
            return Chats;
        }
        //Crear Chat (Grupos)
        public async Task CrearChatApi(Chat chat)
        {
            try
            {
                var uri = new Uri(string.Format(ChatAPI, string.Empty));
                var client = new HttpClient();
                var json = JsonConvert.SerializeObject(chat);
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
        //Obtiene lista de chat
        public async Task<ObservableCollection<Chat>> ObtenerChatsApi()
        {
            List<Chat> chats = new List<Chat>();
            try
            {
                Uri uri = new Uri(string.Format(ChatAPI, string.Empty));
                HttpClient client = new HttpClient();
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var contenido = await response.Content.ReadAsStringAsync();
                    chats = JsonConvert.DeserializeObject<List<Chat>>(contenido);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return chats.ToObservableCollection();
        }

    }
}

using AppMensajeria.Contexts;
using AppMensajeria.Models;
using System;
using System.Collections.ObjectModel;

namespace AppMensajeria.Services
{
    public class ChatService
    {
        private readonly AppDbContext _context;

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

    }
}

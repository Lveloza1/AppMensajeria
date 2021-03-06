﻿using AppMensajeria.Contexts;
using AppMensajeria.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;

namespace AppMensajeria.Services
{
    public class UsuarioService
    {
        private readonly AppDbContext _context;
        private const string UsuarioAPI = "https://appmensajeria.azurewebsites.net/api/Usuarios";

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
        public Usuario ObtenerUsusarioPorTelefono(string telefono)
        {
            Usuario usuario = _context.Usuarios.Where(e => e.Telefono == telefono).FirstOrDefault();
            return usuario;
        }
        //Obtiene lista de usuarios
        public async Task<ObservableCollection<Usuario>> ObtenerUsuariosApi()
        {
            List<Usuario> usuarios = new List<Usuario>();
            try
            {
                Uri uri = new Uri(string.Format(UsuarioAPI, string.Empty));
                HttpClient client = new HttpClient();
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var contenido = await response.Content.ReadAsStringAsync();
                    usuarios = JsonConvert.DeserializeObject<List<Usuario>>(contenido);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return usuarios.ToObservableCollection();
        }
        //Obtiene lista de usuarios
        public async Task<Usuario> ObtenerUsuarioTelefonoApi(string telefono)
        {
            Usuario usuario = new Usuario();
            try
            {
                Uri uri = new Uri(string.Format(UsuarioAPI+"/"+telefono, string.Empty));
                HttpClient client = new HttpClient();
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var contenido = await response.Content.ReadAsStringAsync();
                    usuario = JsonConvert.DeserializeObject<Usuario>(contenido);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return usuario;
        }
        //Crea usuario nuevo y chat privados
        public async Task<Usuario> CrearUsuarioApi(Usuario usuario)
        {
            try
            {
                var uri = new Uri(string.Format(UsuarioAPI, string.Empty));
                var client = new HttpClient();
                var json = JsonConvert.SerializeObject(usuario);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, content);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.StatusCode.ToString());
                }
                else
                {
                    var body = await response.Content.ReadAsStringAsync();
                    usuario = JsonConvert.DeserializeObject<Usuario>(body);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return usuario;
        }
        //llenar usuarios
        public void DBLocalUsuario(ObservableCollection<Usuario> usuarios)
        {
            var usuariosnuevos = usuarios.ToList();
            foreach (var x in usuariosnuevos)
            {
                Usuario validarusuario = _context.Usuarios.Find(x.UsuarioID);

                if (validarusuario == null)
                {
                    _context.Usuarios.Add(x);
                }
            }
            _context.SaveChanges();

        }

    }
}

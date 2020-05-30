using AppMensajeria.Models;
using AppMensajeria.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppMensajeria.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaUsuariosPage : ContentPage
    {
        public ObservableCollection<Usuario> Usuarios { get; set; }
        private readonly UsuarioService service;
        public ListaUsuariosPage()
        {
            InitializeComponent();
            service = new UsuarioService();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            List<UsuarioImagen> usuarioImagens = new List<UsuarioImagen>();
            Usuarios = await service.ObtenerUsuariosApi();
            foreach (var x in Usuarios)
            {
                UsuarioImagen usuario = new UsuarioImagen
                {
                    Nombre = x.Nombre,
                    Telefono = x.Telefono,
                    Imagen = Base64ToImage(x.Imagen),
                    Biografia = x.Biografia
                };
                usuarioImagens.Add(usuario);
            }
            ListUsuarios.ItemsSource = usuarioImagens.ToObservableCollection();
        }
        public ImageSource Base64ToImage(string imagestr)
        {
            ImageSource pic;
            byte[] bytes = Convert.FromBase64String(imagestr);

            using (MemoryStream ms = new MemoryStream(bytes))
            {
                pic = ImageSource.FromStream(() => new MemoryStream(bytes));
            }
            return pic;

        }

        private async void ItemLlamar_Clicked(object sender, EventArgs e)
        {
            var menuItem = sender as ListView;
            var usuario = menuItem.SelectedItem as UsuarioImagen;
            try
            {
                PhoneDialer.Open(usuario.Telefono);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Ocurrio el siguiente error: " + ex.Message, "Ok");
            }
        }

    }
}
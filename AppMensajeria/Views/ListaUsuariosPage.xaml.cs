using AppMensajeria.Models;
using AppMensajeria.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            service = new UsuarioService();
        }

        protected async override void OnAppearing()
        {
            //base.OnAppearing();
            //List<UsuarioImagen> usuarioImagens = new List<UsuarioImagen>();
            //Usuarios = await service.ObtenerUsuariosApi();
            //foreach (var x in Usuarios)
            //{
            //    UsuarioImagen usuario = new UsuarioImagen
            //    {
            //        Nombre = x.Nombre,
            //        Telefono = x.Telefono,
            //        Imagen = Base64ToImage(x.Imagen)
            //    };
            //    usuarioImagens.Add(usuario);
            //}
            //ListUsuarios.ItemsSource= usuarioImagens.ToArray();

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

    }
}
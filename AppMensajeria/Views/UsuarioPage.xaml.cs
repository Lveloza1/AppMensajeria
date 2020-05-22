using Android.Graphics.Drawables;
using AppMensajeria.Models;
using AppMensajeria.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Drawing;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppMensajeria.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UsuarioPage : ContentPage
    {
        public UsuarioPage()
        {
            InitializeComponent();
        }
        private MediaFile _mediaFile;
        private async void btnSelectPic_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Error", "No soportado por el dispositivo.", "OK");
                return;
            }
            else
            {
                var mediaOption = new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Medium
                };
                _mediaFile = await CrossMedia.Current.PickPhotoAsync();
                if (_mediaFile == null)return;
                ImageView.Source = ImageSource.FromStream(() => _mediaFile.GetStream());
            }
        }
        private async void ButtonRegistrar_Clicked(object sender, EventArgs e)
        {
            if (EntryNombre.Text == null || EntryNombre.Text == "")
            {
                await DisplayAlert("Error", "El campo Nombre es obligatorio.", "Aceptar");
            }
            else if (EntryTelefono.Text == null || EntryTelefono.Text == "")
            {
                await DisplayAlert("Error", "El campo Telefono es obligatorio.", "Aceptar");
            }
            else
            {
                try
                {

                    System.Drawing.Image imagen = System.Drawing.Image.FromStream(_mediaFile.GetStream());
                    MemoryStream ms = new MemoryStream();
                    imagen.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    var bytes = ms.ToArray();
        
                    Usuario usuario = new Usuario{
                        Imagen = bytes,
                        Nombre = EntryNombre.Text,
                        Telefono = EntryTelefono.Text,
                        Edad = int.Parse(EntryEdad.Text)
                    };
                    UsuarioService service = new UsuarioService();
                    service.CrearUsuario(usuario);
                    await DisplayAlert("Exito", "Su perfil ha sido almacenado.", "Aceptar");
                    EntryNombre.IsEnabled = false;
                    EntryTelefono.IsEnabled = false;
                    EntryEdad.IsEnabled = false;
                    ButtonRegistrar.IsVisible = false;
                    formBuscarUsuario.IsVisible = false;
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", "Ocurrio el siguiente error: " + ex.Message, "Aceptar");
                }
            }
        }
        private async void ButtonBuscarUsuario_Clicked(object sender, EventArgs e)
        {
            if (EntryBuscarTelefono.Text == null || EntryBuscarTelefono.Text == "")
            {
                await DisplayAlert("Error", "Debe ingresar un número de teléfono", "Aceptar");
            }
            else
            {
                try
                {
                    string telefono = EntryBuscarTelefono.Text;
                    UsuarioService service = new UsuarioService();
                    Usuario usuario = service.ObtenerUsusarioPorTelefono(telefono);
                    if(usuario != null)
                    {
                        EntryNombre.Text = usuario.Nombre;
                        EntryTelefono.Text = usuario.Telefono;
                        EntryEdad.Text = (usuario.Edad).ToString();

                        EntryNombre.IsEnabled = false;
                        EntryTelefono.IsEnabled = false;
                        EntryEdad.IsEnabled = false;
                        EntryBuscarTelefono.IsEnabled = false;
                        formBuscarUsuario.IsVisible = false;
                        ButtonRegistrar.IsVisible = false;

                    }
                    else
                    {
                        await DisplayAlert("Error", "El número de teléfono no fue encontrado", "Aceptar");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", "Ocurrio el siguiente error: " + ex.Message, "Aceptar");
                }
            }
        }
    }
}
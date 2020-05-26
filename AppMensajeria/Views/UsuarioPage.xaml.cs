using Android.Graphics.Drawables;
using AppMensajeria.Models;
using AppMensajeria.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
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
        private MediaFile file;

        public static Usuario this_usuario;
        public static Usuario GetThisUsuario()
        {
            return this_usuario;
        }
        private async void ButtonSelectPic_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Error", "No soportado por el dispositivo.", "OK");
                return;
            }
            else
            {
                file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    PhotoSize = PhotoSize.Medium,
                    CompressionQuality = 40
                });

                if (file == null) return;
                ImageView.Source = ImageSource.FromStream(() => file.GetStream());
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
                    Usuario usuario = new Usuario{
                        Imagen = ImageToBase64(file),
                        Nombre = EntryNombre.Text,
                        Telefono = int.Parse(EntryTelefono.Text),
                    };
                    UsuarioService service = new UsuarioService();
                    await service.CrearUsuarioApi(usuario);
                    this_usuario = usuario;
                    await DisplayAlert("Exito", "Su perfil ha sido almacenado.", "Aceptar");
                    ButtonSelectPic.IsVisible = false;
                    EntryNombre.IsEnabled = false;
                    EntryTelefono.IsEnabled = false;
                    ButtonRegistrar.IsVisible = false;
                    formBuscarUsuario.IsVisible = false;

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
                    int telefono = int.Parse(EntryBuscarTelefono.Text);
                    UsuarioService service = new UsuarioService();
                    Usuario usuario = service.ObtenerUsusarioPorTelefono(telefono);
                    if(usuario != null)
                    {
                        this_usuario = usuario;
                        EntryNombre.Text = usuario.Nombre;
                        EntryTelefono.Text = (usuario.Telefono).ToString();

                        EntryNombre.IsEnabled = false;
                        EntryTelefono.IsEnabled = false;
                        ButtonSelectPic.IsVisible = false;
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
        public string ImageToBase64(MediaFile file)
        {
            if (file == null) return "";
            byte[] imageBytes = File.ReadAllBytes(file.Path);
            string base64String = Convert.ToBase64String(imageBytes);
            return base64String;

        }
    }
}
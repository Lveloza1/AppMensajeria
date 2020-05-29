using AppMensajeria.Models;
using AppMensajeria.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppMensajeria.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UsuarioPage : ContentPage
    {
        PerfilService perfilService;
        private MediaFile file;
        public UsuarioPage()
        {
            InitializeComponent();
            perfilService = new PerfilService();
        }

        protected async override void OnAppearing()
        {
            UsuarioService service = new UsuarioService();
            service.DBLocalUsuario(await service.ObtenerUsuariosApi());

            Perfil this_usuario = perfilService.ObtenerPerfil();
            if (this_usuario == null)
            {
                formBuscarUsuario.IsVisible = true;
                ImageView.Source = "noimage.png";
                ButtonSelectPic.IsVisible = true;
                EntryNombre.IsEnabled = true;
                EntryTelefono.IsEnabled = true;
                EntryNombre.Text = "";
                EntryTelefono.Text = "";
                ButtonRegistrar.IsVisible = true;
                ButtonCerrarSesion.IsVisible = false;
            }
            else
            {
                formBuscarUsuario.IsVisible = false;
                ImageView.Source = Base64ToImage(this_usuario.MiImagen);
                ButtonSelectPic.IsVisible = false;
                EntryNombre.IsEnabled = false;
                EntryTelefono.IsEnabled = false;
                EntryNombre.Text = this_usuario.MiNombre;
                EntryTelefono.Text = this_usuario.MiTelefono;
                ButtonRegistrar.IsVisible = false;
                ButtonCerrarSesion.IsVisible = true;

            }
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
                try
                {
                    Usuario usuario = new Usuario
                    {
                        Imagen = ImageToBase64(file),
                        Nombre = EntryNombre.Text,
                        Telefono = EntryTelefono.Text,
                    };
                    UsuarioService service = new UsuarioService();

                    var current = Connectivity.NetworkAccess;

                    if (current == NetworkAccess.Internet) 
                    {

                        await service.CrearUsuarioApi(usuario);
                        PerfilService perfilService = new PerfilService();
                        perfilService.CrearPerfil(await service.ObtenerUsuarioTelefonoApi(usuario.Telefono)); //Hay que traerse lo sin buscar por teléfono
                        await DisplayAlert("Exito", "Su perfil ha sido almacenado.", "Aceptar");                        
                        ButtonSelectPic.IsVisible = false;
                        EntryNombre.IsEnabled = false;
                        EntryTelefono.IsEnabled = false;
                        ButtonRegistrar.IsVisible = false;
                        formBuscarUsuario.IsVisible = false;
                        ButtonCerrarSesion.IsVisible = true;
                    }
                    else
                    {
                        await DisplayAlert("Error de conexión", "Debe estar conectado para registrarse", "Aceptar");

                    }
                    
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
                    var current = Connectivity.NetworkAccess;

                    if (current == NetworkAccess.Internet)
                    {
                        string telefono = EntryBuscarTelefono.Text;
                        UsuarioService service = new UsuarioService();
                        var this_usuario = await service.ObtenerUsuarioTelefonoApi(telefono);
                        if (this_usuario != null)
                        {
                            PerfilService perfilService = new PerfilService();
                            perfilService.CrearPerfil(await service.ObtenerUsuarioTelefonoApi(telefono));
                            EntryNombre.Text = this_usuario.Nombre;
                            EntryTelefono.Text = (this_usuario.Telefono).ToString();
                            ImageView.Source = Base64ToImage(this_usuario.Imagen);

                            EntryNombre.IsEnabled = false;
                            EntryTelefono.IsEnabled = false;
                            ButtonSelectPic.IsVisible = false;
                            formBuscarUsuario.IsVisible = false;
                            ButtonRegistrar.IsVisible = false;
                            ButtonCerrarSesion.IsVisible = true;


                        }
                        else
                        {
                            await DisplayAlert("Error", "El número de teléfono no fue encontrado", "Aceptar");
                        }

                    }
                    else
                    {
                        await DisplayAlert("Error de conexión", "Debe estar conectado para buscar usuarios", "Aceptar");
                    }

                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", "Ocurrio el siguiente error: " + ex.Message, "Aceptar");
                }
            }
        }

        private async void ButtonCerrarSesion_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Cerrar sesión", "¿Seguro que desea cerrar sesión?", "Yes", "No");
            if (answer)
            {
                Perfil this_usuario = perfilService.ObtenerPerfil();
                perfilService.BorrarPerfil(this_usuario);
                this.OnAppearing();
            }
        }

        public string ImageToBase64(MediaFile file)
        {
            if (file == null)return "";
            byte[] imageBytes = File.ReadAllBytes(file.Path);
            string base64String = Convert.ToBase64String(imageBytes);
            return base64String;
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
using AppMensajeria.Models;
using AppMensajeria.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
using Xamarin.Essentials;
using System.Threading.Tasks;
using System.IO;

namespace AppMensajeria.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MensajeriaPage : ContentPage
    {
        public ObservableCollection<UsuarioChat> UsuarioChats { get; set; }
        private readonly UsuarioChatService service;
        private readonly MensajeService mensajeService;
        private readonly PerfilService perfilService;
        private readonly Perfil this_usuario;
        public Chat ChatSeleccionado { get; set; }
        private MediaFile file;
        public MensajeriaPage()
        {
            InitializeComponent();
            perfilService = new PerfilService();
            this_usuario = perfilService.ObtenerPerfil();
            service = new UsuarioChatService();
            mensajeService = new MensajeService();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            UsuarioChats = await service.ObtenerChatDelUsuarioApi(this_usuario.MiUsuarioID);
            PickerChats.ItemsSource = UsuarioChats;

        }

        private async void ButtonSelectPicEnviar_Clicked(object sender, EventArgs e)
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
            }
        }


        private async void EnviarMensaje_Clicked(object sender, EventArgs e)
        {

            if (EntryMensaje.Text == null || file == null)
            {
                await DisplayAlert("Error", "El mensaje no tiene contenido", "Aceptar");
            }
            else
            {
                try
                {
                    var current = Connectivity.NetworkAccess;

                    if (current == NetworkAccess.Internet)
                    {
                        var usuarioChat = UsuarioChats[PickerChats.SelectedIndex];

                        Mensaje mensaje = new Mensaje
                        {
                            UsuarioChatID = usuarioChat.UsuarioChatID,
                            Contenido = EntryMensaje.Text,        
                            Imagen = ImageToBase64(file),
                            InfoDate = DateTime.Now.ToString("hh:mm tt '-' dd/MM"),
                            Estado = true
                        };
                        UsuarioService service = new UsuarioService();

                        await mensajeService.EnviarMensajeApi(mensaje);
                        await llenarListaUsuario();

                        EntryMensaje.Text= "";

                    }

                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", "Ocurrio el siguiente error: " + ex.Message, "Aceptar");
                }
            }
        }

        private async void ButtonBuscarChat_Clicked(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            if(picker.SelectedIndex >= 0)
            {
                var chat = UsuarioChats[picker.SelectedIndex];
                var mensajes = await mensajeService.ObtenerMensajesDelChatApi(chat.ChatID);
                var Ordenarmensajes = mensajes.OrderBy(x => x.InfoDate);
                ListMensajes.ItemsSource = Ordenarmensajes.ToObservableCollection();
            }
            else
            {
                ListMensajes.ItemsSource = null;
            }
        }


        private async void ItemEscuchar_Clicked(object sender, EventArgs e)
        {
            var menuItem = sender as ListView;
            var mensaje = menuItem.SelectedItem as Mensaje;
            await TextToSpeech.SpeakAsync(mensaje.Contenido);
        }
 
        public async Task llenarListaUsuario()
        {
            var chat = UsuarioChats[PickerChats.SelectedIndex];
            var mensajes = await mensajeService.ObtenerMensajesDelChatApi(chat.ChatID);
            var Ordenarmensajes = mensajes.OrderBy(x => x.InfoDate);
            ListMensajes.ItemsSource = Ordenarmensajes.ToObservableCollection();
        }

        public string ImageToBase64(MediaFile file)
        {
            if (file == null) return "";
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
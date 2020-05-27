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

namespace AppMensajeria.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MensajeriaPage : ContentPage
    {
        public ObservableCollection<UsuarioChat> UsuarioChats { get; set; }
        private readonly UsuarioChatService service;
        private readonly MensajeService mensajeService;
        public Chat ChatSeleccionado { get; set; }
        private MediaFile file;
        public MensajeriaPage()
        {
            InitializeComponent();
            service = new UsuarioChatService();
            mensajeService = new MensajeService();
            ImageEnviar.IsVisible = false;

        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            PerfilService perfil = new PerfilService();
            var p = perfil.ObtenerPerfil();
            UsuarioChats = await service.ObtenerChatDelUsuarioApi(p.Mi_UsuarioID);
            PickerChats.ItemsSource = UsuarioChats;
        }


        private async void EnviarMensaje_Clicked(object sender, EventArgs e)
        {

            if (EntryMensaje.Text == null)
            {
                await DisplayAlert("Error", "Debe ingresar un mensaje", "Aceptar");
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
                            //imagen = cuando la tengamos
                            InfoDate = DateTime.Now.ToString(),
                            Estado = true
                        };
                        UsuarioService service = new UsuarioService();

                        await mensajeService.EnviarMensajeApi(mensaje);

                        EntryMensaje.Text= "";

                    }

                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", "Ocurrio el siguiente error: " + ex.Message, "Aceptar");
                }

            }


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
            ImageEnviar.Source = ImageSource.FromStream(() => file.GetStream());
            ImageEnviar.IsVisible = true;

        }

        private async void ButtonBuscarChat_Clicked(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            var chat = UsuarioChats[picker.SelectedIndex];
            var mensajes = await mensajeService.ObtenerMensajesDelChatApi(chat.ChatID);
            ListMensajes.ItemsSource = mensajes;
        }


        private async void ItemEscuchar_Clicked(object sender, EventArgs e)
        {
            var menuItem = sender as ListView;
            var mensaje = menuItem.SelectedItem as Mensaje;

            var locales = await TextToSpeech.GetLocalesAsync();
            var locale = locales.FirstOrDefault();
            var settings = new SpeechOptions()
            {
                Volume = .75f,
                Pitch = 1.0f,
                Locale = locale
            };

            await TextToSpeech.SpeakAsync(mensaje.Contenido, settings);
        }
    }
}
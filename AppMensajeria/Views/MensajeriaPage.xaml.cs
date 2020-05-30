using AppMensajeria.Models;
using AppMensajeria.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace AppMensajeria.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MensajeriaPage : ContentPage
    {
        public ObservableCollection<UsuarioChat> UsuarioChats { get; set; }
        private readonly UsuarioChatService service;
        private readonly MensajeService mensajeService;
        private readonly PerfilService perfilService;
        private Perfil this_usuario;
        public Chat ChatSeleccionado { get; set; }
        private MediaFile file;
        public MensajeriaPage()
        {
            InitializeComponent();
            perfilService = new PerfilService();
            service = new UsuarioChatService();
            mensajeService = new MensajeService();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            this_usuario = perfilService.ObtenerPerfil();
            UsuarioChats = await service.ObtenerChatDelUsuarioApi(this_usuario.MiUsuarioID);
            PickerChats.ItemsSource = UsuarioChats;
            ButtonCompartir.IsEnabled = false;
            //RefrescarMensajes();

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

            if (EntryMensaje.Text == null && file == null)
            {
                await DisplayAlert("Error", "El mensaje no tiene contenido, Escriba un mensaje o agregue una imagen", "Aceptar");
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
                        await llenarListaMensajes();

                        EntryMensaje.Text= "";
                        file = null;

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
            if (picker.SelectedIndex >= 0)
            {
                await llenarListaMensajes();
                ButtonCompartir.IsEnabled = true;
            }
            else
            {
                ListMensajes.ItemsSource = null;
                ButtonCompartir.IsEnabled = false;
            }
        }


        private async void ItemEscuchar_Clicked(object sender, EventArgs e)
        {
            var menuItem = sender as ListView;
            var mensaje = menuItem.SelectedItem as MensajeImagen;
            if(mensaje.Contenido == null || mensaje.Contenido == "")
            {
                await TextToSpeech.SpeakAsync("Este mensaje no tiene texto");
            }
            else
            {
                await TextToSpeech.SpeakAsync(mensaje.Contenido);
            }
       
        }
 
        public async Task llenarListaMensajes()
        {
            var chat = UsuarioChats[PickerChats.SelectedIndex];
            var mensajes = await mensajeService.ObtenerMensajesDelChatApi(chat.ChatID);
            List<MensajeImagen> mensajeImagens = new List<MensajeImagen>();
            foreach (var x in mensajes.ToList())
            {
                MensajeImagen m = new MensajeImagen
                {
                    MensajeID = x.MensajeID,
                    UsuarioChatID = x.UsuarioChatID,
                    UsuarioChat = x.UsuarioChat,
                    Contenido = x.Contenido,
                    Estado = x.Estado,
                    Imagen = Base64ToImage(x.Imagen),
                    InfoDate = x.InfoDate
                };

                mensajeImagens.Add(m);

            }

            ListMensajes.ItemsSource = mensajeImagens.ToObservableCollection();
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

        private async void ButtonCompartir_Clicked(object sender, EventArgs e)
        {         
            var chat = UsuarioChats[PickerChats.SelectedIndex];

            var mensajes = await mensajeService.ObtenerMensajesDelChatApi(chat.ChatID);
            string conversacion = chat.Chat.Nombre + "\n\n";

            foreach (var m in mensajes)
            {
                conversacion += m.UsuarioChat.Usuario.Nombre + ": " + m.Contenido + " ("+ m.InfoDate +")" +"\n";
            }

            await Share.RequestAsync(new ShareTextRequest
            {
                Text = conversacion,
                Title = "Chat: " + chat.Chat.Nombre
            });

        }

        //private async void RefrescarMensajes()
        //{
        //    var Mensajes = await mensajeService.ObtenerMensajesDelUsuarioApi(this_usuario.MiUsuarioID);
        //    var mensajestemp = new ObservableCollection<Mensaje>();
        //    Task.Factory.StartNew(async() =>
        //    {             
        //        while (true)
        //        {
        //            mensajestemp = await mensajeService.ObtenerMensajesDelUsuarioApi(this_usuario.MiUsuarioID);
        //            if (Mensajes.Count() < mensajestemp.Count())
        //            {
        //                MainThread.BeginInvokeOnMainThread(async () =>
        //                {
        //                    try
        //                    {
        //                        var duration = TimeSpan.FromSeconds(1);
        //                        Vibration.Vibrate(duration);                                
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        DisplayAlert("Error", "Ocurrio el siguiente error" + ex, "Aceptar");
        //                    }
        //                    await llenarListaUsuario();

        //                });
        //                Task.Delay(1000);
        //                Vibration.Cancel();
        //                Mensajes = await mensajeService.ObtenerMensajesDelUsuarioApi(this_usuario.MiUsuarioID);
        //            }
        //        }
        //    });

        //}

    }
}
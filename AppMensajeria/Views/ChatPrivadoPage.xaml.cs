using AppMensajeria.Models;
using AppMensajeria.Services;
using System;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppMensajeria.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatPrivadoPage : ContentPage
    {
        public ObservableCollection<Usuario> Usuarios { get; set; }
        private readonly UsuarioService usuarioService;
        public Usuario UsuarioSeleccionado { get; set; }

        public ChatPrivadoPage()
        {
            InitializeComponent();
            usuarioService = new UsuarioService();
        }
        protected override void OnAppearing()
        {
            Usuarios = usuarioService.ObtenerUsuarios();
            PickerUsuario.ItemsSource = Usuarios;
        }

        private async void ButtonCrearPrivado_Clicked(object sender, EventArgs e)
        {
            
            try
            {
                UsuarioSeleccionado = (Usuario)PickerUsuario.SelectedItem;
                Chat chat = new Chat
                {
                   Nombre = "",
                   Tipo = false
                };
                ChatService service = new ChatService();
                service.CrearChat(chat);

                Usuario this_usuario = UsuarioPage.GetThisUsuario();

                UsuarioChat usuariochat = new UsuarioChat
                {
                    UsuarioID = this_usuario.UsuarioID,
                    ChatID = chat.ChatID
                };
                UsuarioChat usuariochat2 = new UsuarioChat
                {
                    UsuarioID = UsuarioSeleccionado.UsuarioID,
                    ChatID = chat.ChatID
                };

                UsuarioChatService service2 = new UsuarioChatService();
                service2.CrearUsuarioChat(usuariochat);
                service2.CrearUsuarioChat(usuariochat2);

                await DisplayAlert("Exito", "Un nuevo chat privado ha sido creado.", "Aceptar");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Ocurrio el siguiente error: " + ex.Message, "Aceptar");
            }
            
        }
    }
}
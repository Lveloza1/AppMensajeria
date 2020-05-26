using AppMensajeria.Models;
using AppMensajeria.Services;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppMensajeria.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatGrupoPage : ContentPage
    {
        public ObservableCollection<Usuario> Usuarios { get; set; }
        private readonly UsuarioService usuarioService;
        public Usuario UsuarioSeleccionado { get; set; }
        public ObservableCollection<Chat> Grupos { get; set; }
        private readonly ChatService chatService;
        public Chat ChatSeleccionado { get; set; }
        public ChatGrupoPage()
        {
            InitializeComponent();
            chatService = new ChatService();
            usuarioService = new UsuarioService();

        }
        protected override void OnAppearing()
        {
            Grupos = chatService.ObtenerChatsGrupo();
            PickerGrupo.ItemsSource = Grupos;
            Usuarios = usuarioService.ObtenerUsuarios();
            PickerUsuario.ItemsSource = Usuarios;
        }
        private async void ButtonCrearGrupo_Clicked(object sender, EventArgs e)
        {

            try
            {
                if (EntryNombreGrupo.Text == null || EntryNombreGrupo.Text == "")
                {
                    await DisplayAlert("Error", "Debe ingresar un nombre para el grupo.", "Aceptar");
                }
                else
                {
                    Chat chat = new Chat
                    {
                        Nombre = EntryNombreGrupo.Text,
                        Tipo = true
                    };
                    ChatService service = new ChatService();
                    service.CrearChat(chat);

                    Usuario this_usuario = UsuarioPage.GetThisUsuario();

                    UsuarioChat usuariochat = new UsuarioChat
                    {
                        UsuarioID = this_usuario.UsuarioID,
                        ChatID = chat.ChatID
                    };
                    UsuarioChatService service2 = new UsuarioChatService();
                    service2.CrearUsuarioChat(usuariochat);

                    Grupos = chatService.ObtenerChatsGrupo();
                    PickerGrupo.ItemsSource = Grupos;

                    await DisplayAlert("Exito", "Un nuevo grupo ha sido creado.", "Aceptar");
                    EntryNombreGrupo.Text = "";

                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Ocurrio el siguiente error: " + ex.Message, "Aceptar");
            }

        }
        private async void ButtonAgregarAGrupo_Clicked(object sender, EventArgs e)
        {
            try
            {
                ChatSeleccionado = (Chat)PickerGrupo.SelectedItem;
                UsuarioSeleccionado = (Usuario)PickerUsuario.SelectedItem;

                UsuarioChat usuariochat = new UsuarioChat
                {
                    UsuarioID = UsuarioSeleccionado.UsuarioID,
                    ChatID = ChatSeleccionado.ChatID
                };
                UsuarioChatService service = new UsuarioChatService();
                service.CrearUsuarioChat(usuariochat);
                await DisplayAlert("Exito", UsuarioSeleccionado.Nombre + " fue agregado a " +ChatSeleccionado.Nombre, "Aceptar");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Ocurrio el siguiente error: " + ex.Message, "Aceptar");
            }

        }
    }
}
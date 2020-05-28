using AppMensajeria.Models;
using AppMensajeria.Services;
using System;
using System.Collections.ObjectModel;
using Xamarin.Essentials;
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
        public ObservableCollection<UsuarioChat> Grupos { get; set; }
        private readonly UsuarioChatService usuariochatService;
        private readonly PerfilService perfilService;
        public Chat ChatSeleccionado { get; set; }
        public ChatGrupoPage()
        {
            InitializeComponent();
            usuariochatService = new UsuarioChatService();
            usuarioService = new UsuarioService();
            perfilService = new PerfilService();

        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            var this_usuario= perfilService.ObtenerPerfil();

            Grupos = await usuariochatService.ObtenerGruposDelUsuarioApi(this_usuario.MiUsuarioID);
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                Usuarios = await usuarioService.ObtenerUsuariosApi();
            }
            else
            {
                Usuarios = usuarioService.ObtenerUsuarios();
            }          
            PickerUsuario.ItemsSource = Usuarios;
            Grupos = await usuariochatService.ObtenerGruposDelUsuarioApi(this_usuario.MiUsuarioID);
            PickerGrupo.ItemsSource = Grupos;
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
                    var perfil = perfilService.ObtenerPerfil();

                    var chatNuevo = await service.CrearChatApi(chat);


                    UsuarioChat usuariochat = new UsuarioChat
                    {
                        UsuarioID = perfil.MiUsuarioID,
                        ChatID = chatNuevo.ChatID
                    };
                    UsuarioChatService service2 = new UsuarioChatService();
                    await service2.AgregarUsiarioAChatApi(usuariochat);

                    EntryNombreGrupo.Text = "";

                    await DisplayAlert("Exito", "Grupo creado exitosamente ", "Aceptar");
                    this.OnAppearing();
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
                ChatSeleccionado = ((UsuarioChat)PickerGrupo.SelectedItem).Chat;
                UsuarioSeleccionado = (Usuario)PickerUsuario.SelectedItem;

                UsuarioChat usuariochat = new UsuarioChat
                {
                    UsuarioID = UsuarioSeleccionado.UsuarioID,
                    ChatID = ChatSeleccionado.ChatID
                };
                UsuarioChatService service = new UsuarioChatService();
                await service.AgregarUsiarioAChatApi(usuariochat);
                await DisplayAlert("Exito", UsuarioSeleccionado.Nombre + " fue agregado a " +ChatSeleccionado.Nombre, "Aceptar");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Ocurrio el siguiente error: " + ex.Message, "Aceptar");
            }

        }
    }
}
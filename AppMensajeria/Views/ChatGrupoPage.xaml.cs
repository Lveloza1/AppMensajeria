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
        public ObservableCollection<UsuarioChat> Grupos { get; set; }
        private readonly UsuarioChatService usuariochatService;
        public Chat ChatSeleccionado { get; set; }
        public ChatGrupoPage()
        {
            InitializeComponent();
            usuariochatService = new UsuarioChatService();
            usuarioService = new UsuarioService();

        }
        protected async override void OnAppearing()
        {
            PerfilService perfilService = new PerfilService();
            var this_usuario= perfilService.ObtenerPerfil();

            Grupos = await usuariochatService.ObtenerGruposDelUsuarioApi(this_usuario.Mi_UsuarioID);
            PickerGrupo.ItemsSource = Grupos;
            Usuarios = await usuarioService.ObtenerUsuariosApi();
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
                    //var chatdsd = await service.CrearChatApi(chat);

                    //Usuario this_usuario = UsuarioPage.GetThisUsuario();

                    //UsuarioChat usuariochat = new UsuarioChat
                    //{
                    //    UsuarioID = this_usuario.UsuarioID,
                    //    //ChatID = chatdsd.ChatID;
                    //};
                    //UsuarioChatService service2 = new UsuarioChatService();
                    //await service2.AgregarUsiarioAChatApi(usuariochat);

                    //Grupos = await usuariochatService.ObtenerGruposDelUsuarioApi(12);
                    //PickerGrupo.ItemsSource = Grupos;

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
                service.AgregarUsiarioAChatApi(usuariochat);
                await DisplayAlert("Exito", UsuarioSeleccionado.Nombre + " fue agregado a " +ChatSeleccionado.Nombre, "Aceptar");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Ocurrio el siguiente error: " + ex.Message, "Aceptar");
            }

        }
    }
}
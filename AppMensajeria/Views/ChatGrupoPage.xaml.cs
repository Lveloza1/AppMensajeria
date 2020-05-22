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
            Grupos = chatService.ObtenerChats(); //De tipo grupo
            PickerGrupo.ItemsSource = Grupos;
            Usuarios = usuarioService.ObtenerUsuarios();
            PickerUsuario.ItemsSource = Usuarios;
        }
        private async void ButtonCrearGrupo_Clicked(object sender, EventArgs e)
        {

            try
            {
                ChatSeleccionado = (Chat)PickerGrupo.SelectedItem;
                UsuarioSeleccionado = (Usuario)PickerUsuario.SelectedItem;

                Chat chat = new Chat
                {
                    Titulo = EntryNombreGrupo.Text,
                    Tipo = "grupo",
                };
                ChatService service = new ChatService();
                service.CrearChat(chat);
                await DisplayAlert("Exito", "Un nuevo grupo ha sido creado.", "Aceptar");
                EntryNombreGrupo.Text = "";
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Ocurrio el siguiente error: " + ex.Message, "Aceptar");
            }

        }
    }
}
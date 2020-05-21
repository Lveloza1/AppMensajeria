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
        public ObservableCollection<Chat> Grupos { get; set; }
        private readonly ChatService chatService;
        public Chat ChatSeleccionado { get; set; }
        public ChatGrupoPage()
        {
            InitializeComponent();
            chatService = new ChatService();
        }
        protected override void OnAppearing()
        {
            Grupos = chatService.ObtenerChats();
            PickerGrupo.ItemsSource = Grupos;
        }
        private async void ButtonCrearGrupo_Clicked(object sender, EventArgs e)
        {

            try
            {
                ChatSeleccionado = (Chat)PickerGrupo.SelectedItem;

                Chat chat = new Chat
                {
                    Titulo = ChatSeleccionado.Titulo,
                    Tipo = "grupo",
                };
                ChatService service = new ChatService();
                service.CrearChat(chat);
                await DisplayAlert("Exito", "Un nuevo grupo ha sido creado.", "Aceptar");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Ocurrio el siguiente error: " + ex.Message, "Aceptar");
            }

        }
    }
}
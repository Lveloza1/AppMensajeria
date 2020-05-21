using AppMensajeria.Models;
using AppMensajeria.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppMensajeria.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UsuarioPage : ContentPage
    {
        public UsuarioPage()
        {
            InitializeComponent();
        }
        private async void ButtonRegistrar_Clicked(object sender, EventArgs e)
        {
            if (EntryNombre.Text == null || EntryNombre.Text == "")
            {
                await DisplayAlert("Error", "El campo Nombre es obligatorio.", "Aceptar");
            }
            if (EntryTelefono.Text == null || EntryTelefono.Text == "")
            {
                await DisplayAlert("Error", "El campo Telefono es obligatorio.", "Aceptar");
            }
            if (EntryEdad.Text == null || EntryEdad.Text == "")
            {
                await DisplayAlert("Error", "El campo Edad es obligatorio.", "Aceptar");
            }
            else
            {
                try
                {
                    Usuario usuario = new Usuario
                    {
                        Nombre = EntryNombre.Text,
                        Telefono = EntryTelefono.Text,
                        Edad = int.Parse(EntryEdad.Text)
                    };
                    UsuarioService service = new UsuarioService();
                    service.CrearUsuario(usuario);
                    await DisplayAlert("Exito", "Su perfil ha sido almacenado.", "Aceptar");
                    UserID.Text = "x";
                    EntryNombre.IsEnabled = false;
                    EntryTelefono.IsEnabled = false;
                    EntryEdad.IsEnabled = false;
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", "Ocurrio el siguiente error: " + ex.Message, "Aceptar");
                }
            }
        }
        //protected async override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    Usuarios = await service.ObtenerArtistasApi();
        //    ListArtistas.ItemsSource = Artistas;
        //}
    }
}
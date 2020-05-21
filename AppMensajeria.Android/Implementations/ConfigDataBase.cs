using AppMensajeria.Droid.Implementations;
using AppMensajeria.Interfaces;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(ConfigDataBase))]
namespace AppMensajeria.Droid.Implementations
{
    public class ConfigDataBase : IConfigDataBase
    {
        public string GetFullPath(string databaseFileName)
        {
            return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), databaseFileName);
        }
    }
}
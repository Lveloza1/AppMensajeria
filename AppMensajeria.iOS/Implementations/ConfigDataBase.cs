using AppMensajeria.IOS.Implementations;
using AppMensajeria.Interfaces;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(ConfigDataBase))]
namespace AppMensajeria.IOS.Implementations
{
    public class ConfigDataBase : IConfigDataBase
    {
        public string GetFullPath(string databaseFileName)
        {
            return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "..", "Library", databaseFileName);
        }
    }
}
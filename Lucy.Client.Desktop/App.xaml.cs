using Lucy.Client.Desktop.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace Lucy.Client.Desktop
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static WorkspaceModel ActiveWorkSpace { get; set; }


        public static List<string> SearchHistory { get; private set; }

        public App()
        {
            SearchHistory = new List<string>(20);
        }


        public static void NavigateTo(string destination)
        {
           
            ((NavigationWindow)App.Current.MainWindow)
                .Navigate(
                new Uri(destination, UriKind.Relative),
                App.ActiveWorkSpace);


        }
    }
}

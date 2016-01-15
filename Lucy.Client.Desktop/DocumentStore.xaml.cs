using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Lucy.Client.Desktop.ViewModel;

namespace Lucy.Client.Desktop
{
    /// <summary>
    /// Logique d'interaction pour DocumentStore.xaml
    /// </summary>
    public partial class DocumentStore : Page
    {
        public DocumentStore()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ((DocumentStoreViewModel)(DataContext)).LoadDocumentStore.Execute(null);
        }
        
    }
}

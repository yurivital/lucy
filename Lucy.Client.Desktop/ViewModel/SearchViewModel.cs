using Lucy.Client.Desktop.Service;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Lucy.Client.Desktop.ViewModel
{
    public class SearchViewModel : BindableBase, IDisposable
    {
        public SearchViewModel()
        {
            this.PerformSearch = new DelegateCommand<string>(OnPerformSearch);
            this.ManageIndex = new DelegateCommand(OnManageIndex);

        }
        /// <summary>
        ///   Get the command for execution a new search in the lucene index
        /// </summary>
        public ICommand PerformSearch { get; private set; }

        public ICommand ManageIndex { get; private set; }


        public void OnPerformSearch(string querie)
        {
            App.SearchHistory.Add(querie);
            App.NavigateTo(Pages.RESULT);
        }

        public void OnManageIndex()
        {
            App.NavigateTo(Pages.DOCUMENT_STORE);
        }


        public void Dispose()
        {

        }
    }
}

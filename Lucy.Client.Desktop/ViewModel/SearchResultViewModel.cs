﻿using Lucy.Client.Desktop.Service;
using Lucy.Core;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace Lucy.Client.Desktop.ViewModel
{

    public class SearchResultViewModel : BindableBase
    {
        private WorkspaceService _workspaceService = new WorkspaceService();

        private SearchService _search = new SearchService();
        public SearchResultViewModel()
        {
            Result = new ListCollectionView(new ObservableCollection<DocumentIdentity>());
            string workspaceLocation = Path.Combine(this._workspaceService.WorkspaceFolder, App.ActiveWorkSpace.Name);

            _search.LoadStore(workspaceLocation);
            GetResultAsync();

        }
        
        public ICollectionView Result { get; private set; }

        public async void GetResultAsync()
        {
            var result = await Task.Run<IEnumerable<DocumentIdentity>>(() =>
            {
                return _search.Search(App.SearchHistory.Last());
            });

            foreach(var r in result)
            {
                ((ObservableCollection<DocumentIdentity>)Result.SourceCollection).Add(r);
            }

        }
    }
}

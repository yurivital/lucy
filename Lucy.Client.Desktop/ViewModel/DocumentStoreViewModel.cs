using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using Lucy.Client.Desktop.Model;
using Lucy.Client.Desktop.Service;
using Lucy.Core;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;

namespace Lucy.Client.Desktop.ViewModel
{
    /// <summary>
    /// Control the display logic of the DocumentStore page
    /// </summary>
    public class DocumentStoreViewModel : BindableBase, IDisposable
    {
        /// <summary>
        /// Store the value of previously choosen path 
        /// </summary>
        /// 
        private string lastSelectedPath = null;
        /// <summary>
        /// Store the instance of <see cref="DocumentStoreService"/>
        /// </summary>
        private DocumentStoreService _documentStoreService = new DocumentStoreService();

        /// <summary>
        /// Store the intance of <see cref="WorkspaceService"/>
        /// </summary>
        private WorkspaceService _workspaceService = new WorkspaceService();

        /// <summary>
        /// Store the value of the location of the current workpace
        /// </summary>
        private string _workspaceLocation = string.Empty;

        /// <summary>
        /// Store teh instance of the active <see cref="WorkspaceModel"/>
        /// </summary>
        private WorkspaceModel _activeWorkspace = new WorkspaceModel();

        /// <summary>
        /// Store the instan of the current <see cref="DocumentStoreModel"/>
        /// </summary>
        private DocumentStoreModel _storeModel = new DocumentStoreModel();

        /// <summary>
        /// Get the collection of locations
        /// </summary>
        public ICollectionView LocationView { get; private set; }

        /// <summary>
        /// Create a new instance of <see cref="DocumentStoreViewModel"/>
        /// </summary>
        public DocumentStoreViewModel()
        {
            if(App.ActiveWorkSpace != null)
            {
                this._activeWorkspace = App.ActiveWorkSpace;
                this._workspaceLocation = Path.Combine(this._workspaceService.WorkspaceFolder, this.ActiveWorkspace.Name);
            }

            this.LoadDocumentStore = new DelegateCommand(this.OnLoadDocumentStore);
            this.SaveDocumentStore = new DelegateCommand(this.OnSaveDocumentStore);
            this.AddLocation = new DelegateCommand(OnAddLocation);
            this.RemoveLocation = new DelegateCommand(OnRemoveLocation);
            this.Index = new DelegateCommand(OnIndexAsync);
        }

        /// <summary>
        /// Get the command for loading the document store
        /// </summary>
        public ICommand LoadDocumentStore { get; private set; }

        /// <summary>
        /// Get the command for saving the document store
        /// </summary>
        public ICommand SaveDocumentStore { get; private set; }
        
        /// <summary>
        /// Get the command for adding a location in the store
        /// </summary>
        public ICommand AddLocation { get; private set; }

        /// <summary>
        /// Get the command for removing an existing location in the store
        /// </summary>
        public ICommand RemoveLocation { get; private set; }

        /// <summary>
        /// Get the command that perform indexing
        /// </summary>
        public ICommand Index { get; private set; }

        /// <summary>
        /// Get the instance of the current <see cref="DocumentStoreModel"/>
        /// </summary>
        public DocumentStoreModel StoreModel
        {
            get { return _storeModel; }
            private set
            {
                this.SetProperty<DocumentStoreModel>(ref _storeModel, value);
                this.LocationView = new ListCollectionView(_storeModel.Locations);
                this.OnPropertyChanged("LocationView");
            }
        }

        /// <summary>
        /// Handle the indexing logic
        /// </summary>
        public async void OnIndexAsync()
        {
            await Task.Run(() =>
            this._documentStoreService.Index(_storeModel.Locations));

            this._documentStoreService.SaveStore(_storeModel);

        }

        /// <summary>
        /// Handle the document store loading
        /// </summary>
        public void OnLoadDocumentStore()
        {
            StoreModel = this._documentStoreService.LoadStore(this._workspaceLocation);

        }

        /// <summary>
        /// Handle the document store persistance
        /// </summary>
        public void OnSaveDocumentStore()
        {
            this._documentStoreService.SaveStore(this._storeModel);
        }

        /// <summary>
        /// Handle adding a location in the location collection and save the store
        /// </summary>
        public void OnAddLocation()
        {
            bool selected = false;
            using (FolderBrowserDialog fdg = new FolderBrowserDialog())
            {
                fdg.SelectedPath = lastSelectedPath ?? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                selected = fdg.ShowDialog() == DialogResult.OK;
                lastSelectedPath = fdg.SelectedPath;
            }

            if (!selected)
            {
                return;
            }

            AddLocationToModel();
        }

        /// <summary>
        /// Handle remove location from the location collection and save the store
        /// </summary>
        public void OnRemoveLocation()
        {
            DocumentLocation location = (DocumentLocation) this.LocationView.CurrentItem;
            this._storeModel.Locations.Remove(location);
            this.OnSaveDocumentStore();
        }

        /// <summary>
        ///  Add the selected path in the location collection and persist the document store instance
        /// </summary>
        private void AddLocationToModel()
        {
            DocumentLocation location = new DocumentLocation();
            location.Location = lastSelectedPath;
            location.State = DiscoveryStates.NotExplored;
            bool canAdd = !StoreModel.Locations.Contains(location) 
                && !StoreModel.IntersectWith(location); 
            if (canAdd)
            {
                this.StoreModel.Locations.Add(location);
                this.OnSaveDocumentStore();
            }
        }

        /// <summary>
        /// Get the instance of the active <see cref="WorkspaceModel"/>
        /// </summary>
        public WorkspaceModel ActiveWorkspace
        {
            get
            {
                return this._activeWorkspace;
            }
            private set
            {
                SetProperty<WorkspaceModel>(ref this._activeWorkspace, value);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this._documentStoreService.Dispose();
                }

                disposedValue = true;
            }
        }


        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}


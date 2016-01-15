using System;
using System.Collections.ObjectModel;
using Lucy.Core;
using Microsoft.Practices.Prism.Mvvm;

namespace Lucy.Client.Desktop.Model
{
    [Serializable]
    public class DocumentStoreModel : BindableBase
    {
        private ObservableCollection <DocumentLocation> _locations = new ObservableCollection<DocumentLocation>();
        public ObservableCollection<DocumentLocation> Locations
        {
            get
            {
                return _locations;
            }

            private set
            {
                _locations = value;
                OnPropertyChanged("Locations");
            }
        }

        public bool IntersectWith(DocumentLocation location)
        {
           foreach(var loc in _locations)
            {
                bool result = loc.Contain(location);
                if(result)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

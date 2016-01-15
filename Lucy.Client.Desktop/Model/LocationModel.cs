using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucy.Core;
using Microsoft.Practices.Prism.Mvvm;

namespace Lucy.Client.Desktop.Model
{
  public   class LocationModel : BindableBase
    {
        private DocumentLocation _location;
        public LocationModel(DocumentLocation location)
        {
            _location = location;
        }

        public virtual string Location
        {
            get
            {
                return _location.Location;
            }

            set
            {
                _location.Location = value;
                this.OnPropertyChanged("Location");
            }
        }

        public virtual Boolean Exploring
        {
            get
            {
                return _location.State == DiscoveryStates.Exploring;
            }

        }

        public virtual DateTime? LastDiscovered
        {
            get
            {
                return _location.LastDiscovered;
            }

            set
            {
                _location.LastDiscovered = value;
                this.OnPropertyChanged("LastDiscovered");
            }
        }
    }
}

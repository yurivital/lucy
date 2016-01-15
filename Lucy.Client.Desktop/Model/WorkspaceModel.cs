using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;

namespace Lucy.Client.Desktop.Model
{
    /// <summary>
    /// Represente a Worskpace location
    /// </summary>
    public class WorkspaceModel : BindableBase
    {
        private string _Name;
        private DateTime _LastModified;

        /// <summary>
        /// Directory of the workspace
        /// </summary>
        public string Name
        {
            get { return this._Name; }
            set
            {
                this._Name = value;
                OnPropertyChanged("Name");         
            }
        }

        public DateTime LastModified
        {
            get { return this._LastModified; }
            set
            {
                this._LastModified = value;
                OnPropertyChanged("LastModified");
            }
        }
    }
}

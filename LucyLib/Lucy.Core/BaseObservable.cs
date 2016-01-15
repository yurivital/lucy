using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lucy.Core
{
    public abstract class BaseObservable : INotifyPropertyChanged
    {
        protected void SetProperty<T>(ref T store, T value, [CallerMemberName] string name = null)
        {
            store = value;
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

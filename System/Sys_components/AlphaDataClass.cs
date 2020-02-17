using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys_components.Elements
{
    public class AlphaDataClass : INotifyPropertyChanged
    {
        private string _propertyName;
        private string _value;

        public event PropertyChangedEventHandler PropertyChanged;
        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        public string PropertyName
        {
            get { return _propertyName; }
            set
            {
                if (_propertyName != value)
                {
                    _propertyName = value;
                    NotifyPropertyChanged("PropertyName");
                }
            }
        }

        public string Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    NotifyPropertyChanged("Value");
                }
            }
        }

    }
}

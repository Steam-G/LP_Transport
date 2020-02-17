using System;
using System.ComponentModel;

namespace Sys_components.Elements
{
    public class PropertyClass : INotifyPropertyChanged
    {
        private string _valName;
        private ushort _val00;
        private ushort _val01;
        private ushort _val02;
        private ushort _val03;
        private float _val04;


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


        public string ValName
        {
            get { return _valName; }
            set
            {
                if (_valName != value)
                {
                    _valName = value;
                    NotifyPropertyChanged("ValName");
                }
            }
        }

        public ushort Val00
        {
            get { return _val00; }
            set
            {
                if (_val00 != value)
                {
                    _val00 = value;
                    NotifyPropertyChanged("Val00");
                }
            }
        }

        public ushort Val01
        {
            get { return _val01; }
            set
            {
                if (_val01 != value)
                {
                    _val01 = value;
                    NotifyPropertyChanged("Val01");
                }
            }
        }

        public ushort Val02
        {
            get { return _val02; }
            set
            {
                if (_val02 != value)
                {
                    _val02 = value;
                    NotifyPropertyChanged("Val02");
                }
            }
        }

        public ushort Val03
        {
            get { return _val03; }
            set
            {
                if (_val03 != value)
                {
                    _val03 = value;
                    NotifyPropertyChanged("Val03");
                }
            }
        }

        public float Val04
        {
            get { return _val04; }
            set
            {
                if (_val04 != value)
                {
                    _val04 = value;
                    NotifyPropertyChanged("Val04");
                }
            }
        }

        
    }
}

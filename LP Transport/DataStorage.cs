using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LP_Transport
{
    public class DataStorage : INotifyPropertyChanged
    {
        private double _ValZaboi;
        private double _ValDoloto;
        private string _ValZaboiStr;
        private string _ValDolotoStr;
        private string _IpAddr;

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

        public string IpAddr
        {
            get
            {
                return _IpAddr;
            }

            set
            {
                if (_IpAddr != value)
                {
                    _IpAddr = value;
                    NotifyPropertyChanged("IpAddr");

                }
            }
        }

        public double ValZaboi
        {
            get
            {
                return _ValZaboi;
            }

            set
            {
                if (_ValZaboi != value)
                {
                    _ValZaboi = value;
                    NotifyPropertyChanged("ValZaboi");
                    ValZaboiStr = value.ToString("#.##");
                }
            }
        }

        public double ValDoloto
        {
            get
            {
                return _ValDoloto;
            }

            set
            {
                if (_ValDoloto != value)
                {
                    _ValDoloto = value;
                    NotifyPropertyChanged("ValDoloto");
                    ValDolotoStr = value.ToString("#.##");
                }
            }
        }

        public string ValDolotoStr
        {
            get { return _ValDolotoStr; }
            set
            {
                if (_ValDolotoStr != value)
                {
                    _ValDolotoStr = value;
                    NotifyPropertyChanged("ValDolotoStr");
                }
            }
        }

        public string ValZaboiStr
        {
            get
            {
                return _ValZaboiStr;
            }

            set
            {
                if (_ValZaboiStr != value)
                {
                    _ValZaboiStr = value;
                    NotifyPropertyChanged("ValZaboiStr");
                }
            }
        }

    }
}

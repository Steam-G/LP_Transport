using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Sys_components
{
    public partial class PropertyBox : UserControl
    {
        private string _PropertyName;
        private string _Value_0;
        private string _Value_1;
        private string _Value_2;
        private string _Value_3;
        private string _Value_4;


        /*public event PropertyChangedEventHandler PropertyChanged;
        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }*/


        public string PropertyName
        {
            get { return _PropertyName; }
            set
            {
                if (_PropertyName != value)
                {

                    _PropertyName = value;
                    lbl00.Text = _PropertyName;
                    //NotifyPropertyChanged("PropertyName");
                }
            }
        }

        public string Value_0
        {
            get { return _Value_0; }
            set
            {
                if (_Value_0 != value)
                {

                    _Value_0 = value;
                    val00.Text = _Value_0;
                    //NotifyPropertyChanged("Value_0");
                }
            }
        }

        public string Value_1
        {
            get { return _Value_1; }
            set
            {
                if (_Value_1 != value)
                {

                    _Value_1 = value;
                    val01.Text = _Value_1;
                    //NotifyPropertyChanged("Value_1");
                }
            }
        }

        public string Value_2
        {
            get { return _Value_2; }
            set
            {
                if (_Value_2 != value)
                {

                    _Value_2 = value;
                    val02.Text = _Value_2;
                    //NotifyPropertyChanged("Value_2");
                }
            }
        }

        public string Value_3
        {
            get { return _Value_3; }
            set
            {
                if (_Value_3 != value)
                {

                    _Value_3 = value;
                    val03.Text = _Value_3;
                    //NotifyPropertyChanged("Value_3");
                }
            }
        }

        public string Value_4
        {
            get { return _Value_4; }
            set
            {
                if (_Value_4 != value)
                {

                    _Value_4 = value;
                    val04.Text = _Value_4;
                    //NotifyPropertyChanged("Value_4");
                }
            }
        }

        public PropertyBox()
        {
            InitializeComponent();
            //TextBox.CheckForIllegalCrossThreadCalls = false;

        }
    }
}

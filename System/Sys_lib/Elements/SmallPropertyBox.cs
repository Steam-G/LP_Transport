using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sys_components
{
    public partial class SmallPropertyBox : UserControl
    {
        private string _PropertyName;
        private string _Value;

        public string PropertyName
        {
            get { return _PropertyName; }
            set
            {
                if (_PropertyName != value)
                {

                    _PropertyName = value;
                    lbl0.Text = _PropertyName;
                    //NotifyPropertyChanged("PropertyName");
                }
            }
        }

        public string Value
        {
            get { return _Value; }
            set
            {
                if (_Value != value)
                {

                    _Value = value;
                    lbl1.Text = _Value;
                    //NotifyPropertyChanged("Value_0");
                }
            }
        }

        public SmallPropertyBox()
        {
            InitializeComponent();
        }
    }
}

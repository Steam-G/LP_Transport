using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sys_lib
{
    public partial class ComSettings : Form
    {
        private Int32 _BaudRate = 9600;
        private System.IO.Ports.Parity _Parity = System.IO.Ports.Parity.None;
        private Int32 _DataBits = 8;
        private System.IO.Ports.StopBits _StopBit = System.IO.Ports.StopBits.One;
        private byte _DeviceAddress = 240;

        public ComSettings()
        {
            InitializeComponent();
        }

        private void ComSettings_Load(object sender, EventArgs e)
        {
            // Определяем список возможных скоростей порта и заносим в комбобокс
            string[] baudrate = { "2400", "4800", "9600", "14400", "19200", "28800", "38400", "57600", "115200" };
            comboBox1.Items.AddRange(baudrate);
            comboBox1.SelectedIndex = comboBox1.FindString(_BaudRate.ToString());

            // Получаем список доступных вариантов четности и заносим в комбобокс
            comboBox2.DataSource = Enum.GetValues(typeof(System.IO.Ports.Parity));
            comboBox2.SelectedIndex = comboBox2.FindString(_Parity.ToString());

            // Определяем варианты длины слова
            string[] databits = { "7", "8" };
            comboBox3.Items.AddRange(databits);
            comboBox3.SelectedIndex = comboBox3.FindString(_DataBits.ToString());

            // Получаем список возможных стоповых битов
            comboBox4.DataSource = Enum.GetValues(typeof(System.IO.Ports.StopBits));
            comboBox4.SelectedIndex = comboBox4.FindString(_StopBit.ToString());

            textBox1.Text = _DeviceAddress.ToString();
        }

        public Int32 BaudRate
        {
            get { return Convert.ToInt32(comboBox1.SelectedItem.ToString()); }
            set { if (_BaudRate != value) _BaudRate = value; /*comboBox1.SelectedIndex = comboBox1.FindString(value.ToString()); */}
        }

        public System.IO.Ports.Parity Parity
        {
            get { return (System.IO.Ports.Parity)comboBox2.SelectedItem; }
            set { if (_Parity != value) _Parity = value; /*comboBox2.SelectedIndex = comboBox2.FindString(value.ToString()); */}
        }

        public Int32 DataBits
        {
            get { return Convert.ToInt32(comboBox3.SelectedItem.ToString()); }
            set { if (_DataBits != value) _DataBits = value; /*comboBox3.SelectedIndex = comboBox1.FindString(value.ToString()); */}
        }

        public System.IO.Ports.StopBits StopBits
        {
            get { return (System.IO.Ports.StopBits)comboBox4.SelectedItem; }
            set { if (_StopBit != value) _StopBit = value; /* comboBox4.SelectedIndex = comboBox1.FindString(value.ToString()); */}
        }

        public byte DeviceAddress
        {
            get { return Convert.ToByte(textBox1.Text); }
            set { if (_DeviceAddress != value) textBox1.Text = value.ToString(); }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sys_components;

namespace LP_Transport
{
    public partial class Form1 : Form
    {
        ProcTelem ProcTelem;
      LeuzaRegReceiver leuzaRegReceiver = null;
        
        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            leuzaRegReceiver = new LeuzaRegReceiver();
            leuzaRegReceiver.Init();
            leuzaRegReceiver.StatusLabel = toolStripStatusLabel1;

            // Привязка элементов на экране к элементам объекта
            for (int i = 0; i < leuzaRegReceiver.SmallProperty.Count; i++)
            {
                panel2.Controls[i].DataBindings.Add("PropertyName", leuzaRegReceiver.SmallProperty[i], "PropertyName", true, DataSourceUpdateMode.OnPropertyChanged);
                panel2.Controls[i].DataBindings.Add("Value", leuzaRegReceiver.SmallProperty[i], "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            }

            //Запускаем прослушивание UDP пакетов на 138 порту, IP адреса всех соответствующих некоторым критериям источников заносим в список
            leuzaRegReceiver.SearchIP(comboBox1,button1);

            toolStripStatusLabel1.Text = "Дождитесь получения списка доступных IP адресов и осуществите подключение.";

            ProcTelem = new ProcTelem();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // работа параметрами кнопки, на которую нажали
            var b = sender as Button;
            if (b.Text == "Запустить прием")
            {
                b.Text = "Остановить прием";
  
                //leuzaRegReceiver.UDPtracking(true);
                leuzaRegReceiver.tcpClientReadPacket(comboBox1.SelectedItem.ToString());

                //Заблокируем выбор IP адреса, пока опрос не будет остановлен
                comboBox1.Enabled = false;

                // При старте в строке состояния должно быть зеленое сообщение, полужирным шрифтом
                //toolStripStatusLabel1.Text = string.Format("IP: {0}, идет опрос...",leuzaRegReceiver.SmallProperty[0].Value);
                //toolStripStatusLabel1.Font = new Font(toolStripStatusLabel1.Name, 9, FontStyle.Bold);
                //toolStripStatusLabel1.ForeColor = Color.Green;
            }
            else
            {
                b.Text = "Запустить прием";
                leuzaRegReceiver.tcpClientReadPacketStop();

                comboBox1.Enabled = true;
                // По остановке опроса сообщение в строке состояния будет перекрашено в серый цвет и изменится на обычный вид
                //toolStripStatusLabel1.Text = string.Format("IP: {0}, опрос завершен.", leuzaRegReceiver.SmallProperty[0].Value);
                //toolStripStatusLabel1.Font = new Font(toolStripStatusLabel1.Name, 9, FontStyle.Regular);
                //toolStripStatusLabel1.ForeColor = Color.Gray;
            }
        }

        #region Наброски кода

        private void UDPtracking(bool start)
        {
            //await Task.Run(() =>
            //{
            // Что-то делаем

            int port = 138;
            UdpClient server = null;
            

            try
            {
                server = new UdpClient(port);
                // Создаем переменную IPEndPoint, чтобы передать ссылку на нее в Receive()
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, port);

                //ThreadPool.QueueUserWorkItem(new WaitCallback((obj) =>
                //{
                //int i = 0;
                    // Получаем и отдаем сразу. Эхо сервер
                    while (start)
                    {
                    //label4.Text = "testtttt";
                        byte[] bytes = server.Receive(ref remoteEP);
                        //server.Send(bytes, bytes.Length, remoteEP);
                        string results = Encoding.UTF8.GetString(bytes);

                        string subString = @"\MAILSLOT\chromatograph";
                        int indexOfSubstring = results.IndexOf(subString); // равно 6

                        if (indexOfSubstring > 0)
                        {

                            //Console.WriteLine("ip: {0}, offset: {1}", remoteEP.Address, indexOfSubstring);
                            string ipServer = remoteEP.Address.ToString();

                            //lbVal3.Text = ipServer;
                            //dataStorage.IpAddr = ipServer;
                            //tcpClientReadPacket(ipServer, dataStorage);
                        leuzaRegReceiver.tcpClientReadPacket(ipServer);
                        }


                        //Console.WriteLine(remoteEP.ToString() + " отправил:" + results);
                        //if (results.ToLower().Equals("stop server")) break;
                        //Thread.Sleep(100);
                        Task.Delay(1000).Wait();
                    }
                //}));
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (server != null) server.Close();
            }
            //tcpClientReadPacket();
            //});
        }

        private void tcpClientReadPacket(string ip, DataStorage dataStorage)
        {

            const int port = 65004;

            try
            {
                TcpClient client = new TcpClient();
                client.Connect(ip, port);

                byte[] data = new byte[1514];
                StringBuilder response = new StringBuilder();
                NetworkStream stream = client.GetStream();

                byte[] buf = new byte[4096];
                int i = 0; // это номер пакета данных, регистрация шлет их несколькими пачками
                
                    do
                    {
                        int bytes = stream.Read(data, 0, data.Length);
                        response.Append(Encoding.Default.GetString(data, 0, bytes));
                        i++;
                        if (i == 2) // интересует второй пакет, там расположены забой и долото
                        {
                            string subString = @"UDataStorage";
                            int indexOfSubstring = response.ToString().IndexOf(subString); // равно 6

                            // Convert byte array elements to double values.
                            //Console.WriteLine(indexOfSubstring);
                            double value1 = BitConverter.ToDouble(data, indexOfSubstring + 19 - 1514);
                            double value2 = BitConverter.ToDouble(data, indexOfSubstring + 19 + 8 - 1514);

                            //lbVal1.Text = value1.ToString();
                            //lbVal2.Text = value2.ToString();
                            dataStorage.ValZaboi = value1;
                            dataStorage.ValDoloto = value2;
                        }
                    }
                    while (stream.DataAvailable); // пока данные есть в потоке

                    // Закрываем потоки
                    stream.Close();
                    client.Close();
                client.Dispose();
                
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }

            //Console.WriteLine("Запрос завершен...");
        }

        // Convert eight byte array elements to a double and display it.
        public static void BAToDouble(byte[] bytes, int index)
        {
            double value = BitConverter.ToDouble(bytes, index);

            //Console.WriteLine(formatter, index,
            //    BitConverter.ToString(bytes, index, 8), value);
            Console.WriteLine(value.ToString());
        }


        private void StartWork(object sender, EventArgs e)
        {
            var b = sender as Button;
            if (b.Text == "Старт")
            {
                b.Text = "Стоп";
                //RegReceiver.UDPtracking(true);
            }
            else
            {
                b.Text = "Старт";
                //RegReceiver.UDPtracking(false);
            }
        }


        private void TcpListnerTest(IPAddress myAddress, int port)
        {

            try
            {
                TcpListener listen = new TcpListener(myAddress, port);
                listen.Start();
                Byte[] bytes;
                while (true)
                {
                    TcpClient client = listen.AcceptTcpClient();
                    NetworkStream ns = client.GetStream();
                    if (client.ReceiveBufferSize > 0)
                    {
                        bytes = new byte[client.ReceiveBufferSize];
                        ns.Read(bytes, 0, client.ReceiveBufferSize);
                        string msg = Encoding.ASCII.GetString(bytes); //the message incoming
                        MessageBox.Show(msg);
                    }
                }
            }
            catch (Exception e)
            {
                //e
            }
        }
        #endregion

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "Обновить список") leuzaRegReceiver.SearchIP((ComboBox)sender, button1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            leuzaRegReceiver.tcpClientReadPacketStop();
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
                notifyIcon1.Visible = false;
            }
        }

        private void openMenuItem_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
                notifyIcon1.Visible = false;
            }
        }

        private void closeMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }


    }


}

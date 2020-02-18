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
        LeuzaRegReceiver leuzaRegReceiver = null;
        //DataStorage dataStorage = new DataStorage();
                            //LeuzaRegReceiver RegReceiver = new LeuzaRegReceiver();
        
        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //Передаем объект для сбора параметров в объект работы с сетевым трафиком
            //RegReceiver.DataStorage = new DataStorage();
            leuzaRegReceiver = new LeuzaRegReceiver();
            leuzaRegReceiver.Init();
            // Привязка элементов на экране к элементам объекта
            for (int i = 0; i < leuzaRegReceiver.SmallProperty.Count; i++)
            {
                panel2.Controls[i].DataBindings.Add("PropertyName", leuzaRegReceiver.SmallProperty[i], "PropertyName", true, DataSourceUpdateMode.OnPropertyChanged);
                panel2.Controls[i].DataBindings.Add("Value", leuzaRegReceiver.SmallProperty[i], "Value", true, DataSourceUpdateMode.OnPropertyChanged);

            }

            // Получаем список доступных портов и заносим в комбобокс
            comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
            if (comboBox1.Items.Count == 0) comboBox1.Items.Add("не найдено");
            comboBox1.Items.Add("Обновить список");
            comboBox1.SelectedIndex = 0;


            toolStripStatusLabel1.Text = "Для работы запустите прием данных и укажите ip Проводки для переправки данных";
            
            // Бинд параметров из объекта "LeuzaRegReceiver" в лейблы на форме
            //lbVal1.DataBindings.Add("Text", RegReceiver.DataStorage, "ValZaboiStr", true, DataSourceUpdateMode.OnPropertyChanged);
            //lbVal2.DataBindings.Add("Text", RegReceiver.DataStorage, "ValDolotoStr", true, DataSourceUpdateMode.OnPropertyChanged);
            //lbVal3.DataBindings.Add("Text", RegReceiver.DataStorage, "IpAddr", true, DataSourceUpdateMode.OnPropertyChanged);
            //new Thread(() =>
            //{
            //    Invoke((MethodInvoker)(() =>
            //    {
            //        UDPtracking();
            //    }));
            //}).Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region Пробники

            //UDPtracking(lbVal1, lbVal2, lbVal3);
            //UDPtracking(true);
            //new Thread(() => { Invoke((MethodInvoker)(() => { label1.Text = "проверка"; })); }).Start();
            //new Thread(() => { Invoke((MethodInvoker)(() => { UDPtracking(true); })); }).Start();


            //await Task.Factory.StartNew(() =>
            //{
            //    UDPtracking(true);
            //});



            //new Thread(() =>
            //{
            //    Action action = () =>
            //    {
            //        RegReceiver.UDPtracking(true);
            //    };
            //    if (InvokeRequired)
            //        Invoke(action);
            //    else
            //        action();

            //}).Start();


            //RegReceiver.UDPtracking(true);
            #endregion

            // работа параметрами кнопки, на которую нажали
            var b = sender as Button;
            if (b.Text == "Старт")
            {
                b.Text = "Стоп";
                //b.Enabled = false;
                //new Thread(() => 
                //{
                //    Invoke((MethodInvoker)(() =>
                //    {
                    
                //    //lbVal1.Text = dataStorage.ValZaboiStr;

                //    }));
                //}) { IsBackground = false }.Start();

                //leuzaRegReceiver.Start("192.168.1.5");

                leuzaRegReceiver.UDPtracking(true);
                leuzaRegReceiver.tcpClientReadPacket(comboBox1.SelectedItem.ToString());
                //UDPtracking(true);

                // При старте в строке состояния должно быть зеленое сообщение, полужирным шрифтом
                toolStripStatusLabel1.Text = string.Format("IP: {0}, идет опрос...",leuzaRegReceiver.SmallProperty[0].Value);
                toolStripStatusLabel1.Font = new Font(toolStripStatusLabel1.Name, 9, FontStyle.Bold);
                toolStripStatusLabel1.ForeColor = Color.Green;
            }
            else
            {
                b.Text = "Старт";
                //RegReceiver.UDPtracking(false);
                leuzaRegReceiver.Stop();
                //leuzaRegReceiver.UDPtracking(false);

                // По остановке опроса сообщение в строке состояния будет перекрашено в серый цвет и изменится на обычный вид
                toolStripStatusLabel1.Text = string.Format("IP: {0}, опрос завершен.", leuzaRegReceiver.SmallProperty[0].Value);
                toolStripStatusLabel1.Font = new Font(toolStripStatusLabel1.Name, 9, FontStyle.Regular);
                toolStripStatusLabel1.ForeColor = Color.Gray;
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
            if (comboBox1.SelectedItem.ToString() == "Обновить список")
            {
                comboBox1.Items.Clear();
                leuzaRegReceiver.SearchIP();
                comboBox1.Items.AddRange(leuzaRegReceiver.IPList.ToArray());
                if (comboBox1.Items.Count == 0) comboBox1.Items.Add("не найдено");
                comboBox1.Items.Add("Обновить список");
                comboBox1.SelectedIndex = 0;
            }
        }
    }


}

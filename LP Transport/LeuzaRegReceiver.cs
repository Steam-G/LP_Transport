using Sys_components;
using Sys_components.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LP_Transport
{
    public class LeuzaRegReceiver
    {
        // Объект с параметрами (далото, забой)
        private DataStorage _dataStorage = new DataStorage();
        // Список объектов, включающих в себя имя параметра и его значение (PropertyName, Value)
        private List<AlphaDataClass> _SmallProperty = new List<AlphaDataClass>();

        public DataStorage DataStorage
        {
            get { return _dataStorage; }
            set { _dataStorage = value; }
        }
        public List<AlphaDataClass> SmallProperty
        {
            get { return _SmallProperty; }
            set { _SmallProperty = value; }
        }

        // Перечисляем названия параметров, с которыми работаем
        private string[] ParameterNames = {
            "IP адрес",
            "Забой, м",
            "Долото, м"
        };

        public string[] testIP =
        {
            "192.168.0.1",
            "192.168.0.2",
            "192.168.0.3",
            "192.168.0.4",
            "192.168.0.5"
        };

        public List<string> IPList
        {
            get { return _iplist; }
            set { _iplist = value; }
        }
        private List<string> _iplist = new List<string>();

        //источник токена отмены
        CancellationTokenSource _tokenSource;



        public void Init()
        {
            //Заготовка для работы с пользовательским элементом SmallPropertyBox
            //Просто вносим в элементы на форме названия параметров
            for (ushort i = 0; i < ParameterNames.Length; i++)
            {
                SmallProperty.Add(new AlphaDataClass() { PropertyName = /*i.ToString() + ") " +*/ ParameterNames[i] });
            }
        }

        async public void SearchIP(ComboBox comboBox)
        {
            comboBox.Enabled = false;
            int port = 138;
            UdpClient server = null;
            List<string> bufIP = new List<string>();
            int iter = 0;

            try
            {
                server = new UdpClient(port);
                
                // Создаем переменную IPEndPoint, чтобы передать ссылку на нее в Receive()
                IPEndPoint remoteEP = null;

                // Получаем и отдаем сразу. Эхо сервер
                while (iter<10)
                {
                    UdpReceiveResult x = await server.ReceiveAsync();
                    byte[] bytes = x.Buffer;

                    string results = Encoding.UTF8.GetString(bytes);

                    string subString = @"\MAILSLOT\chromatograph";
                    int indexOfSubstring = results.IndexOf(subString); // равно 6

                    if (indexOfSubstring > 0)
                    {
                        string ipServer = x.RemoteEndPoint.Address.ToString();

                        bufIP.Add(ipServer);

                    }


                    //Console.WriteLine(remoteEP.ToString() + " отправил:" + results);
                    //if (results.ToLower().Equals("stop server")) break;
                    //Thread.Sleep(100);
                    iter++;
                    string progress = new String('.', iter);
                    await Task.Delay(100);
                    comboBox.Text = progress;
                    //comboBox.Items.Clear();
                    //comboBox.Items.Add(progress);
                }
                        return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (server != null) server.Close();
                _iplist = new List<string>(bufIP.Distinct());

                comboBox.Items.Clear();
                comboBox.Items.AddRange(IPList.ToArray());
                if (comboBox.Items.Count == 0) comboBox.Items.Add("не найдено");
                comboBox.Items.Add("Обновить список");
                comboBox.SelectedIndex = 0;
                comboBox.Enabled = true;
            }
        }

        async public void UDPtracking(bool start)
        {
            int port = 138;
            UdpClient server = null;


            try
            {
                server = new UdpClient(port);
                // Создаем переменную IPEndPoint, чтобы передать ссылку на нее в Receive()
                IPEndPoint remoteEP = null;

                // Получаем и обрабатываем сразу.
                while (start)
                {
                    byte[] bytes = server.Receive(ref remoteEP);
                    //server.Send(bytes, bytes.Length, remoteEP);
                    string results = Encoding.UTF8.GetString(bytes);

                    string subString = @"\MAILSLOT\chromatograph";
                    int indexOfSubstring = results.IndexOf(subString); // равно 6

                    if (indexOfSubstring > 0)
                    {
                        string ipServer = remoteEP.Address.ToString();

                        _dataStorage.IpAddr = ipServer;

                        tcpClientReadPacket(ipServer);
                        return;
                    }

                    await Task.Delay(100);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (server != null) server.Close();
            }
        }


        bool status = true;

        async public void tcpClientReadPacket(string ip)
        {
            SmallProperty[0].Value = ip;
            //_dataStorage.IpAddr = ip;
            const int port = 65004;

            //готовим токен отмены
            _tokenSource = new CancellationTokenSource();
            CancellationToken cancelToken = _tokenSource.Token;
            status = true;

            try
            {
                byte[] buf = new byte[4096];
                int i = 0; // это номер пакета данных, регистрация шлет их несколькими пачками

                while (status)
                {
                    TcpClient client = new TcpClient();
                    
                    await client.ConnectAsync(ip, port);
                    //client.Connect(ip, port);

                    byte[] data = new byte[1514];
                    StringBuilder response = new StringBuilder();
                    NetworkStream stream = client.GetStream();

                    do
                    {
                        int bytes = await stream.ReadAsync(data, 0, data.Length);
                        //int bytes = stream.Read(data, 0, data.Length);

                        response.Append(Encoding.Default.GetString(data, 0, bytes));
                        i++;
                        if (i == 2) // интересует второй пакет, там расположены забой и долото
                        {
                            //FindAndReadUDataStorage(data, response);

                            string subString = @"UDataStorage";
                            int indexOfSubstring = response.ToString().IndexOf(subString); // равно 6

                            SmallProperty[1].Value = BitConverter.ToDouble(data, indexOfSubstring + 19 - 1514).ToString("#.##");
                            SmallProperty[2].Value = BitConverter.ToDouble(data, indexOfSubstring + 19 + 8 - 1514).ToString("#.##");
                            break;
                        }
                        //await Task.Delay(1);
                    }
                    while (stream.DataAvailable); // пока данные есть в потоке

                    response.Clear();
                    stream.Dispose();
                    // Закрываем потоки
                    stream.Close();
                    client.Close();

                    i = 0;

                    //здесь будет выброшено исключение в случае нажатия на кнопку отмены
                    cancelToken.ThrowIfCancellationRequested();

                }


            }
            catch (SocketException e)
            {
                MessageBox.Show(e.Message);
                //Console.WriteLine("SocketException: {0}", e);
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Задача отменена.");
                status = false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                //Console.WriteLine("Exception: {0}", e.Message);
            }
        }

        private void FindAndReadUDataStorage(byte[] data, StringBuilder response)
        {
            string subString = @"UDataStorage";
            int indexOfSubstring = response.ToString().IndexOf(subString); // равно 6

            SmallProperty[1].Value = BitConverter.ToDouble(data, indexOfSubstring + 19 - 1514).ToString("#.##");
            SmallProperty[2].Value = BitConverter.ToDouble(data, indexOfSubstring + 19 + 8 - 1514).ToString("#.##");
        }


        private bool b;
        

        public void Start(string ip)
        {
            SmallProperty[0].Value = ip;
           //DataStorage.IpAddr = ip;
            const int port = 65004;

            b = true;
            try
            {
                TcpClient client = new TcpClient();
                client.Connect(ip, port);
                byte[] data = new byte[1514];
                StringBuilder response = new StringBuilder();
                NetworkStream stream = client.GetStream();

                byte[] buf = new byte[4096];
                int i = 0; // это номер пакета данных, регистрация шлет их несколькими пачками

                ThreadPool.QueueUserWorkItem(new WaitCallback((obj) =>
                {
                    while (b)
                    {
                        if (stream.DataAvailable)
                        {
                            int bytes = stream.Read(data, 0, data.Length);
                            response.Append(Encoding.Default.GetString(data, 0, bytes));
                            Thread.Sleep(300); // Delay 100ms
                            i++;
                            if (i == 2) // интересует второй пакет, там расположены забой и долото
                            {
                                string subString = @"UDataStorage";
                                int indexOfSubstring = response.ToString().IndexOf(subString); // равно 6

                                //SmallProperty[1].Value = i.ToString();
                                //SmallProperty[1].Value = BitConverter.ToDouble(data, indexOfSubstring + 19 - 1514).ToString("#.##");
                                //SmallProperty[2].Value = BitConverter.ToDouble(data, indexOfSubstring + 19 + 8 - 1514).ToString("#.##");
                            }

                        }
                        //Thread.Sleep(1000); // Delay 100ms
                        int gg = 25;
                        SmallProperty[2].Value = gg.ToString();
                    }
                    if (b == false)
                    {
                        response.Clear();
                        stream.Dispose();
                        // Закрываем потоки
                        stream.Close();
                        client.Close();
                    }
                }));
            }
            catch (Exception ex)
            {
                //throw ex;
                MessageBox.Show(ex.Message);
            }
        }

        public void Stop()
        {
            b = false;
            _tokenSource.Cancel();
            //serialPort1.Close();
        }
    }
}

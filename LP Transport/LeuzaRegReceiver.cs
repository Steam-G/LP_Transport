using Sys_components;
using Sys_components.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        public ToolStripStatusLabel StatusLabel
        {
            get { return _StatusLabel; }
            set { _StatusLabel = value; }
        }
        private ToolStripStatusLabel _StatusLabel;

        // Перечисляем названия параметров, с которыми работаем
        private string[] ParameterNames = {
            "IP адрес",
            "Забой, м",
            "Долото, м"
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
            //При поиске IP адресов блокируем выбор и ввод
            comboBox.Enabled = false;
            int port = 138;
            UdpClient server = null;
            List<string> bufIP = new List<string>();
            int iter = 0;

            try
            {
                server = new UdpClient(port);

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

                    //Индикатор прогресса поиска адресов
                    iter++;
                    string progress = new String('.', iter);
                    await Task.Delay(100);
                    comboBox.Text = progress;

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

                            Def.ZABOI = (decimal)BitConverter.ToDouble(data, indexOfSubstring + 19 - 1514);
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
                _StatusLabel.Text = string.Format("IP: {0}, прием данных завершен.", ip);
                _StatusLabel.Font = new Font(_StatusLabel.Name, 9, FontStyle.Regular);
                _StatusLabel.ForeColor = Color.Gray;
                //MessageBox.Show("Задача отменена.");
                status = false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                //Console.WriteLine("Exception: {0}", e.Message);
            }
        }



        // не используется, но выглядит наглядно
        private void FindAndReadUDataStorage(byte[] data, StringBuilder response)
        {
            string subString = @"UDataStorage";
            int indexOfSubstring = response.ToString().IndexOf(subString); // равно 6

            SmallProperty[1].Value = BitConverter.ToDouble(data, indexOfSubstring + 19 - 1514).ToString("#.##");
            SmallProperty[2].Value = BitConverter.ToDouble(data, indexOfSubstring + 19 + 8 - 1514).ToString("#.##");
        }

        public void tcpClientReadPacketStop()
        {
            _tokenSource.Cancel();
        }
    }
}

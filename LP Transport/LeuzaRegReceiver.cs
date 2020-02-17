﻿using Sys_components;
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
        private List<AlphaDataClass> _SmallProperty = new List<Sys_components.Elements.AlphaDataClass>();

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

        //источник токена отмены
        CancellationTokenSource _tokenSource;



        public void Init()
        {
            //Заготовка для работы с пользовательским элементом SmallPropertyBox
            //Просто вносим в элементы на форме названия параметров
            for (ushort i = 0; i < ParameterNames.Length; i++)
            {
                SmallProperty.Add(new AlphaDataClass() { PropertyName = i.ToString() + ") " + ParameterNames[i] });
            }
        }

        async public void UDPtracking(bool start)
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
                IPEndPoint remoteEP = null;

                //ThreadPool.QueueUserWorkItem(new WaitCallback((obj) =>
                //{

                // Получаем и отдаем сразу. Эхо сервер
                while (start)
                {
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
                        _dataStorage.IpAddr = ipServer;

                        //new Thread(() => { }) { IsBackground = true }.Start();
                        tcpClientReadPacket(ipServer, true);
                        return;
                    }


                    //Console.WriteLine(remoteEP.ToString() + " отправил:" + results);
                        //if (results.ToLower().Equals("stop server")) break;
                    //Thread.Sleep(100);
                    await Task.Delay(100);
                }
                //}));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (server != null) server.Close();
            }
            //tcpClientReadPacket();
            //});
        }


        bool state = false;
        async public void tcpClientReadPacket(string ip, bool inwork)
        {

            SmallProperty[0].Value = ip;
            //_dataStorage.IpAddr = ip;
            const int port = 65004;
            int k = 0;

            state = inwork;

            //готовим токен отмены
            _tokenSource = new CancellationTokenSource();
            CancellationToken cancelToken = _tokenSource.Token;


            try
            {

                byte[] buf = new byte[4096];
                int i = 0; // это номер пакета данных, регистрация шлет их несколькими пачками

                while (state)
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

                            //SmallProperty[1].Value = BitConverter.ToDouble(data, indexOfSubstring + 19 - 1514).ToString("#.##");
                            SmallProperty[2].Value = BitConverter.ToDouble(data, indexOfSubstring + 19 + 8 - 1514).ToString("#.##");
                            break;
                        }
                        //await Task.Delay(1);
                    }
                    while (stream.DataAvailable); // пока данные есть в потоке


                    if (stream.DataAvailable)
                    {
                        SmallProperty[1].Value = k++.ToString();
                    }

                    response.Clear();
                    stream.Dispose();
                    // Закрываем потоки
                    stream.Close();
                    client.Close();

                    i = 0;

                    //здесь будет выброшено исключение в случае нажатия на кнопку отмены
                    cancelToken.ThrowIfCancellationRequested();

                    //await Task.Delay(1000);
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
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                //Console.WriteLine("Exception: {0}", e.Message);
            }


            //Console.WriteLine("Запрос завершен...");
        }

        private void FindAndReadUDataStorage(byte[] data, StringBuilder response)
        {
            string subString = @"UDataStorage";
            int indexOfSubstring = response.ToString().IndexOf(subString); // равно 6

            SmallProperty[1].Value = BitConverter.ToDouble(data, indexOfSubstring + 19 - 1514).ToString("#.##");
            SmallProperty[2].Value = BitConverter.ToDouble(data, indexOfSubstring + 19 + 8 - 1514).ToString("#.##");
            
            // Convert byte array elements to double values.
            //Console.WriteLine(indexOfSubstring);

            //taStorage.ValZaboiStr = BitConverter.ToDouble(data, indexOfSubstring + 19 - 1514).ToString("#.##");
            //taStorage.ValDoloto = BitConverter.ToDouble(data, indexOfSubstring + 19 + 8 - 1514);

            //lbVal1.Text = value1.ToString();
            //lbVal2.Text = value2.ToString();
            
            //DataStorage.ValZaboi = value1;
            //DataStorage.ValDoloto = value2;
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
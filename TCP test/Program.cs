using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCP_test
{
    class Program
    {
        
        //private const string server = "192.168.1.82";

        static void Main(string[] args)
        {
            UDPtracking();
            Console.ReadLine();
        }

        private static void UDPtracking()
        {
            
            int port = 138;
            UdpClient server = null;
            try
            {

                server = new UdpClient(port);
                // Создаем переменную IPEndPoint, чтобы передать ссылку на нее в Receive()
                IPEndPoint remoteEP = null;
                // Получаем и отдаем сразу. Эхо сервер
                while (true)
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
                        tcpClientReadPacket(ipServer);
                        //break;
                    }


                    //Console.WriteLine(remoteEP.ToString() + " отправил:" + results);
                    if (results.ToLower().Equals("stop server")) break;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (server != null) server.Close();
            }



            //tcpClientReadPacket();
        }

        private static void tcpClientReadPacket(string ip)
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
                    //response.Append(Encoding.ASCII.GetString(data, 0, bytes));
                    //response.Append(Encoding.Unicode.GetString(data, 0, bytes));

                    i++;
                    if (i == 2) // интересует второй пакет, там расположены забой и долото
                    {
                        string subString = @"UDataStorage";
                        int indexOfSubstring = response.ToString().IndexOf(subString); // равно 6
                        // Convert byte array elements to double values.
                        Console.Clear();
                        Console.WriteLine("ip: {0}, indexSub: {1}", ip, indexOfSubstring);
                        //Console.WriteLine(indexOfSubstring);
                        BAToDouble(data, indexOfSubstring + 19 - 1514); // попал, поправить отображение числа
                        BAToDouble(data, indexOfSubstring + 19 + 8 - 1514);
                        break;
                    }
                }
                while (stream.DataAvailable); // пока данные есть в потоке

                //Выводим в консоли ответ от сервера в представленной кодировке
                //Console.WriteLine(response.ToString());

                // Закрываем потоки
                stream.Close();
                client.Close();

                //WriteMultiLineByteArray(data);

                //Console.WriteLine(formatter, "index", "array elements", "double");
                //Console.WriteLine(formatter, "-----", "--------------", "------");

                //// Convert byte array elements to double values.
                //BAToDouble(data, 2120);
                //BAToDouble(data, 2128);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }


            //string msg = Exchange("192.168.1.45", 65004, "");


            //Console.WriteLine(msg);

            Console.WriteLine("Запрос завершен...");
           


            //Console.Read();
        }

        static private string Exchange(string address, int port, string outMessage)
        {
            // Инициализация
            TcpClient client = new TcpClient(address, port);
            Byte[] data = Encoding.UTF8.GetBytes(outMessage);
            NetworkStream stream = client.GetStream();
            try
            {
                // Отправка сообщения
                stream.Write(data, 0, data.Length);
                // Получение ответа
                Byte[] readingData = new Byte[256];
                String responseData = String.Empty;
                StringBuilder completeMessage = new StringBuilder();
                int numberOfBytesRead = 0;
                do
                {
                    numberOfBytesRead = stream.Read(readingData, 0, readingData.Length);
                    completeMessage.AppendFormat("{0}", Encoding.UTF8.GetString(readingData, 0, numberOfBytesRead));
                }
                while (stream.DataAvailable);
                responseData = completeMessage.ToString();
                return responseData;
            }
            finally
            {
                stream.Close();
                client.Close();
            }
        }


        public static void ReceiveFrom1()
        {

            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint iep = new IPEndPoint(IPAddress.Any, 1280);
            sock.Bind(iep);
            EndPoint ep = (EndPoint)iep;
            Console.WriteLine("Ready ...");

            byte[] data = new byte[1024];
            int recv = sock.ReceiveFrom(data, ref ep);
            while (true)
            {
                string stringData = Encoding.ASCII.GetString(data, 0, recv);
                Console.WriteLine("received: {0}  from: {1}", stringData, ep.ToString());
            }

        }





        const string formatter = "{0,5}{1,27}{2,27:E16}";

            // Convert eight byte array elements to a double and display it.
            public static void BAToDouble(byte[] bytes, int index)
            {
                double value = BitConverter.ToDouble(bytes, index);

            //Console.WriteLine(formatter, index,
            //    BitConverter.ToString(bytes, index, 8), value);
            Console.WriteLine(value.ToString("#.##"));
            }

            // Display a byte array, using multiple lines if necessary.
            public static void WriteMultiLineByteArray(byte[] bytes)
            {
                const int rowSize = 16;
                int iter;

                Console.WriteLine("initial byte array");
                Console.WriteLine("------------------");

                for (iter = 0; iter < bytes.Length - rowSize; iter += rowSize)
                {
                    Console.Write(
                        BitConverter.ToString(bytes, iter, rowSize));
                    Console.WriteLine("___" + iter.ToString());
                }

                Console.WriteLine(BitConverter.ToString(bytes, iter));
                Console.WriteLine();
            }
        
    }
}

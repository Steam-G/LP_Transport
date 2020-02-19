using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Remoting.Messaging;
using System.Timers;
namespace LP_Transport
{

    public delegate void int1Decimal1ChangedHandler(int nomer, decimal val);
    public delegate void asyncDelTelem(out string str);
    public delegate void TextToReport(string str);

    class ProcTelem
    {
        // Протокол ТСР
        // Данные идут в посылках по одному параметру или нескольким параметром - имяПараметра=значение имяПараметра=значение 
        // высылается в ответ на прием текущая глубина
        //
        //
        int nModuls = 4;
        bool[] isModul = new bool[9];
        //const int NumberParam=26;
        string[] ValNames;


        string[] endValueSourceId;


        string[] telemParameters;

        public static bool test = false;

        public string buffer;

        // основной блок телеметрии ---------------------------------
        const int nParMather = 18;
        string[] ValNamesMather = new string[nParMather]
   {   "Зенитный угол", "Азимут сырой", "Отклонитель грав",
            "Обороты генератора", "Температура","Напряжение",
            "Вибрация", "Ток","Глубина",
            "Азимут отклонителя",   "Время", "Ускорение g",
            "Сопротивление 1", "Сопротивление 2", "Сопротивление 3",
            "Азимут коррекции", "ГК-ЗТС",      "Частота передачи"};


        string[] endValueSourceMather = new string[nParMather]  {   "210",  "211",  "212",
                                                                   "213",  "214",  "215",
                                                                   "216",  "217",  "218",
                                                                   "219",  "21A",  "21B",
                                                                   "21C",  "21D",  "21E",
                                                                   "21F",  "220",  "221"};


        string[] telemParamMather = new string[nParMather]
        { "параметр01", "параметр02", "параметр03",
            "параметр04","параметр05", "параметр06",
            "параметр07", "параметр08",
        // 0 зенит      1азим сырой  2отклонит    3 обороты     4 температура   5 напряж        6вибрация    7 ток             
        "параметр09", "параметр10", "параметр11","параметр12", "параметр13", "параметр14", "параметр15",
        // 8 глубина    9  отклонит2  10  ВРЕМЯ   11 МОДУЛЬ G    12 R1 ЭК      13 R2 ЭК        14 R3 ЭК                                                                 
        "параметр16",  "параметр19", "freq"};
        // азимут корр   гк           частота

        //  блок наддолотный телеметрии горизонта    ---------------------------------
        const int nParNdmGoriz = 8;
        string[] ValNamesNdmGoriz = new string[nParNdmGoriz]
        {
            "ГК НДМ_Гориз", "ТОК НДМ_Гориз","НАПРЯЖЕНИЕ НДМ_Гориз",        "ИНДИКАТОР_Гориз" ,
            "ЗУ ИНК_Гориз", " КС НДМ_Гориз", "ПотрЭерг НДМ_Гориз",   "НапрСигналНДМ_Гориз"  };



        string[] endValueSourceNdmGoriz = new string[nParNdmGoriz] {   "230",  "231",   "232", "233",
                                                           "234",  "235",   "236", "237" };


        static string[] telemParamNdmGoriz = new string[nParNdmGoriz] { "параметр20", "параметр21", "параметр22", "параметр23", "параметр24", "параметр25", "параметр26", "параметр27" };

        //  блок наддолотный телеметрии фирмы ЗТК    ---------------------------------
        const int nParNdmZTK = 9;


        string[] ValNamesNdmZTK = new string[nParNdmZTK]
        {
            "ЗенитZTK", "ГКверхZTK","ГКнизZTK",   "ОборотыZTK" ,
            "Осевая нагрузкаZTK", "RндмZTK", "ДавлениеZTK",   "Индикатор НДМZTK" ,   "НапрСигналОтНдмZTK" };


        string[] endValueSourceNdmZTK = new string[nParNdmZTK]   {   "240",  "241",   "242", "243",
                                                           "244",  "245",   "246", "247"  , "248"};


        static string[] telemParamNdmZTK = new string[nParNdmZTK] { "параметр30", "параметр31", "параметр32", "параметр33", "параметр34", "параметр35", "параметр36", "параметр37", "параметр38" };

        //  блок модуля НДМ-АМК     ---------------------------------



        const int nParNdmAMK = 15;


        string[] ValNamesNdmAMK = new string[nParNdmAMK]
        {
            "ЗенитAMK", "ГКндмAMK","ТемператураAMK",   "ОборотыAMK" ,
            "Осевая нагрузкаAMK", "ТОКндмAMK",   "Индикатор НДМAMK" ,   "НапрСигналОтНдмAMK"
        , "КСAMK ", "ГеомЗондФакторAMK ", "НапрЭлектрНДМAMK " , "КрутМоментAMK ", "ДавлВнутТрAMK ",
            "ГКВверхAMK ", "ГКВнизAMK "};


        string[] endValueSourceNdmAMK = new string[nParNdmAMK]   {   "250",  "251",   "252",
            "253",  "254",  "255",   "256", "257", "258",  "259",  "25A",   "25B", "25C"
            ,   "25D", "25E"  };


        static string[] telemParamNdmAMK = new string[nParNdmAMK]
        { "параметр40", "параметр41", "параметр42",
            "параметр43", "параметр44", "параметр45", "параметр46", "параметр47" ,
            "параметр48", "параметр49", "параметр50", "параметр51", "параметр52"
            , "параметр53", "параметр55"};

        //  блок следующий   телеметрии     ---------------------------------








        int nVadimParameters = 0;




        public event int1Decimal1ChangedHandler newData;
        public event EventHandler noData;

        public bool IsOn = true;
        bool isScan = false;
        int serverPort;
        string serverIP, serverName;
        byte[] data = new byte[1024];
        int bytesRec;
        string str;
        Socket MySock, handler;
        public IPAddress ipAdresServ;
        List<decimal> dec;
        public decimal[] telemDat = new decimal[100];
        public bool IsNewDate = false;
        public ParseTelem parse;
        System.Timers.Timer timer;
        float glub;
        uint oldVal = 0, newVal = 0;
        string strin;
        string format = " 12345 1k {0,-10} po ";
        int nParam;
        decimal valParam;
        // string[] paramNameVadim;
        string nameLog;


        public event TextToReport eText;



        string GlubinaForTelem()
        {
            glub = (float)Def.ZABOI;
            strin = string.Format(format, Def.ZABOI);
            return strin;
        }



        public ProcTelem()
        {
            Init();
            parse = new ParseTelem(ValNames);
            LoadConfig();
            ipAdresServ = IPAddress.Parse(serverIP);
            if (Def.test) InitLog();
            if (Def.test) toLog("begin    " + serverIP + "  " + ipAdresServ.ToString() + "  " + serverPort);

            timer = new System.Timers.Timer(13000);
            timer.Elapsed += OnTimerElapsed;
            timer.Start();
            RunScan();

        }
        void LoadConfig()
        {
            serverIP = Properties.Settings.Default.tcpIP;
            serverName = Properties.Settings.Default.tcpName;
            serverPort = Properties.Settings.Default.tcpPort;
            IsOn = Properties.Settings.Default.tcpOn;
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (oldVal == newVal)
                if (noData != null) noData(this, null);
            oldVal = newVal;
            if (isScan == false) RunScan();
        }

        public void RunScan()//gas
        {
            LoadConfig();
            if (IsOn == false)
            {
                if (Def.test) toLog(" scaner is Off");
                return;
            }
            isScan = true;
            if (Def.test) toLog(" run scan");
            asyncGetTelem();
        }


        void asyncGetTelem()
        {
            string paket;
            if (IsOn)
            {
                asyncDelTelem ad = new asyncDelTelem(TcpOn);
                AsyncCallback callbackTelem = new AsyncCallback(getTelemPaket);
                IAsyncResult ar = ad.BeginInvoke(out paket, callbackTelem, null);
            }
        }

        public void getTelemPaket(IAsyncResult ar)
        {
            string paket;
            asyncDelTelem ad = (asyncDelTelem)((AsyncResult)ar).AsyncDelegate;
            ad.EndInvoke(out paket, ar);
            isScan = false;
        }


        public void TcpOn(out string stringData)
        {


            //string myHost = System.Net.Dns.GetHostName();
            //IPAddress MyIpAddress = System.Net.Dns.GetHostByName(myHost).AddressList[0];

            IPHostEntry ipHostInfo = Dns.Resolve(serverIP);
            IPAddress MyIpAddress = ipHostInfo.AddressList[0];

            MySock = new Socket(ipAdresServ.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint MyEP = new IPEndPoint(MyIpAddress, serverPort);
            byte[] msg3;
            try
            {
                MySock.Bind(MyEP);
                MySock.Listen(10);
                bool isBeginning = true;

                while (true)
                {

                    if (isBeginning)
                    {
                        if (Def.test) toLog("waiting" + MyIpAddress.ToString());
                        handler = MySock.Accept();// Мы дождались клиента, пытающегося с нами соединиться
                        if (Def.test) toLog("open soket" + handler.AddressFamily.ToString());
                        isBeginning = false;

                    }
                    bytesRec = handler.Receive(data);
                    str = Encoding.Default.GetString(data, 0, bytesRec);

                    if (Def.test) toLog(str + "   " + DateTime.Now);
                    //tic++;
                    nParam = -1;
                    valParam = 0;
                    parse.GetParametrVadim(str, out nParam, out valParam);

                    if (nParam >= 0)
                    {
                        IsNewDate = true;
                        str = Encoding.Default.GetString(data, 0, bytesRec);
                        telemDat[nParam] = valParam; // param save in vector  
                        if (newData != null) newData(nParam, valParam);
                        newVal++;
                    }

                    //for (int i = 0; i < 10; i++) if (newData != null) newData(i, (decimal)(i+tic));

                    //fl += (float)0.1;
                    msg3 = Encoding.ASCII.GetBytes(GlubinaForTelem());
                    handler.Send(msg3);
                    if (str.IndexOf("<TheEnd>") > -1)
                    {
                        str = "Сервер завершил соединение с клиентом.";
                        handler.Shutdown(SocketShutdown.Both);
                        handler.Close();
                        MySock.Close();
                        IsOn = false;
                        return;
                    }
                }//while
            }//try
            catch (Exception e)
            {
                str = str + "закрытие сеанса связи ";
                if (Def.test) toLog(str);
                MySock.Close();
                return;

            }
            finally
            {
                stringData = str + "eeend";
                stringData = "eeend";
                if (Def.test) toLog(stringData);
            }
        }

        FileStream log;
        BinaryWriter writerLog;
        public void InitLog()
        {
            string fileName = "c:\\sensor\\logTelLeuza.txt";
            log = File.Create(fileName);
            writerLog = new BinaryWriter(log, Encoding.Default);
        }


        public void toLog(string str)
        {
            byte[] msg2 = Encoding.Default.GetBytes(DateTime.Now.ToString() + "      " + str + "\r\n");
            for (int i = 0; i < msg2.Length; i++) writerLog.Write(msg2[i]);
            buffer += str + "\r\n";
            // if (eText != null) eText(str + "\r\n");
        }

        public void closeLog()
        {
            writerLog.Close();
            log.Close();
        }

        void Init()
        {
            nVadimParameters = 0;
            for (int i = 0; i < nModuls; i++)
            {

                switch (i)
                {
                    case 0:
                        isModul[i] = Properties.Settings.Default.isModul1;
                        if (isModul[i]) nVadimParameters += telemParamMather.Length; break;
                    case 1:
                        isModul[i] = Properties.Settings.Default.isModul2;
                        if (isModul[i]) nVadimParameters += telemParamNdmGoriz.Length; break;
                    case 2:
                        isModul[i] = Properties.Settings.Default.isModul3;
                        if (isModul[i]) nVadimParameters += telemParamNdmZTK.Length; break;
                    case 3:
                        isModul[i] = Properties.Settings.Default.isModul4;
                        if (isModul[i]) nVadimParameters += telemParamNdmAMK.Length; break;
                }
            }

            if (nVadimParameters == 0)
            {
                nVadimParameters = telemParamMather.Length;
                isModul[0] = true;
            }

            ValNames = new string[nVadimParameters];
            endValueSourceId = new string[nVadimParameters];
            telemParameters = new string[nVadimParameters];

            int n = 0;
            for (int i = 0; i < nModuls; i++)
            {
                switch (i)
                {
                    case 0:
                        if (isModul[i])
                        {
                            for (int j = 0; j < telemParamMather.Length; j++)
                            {
                                ValNames[n] = ValNamesMather[j];
                                endValueSourceId[n] = endValueSourceMather[j];
                                telemParameters[n] = telemParamMather[j];
                                n++;
                            }
                        }
                        break;
                    case 1:
                        if (isModul[i])
                        {
                            for (int j = 0; j < telemParamNdmGoriz.Length; j++)
                            {
                                ValNames[n] = ValNamesNdmGoriz[j];
                                endValueSourceId[n] = endValueSourceNdmGoriz[j];
                                telemParameters[n] = telemParamNdmGoriz[j];
                                n++;
                            }
                        }
                        break;
                    case 2:
                        if (isModul[i])
                        {
                            for (int j = 0; j < telemParamNdmZTK.Length; j++)
                            {
                                ValNames[n] = ValNamesNdmZTK[j];
                                endValueSourceId[n] = endValueSourceNdmZTK[j];
                                telemParameters[n] = telemParamNdmZTK[j];
                                n++;
                            }
                        }
                        break;
                    case 3:
                        if (isModul[i])
                        {
                            for (int j = 0; j < telemParamNdmAMK.Length; j++)
                            {
                                ValNames[n] = ValNamesNdmAMK[j];
                                endValueSourceId[n] = endValueSourceNdmAMK[j];
                                telemParameters[n] = telemParamNdmAMK[j];
                                n++;
                            }
                        }
                        break;
                }

            }
        }



    }
}

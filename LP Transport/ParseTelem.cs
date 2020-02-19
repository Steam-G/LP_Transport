using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP_Transport
{
    class ParseTelem
    {
        // ВЫДЕЛЕНИЕ ИЗ  СТРОКИ 10:34:01      параметр02=2.0 параметр03=3.0 параметр04=4.0 параметр05=5.0 параметр06=6.0 параметр07=7.0 параметр08=8.0 параметр09=9.0 параметр10=10.0   ЧИСЛОВЫХ ЗНАЧЕНИЙ 
        const int maxlenghPacket = 2000;
        public int NumberParamVadim;

        char[] car = new char[maxlenghPacket];
        static string
            probel = " ",
            pabno = "=",
            nextString = "\n",
            begStr = "\r",
            zapjataja = ",",
            point = ".",
            Minus = "-",
            Zero = "\0";

        string[] telemParam1;

        static decimal parAndValParam;
        static char[] carrrr = point.ToCharArray();
        static char pointChar = carrrr[0];
        static char[] carrr = zapjataja.ToCharArray();
        static char zapjatajaChar = carrr[0];
        public static bool isNewDate = false;

        string word = "", data = "";
        bool isWord = false, isData = false;
        float ValParam;

        public ParseTelem(string[] parNames)
        {
            NumberParamVadim = parNames.Length;
            telemParam1 = parNames;
        }
        public void GetParametrVadim(string paket, out int nn, out decimal value)  //  ВЫДЕЛЕНИЕ ЛЕКСЕМ ТИПА ПАРАМЕТР=ЧИСЛО
        {
            //  параметры меньше чем 98 000
            // к значению параметра добавляется его номер (если распознан , умноженное на 100 000)
            car = paket.ToCharArray();

            word = "";
            data = "";
            isWord = false;
            isData = false;
            nn = -1;
            value = 0;




            for (int i = 0; i < paket.Length; i++)
            {
                if (car[i].ToString() == pabno)
                {
                    if (word == "") isWord = false; else isWord = true;
                    data = word;
                    word = "";
                    isData = true;
                    continue;
                }
                if ((car[i].ToString() == probel) | (car[i].ToString() == begStr) | (car[i].ToString() == nextString))
                {
                    if ((isWord) & (isData))
                    {

                        try
                        {
                            ValParam = float.Parse(word);

                        }

                        catch (Exception ex)
                        {
                            ValParam = 99999;
                        }

                        for (int ii = 0; ii < NumberParamVadim; ii++)
                        {
                            if (telemParam1[ii] == data)
                            {
                                nn = ii;
                                value = (decimal)ValParam;
                                return;
                            }
                        }
                        data = ""; isData = false;
                        word = ""; isWord = false;

                        continue;
                    }//if ((isWord) & (isData))

                } // if ((car[i].ToString() == probel) | (car[i].ToString() == begStr) | (car[i].ToString() == nextString))
                word += car[i];
            } //for (int i = 0; i < paket.Length; i++)
            return;
        } // end 


    }
}

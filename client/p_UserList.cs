using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{   
    class p_UserList
    {
        private const int POCKETTYPE = 3; //1 байт
        private int length; //4 байта
        public List<string> names = new List<string>(); // n*16 байт
        
        public p_UserList(List<ClClass> input)
        {
            foreach(ClClass j in input)
            {
                names.Add(j.name);
                length += (ASCIIEncoding.GetEncoding(1251).GetBytes(j.name)).Length + 5;
            }
        }

        public byte[] MakePocket()
        {
            byte[] bts0, bts1, bts2;
            bts0 = ASCIIEncoding.GetEncoding(1251).GetBytes(Convert.ToString(POCKETTYPE));

            if (length < 10)
            {
                bts1 = ASCIIEncoding.GetEncoding(1251).GetBytes("000" + Convert.ToString(length));
            }
            else
            {
                if (length < 100)
                {
                    bts1 = ASCIIEncoding.GetEncoding(1251).GetBytes("00" + Convert.ToString(length));
                }
                else
                {
                    if (length < 1000)
                    {
                        bts1 = ASCIIEncoding.GetEncoding(1251).GetBytes("0" + Convert.ToString(length));
                    }
                    else
                    {   
                        bts1 = ASCIIEncoding.GetEncoding(1251).GetBytes(Convert.ToString(length));
                    }
                }
            }

            bts0 = bts0.Concat(bts1).ToArray();

            bts2 = ASCIIEncoding.GetEncoding(1251).GetBytes("All/");
            bts0 = bts0.Concat(bts2).ToArray();

            foreach(string i in names)
            {
                bts2 = ASCIIEncoding.GetEncoding(1251).GetBytes(i + "/");
                bts0 = bts0.Concat(bts2).ToArray();
            }


            return bts0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace server
{
    class ClClass : Program
    {
        public Socket sockclient;
        public string name;
        public string password;
        private Thread thR;
        private byte[] bts = new byte[168];
         
        public ClClass(Socket sock, string n, string p)
        {
            name = n;
            sockclient = sock;
            password = p;
            thR = new Thread(Receive);
            thR.Start();
        }

        public void Receive()
        {
            int nbyte = 0;

            while (true)
            {
                try
                {
                    nbyte = sockclient.Receive(bts);
                }
                catch (SocketException)
                {
                    Console.WriteLine("Клиент " + name + " Отключился");
                    baseList.cls.Remove(baseList.Search4User(name));
                    if (baseList.cls.Count != 0)
                    {
                        p_UserList userList = new p_UserList(baseList.cls);
                        byte[] bts1 = userList.MakePocket();
                        foreach (ClClass i in baseList.cls)
                        {
                            i.sockclient.Send(bts1);
                        }
                    }
                    thR.Abort();
                }
                
                p_Message rMessage = new p_Message(bts);
                if (rMessage.receiver != null)
                {
                    if (rMessage.receiver == "All")
                    {
                        foreach (var i in baseList.cls)
                        {
                            if (i.name != name)
                            {
                                p_Message sMessage = new p_Message("(All) " + name, rMessage.message);
                                i.sockclient.Send(sMessage.MakePocket());
                            }
                        }
                    }
                    else
                    {
                        ClClass receiver = baseList.Search4User(rMessage.receiver);
                        if (receiver != null)
                        {
                            Send(receiver, rMessage.message);
                        }
                        else
                        {
                            Console.WriteLine("Не найден адресат");
                            continue;
                        }
                    }
                    Thread.Sleep(10);
                }
            }
        }

        private void Send(ClClass receiver, string s)
        {
            try
            {
                p_Message sMessage = new p_Message(name, s);
                receiver.sockclient.Send(sMessage.MakePocket());
            }
            catch (SocketException)
            {
                Console.WriteLine("Ошибка отправки");
            }
        }
    }
}

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

 
    class ClientList 
    {
        public List<ClClass> cls = new List<ClClass>();
        private byte[] bts = new byte[28];
        private Socket sockserver;

        public  ClientList (Socket sk)
        {
            sockserver = sk;
            Thread checkConnection = new Thread(Сonnect);    
            checkConnection.Start();
        }
        
        private void Сonnect ()
        {
            while (true)
            {
                int nbyte = 0;



                sockserver.Listen(1);
                Socket sockclient = sockserver.Accept();
                nbyte = sockclient.Receive(bts); 

                if (nbyte != 0)
                {
                    p_Identificator id = new p_Identificator(bts);
                    
                    ClClass user = Search4User(id.name);
      
                    if (user != null)
                    {
                        if (user.password == id.password)
                        {
                            continue;
                        }

                    }
                    else
                    {
                        ClClass client = new ClClass(sockclient, id.name, id.password) ;
                        cls.Add(client);

                        if (cls.Count != 0)
                        {
                            p_UserList userList = new p_UserList(cls);
                            byte[] bts1 = userList.MakePocket();
                            foreach (ClClass i in cls)
                            {
                                i.sockclient.Send(bts1);
                            }
                        }
                        Console.WriteLine("Подключился " + id.name);
                    }
                }
            }
        }

        public ClClass Search4User(string s)
        {
            int found = cls.FindIndex(p => p.name == s);
            if (found == -1)
                return null;
            else
                return cls[found];
        }

        public ClClass Search4User(Socket sk)
        {
            int found = cls.FindIndex(p => p.sockclient == sk);
            if (found == -1)
            {
                return null;
            }
            else
            {
                return cls[found];
            }
        }
    }
}

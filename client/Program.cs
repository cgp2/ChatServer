using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace server
{
    class Program
    {
        public static Socket sockserver;
        public static ClientList baseList;
        static void Main(string[] args)
        {

            sockserver = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var address = IPAddress.Any;
            var endpoint = new IPEndPoint(address, 21322);

            baseList = new ClientList(sockserver);

            sockserver.Bind(endpoint);

        }
    }

}

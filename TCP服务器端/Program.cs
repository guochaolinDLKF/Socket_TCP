using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace TCP服务器端
{
    class Program
    {
        static void Main(string[] args)
        {
            MainServer server = new MainServer(IPAddress.Parse("127.0.0.1"), 8866, 1000);
            server.Start();
            while (true)
            {
                string s = Console.ReadLine();
                if (s.Contains("s"))
                {
                    Random rd = new Random();
                    int index = rd.Next(1, 15);

                    byte[] send = Message.ProtoBufDataSerialize("发送给客户端的数据");
                    if (server.client!=null)
                    {
                        server.client.SendResponse((ActionCode)index, send);
                    }
                   
                }

            }



            //Console.ReadKey();
        }
    }
}

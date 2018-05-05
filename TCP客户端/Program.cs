using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;

namespace TCP客户端
{
    class Program
    {
        static void Main(string[] args)
        {
            ClientManager cl = new ClientManager();
            cl.Connect();
            while (true)
            {
                string s = Console.ReadLine();
                if (s.Contains("d"))
                {
                    Random r = new Random();
                    int index = r.Next(1, 15);
                    int indexA = r.Next(1, 3);
                   //Dictionary<int,object> send=new Dictionary<int, object>();
                   // send.Add(12, );
                    byte[] dataList = Message.ProtoBufDataSerialize("发送给Server的数据");
                    cl.SendRequest((RequestCode)indexA, (ActionCode)index, dataList); 
                }
                  
            }

            //Console.ReadKey();
        }
    }
}

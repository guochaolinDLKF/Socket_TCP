using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using ProtoBuf;
using LitJson;

namespace TCP服务器端
{
    class DataClass
    {
        public DataClass() { }
        public Dictionary<string, ChildClass> dataList;
    }
    [ProtoContract]
    class ChildClass
    {
        public ChildClass() { }
        public ChildClass(string Data)
        {
            DataStr = Data;
        }
        [ProtoMember(1)]
        public string DataStr { get; set; }
    }

    public enum Msg
    {
        child,
        first 
    }

    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int,ChildClass> chList=new Dictionary<int, ChildClass>();
            chList.Add(1001,new ChildClass("nfoqfroirofoi"));
            chList.Add(1002,new ChildClass("vbuagafre"));
            chList.Add(1003,new ChildClass("15123115345456"));
            byte[] data = Message.ProtoBufDataSerialize(chList);
            Console.WriteLine(data.Length);
            Dictionary<int, ChildClass> receData = Message.ProtoBufDataDeSerialize<Dictionary<int, ChildClass>>(data);

            foreach (var childClass in receData.Values)
            {
                 Console.WriteLine(childClass.DataStr);
            }

          
            


            Console.ReadKey();
            //MainServer server = new MainServer(IPAddress.Parse("127.0.0.1"), 8866, 1000);
            //server.Start();
            //while (true)
            //{
            //    string s = Console.ReadLine();
            //    if (s.Contains("s"))
            //    {
            //        Random rd = new Random();
            //        int index = rd.Next(1, 15);
            //        UserModel user=new UserModel("大家好","4643334");
            //        string str = "哈哈哈哈哈";
            //        byte[] send = Message.ProtoBufDataSerialize(str);
            //        if (server.client != null)
            //        {
            //            server.client.SendResponse((ActionCode)index, send);
            //        }

            //    }

            //}




        }
    }
}

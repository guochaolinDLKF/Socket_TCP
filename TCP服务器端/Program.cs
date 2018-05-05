using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using ProtoBuf;

namespace TCP服务器端
{
    [ProtoContract]
    class DataClass
    {
        public DataClass() { }
        [ProtoMember(1)]
        public Dictionary<int, ChildClass> dataList; 
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



    class Program
    {
        static void Main(string[] args)
        {
            //ChildClass dataClass = new ChildClass("nnrfrenfrif");
            //DataClass dataList = new DataClass();
            //dataList.dataList = new Dictionary<int, ChildClass>();
            //dataList.dataList.Add(1, dataClass);



            //byte[] data = Message.ProtoBufDataSerialize(dataList);
            //Console.WriteLine(data.Length);
            //DataClass receData = Message.ProtoBufDataDeSerialize<DataClass>(data);
            //ChildClass child;
            //receData.dataList.TryGetValue(1, out child);

            //Console.WriteLine(child.DataStr);


            //Console.ReadKey();
            MainServer server = new MainServer(IPAddress.Parse("127.0.0.1"), 8866, 1000);
            server.Start();
            while (true)
            {
                string s = Console.ReadLine();
                if (s.Contains("s"))
                {
                    Random rd = new Random();
                    int index = rd.Next(1, 15);
                    // UserModel user=new UserModel("大家好","4643334");
                    string str = "哈哈哈哈哈";
                    byte[] send = Message.ProtoBufDataSerialize(str);
                    if (server.client != null)
                    {
                        server.client.SendResponse((ActionCode)index, send); 
                    }

                }

            }




        }
    }
}

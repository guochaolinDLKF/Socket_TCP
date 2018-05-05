using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCP客户端
{
    public enum ActionCode
    {
        None = 0,
        Login,
        Register,
        ListRoom,
        CreateRoom,
        JoinRoom,
        UpdateRoom,
        QuitRoom,
        StartGame,
        ShowTimer,
        StartPlay,
        Move,
        Shoot,
        Attack,
        GameOver,
        UpdateResult,
        QuitBattle
    }
    public enum RequestCode
    {
        None = 0,
        User,
        Room,
        Game,
    }
    public enum ReturnCode
    {
        Success,
        Fail,
        NotFound
    }
    class Message
    {
        private byte[] data = new byte[5120];
        private int startIndex = 0;//我们存取了多少个字节的数据在数组里面

        public delegate void ReceiveData(byte[] data);

        public event ReceiveData GetReceiveData;

        public delegate void DataPacker(ActionCode ac, byte[] data);

        public event DataPacker ParsedDataPacker;
        public List<byte> mBufferList;
        public byte[] Data
        {
            get { return data; }
        }
        public int StartIndex
        {
            get { return startIndex; }
        }
        public int RemainSize
        {
            get { return data.Length - startIndex; }
        }

        public Message()
        {
            GetReceiveData += ParseReceiveData;

        }
        /// <summary>
        /// 读取数据
        /// </summary>
        public void ReadMessage(int newDataAmount)
        {
            try
            {
                if (newDataAmount > 0)
                {
                    lock (mBufferList)
                    {
                        mBufferList.AddRange(data);
                    }
                    while (mBufferList.Count > 4)
                    {
                        byte[] lenBytes = mBufferList.GetRange(0, 4).ToArray();
                        int packageLen = BitConverter.ToInt32(lenBytes, 0);
                        if (packageLen <= mBufferList.Count - 4)
                        {
                            //包够长时,则提取出来,交给后面的程序去处理  
                            byte[] rev = mBufferList.GetRange(4, packageLen).ToArray();
                            if (GetReceiveData != null)
                            {
                                GetReceiveData(rev);
                            }
                            //从数据池中移除这组数据,为什么要lock,你懂的  
                            lock (mBufferList)
                            {
                                mBufferList.RemoveRange(0, packageLen + 4);
                                mBufferList.Clear();
                            }
                        }
                        else
                        {
                            //长度不够,还得继续接收,需要跳出循环  
                            break;
                        }
                    }
                }

            }
            catch (Exception xe)
            {
                Console.WriteLine(xe);
            }
        }
        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="data"></param>
        void ParseReceiveData(byte[] data)
        {
            MSGCallBack receiveData = ProtoBufDataDeSerialize<MSGCallBack>(data);
            if (ParsedDataPacker != null)
            {
                ParsedDataPacker(receiveData.ActionCode, receiveData.DataList);
            }
        }


        public static byte[] PackData(RequestCode requestData, ActionCode actionCode, byte[] data)
        {
            
            MSGCallBack msg = new MSGCallBack(requestData, actionCode, data);
            byte[] dataBytes = ProtoBufDataSerialize(msg);
            int dataAmount = dataBytes.Length;
            Console.WriteLine("data:" + dataAmount);
            byte[] dataAmountBytes = BitConverter.GetBytes(dataAmount);
            return dataAmountBytes.Concat(dataBytes).ToArray<byte>();
        }

        public static byte[] ProtoBufDataSerialize<T>(T data)
        {
            try
            {
                //涉及格式转换，需要用到流，将二进制序列化到流中
                using (MemoryStream ms = new MemoryStream())
                {
                    //使用ProtoBuf工具的序列化方法
                    ProtoBuf.Serializer.Serialize<T>(ms, data);
                    //定义二级制数组，保存序列化后的结果
                    byte[] result = new byte[ms.Length];
                    //将流的位置设为0，起始点
                    ms.Position = 0;
                    //将流中的内容读取到二进制数组中
                    ms.Read(result, 0, result.Length);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("序列化失败: " + ex.ToString());
                return null;
            }
            //byte[] bytes = null;
            //using (var ms = new MemoryStream())
            //{
            //    Serializer.Serialize<T>(ms, data);
            //    bytes = new byte[ms.Position];
            //    var fullBytes = ms.GetBuffer();
            //    Array.Copy(fullBytes, bytes, bytes.Length);
            //}
            //return bytes;
        }
        public static T ProtoBufDataDeSerialize<T>(byte[] data)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    //将消息写入流中
                    ms.Write(data, 0, data.Length);
                    //将流的位置归0
                    ms.Position = 0;
                    //使用工具反序列化对象
                    T result = ProtoBuf.Serializer.Deserialize<T>(ms);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("反序列化失败: " + ex.ToString());
                return default(T);
            }
        }
        //using (var ms = new MemoryStream(data))
        //{
        //    return Serializer.Deserialize<T>(ms);
        //}

    }

}


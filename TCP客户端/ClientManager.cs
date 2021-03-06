﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TCP客户端
{
    class ClientManager : IDisposable
    {
        private const string IP = "127.0.0.1";
        private const int PORT = 8866;

        public Socket clientSocket;
        private Message msg = new Message();
        private IPEndPoint ipEndP;

        public ClientManager()
        {
            ipEndP = new IPEndPoint(IPAddress.Parse(IP), PORT);
            msg.ParsedDataPacker += OnProcessDataCallback;
            clientSocket = new Socket(ipEndP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            msg.mBufferList = new List<byte>();
        }

        public void Connect()
        {

            clientSocket.BeginConnect(ipEndP, (delegate (IAsyncResult ar)
             {
                 clientSocket.EndConnect(ar);
                 if (ar.AsyncWaitHandle.WaitOne(5000))
                 {
                     Thread th = new Thread(new ThreadStart(StartReceive));
                     th.IsBackground = true;
                     th.Start();
                        //StartReceive();
                    }
             }), clientSocket);


        }
        private void StartReceive()
        {
            clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallback, null);
        }
        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                if (clientSocket == null || clientSocket.Connected == false) return;

                if (ar.AsyncWaitHandle.WaitOne(5000))
                {
                    int count = clientSocket.EndReceive(ar);
                    msg.ReadMessage(count);

                    Thread th = new Thread(new ThreadStart(StartReceive));
                    th.IsBackground = true;
                    th.Start();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        private void OnProcessDataCallback(ActionCode actionCode, byte[] data)
        {
            Console.WriteLine(string.Format("接受到服务器的数据：ActionCode:{0};Data:{1}", actionCode, Message.ProtoBufDataDeSerialize<string>(data)));
            //facade.HandleReponse(actionCode, data);
        }
        public void SendRequest(RequestCode requestCode, ActionCode actionCode, byte[] data)
        {
            byte[] bytes = Message.PackData(requestCode, actionCode, data);
            clientSocket.Send(bytes);
        }

        public void Dispose()
        {
            try
            {
                msg.ParsedDataPacker -= OnProcessDataCallback;
                clientSocket.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("无法关闭服务器端！！" + e);
            }
        }
    }
}

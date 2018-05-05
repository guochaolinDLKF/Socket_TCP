using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
//using Common;
//using MySql.Data.MySqlClient;
//using GameServer.Tool;
//using GameServer.Model;
//using GameServer.DAO;
namespace TCP服务器端
{
    class ClientPeer
    {
        //private static Socket mClientSocket;
        private MainServer mServer;
        private Message msg ;
        /// <summary>
        /// 接收数据缓冲区
        /// </summary>
        private byte[] _recvBuffer;
        /// <summary>
        /// 接收数据缓冲区 
        /// </summary>
        public byte[] RecvDataBuffer
        {
            get
            {
                return _recvBuffer;
            }
            set
            {
                _recvBuffer = value;
            }
        }
        /// <summary>
        /// 客户端的Socket
        /// </summary>
        private Socket _clientSock;
        /// <summary>
        /// 获得与客户端会话关联的Socket对象
        /// </summary>
        public Socket ClientSocket
        {
            get
            {
                return _clientSock;

            }
        }



        //private MySqlConnection mysqlConn;
        //private Room room;
        // private User user;
        //private Result result;
        //private ResultDAO resultDAO = new ResultDAO();
        //private bool connected;
        //public bool Connected
        //{
        //    get
        //    {
        //        return mClientSocket != null && mClientSocket.Connected;
        //    }
        //    set { connected = value; }
        //}
        public int HP
        {
            get; set;
        }
        public bool TakeDamage(int damage)
        {
            HP -= damage;
            HP = Math.Max(HP, 0);
            if (HP <= 0) return true;
            return false;
        }
        public bool IsDie()
        {
            return HP <= 0;
        }
        //public MySqlConnection MySQLConn
        //{
        //    get { return mysqlConn; }
        //}
        //public void SetUserData(User user,Result result)
        //{
        //    this.user = user;
        //    this.result = result;
        //}
        //public string GetUserData()
        //{
        //    return user.Id+","+ user.Username + "," + result.TotalCount + "," + result.WinCount;
        //}
        //public Room Room
        //{
        //    set { room = value; }
        //    get { return room; }
        //}
        //public int GetUserId()
        //{
        //    return user.Id;
        //}

        public ClientPeer() { }
        public ClientPeer(Socket Socket, MainServer server)
        {
            msg = new Message();
            _clientSock = Socket;
            this.mServer = server;

           // msg.ParsedDataObjPacker += OnProcessMessage;
           
            //mysqlConn = ConnHelper.Connect();
        }
        //public void Start()
        //{
        //    if (mClientSocket == null || mClientSocket.Connected == false) return;
        //    mClientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallback, null);
        //}

        //private void ReceiveCallback(IAsyncResult ar)
        //{
        //    try
        //    {
        //        if (mClientSocket == null || mClientSocket.Connected == false) return;
        //        int count = mClientSocket.EndReceive(ar);
        //        if (count == 0)
        //        {
        //            Close();
        //        }
        //        msg.ReadMessage(count, Connected);
        //        Start();
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("一个客户端断开连接");
        //        Close();
        //    }
        //}
       public void OnProcessMessage(RequestCode requestCode, ActionCode actionCode, byte[] data)
        {
            Console.WriteLine("接受数据");
            Console.WriteLine(string.Format("接受到客户端的数据：RequestCode:{0};" +
                                            "ActionCode:{1};Data:{2}",
                                            requestCode, actionCode, Message.ProtoBufDataDeSerialize<string>(data)));
            //server.HandleRequest(requestCode, actionCode, data, this);
        }        /// <summary>
                 /// 关闭会话
                 /// </summary>
        public void Close()
        {
            try
            {
                //关闭数据的接受和发送
                _clientSock.Shutdown(SocketShutdown.Both);
            }
            catch (Exception) { }
            if (_clientSock != null)
                _clientSock.Close();
        }
        //private void Close()
        //{
        //    //ConnHelper.CloseConnection(mysqlConn);

        //    //if (room != null)
        //    //{
        //    //    room.QuitRoom(this);
        //    //}
        //    msg.ParsedDataPacker -= OnProcessMessage; 
        //    mServer.RemoveClient(this);
        //    try
        //    {
        //        mClientSocket.Shutdown(SocketShutdown.Send);
        //    }
        //    catch (Exception) { }
        //    if (mClientSocket != null)
        //        mClientSocket.Close();
        //}


        public  void SendResponse(ActionCode actionCode, byte[] data)
        {
            Send(actionCode, data);
        }
        public  void Send(ActionCode actionCode, byte[] data)
        {
            try
            {
                byte[] bytes = Message.PackData(actionCode, data);
                _clientSock.Send(bytes);
            }
            catch (Exception e)
            {
                Console.WriteLine("无法发送消息:" + e);
            }
        }

        //public bool IsHouseOwner()
        //{
        //   // return room.IsHouseOwner(this);
        //}
        public void UpdateResult(bool isVictory)
        {
            UpdateResultToDB(isVictory);
            UpdateResultToClient();
        }
        private void UpdateResultToDB(bool isVictory)
        {
            // result.TotalCount++;
            if (isVictory)
            {
                //result.WinCount++;
            }
            //resultDAO.UpdateOrAddResult(mysqlConn, result);
        }
        private void UpdateResultToClient()
        {
            //Send(ActionCode.UpdateResult, string.Format("{0},{1}", result.TotalCount, result.WinCount));
        }
    }
}

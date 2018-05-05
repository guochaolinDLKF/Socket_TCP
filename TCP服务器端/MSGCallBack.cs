using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace TCP服务器端
{
    [ProtoContract]
    class MSGCallBack
    {
        public MSGCallBack() { }
        public MSGCallBack(RequestCode request, ActionCode action, byte[] dataList)
        {
            RequestCode = request;
            ActionCode = action;
            DataList = dataList;
        }
        public MSGCallBack(ActionCode action, byte[] dataList)
        {
            ActionCode = action;
            DataList = dataList;
        }
        [ProtoMember(1)]
        public RequestCode RequestCode { get; set; }
        [ProtoMember(2)]
        public ActionCode ActionCode { get; set; }
        [ProtoMember(3)]
        public byte[] DataList; 
    }

    //[ProtoContract]
    //class UserModel
    //{ 
    //     public UserModel() { }

    //    public UserModel(string uname, string pword)
    //    {
    //        UName = uname;
    //        PWord
    //    }
    //    public string UName { get ; set; }
    //    public string PWord { get; set; }
    //}
}

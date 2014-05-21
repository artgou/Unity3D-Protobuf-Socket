/*
================================================================================
FileName    : PacketProcesser
Description : 消息派发，将解析后的消息对象交给不同的消息处理函数，并广播消息事件或进行其他的事情
Date        : 2014-05-09
Author      : Linkrules
Notes:      : !!!!!!!!!!!!!!  游戏中添加新的消息，需要添加3个东西  1. 消息事件  2. 消息处理函数  3. 注册消息
================================================================================
*/
using UnityEngine;
using System.Collections;
using com.CR.GameDataModel;

namespace GamePacketData {

    public delegate void MessageProcCallback( object obj );
    public class PacketProcesser {

        /// 消息事件，当来自网络的数据包被解析并传递给不同的消息处理函数后，将广播对应的事件
        public delegate void mPlayer( Player player );
        public event mPlayer event_Player;

        public delegate void mLogin( Login login );
        public event mLogin event_Login;

        public delegate void mAttack( Attack attack );
        public event mAttack event_Attack;




        /// <summary>
        /// 保存消息和对应的消息处理对象
        /// </summary>
        private Hashtable hMessageProcs = new Hashtable();

        public PacketProcesser() {
            InitProcessFuncBinding();
        }


        /// <summary>
        /// 消息派发处理，使用消息名称，调用 Hashtable　中不同的消息处理对象的处理函数
        /// </summary>
        /// <param name="messageObj">消息名称</param>
        /// <param name="messageName">消息对象</param>
        public void PacketDispatch( object messageObj, string messageName ) {
            if ( hMessageProcs.ContainsKey( messageName ) ) {
                MessageProcFunc mpf = (MessageProcFunc)hMessageProcs[messageName];
                if ( mpf.callback != null ) {
                    mpf.callback( messageObj );
                }
            }
        }


        /// <summary>
        /// 注册消息名和对应的处理函数，在类的构造函数中被调用，每添加一种消息，需要手动在本函数中添加注册(很那什么...如果你有更好的方法，请联系楼主）
        /// </summary>
        private void InitProcessFuncBinding() {
            RegisterProcForMessage( "Player", MessageProc_Player );
            RegisterProcForMessage( "Login", MessageProc_Login );
            RegisterProcForMessage("Attack",MessageProc_Attack);
        }


        /// <summary>
        /// 将消息名和对应的处理函绑定到一起，不同的消息处理函数会将解析后的 object 对象强制转为不同的 protobuf 消息对象，然后进行广播等
        /// </summary>
        /// <param name="messageName">消息名称</param>
        /// <param name="procFunc">消息处理函数</param>
        private void RegisterProcForMessage( string messageName, MessageProcCallback procFunc ) {
            hMessageProcs.Add( messageName, new MessageProcFunc( procFunc ) );
        }


        
        ////////////////////////////////////// 以下是不同的消息处理函数 ///////////////////////////////////////////
        
        private void MessageProc_Player( object obj ) {
            event_Player((Player)obj);
        }


        private void MessageProc_Login( object obj ) {
            event_Login( (Login)obj );
        }

        private void MessageProc_Attack( object obj ) {
            event_Attack( (Attack)obj );
        }

    }



    /// <summary>
    /// 消息处理函数类
    /// </summary>
    class MessageProcFunc {
        public MessageProcCallback callback = null;

        public MessageProcFunc( MessageProcCallback callback ) {
            this.callback = callback;
        }
    }

}



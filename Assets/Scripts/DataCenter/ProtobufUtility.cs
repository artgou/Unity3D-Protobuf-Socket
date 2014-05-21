/*
================================================================================
FileName    : ProtobufUtility
Description : Serialize and Deserialize Protobuf class
Date        : 2014-05-05
Author      : Linkrules
================================================================================
*/
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ProtoBuf;

    public class ProtobufUtility {

        private Dictionary<string,System.Type> mProtobufType = new Dictionary<string, System.Type>();


        public ProtobufUtility() {
            InitProtobufTypes( this.GetType().Assembly );
        }


        /// <summary>
        /// 将 Protobuf 消息类打包成二进制数据流
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] Serialize( object data ) {
            byte[] buffer = null;
            using ( MemoryStream m = new MemoryStream() ) {
                Serializer.Serialize( m, data );
                m.Position = 0;
                int len = (int)m.Length;
                buffer = new byte[len];
                m.Read( buffer, 0, len );
            }
            return buffer;
        }


        /// <summary>
        /// 解析 Protobuf 二进制数据流，返回 object (需要根据不同的消息类型回调以注册的处理函数)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="messageName"></param>
        /// <returns></returns>
        public object Deserialize( byte[] data, string messageName ) {
            System.Type type = GetTypeByName( messageName );
            using ( MemoryStream m = new MemoryStream( data ) ) {
                return ProtoBuf.Meta.RuntimeTypeModel.Default.Deserialize( m, null, type );
            }
        }


        /// <summary>
        /// 遍历所有的 protobuf 消息类，将类型及类名存入字典
        /// </summary>
        /// <param name="assembly"></param>
        private void InitProtobufTypes( System.Reflection.Assembly assembly ) {
            foreach ( System.Type t in assembly.GetTypes() ) {
                ProtoBuf.ProtoContractAttribute[] pc = (ProtoBuf.ProtoContractAttribute[])t.GetCustomAttributes( typeof( ProtoBuf.ProtoContractAttribute ), false );
                if ( pc.Length > 0 ) {
                    mProtobufType.Add( t.Name, t );
                }
            }
        }


        /// <summary>
        /// 通过 protobuf 消息名，获取消息类型
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public System.Type GetTypeByName( string name ) {
            return mProtobufType[name];
        }



    }

/*
================================================================================
FileName    : PacketBuilder
Description : 将消息对象加上数据包头打包成最终网络数据包，用于传送给服务器
Date        : 2014-05-06
Author      : Linkrules
================================================================================
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

namespace GamePacketData {
    public class PacketBuilder {

        private ushort len = 0;              // 数据包总长度
        private ushort nameLen = 0;          // 消息字符串长度 
        private string messageName = null;   // 消息字符串
        private int checkCode = 0;           // 校验值



        /// <summary>
        /// 打包 protobuf 消息数据
        /// </summary>
        /// <param name="protobufObject"></param>
        /// <returns>打包后的二进制流</returns>
        public byte[] Build( object protobufObject ) {

            if ( protobufObject == null ) {
                Debug.LogError( "Error: protobuf class and name can not be null!" );
                return null;
            }

            int writeIndex = 0;

            byte[] protobufData = DataCenter.ProtobufUtility.Serialize( protobufObject );        // 将 protobuf 消息对象序列化为二进制

            messageName = protobufObject.GetType().Name;                                         // 获取消息对象的名字作为消息名
            byte[] messageNameBytes = Encoding.UTF8.GetBytes( messageName + '\0' );
            nameLen = (ushort)messageNameBytes.Length;

            len = (ushort)( 2 + 2 + 4 + nameLen + protobufData.Length );
            byte[] packet = new byte[len];
            
            WriteUInt16(ref packet,ref writeIndex,len);                                          // 数据包总长度
            WriteUInt16( ref packet, ref writeIndex, nameLen );                                  // 消息名长度
            WriteBytes(ref packet,ref writeIndex,messageNameBytes,nameLen);                      // 消息名字符串
            WriteBytes( ref packet, ref writeIndex, protobufData, protobufData.Length );         // 消息信息， protobuf
            WriteInt32( ref packet, ref writeIndex, checkCode );                                 // 校验值

            Debug.Log( "Build: Len: " + len );  

            return packet;
        }

        private void WriteUInt16( ref byte[] packet, ref int writeIndex, ushort num ) {
            byte[] tmp = BitConverter.GetBytes( num );
            Array.Copy( tmp, 0, packet, writeIndex, 2 );
            writeIndex += 2;
        }

        private void WriteInt32( ref byte[] packet, ref int writeIndex, int num ) {
            byte[] tmp = BitConverter.GetBytes( num );
            Array.Copy( tmp, 0, packet, writeIndex, 4 );
            writeIndex += 4;
        }

        private void WriteBytes( ref byte[] packet, ref int writeIndex, byte[] data, int len ) {
            Array.Copy( data, 0, packet, writeIndex, len );
            writeIndex += len;
        }
    }
}

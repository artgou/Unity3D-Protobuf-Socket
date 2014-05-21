/*
================================================================================
FileName    : PacketParser
Description : 将网络数据包解释成 protobuf 对象，然后传给 PacketProcesser进行处理和派发
Date        : 2014-05-06
Author      : Linkrules
================================================================================
*/
using UnityEngine;
using System.Collections;
using System;
using System.Text;
using com.CR.GameDataModel;

namespace GamePacketData {
    public class PacketParser {

        private int checkSum = 0;


        /// <summary>
        /// 解析从网络收到的完整数据包
        /// </summary>
        /// <param name="data"></param>
        public void Parse( byte[] data ) {
            Debug.Log( "Parse: data.Length: " + data.Length );
            // 解析数据包头
            int readIndex = 0;
            ushort len = ReadUInt16( data, readIndex );
            readIndex += 2;

            ushort nameLen = ReadUInt16( data, readIndex );                         // 读取消息名长度
            readIndex += 2;

            string messageName = ReadString( data, readIndex, nameLen - 1 );        // 读取消息名字符串， -1 ，不读取字符串结尾处的'\0'
            readIndex += nameLen;

            int objDataLen = data.Length - readIndex - 4;
            byte[] objData = ReadBytes( data, readIndex, objDataLen);               // 读取 protobuf 对象数据
            readIndex += objDataLen;

            int checkCode = ReadInt32( data, readIndex );                           // 读取校验值
            readIndex += 4;

            object obj = DataCenter.ProtobufUtility.Deserialize( objData, messageName);   // 反序列化 Protobuf 对象
            DataCenter.packetProcesser.PacketDispatch( obj, messageName);                 // 将 protobuf 消息对象传给 PacketProcesser 进行处理和派发

        }


        private byte ReadByte( byte[] data, int from ) {
            byte []array = new byte[1];
            Array.Copy( data, from, array, 0, 1 );
            return array[0];
        }


        private byte[] ReadBytes( byte[] data, int from, int len ) {
            byte[]array = new byte[len];
            Array.Copy( data, from, array, 0, len );
            return array;
        }


        private ushort ReadUInt16( byte[] data, int from ) {
            byte[] array = new byte[2];
            Array.Copy( data, from, array, 0, 2 );
            ushort result = BitConverter.ToUInt16( array, 0 );
            return result;
        }


        private int ReadInt32( byte[] data, int from ) {
            byte[] array = new byte[4];
            Array.Copy( data, from, array, 0, 4 );
            int result = BitConverter.ToInt32( array, 0 );
            return result;
        }


        private string ReadString( byte[] data, int from, int len ) {
            byte[] array = new byte[len];
            Array.Copy( data, from, array, 0, len );
            string result = Encoding.UTF8.GetString( array );
            return result;
        }
    }
}

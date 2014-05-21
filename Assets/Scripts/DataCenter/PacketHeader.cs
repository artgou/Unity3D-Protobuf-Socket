/*
================================================================================
FileName    : PacketHeader
Description : 数据包头的定义及打包
Date        : 2014-05-09
Author      : Linkrules
================================================================================
*/

/*
using UnityEngine;
using System.Text;
using System;
using System.Runtime.InteropServices;

namespace GamePacketData {
    public class PacketHeader {
        public ushort len = 0;                  // 2
        public ushort nameLen = 0;              // 2
        public string messageName = null;       // dynamic


        public PacketHeader() {

        }

        /// <summary>
        /// you should specify the protobuf class name, that is 'messageName'
        /// </summary>
        /// <param name="messageName"></param>
        public PacketHeader( string messageName ) {
            this.messageName = messageName;
            nameLen = (ushort)(Encoding.UTF8.GetBytes( messageName ).Length + 1);        // <linkrules> [2014-05-09] '\0'
        }

        /// <summary>
        /// package header to binary array
        /// </summary>
        /// <returns>byte[]</returns>
        public byte[] GetBuffer() {
            if ( messageName == null ) {
                Debug.LogError( "Error: messageName can not be null!" );
                return null;
            }

            byte[] packetHeaderData = null;
            int index = 0;
            byte[] tmp = null;

            byte[] messageNameBytes = Encoding.UTF8.GetBytes( messageName + '\0' );
            nameLen = (ushort)messageName.Length;

            packetHeaderData = new byte[Marshal.SizeOf(len) + Marshal.SizeOf(nameLen) + nameLen];

            tmp = BitConverter.GetBytes( len );          
            Array.Copy( tmp, 0, packetHeaderData, index, Marshal.SizeOf(len) );
            index += 4;

            tmp = BitConverter.GetBytes( nameLen );        
            Array.Copy( tmp, 0, packetHeaderData, index, Marshal.SizeOf(nameLen) );
            index += 2;

            Array.Copy( messageNameBytes, 0, packetHeaderData, index, nameLen );        // dynamic size
            index += nameLen;


            return packetHeaderData;
        }

    }
}
*/

//Copyright 2022 Vladislav Sergachev (VladiSLAV)
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.

using System;
using System.IO;


namespace SCLL
{
    /// <summary>
    /// Base class for pasers
    /// </summary>
    /// <remarks>
    /// Implemented by <see cref="MessageParser"/> and <see cref="MetadataParser"/>
    /// </remarks>
    /// 
    /// <typeparam name="ParsingResultType">Type of expected parsing result</typeparam>

    public abstract class Parser<ParsingResultType>
    {
        public Stream InputStream;

        /// <param name="inStream">Stream containing data for parsing</param>

        public Parser(Stream inStream)
        {
            InputStream = inStream;
        }

        public abstract ParsingResultType Parse();
    }
    
    
    
    /// <summary>
    /// Ultravox-stream parser
    /// </summary>
    public sealed class MessageParser : Parser<Message>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream">Stream containing Ultravox-messages</param>
        public MessageParser(Stream stream) : base(stream)
        {

        }

        /// <summary>
        /// Parses current message found by <see cref="FindNext"/>
        /// </summary>

        public override Message Parse()
        {
            Message message = new Message();
            byte[] msgHeader = new byte[5];

            InputStream.Read(msgHeader);

            message.ResQos = msgHeader[0];
            message.type = (MessageType)((msgHeader[1] << 8) + msgHeader[2]);
            message.PayloadLength = (ushort)((msgHeader[3] << 8) + msgHeader[4]);
            message.Payload = new MemoryStream(message.PayloadLength);

            byte[] buffer = new byte[message.PayloadLength];
            
            InputStream.Read(buffer);
            message.Payload.Write(buffer);

            return message;
        }

        /// <summary>
        /// Seeking to next (or first) message in stream. That message should further be parsed with <see cref="Parse"/>
        /// </summary>
        
        public void FindNext()
        {
            bool isSyncByte = false;

            while (!isSyncByte)
            {
                int orderedByte = InputStream.ReadByte();      
                isSyncByte = (orderedByte == Message.ULTRAVOX_SYNC_BYTE);
            }
        }


        public Stream UltravoxStream
        {
            set => InputStream = value;
        }
    }
    public sealed class MetadataParser : Parser<MetadataPackage>
    {
        public MetadataParser(Stream input) : base(input)
        {
            InputStream = input;
        }

        public override MetadataPackage Parse()
        {
            UInt16 ID, Span, Order;
            byte[] packageInfo = new byte[6];

            InputStream.Read(packageInfo);

            ID = (UInt16) ( (packageInfo[0] << 8) + packageInfo[1] );
            Span = (UInt16) ( (packageInfo[2] << 8) + packageInfo[3] );
            Order = (UInt16) ( (packageInfo[4] << 8) + packageInfo[5] );

            return new MetadataPackage(ID, Span, Order);
        }
    }
}

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

using System.IO;


namespace SCLL
{

    public abstract class Parser<ParsingResultType>
    {
        protected Stream inputStream;
        public Parser(Stream inStream)
        {
            inputStream = inStream;
        }

        public abstract ParsingResultType Parse();
    }
    
    
    
    public sealed class MessageParser : Parser<Message>
    {
        public MessageParser(Stream stream) : base(stream)
        {

        }

        public override Message Parse()
        {
            Message message = new Message();
            byte[] msgHeader = new byte[5];

            inputStream.Read(msgHeader);

            message.ResQos = msgHeader[0];
            message.type = (MessageType)((msgHeader[1] << 8) + msgHeader[2]);
            message.PayloadLength = (ushort)((msgHeader[3] << 8) + msgHeader[4]);
            message.Payload = new byte[message.PayloadLength];

            inputStream.Read(message.Payload);

            return message;
        }

        public void FindNext()
        {
            bool isSyncByte = false;

            while (!isSyncByte)
            {
                int orderedByte = inputStream.ReadByte();      
                isSyncByte = (orderedByte == Message.ULTRAVOX_SYNC_BYTE) ? true : false;
            }
        }


        public Stream UltravoxStream
        {
            set => inputStream = value;
        }
    }
    public sealed class MetadataParser
    {

    }
}

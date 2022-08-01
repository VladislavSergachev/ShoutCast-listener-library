using System.IO;


namespace SCLL
{
    public sealed class Parser
    {
        private Stream _uvoxStream;

        public Parser(Stream stream) => _uvoxStream = stream;

        public Message Parse()
        {
            Message message = new Message();
            byte[] msgHeader = new byte[5];

            _uvoxStream.Read(msgHeader);

            message.ResQos = msgHeader[0];
            message.type = (MessageType)((msgHeader[1] << 8) + msgHeader[2]);
            message.PayloadLength = (ushort)((msgHeader[3] << 8) + msgHeader[4]);
            message.Payload = new byte[message.PayloadLength];

            _uvoxStream.Read(message.Payload);

            return message;
        }

        public void FindNext()
        {
            bool isSyncByte = false;

            while (!isSyncByte)
            {
                int orderedByte = _uvoxStream.ReadByte();      
                isSyncByte = (orderedByte == Message.ULTRAVOX_SYNC_BYTE) ? true : false;
            }
        }


        public Stream UltravoxStream
        {
            set => _uvoxStream = value;
        }
    }
}

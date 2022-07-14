using System.IO;


namespace SCLL
{
    public sealed class Parser
    {
        private Stream _uvoxStream;

        public Parser(Stream stream) => _uvoxStream = stream;

        public Message ParseNext()
        {
            Message message = new Message();
            byte[] msgHeader = new byte[5];

            _uvoxStream.ReadByte();

            _uvoxStream.Read(msgHeader);

            message.ResQos = msgHeader[0];
            message.type = (MessageType)((msgHeader[1] << 8) + msgHeader[2]);
            message.PayloadLength = (ushort)((msgHeader[3] << 8) + msgHeader[4]);
            message.Payload = new byte[message.PayloadLength];

            _uvoxStream.Read(message.Payload);

            _uvoxStream.ReadByte();
            return message;
        }

        public Stream UltravoxStream
        {
            set => _uvoxStream = value;
        }
    }
}

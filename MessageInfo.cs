namespace SCLL
{

    public class MessageInfo
    {
        private byte msgClass;
        private ushort type;
        private ushort payloadLength;
        public const byte ULTRAVOX_SYNC_BYTE = 0x5A;

        public MessageInfo()
        {

        }


        public MessageInfo(byte mClass, ushort type, ushort payloadLength)
        {
            this.msgClass = mClass;
            this.type = type;
            this.payloadLength = payloadLength;
        }

        public byte Class
        {
            get => msgClass;
        }

        public ushort Type
        {
            get => type;
        }

        public ushort PayloadLength
        {
            get => payloadLength;
        }

        public void FindNext(Stream input) // skips all "trash" traffic
        {
            while ((input.ReadByte() != ULTRAVOX_SYNC_BYTE))
            {

            }
        }

        public void Parse(Stream input)
        {
            byte[] msgHeader = new byte[4];

            input.ReadByte();
            input.Read(msgHeader); // reads message header skipping Res/QoS

            msgClass = (byte)(msgHeader[0] >> 4);
            type = (ushort) ( ( (msgHeader[0] << 8) + msgHeader[1]) - (msgClass << 12) );

            payloadLength = (ushort)((msgHeader[2] << 8) + msgHeader[3]);
        }
    }
}

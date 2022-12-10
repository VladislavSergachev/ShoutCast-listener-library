using System.IO;


namespace SCLL
{
    public abstract class AbstractMessageFactory
    {
        protected MessageInfo _info;

        public AbstractMessageFactory(MessageInfo info)
        {
            _info = info;
        }

        public abstract BaseMessage CreateMessage(Stream inputStream);
    }


    public class BinaryMessageFactory : AbstractMessageFactory
    {
        public BinaryMessageFactory(MessageInfo info) : base(info)
        {

        }

        public override BaseMessage CreateMessage(Stream inputStream)
        {
            Stream payload = new MemoryStream(_info.PayloadLength);

            byte[] buffer = new byte[_info.PayloadLength];

            inputStream.Read(buffer);
            payload.Write(buffer);
            payload.Position = 0;

            return new BaseMessage(_info, payload);
        }
    }


    public class MetadataMessageFactory : AbstractMessageFactory
    {
        public MetadataMessageFactory(MessageInfo info) : base(info)
        {

        }

        public override BaseMessage CreateMessage(Stream inputStream)
        {
            Stream payload = new MemoryStream((_info.PayloadLength - 6));

            byte[] payloadBuffer = new byte[(_info.PayloadLength - 6)];
            byte[] metaInfoBuffer = new byte[6];

            inputStream.Read(metaInfoBuffer);
            inputStream.Read(payloadBuffer);

            payload.Write(payloadBuffer);
            payload.Position = 0;

            ushort id = (ushort)((metaInfoBuffer[0] << 8) + metaInfoBuffer[1]);
            ushort span = (ushort)((metaInfoBuffer[2] << 8) + metaInfoBuffer[3]);
            ushort index = (ushort)((metaInfoBuffer[4] << 8) + metaInfoBuffer[5]);

            return new MetadataMessage(_info, payload, id, index, span);
        }
    }
}

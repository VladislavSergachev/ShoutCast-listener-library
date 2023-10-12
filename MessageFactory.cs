namespace SCLL
{  
    public abstract class AbstractMessageFactory
    {
        public AbstractMessageFactory()
        {
            
        }

        public abstract BaseMessage CreateMessage(MessageInfo info, Stream inputStream);
    }


    public class BinaryMessageFactory : AbstractMessageFactory
    {
        public BinaryMessageFactory()
        {

        }

        public override BaseMessage CreateMessage(MessageInfo info, Stream inputStream)
        {
            Stream payload = new MemoryStream(info.PayloadLength);

            byte[] buffer = new byte[info.PayloadLength];

            inputStream.Read(buffer);
            payload.Write(buffer);
            payload.Position = 0;

            return new BaseMessage(info, payload);
        }
    }


    public class MetadataMessageFactory : AbstractMessageFactory
    {
        public MetadataMessageFactory()
        {

        }

        public override BaseMessage CreateMessage(MessageInfo info, Stream inputStream)
        {
            Stream payload = new MemoryStream((info.PayloadLength - 6));

            byte[] payloadBuffer = new byte[(info.PayloadLength - 6)];
            byte[] metaInfoBuffer = new byte[6];

            inputStream.Read(metaInfoBuffer);
            inputStream.Read(payloadBuffer);

            payload.Write(payloadBuffer);
            payload.Position = 0;

            ushort id = (ushort)((metaInfoBuffer[0] << 8) + metaInfoBuffer[1]);
            ushort span = (ushort)((metaInfoBuffer[2] << 8) + metaInfoBuffer[3]);
            ushort index = (ushort)((metaInfoBuffer[4] << 8) + metaInfoBuffer[5]);

            return new MetadataMessage(info, payload, id, index, span);
        }
    }
}

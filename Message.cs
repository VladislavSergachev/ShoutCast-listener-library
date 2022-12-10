using System.IO;


namespace SCLL
{
    public class BaseMessage
    {
        public readonly Stream Payload;
        private MessageInfo _info;

        public BaseMessage(MessageInfo info, Stream Payload)
        {
            _info = info;
            this.Payload = Payload;
        }
    }

    public class MetadataMessage : BaseMessage
    {
        public ushort _packageId;
        public ushort _packageIndex;
        public ushort _span;

        public MetadataMessage(MessageInfo Info, Stream Payload, ushort pkgId, ushort pkgIndex, ushort span) : base(Info, Payload)
        {
            _packageId = pkgId;
            _packageIndex = pkgIndex;
            _span = span;
        }
    }
}

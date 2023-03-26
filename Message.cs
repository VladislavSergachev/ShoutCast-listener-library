namespace SCLL
{
    public class BaseMessage
    {
        public readonly Stream Payload;
        public readonly MessageInfo Info;

        public BaseMessage(MessageInfo info, Stream Payload)
        {
            Info = info;
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

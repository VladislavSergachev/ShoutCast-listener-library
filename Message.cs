using System.IO;


namespace SCLL
{
    public class BinaryMessage
    {
        public Stream _payload;
        private MessageInfo _info;

        public BinaryMessage(MessageInfo info, Stream Payload)
        {
            _info = info;
            _payload = Payload;
        }
    }

    public class MetadataMessage : BinaryMessage
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

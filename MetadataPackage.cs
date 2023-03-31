namespace SCLL
{
    public class MetadataPackage
    {
        // id, span , current, messages

        public readonly uint Id;
        public readonly uint Span;
        public readonly uint Type;
        public readonly uint Class;
        public ulong TotalPayloadSize;
        
        MetadataMessage[] _messages;

        public uint CountOfMessages => (uint)_messages.Length;

        public MetadataPackage(uint Id, uint Span, uint Type, uint Class)
        {
            this.Id = Id;
            this.Span = Span;
            this.Type = Type;
            this.Class = Class;
            this.TotalPayloadSize = 0;

            _messages = new MetadataMessage[Span];
            
        }

        public void Append(MetadataMessage metadata)
        { 
            _messages[(metadata._packageIndex - 1)] = metadata;
            TotalPayloadSize += (ulong)metadata.Payload.Length;
        }

        public bool IsComplete => _messages.Length == Span;
        
        public MetadataMessage this[uint index] => _messages[index - 1];
    }
}

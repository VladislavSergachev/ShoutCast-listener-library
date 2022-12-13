using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCLL
{
    public class MetadataPackage
    {
        // id, span , current, messages

        public readonly uint Id;
        public readonly uint Span;
        public readonly uint Type;
        public readonly uint Class;
        
        MetadataMessage[] _messages;

        public uint CountOfMessages => (uint)_messages.Length;

        public MetadataPackage(uint Id, uint Span, uint Type, uint Class)
        {
            this.Id = Id;
            this.Span = Span;
            this.Type = Type;
            this.Class = Class;

            _messages = new MetadataMessage[Span];
            
        }

        public void Append(MetadataMessage metadata) => _messages[metadata._packageIndex] = metadata;
        public bool IsComplete => _messages.Length == Span;

        public void CopyData(ref Stream output)
        {
            Array.Sort(_messages, compareIndices);

            foreach(MetadataMessage iterable in _messages)
            {
                iterable.Payload.CopyTo(output);
                iterable.Payload.Position = 0;
            }
        }

        private int compareIndices(MetadataMessage messsageA, MetadataMessage messageB) => (messsageA._packageIndex >= messageB._packageIndex) ? 1 : -1;
    }
}

using System;
using System.IO;


namespace SCLL
{
    public class MetadataReceivedArgs : EventArgs
    {
        private MessageType _msgType;
        private byte[] _msgPayload;

        public MetadataReceivedArgs(MessageType msgType, byte[] msgPayload)
        {
            _msgType = msgType;
            _msgPayload = msgPayload;
        }

        public Stream MessagePayload
        {
            get =>
                new MemoryStream(_msgPayload);
        }

        public MessageType MessageType
        {
            get =>
              _msgType;
        }
    }
}

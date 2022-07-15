using System.IO;

namespace SCLL
{
    public class TrafficProcessor
    {
        protected Parser _parser;
        protected Message _lastMessage;
        protected QueueStream _mp3Stream;

        public event NonSoundStreamReceivedHandler NonSoundStreamReceived;

        public TrafficProcessor(Stream uvoxStream)
        {
            _parser = new Parser(uvoxStream);
        }

        public virtual void Process()
        {
            _lastMessage = _parser.ParseNext();
            _mp3Stream = new QueueStream();

            if (_lastMessage.type != MessageType.DataMP3)
                NonSoundStreamReceived(this, new MetadataReceivedArgs(_lastMessage.type, _lastMessage.Payload));
            else
                _mp3Stream.Write(_lastMessage.Payload);
        }

        public QueueStream MP3Stream
        {
            get =>
                _mp3Stream;
        }

        public delegate void NonSoundStreamReceivedHandler(
          TrafficProcessor sender,
          MetadataReceivedArgs args);
    }

}

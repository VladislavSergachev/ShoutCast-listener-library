using System.IO;

namespace SCLL
{
    public class StationListener
    {
        protected Parser _parser;
        protected Message _lastMessage;
        protected long _mp3StreamTail;
        protected Stream _mp3Stream;

        public event NonSoundStreamReceivedHandler NonSoundStreamReceived;

        public StationListener(Stream uvoxStream)
        {
            _mp3StreamTail = 0;
            _parser = new Parser(uvoxStream);
        }

        public virtual void Process()
        {
            _lastMessage = _parser.ParseNext();
            _mp3Stream = new MemoryStream();

            if (_lastMessage.type != MessageType.DataMP3)
                NonSoundStreamReceived(this, new MetadataReceivedArgs(_lastMessage.type, _lastMessage.Payload));
            else
                _mp3Stream.Write(_lastMessage.Payload);
        }

        public Stream MP3Stream
        {
            get
            {
                Stream mp3Stream = _mp3Stream;

                mp3Stream.Seek(0L, SeekOrigin.Begin);
                return mp3Stream;
            }
        }

        public delegate void NonSoundStreamReceivedHandler(
          StationListener sender,
          MetadataReceivedArgs args);
    }

}

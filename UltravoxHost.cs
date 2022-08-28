using System.IO;

namespace SCLL
{
    public class UltravoxHost
    {
        private Message lastMessage;
        private Stream uvoxStream;
        private QueueStream audioStream;
        private Processor currentProcessor, audioProcessor, metadataProcessor;
        private MessageParser messageParser;

        public UltravoxHost(Stream input)
        {
            uvoxStream = input;
            audioStream = new QueueStream();
            
            messageParser = new MessageParser(input);

            audioProcessor = new AudioDataProcessor();
            metadataProcessor = new MetadataProcessor();
        }

        public void Process()
        {
            messageParser.FindNext();
            lastMessage = messageParser.Parse(uvoxStream);

            if (lastMessage.type == DataType.DataMP3)
                currentProcessor = audioProcessor;
            else
                currentProcessor = metadataProcessor;

            currentProcessor.Input = lastMessage.Payload;
            currentProcessor.Process();
        }

        public QueueStream AudioStream
        {
            get =>
                audioStream;
        }


    }
}

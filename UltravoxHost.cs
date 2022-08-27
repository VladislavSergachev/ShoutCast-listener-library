using System.IO;

namespace SCLL
{
    public class UltravoxHost
    {
        private Message lastMessage;
        private Stream uvoxStream;
        private Processor dataProcessor;
        private MessageParser messageParser;

        public UltravoxHost(Stream input)
        {
            uvoxStream = input;
            messageParser = new MessageParser(input);
        }

        public void Process()
        {
            messageParser.FindNext();
            lastMessage = messageParser.Parse();

            if (lastMessage.type == MessageType.DataMP3)
                dataProcessor = new AudioDataProcessor(lastMessage.Payload);
            else
                dataProcessor = new MetadataProcessor(lastMessage.Payload);

            dataProcessor.Process();
        }
    }
}

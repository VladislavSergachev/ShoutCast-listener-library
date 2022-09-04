using System;
using System.IO;

namespace SCLL
{
    //TODO: Fix metadata parser 
    
    public class UltravoxHost : IDataReceiver, IReceiver
    {
        private Message lastMessage;
        private Stream uvoxStream;
        private QueueStream audioStream;
        private Processor currentProcessor, audioProcessor, metadataProcessor;
        private MessageParser messageParser;

        public event MetadataReceived OnMetadataReceived;
        public delegate void MetadataReceived(UltravoxHost sender, MetadataReceivedArgs args);

        public UltravoxHost(Stream input)
        {
            uvoxStream = input;
            audioStream = new QueueStream();

            messageParser = new MessageParser(input);

            audioProcessor = new AudioDataProcessor(this);
            metadataProcessor = new MetadataProcessor(this);
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

        public void Accept()
        {

        }

        public void Accept(Stream data, DataType type)
        {
            
        }
    }
}

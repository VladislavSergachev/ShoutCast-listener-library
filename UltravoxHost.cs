using System;
using System.IO;

namespace SCLL
{
    public class UltravoxHost : IDataReceiver, IReceiver
    {
        private Message lastMessage;
        private Stream uvoxStream;
        private QueueStream audioStream;
        private Processor currentProcessor, audioProcessor, metadataProcessor;
        private MessageParser messageParser;

        public delegate void OnMetaReceivedHandler(UltravoxHost sender, MetadataReceivedArgs args);
        public event OnMetaReceivedHandler OnMetadataReceived;
        
        public UltravoxHost(Stream input)
        {
            uvoxStream = input;
            audioStream = new QueueStream();
            
            messageParser = new MessageParser();

            audioProcessor = new AudioDataProcessor(this);
            metadataProcessor = new MetadataProcessor(this);
        }

        public void Process()
        {
            messageParser.FindNext(uvoxStream);
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
            if (type == DataType.DataMP3)
                data.CopyTo(audioStream, (int)data.Length);

            else
                OnMetadataReceived?.Invoke(this, new MetadataReceivedArgs(type, data));
        }
    }
}

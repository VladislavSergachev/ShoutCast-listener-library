using System;
using System.IO;

namespace SCLL
{
    public class UltravoxHost : IDataReceiver, IReceiver
    {
        private Message lastMessage;
        private Stream uvoxStream;
        private QueueStream audioStream;
        private MessageProcessor currentProcessor, audioProcessor, metadataProcessor;
        private MessageParser messageParser;

        public delegate void OnMetaReceivedHandler(UltravoxHost sender, MetadataReceivedArgs args);
        public event OnMetaReceivedHandler OnMetadataReceived;

        public delegate void OnDistPointSignalReceivedHandler(UltravoxHost sender, DistPointSignalReceivedArgs args);
        public event OnDistPointSignalReceivedHandler OnDistPointSignalReceived;

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

            currentProcessor.Input = lastMessage;
            currentProcessor.Process();
        }

        public QueueStream AudioStream
        {
            get =>
                audioStream;
        }

        public void Accept(DataType type)
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

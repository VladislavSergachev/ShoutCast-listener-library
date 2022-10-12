using System;
using System.Collections.Generic;
using System.IO;

namespace SCLL
{
    public class UltravoxHost : IReceiver<Stream>, IReceiver<SongInfo>
    {
        private Message lastMessage;
        private Stream uvoxStream;
        private QueueStream audioStream;
        private MetadataProcessor metadataProcessor;
        private XmlProcessor xmlProcessor;
        private MessageParser messageParser;
        private SongInfo currentSongInfo;

        public delegate void OnMetaReceivedHandler(UltravoxHost sender, MetadataReceivedArgs args);
        public event OnMetaReceivedHandler OnMetadataReceived;

        public delegate void OnDistPointSignalReceivedHandler(UltravoxHost sender, DistPointSignalReceivedArgs args);
        public event OnDistPointSignalReceivedHandler OnDistPointSignalReceived;

        public UltravoxHost(Stream input)
        {
            uvoxStream = input;
            audioStream = new QueueStream();

            messageParser = new MessageParser();

            xmlProcessor = new XmlProcessor(this);
            metadataProcessor = new MetadataProcessor(xmlProcessor);
        }

        public void Process()
        {
            messageParser.FindNext(uvoxStream);
            lastMessage = messageParser.Parse(uvoxStream);

            if (lastMessage.type == DataType.DataMP3)
            {
                byte[] buffer = new byte[lastMessage.Payload.Length];
                lastMessage.Payload.Read(buffer);

                audioStream.Write(buffer);
            }
            else
            {
                metadataProcessor.Accept(lastMessage);
            }
        }

        public QueueStream AudioStream
        {
            get =>
                audioStream;
        }

        public void Accept(SongInfo info) =>
            currentSongInfo = info;

        public void Accept(Stream data) =>
             data.CopyTo(audioStream, (int)data.Length);

    }
}

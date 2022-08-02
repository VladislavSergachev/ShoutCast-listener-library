//Copyright 2022 Vladislav Sergachev (VladiSLAV)
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.


using System.IO;

namespace SCLL
{
    public class TrafficProcessor
    {
        protected MessageParser _parser;
        protected Message _lastMessage;
        protected QueueStream _mp3Stream;

        public event NonSoundStreamReceivedHandler NonSoundStreamReceived;

        public TrafficProcessor(Stream uvoxStream)
        {
            _parser = new MessageParser(uvoxStream);
            _mp3Stream = new QueueStream();
        }

        public virtual void Process()
        {
            _parser.FindNext();
            _lastMessage = _parser.Parse();

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

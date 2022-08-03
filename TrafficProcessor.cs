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
    /// <summary>
    /// General Ultravox-process unit. Its purposes are to provide MP3 stream and to notify client about received metadata 
    /// </summary>


    public class TrafficProcessor
    {
        protected MessageParser _parser;
        protected Message _lastMessage;
        protected QueueStream _mp3Stream;

        /// <summary>
        /// Raises when received message which type isn`t <see cref="MessageType.DataMP3"/>
        /// </summary>

        public event NonSoundStreamReceivedHandler NonSoundStreamReceived;


        /// <param name="uvoxStream">Stream containing Ultravox messages (Ultravox-server response)</param>

        public TrafficProcessor(Stream uvoxStream)
        {
            _parser = new MessageParser(uvoxStream);
            _mp3Stream = new QueueStream();
        }

        /// <summary>
        /// Finds next Ultravox-message in the stream and parses it. If it`s MP3 data <see cref="MP3Stream"/> is appended with this data. 
        /// In other case <see cref="NonSoundStreamReceived"/> is raised.
        /// </summary>

        public virtual void Process()
        {
            _parser.FindNext();
            _lastMessage = _parser.Parse();

            if (_lastMessage.type != MessageType.DataMP3)
                NonSoundStreamReceived(this, new MetadataReceivedArgs(_lastMessage.type, _lastMessage.Payload));
            else
                _mp3Stream.Write(_lastMessage.Payload);
        }

        /// <summary>
        /// Stream containing MP3 data
        /// </summary>
        
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

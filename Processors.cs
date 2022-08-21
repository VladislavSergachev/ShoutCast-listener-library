﻿//Copyright 2022 Vladislav Sergachev (VladiSLAV)
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


//TODO: Implement metadata synchronization 

using System;
using System.Collections.Generic;
using System.IO;

namespace SCLL
{
    /// <summary>
    /// General Ultravox-process unit. Its purposes are to provide MP3 stream and to notify client about received metadata 
    /// </summary>

    public class TrafficProcessor : Processor<Stream, QueueStream>
    {
        protected MessageParser _parser;
        protected Message _lastMessage;

        /// <summary>
        /// Raises when received message which type isn`t <see cref="MessageType.DataMP3"/>
        /// </summary>

        public event NonSoundStreamReceivedHandler NonSoundStreamReceived;


        /// <param name="uvoxStream">Stream containing Ultravox messages (Ultravox-server response)</param>

        public TrafficProcessor(Stream uvoxStream) : base(uvoxStream)
        {
            _parser = new MessageParser(input);
            output = new QueueStream();
        }

        /// <summary>
        /// Finds next Ultravox-message in the stream and parses it. If it`s MP3 data <see cref="MP3Stream"/> is appended with this data. 
        /// In other case <see cref="NonSoundStreamReceived"/> is raised.
        /// </summary>

        public override void Process()
        {
            _parser.FindNext();
            _lastMessage = _parser.Parse();

            if (_lastMessage.type != MessageType.DataMP3)
                InvokeDelegated(this, new DelegationEventArgs<Stream>() { Data = new MemoryStream(_lastMessage.Payload) });
            else
                input.Write(_lastMessage.Payload);
        }
        
        public delegate void NonSoundStreamReceivedHandler(
          TrafficProcessor sender,
          MetadataReceivedArgs args);
    }

    public class MetadataProcessor : Processor<Stream, MetaDataPackage>
    {
        private Dictionary<UInt16, MetaDataPackage> packages;

        public MetadataProcessor(Stream metadataPayload) : base(metadataPayload)
        {
            packages = new Dictionary<UInt16, MetaDataPackage>();
        }

        public override void Process()
        {
            throw new NotImplementedException();
        }
    }

    public abstract class Processor<TInput, TOutput>
    {
        protected TInput input;
        protected TOutput output;

        public Processor(TInput inputData)
        {
            this.input = inputData;
        }

        public abstract void Process();

        protected void InvokeDelegated(Processor<TInput, TOutput> sender, DelegationEventArgs<Stream> args) => 
            Delegated?.Invoke(sender, args);


        public TOutput Output
        {
            get =>
                output;
        }
        
        public TInput Input
        {
            set => 
                input = value;
        }

        public event EventHandler < DelegationEventArgs<Stream> > Delegated;
    }


}
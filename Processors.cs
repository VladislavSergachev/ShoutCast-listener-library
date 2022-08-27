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

    public class AudioDataProcessor : Processor
    {
        /// <param name="audioPayload">Stream containing Ultravox messages (Ultravox-server response)</param>

        public AudioDataProcessor(Stream audioPayload) : base(audioPayload)
        {
            output = new QueueStream();
        }

        /// <summary>
        /// Finds next Ultravox-message in the stream and parses it. If it`s MP3 data <see cref="MP3Stream"/> is appended with this data. 
        /// In ....
        /// </summary>

        public override void Process()
        {
            byte[] rawAudioData = new byte[input.Length];

            input.Read(rawAudioData);
            output.Write(rawAudioData);
        }
    }

    public class MetadataProcessor : Processor
    {
        private Dictionary<UInt16, MetadataPackage> packages;
        private MetadataParser parser;

        public MetadataProcessor(Stream metadataStream) : base(metadataStream)
        {
            packages = new Dictionary<UInt16, MetadataPackage>();
            parser = new MetadataParser(metadataStream);
        }

        public override void Process()
        {
            MetadataPackage package = parser.Parse();
        }
    }

    public abstract class Processor
    {
        protected Stream input;
        protected Stream  output;

        public Processor(Stream inputData)
        {
            this.input = inputData;
        }

        public abstract void Process();


        public Stream Output
        {
            get =>
                output;
        }
        
        public Stream Input
        {
            set => 
                input = value;
        }
    }


}

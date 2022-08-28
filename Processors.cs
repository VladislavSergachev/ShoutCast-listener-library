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


//TODO: Implement metadata synchronization 

using System;
using System.Collections.Generic;
using System.IO;

namespace SCLL
{
    /// <summary>
    /// General Ultravox-process unit. Its purposes are to provide MP3 stream and to notify client about received metadata 
    /// </summary>

    public class AudioDataProcessor : DataProcessor
    {
        /// <param name="audioPayload">Stream containing Ultravox messages (Ultravox-server response)</param>

        public AudioDataProcessor(IDataReceiver receiver) : base(receiver)
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

            receiver.Accept(output, DataType.DataMP3);
        }
    }

    public class MetadataProcessor : DataProcessor
    {
        private Dictionary<UInt16, MetadataPackage> packages;
        private MetadataParser parser;

        public MetadataProcessor(IDataReceiver receiver) : base(receiver)
        {
            packages = new Dictionary<UInt16, MetadataPackage>();
            parser = new MetadataParser();
        }

        public override void Process()
        {
            MetadataPackage package = parser.Parse(input);
            bool isPackageFinalized = false;
            
            if (!(packages.ContainsValue(package)))
                packages.Add(package.Id, package);
            else
                isPackageFinalized = packages[package.Id].Merge(package);


            if (isPackageFinalized)
            {
                Stream packageData = packages[package.Id].ToStream();
                DataType dataType = DataType.XmlShoutcast;

                receiver.Accept(packageData, dataType);
            }
        }
    }

    public abstract class DataProcessor : Processor
    {
        protected new IDataReceiver receiver;
        
        protected Stream output;

        public DataProcessor(IDataReceiver receiver) : base(null)
        {
            this.receiver = receiver;
        }

        public Stream Output
        {
            get =>
                output;
        }
    }

    public abstract class Processor
    {
        protected IReceiver receiver;

        protected Stream input;

        public Processor(IReceiver receiver)
        {
            this.receiver = receiver;
        }

        public abstract void Process();

        public Stream Input
        {
            set =>
                input = value;
        }

        protected void Accept() =>
            receiver.Accept();
    }


}

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

    public class XmlProcessor : DataProcessor<MetadataPackage, SongInfo>
    {
        public XmlProcessor(UltravoxHost host) : base(host)
        {

        }

        public override void Accept(MetadataPackage input)
        {

        }
    }
    
    public class MetadataProcessor : DataProcessor<Message, MetadataPackage>
    {
        private Dictionary<UInt16, MetadataPackage> packages;
        private MetadataParser parser;

        public MetadataProcessor(XmlProcessor receiver) : base(receiver)
        {
            packages = new Dictionary<UInt16, MetadataPackage>();
            parser = new MetadataParser();
        }

        public override void Accept(Message input) 
        {
            MetadataPackage package = parser.Parse(input.Payload); 
            bool isPackageFinalized = false;
            
            if (!packages.ContainsKey(package.Id))
            {
                packages.Add(package.Id, new MetadataPackage(package.Id, package.Span, package.Order, package.Type));
            }
                
            isPackageFinalized = packages[package.Id].Append(package);

            if (isPackageFinalized)
            {
                receiver.Accept(packages[package.Id]);
            }
        }
    }

    public abstract class DataProcessor<ExpectingInputType, TargetReceiverType> : IReceiver<ExpectingInputType>
    {
        protected IReceiver<TargetReceiverType> receiver;
        
        protected Message input;
        protected Stream output;

        public DataProcessor(IReceiver<TargetReceiverType> targetReceiver)
        {
            this.receiver = targetReceiver;
        }

        public abstract void Accept(ExpectingInputType input);

        public Stream Output
        {
            get =>
                output;
        }
    }
}

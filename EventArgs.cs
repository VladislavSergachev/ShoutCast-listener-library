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

using System;
using System.IO;


namespace SCLL
{
    /// <summary>
    /// Contains information about message which has received
    /// </summary>
    public class MetadataReceivedArgs : EventArgs
    {
        private DataType _msgType;
        private Stream _msgPayload;

        public MetadataReceivedArgs(DataType msgType, Stream msgPayload)
        {
            _msgType = msgType;
            _msgPayload = msgPayload;
        }

        public Stream MessagePayload
        {
            get =>
                _msgPayload;
        }

        public DataType MessageType
        {
            get =>
              _msgType;
        }
    }

    public class DelegationEventArgs<TData> : EventArgs
    {
        public TData Data { get; set; }
    }
}

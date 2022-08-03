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


//TODO: Add documentation to QueueStream

using System.IO;
using System.Collections.Generic;
using System;

namespace SCLL
{
    public sealed class QueueStream : Stream
    {
        private Queue<byte> _payload;

        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => true;

        public override bool CanTimeout => false;

        public override long Position
        { 
            get =>
                throw new System.NotSupportedException();
            set =>
                throw new System.NotSupportedException();
        }

        public override long Length => throw new System.NotSupportedException();

        public override long Seek(long offset, SeekOrigin origin) => throw new System.NotSupportedException();

        public override void SetLength(long value) => throw new System.NotSupportedException();

        /// <summary>
        ///  Clean <code>QueueStream</code> instance up (not Dispose())
        /// </summary>
        public override void Flush() => _payload.Clear();

        
        /// <param name="buffer">Buffer to read into</param>
        /// <param name="offset">Not used. Should be 0</param>
        /// <param name="count">Count of elements to be read</param>
        /// <returns>Count of bytes read</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            int bytesRead = 0;

            bool byteIndexIsInBounds(int idx) => (idx < count) && (idx < buffer.Length) && (_payload.Count > 0);
            
            for (int idx = 0; byteIndexIsInBounds(idx); idx++)
            {
                buffer[idx] = _payload.Dequeue();
                bytesRead++;
            }

            return bytesRead;
        }

        public override int Read(Span<byte> buffer)
        {
            int bytesRead = 0;
            int bufferLength = buffer.Length;

            bool byteIndexIsInBounds(int idx, int bufferLength) => (idx < bufferLength) && (_payload.Count > 0);

            for (int idx = 0; byteIndexIsInBounds(idx, bufferLength); idx++)
            {
                buffer[idx] = _payload.Dequeue();
                bytesRead++;
            }

            return bytesRead;
        }

        /// <param name="buffer">Buffer to write from</param>
        /// <param name="offset">Position to begin writing from</param>
        /// <param name="count">Count of elements to be write</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
             for(int idx = offset; idx < count; idx++)
             {
                _payload.Enqueue(buffer[idx]);
             }
        }

        public override void Write(ReadOnlySpan<byte> buffer)
        {
            foreach(byte b in buffer)
                _payload.Enqueue(b);
        }

        public QueueStream(byte[] data)
        {
            _payload = new Queue<byte>(data);
        }

        public QueueStream()
        {
            _payload = new Queue<byte>();
        }
    }
}

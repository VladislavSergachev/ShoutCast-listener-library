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

namespace SCLL
{
    public enum MessageType
    {
        TemporaryInterruption = 0x2001,
        BroadcastTermination = 0x2003,
        XmlAol = 0x3901,
        XmlShoutcast = 0x3902,
        StationLogoJPEG = 0x4000,
        StationLogoPNG = 0x4001,
        StationLogoBMP = 0x4002,
        StationLogoGIF = 0x4003,
        AlbumArtJPEG = 0x4100,
        AlbumArtPNG = 0x4101,
        AlbumArtBMP = 0x4102,
        AlbumArtGIF = 0x4103,
        DataMP3 = 0x7000,
    }

    /// <summary>
    /// Representing Ultravox-message
    /// </summary>
    /// <remarks>
    /// See <see href="http://wiki.shoutcast.com/wiki/SHOUTcast_2_(Ultravox_2.1)_Protocol_Details#Ultravox_Messages"> for details.
    /// </remarks>
    public sealed class Message
    {
        public const int ULTRAVOX_SYNC_BYTE = 0x5A;
        public const int ULTRAVOX_TRAILING_BYTE = 0x00;

        public byte ResQos;
        public MessageType type;
        public byte[] Payload;
        public ushort PayloadLength;
    }
}

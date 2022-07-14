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

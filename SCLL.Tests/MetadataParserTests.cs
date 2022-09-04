using SCLL;
using Xunit;
using System.IO;

namespace ScllTests
{

    public class MetadataParserTests
    {
        static byte[] payloadSecondPart = { 0x00, 0x2b, 0x00, 0x02, 0x00, 0x02, 0x7, 0x8, 0x9, 0xA, 0xB, 0XC, 0XD };
        static byte[] payloadFirstPart = { 0x00, 0x2b, 0x00, 0x02, 0x00, 0x01, 0X1, 0X2, 0X3, 0X4, 0X5, 0X6 };

        byte[] metaData2 = { 0xff, 0xff, 0x0, 0x3, 0x0, 0x1, 0x73, 0x68, 0x69};
        byte[] metaData3 = { 0xff, 0xff, 0x0, 0x3, 0x0, 0x2, 0x66, 0x74, 0x65 };
        byte[] metaData1 = { 0xff, 0xff, 0x0, 0x3, 0x0, 0x3, 0x72, 0x73 };

        Message first = new Message
        {
            ResQos = 0,
            type = DataType.XmlShoutcast,
            PayloadLength = (ushort)payloadSecondPart.Length,
            Payload = new MemoryStream(payloadSecondPart)
        };

        Message second = new Message
        {
            ResQos = 0,
            type = DataType.XmlShoutcast,
            PayloadLength = (ushort)payloadFirstPart.Length,
            Payload = new MemoryStream(payloadFirstPart)
        };

        MetadataParser parser;

        [Fact]
        public void ShouldGetCorrectPackageParameters()
        {
            MetadataPackage template = new MetadataPackage(0x2b, 0x02, 0x02, new byte[] { 1, 2, 3 }, DataType.XmlShoutcast);

            parser = new MetadataParser();
            MetadataPackage pkg = parser.Parse(first.Payload);

            Assert.Equal(template.Span, pkg.Span);
            Assert.Equal(template.Id, pkg.Id);
            Assert.Equal(template.Order, pkg.Order);
        }

        [Fact]
        public void ShouldGetFullCorrectPackage()
        {
            bool finalized = false;
            Stream str = new MemoryStream(metaData1.Length + metaData2.Length + metaData3.Length);
            parser = new MetadataParser();

            str.Write(metaData1);
            str.Write(metaData2);
            str.Write(metaData3);

            str.Position = 0;

            MetadataPackage pkg = new MetadataPackage();

            while(!finalized)
            {
                MetadataPackage ordered = parser.Parse(str);
                finalized = pkg.Merge(ordered);
            }

            Stream data = pkg.ToStream();
            string result = "";

            for (int i = 0; i < data.Length; i++)
                result += (char)str.ReadByte();

            Assert.Equal("shifters", result);
        }
    }
}

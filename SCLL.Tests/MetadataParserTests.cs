using SCLL;
using Xunit;
using System.IO;

namespace ScllTests
{

    public class MetadataParserTests
    {
        static byte[] payloadSecondPart = { 0x00, 0x2b, 0x00, 0x02, 0x00, 0x02, 0x7, 0x8, 0x9, 0xA, 0xB, 0XC, 0XD };
        static byte[] payloadFirstPart = { 0x00, 0x2b, 0x00, 0x02, 0x00, 0x01, 0X1, 0X2, 0X3, 0X4, 0X5, 0X6 };

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
        public void ShouldReturnCorrectPackage()
        {
            MetadataPackage template = new MetadataPackage(0x2b, 0x02, 0x02, new byte[] {0x00, 0x00}, DataType.XmlShoutcast);

            parser = new MetadataParser();
            MetadataPackage pkg = parser.Parse(first.Payload);

            Assert.Equal(template.Span, pkg.Span);
            Assert.Equal(template.Id, pkg.Id);
            Assert.Equal(template.Order, pkg.Order);
        }
    }
}

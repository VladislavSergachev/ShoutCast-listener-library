using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SCLL;
using Xunit;

namespace ScllTests
{
    public class MetadataPackageTests
    {
        private byte[] mdPart1 = new byte[] { 0x6f, 0x72, 0x6c, 0x64 };
        private byte[] mdPart2 = new byte[] { 0x68, 0x65 };
        private byte[] mdPart3 = new byte[] { 0x6c, 0x6c, 0x6f, 0x57 };
        
        [Fact]
        public void ShouldGetHelloWorldString()
        {
            MetadataPackage pkg1 = new MetadataPackage(0xffff, 0x0003, 0x0002, mdPart3, DataType.XmlShoutcast);
            MetadataPackage pkg2 = new MetadataPackage(0xffff, 0x0003, 0x0003, mdPart1, DataType.XmlShoutcast);
            MetadataPackage pkg3 = new MetadataPackage(0xffff, 0x0003, 0x0001, mdPart2, DataType.XmlShoutcast);

            pkg1.Append(pkg2);
            pkg1.Append(pkg3);

            Stream result = pkg1.GetAsSortedStream();
            byte[] beffer = new byte[result.Length];

            result.Read(beffer);

            string resultStr = Encoding.ASCII.GetString(beffer);
            Assert.Equal("helloWorld", resultStr);
        }
    }
}

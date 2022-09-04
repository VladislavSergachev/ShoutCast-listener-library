using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using SCLL;
using System.IO;

namespace ScllTests
{
    public class MetadataPackageTests
    {
        private MetadataPackage accumulator, package;

        [Fact]
        public void ShouldMergePackageCorrectly()
        {
            accumulator = new MetadataPackage(0xff, 2, 2, Encoding.ASCII.GetBytes("lib"), DataType.XmlShoutcast);
            package = new MetadataPackage(0xff, 2, 1, Encoding.ASCII.GetBytes("scl"), DataType.XmlShoutcast);

            accumulator.Merge(package);
            Stream result = accumulator.ToStream();

            result.Position = 0;

            string resultStr = string.Empty;
            for(int i = 0; i < result.Length; i++)
                resultStr += (char)result.ReadByte();

            Assert.Equal("scllib", resultStr);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using SCLL;
using Xunit;
using System.IO;

namespace ScllTests
{
    public class UltravoxHostTests
    {
        byte[] soundMessage1 = { 0x5a, 0x00, 0x70, 0x00, 0x0, 0x3, 0x76, 0x65, 0x72, 0x00 };
        byte[] soundMessage2 = { 0x5a, 0x00, 0x70, 0x00, 0x0, 0x3, 0x76, 0x65, 0x72, 0x00 };

        byte[] metaMessage2 = { 0x5a, 0x00, 0x39, 0x02, 0x0, 0x9, 0xff, 0xff, 0x0, 0x3, 0x0, 0x1, 0x73, 0x68, 0x69, 0x00 };
        byte[] metaMessage3 = { 0x5a, 0x00, 0x39, 0x02, 0x0, 0x9, 0xff, 0xff, 0x0, 0x3, 0x0, 0x2, 0x66, 0x74, 0x65, 0x00 };
        byte[] metaMessage1 = { 0x5a, 0x00, 0x39, 0x02, 0x0, 0x8, 0xff, 0xff, 0x0, 0x3, 0x0, 0x3, 0x72, 0x73, 0x00 };

        UltravoxHost host;

        [Fact]
        public void AudioStreamShouldContainTwoVERs()
        {
            Stream str = new MemoryStream(20);
            StreamReader reader;

            str.Write(soundMessage1);
            str.Write(soundMessage2);

            str.Position = 0;

            host = new UltravoxHost(str);

            host.Process();
            host.Process();

            reader = new StreamReader(host.AudioStream);

            string result = reader.ReadToEnd();

            Assert.NotNull(result);
        }

        [Fact]
        public void SHouldGetShiftersString()
        {
            Stream str = new MemoryStream(metaMessage1.Length + metaMessage2.Length + metaMessage3.Length);
            StreamReader reader;

            str.Write(metaMessage1);
            str.Write(metaMessage2);
            str.Write(metaMessage3);

            str.Position = 0;

            host = new UltravoxHost(str);

            host.Process();
            host.Process();
            host.Process();

            reader = new StreamReader(host.AudioStream);

            string result = reader.ReadToEnd();

            Assert.Equal("shifters", result);
        }
    }


}

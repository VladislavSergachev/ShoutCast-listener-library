using SCLL;
using Xunit;
using System.IO;
using System.Collections.Generic;

namespace ScllTests
{

    public class MessageParserTests
    {
        Stream bareUvoxData, corruptedUvoxData;

        byte[] BARE_DATA = new byte[]
        {
            0x5a, 0x00, 0x39, 0x02, 0x00, 0x05, 0x01, 0x02, 0x03, 0x04, 0x05, 0x00,
            0x5a, 0x00, 0x39, 0x02, 0x00, 0x05, 0x01, 0x06, 0x03, 0x04, 0x05, 0x00,
            0x5a, 0x00, 0x39, 0x02, 0x00, 0x05, 0x03, 0x02, 0x03, 0x04, 0x05, 0x00
        };

        byte[] CORRUPTED_DATA = new byte[]
        {
            0xff, 0x30, 0x32, 0xdd, 0x5a, 0x00, 0x39, 0x02, 0x00, 0x05, 0x01, 0x02, 0x03, 0x04, 0x05, 0x00,
            0x78, 0x6f, 0xdd, 0x5a, 0x00, 0x39, 0x02, 0x00, 0x05, 0x01, 0x06, 0x03, 0x04, 0x05, 0x00,
            0x5f, 0x5a, 0x00, 0x39, 0x02, 0x00, 0x05, 0x03, 0x02, 0x03, 0x04, 0x05, 0x00
        };

        [Fact]
        public void ShouldGetThreeCorrectMessagesFromBare()
        {
            int correctMessages = 0;

            List<Message> messages = new List<Message>();
            bareUvoxData = new MemoryStream(BARE_DATA); // this file should contain 3 ordered Uvox messages (type XmlShoutcast, payload 10 bytes of any content)

            MessageParser parser = new MessageParser(bareUvoxData);

            for (int m = 0; m < 3; m++)
            {
                parser.FindNext();
                messages.Add(parser.Parse());
            }

            foreach (Message message in messages)
                correctMessages += (message.type == MessageType.XmlShoutcast) ? 1 : 0;

            Assert.True(correctMessages == 3);
        }

        [Fact]
        public void ShouldGetThreeCorrectMessagesFromCorrupted()
        {
            int correctMessages = 0;

            List<Message> messages = new List<Message>();
            corruptedUvoxData = new MemoryStream(CORRUPTED_DATA); // this file should contain 3 ordered Uvox messages (type XmlShoutcast, payload 10 bytes of any content) with garbage bytes

            MessageParser parser = new MessageParser(corruptedUvoxData);

            for (int m = 0; m < 3; m++)
            {
                parser.FindNext();
                messages.Add(parser.Parse());
            }

            foreach (Message message in messages)
                correctMessages += (message.type == MessageType.XmlShoutcast) ? 1 : 0;

            corruptedUvoxData.Close();

            Assert.True(correctMessages == 3);
        }
    }

}

using SCLL;
using Xunit;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

namespace ScllTests
{
   public class QueueStreamTests
    {
        static QueueStream stream;
        
        [Theory]
        [InlineData("Hello")]
        public void SourceAndDestStringsAreEqual(string arg)
        {
            byte[] readBuffer = new byte[256];
            stream = new QueueStream();

            stream.Write(Encoding.ASCII.GetBytes(arg), 0, arg.Length);

            stream.Read(readBuffer, 0, readBuffer.Length);

            string result = Encoding.ASCII.GetString(readBuffer).Replace("\0", "");   
            Assert.Equal(arg, result);
        }

        [Theory]
        [InlineData("Hello")]
        public void SourceAndDestStringsAreEqualSpanBased(string arg)
        {
            byte[] readBuffer = new byte[256];
            stream = new QueueStream();

            stream.Write(Encoding.ASCII.GetBytes(arg));

            stream.Read(readBuffer);

            string result = Encoding.ASCII.GetString(readBuffer).Replace("\0", "");
            Assert.Equal(arg, result);
        }
    }

    public class ParserTests
    {
        FileStream bareUvoxData, corruptedUvoxData;

        [Fact]
        public void ShouldGetThreeCorrectMessagesFromBare()
        {
            int correctMessages = 0;
            
            List<Message> messages = new List<Message>();
            bareUvoxData = new FileStream("bareUvox.dump", FileMode.Open, FileAccess.Read); // this file should contain 3 ordered Uvox messages (type XmlShoutcast, payload 10 bytes of any content)

            MessageParser parser = new MessageParser(bareUvoxData);

            for (int m = 0; m < 3; m++)
            {
                parser.FindNext();
                messages.Add(parser.Parse());
            }

            foreach (Message message in messages)
                correctMessages += (message.type == MessageType.XmlShoutcast) ? 1 : 0;

            bareUvoxData.Close();

            Assert.True(correctMessages == 3);
        }
        
        [Fact]
        public void ShouldGetThreeCorrectMessagesFromCorrupted()
        {
            int correctMessages = 0;
            
            List<Message> messages = new List<Message>();
            corruptedUvoxData = new FileStream("corruptedUvox.dump", FileMode.Open, FileAccess.Read); // this file should contain 3 ordered Uvox messages (type XmlShoutcast, payload 10 bytes of any content) with garbage bytes

            MessageParser parser = new MessageParser(corruptedUvoxData);
            
            for(int m = 0; m < 3; m++)
            {
               parser.FindNext();
               messages.Add(parser.Parse());
            }

            foreach(Message message in messages)
                correctMessages += (message.type == MessageType.XmlShoutcast) ? 1 : 0;

            corruptedUvoxData.Close();

            Assert.True(correctMessages == 3);
        }


    }
}

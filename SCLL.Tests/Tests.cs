using SCLL;
using Xunit;
using System.Text;
using System.IO;
using System.Collections.Generic;

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
        public void ShouldGetThreeCorrectMessageûFromBare()
        {
            int correctMessages = 0;
            
            List<Message> messages = new List<Message>();
            bareUvoxData = new FileStream("bareUvox.dump", FileMode.Open, FileAccess.Read); // this file should contain 3 ordered Uvox messages (type XmlShoutcast, payload 10 bytes of any content)

            Parser parser = new Parser(bareUvoxData);

            for (int m = 0; m < 3; m++)
            {
                int b = bareUvoxData.ReadByte();
                if (b != Message.ULTRAVOX_SYNC_BYTE)
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

            Parser parser = new Parser(corruptedUvoxData);
            
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

using SCLL;
using Xunit;
using System.Text;

namespace QueueStreamTests
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

}

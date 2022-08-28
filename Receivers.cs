using System.IO;

namespace SCLL
{
    public interface IReceiver
    {
        void Accept();
        void Accept(Stream data, DataType type);
    }
}

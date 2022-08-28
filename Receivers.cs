using System.IO;

namespace SCLL
{
    public interface IReceiver
    {
        public abstract void Accept();
    }    
         
    public interface IDataReceiver
    {    
        public void Accept(Stream data, DataType type);
    }
}

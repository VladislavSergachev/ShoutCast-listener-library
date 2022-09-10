using System.IO;

namespace SCLL
{
    public interface IReceiver
    {
        public abstract void Accept(DataType type);
    }    
         
    public interface IDataReceiver
    {    
        public void Accept(Stream data, DataType type);
    }
}

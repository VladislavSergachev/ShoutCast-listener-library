using System.IO;

namespace SCLL
{
    public interface IReceiver<ExpectingDataType>
    {
        public abstract void Accept(ExpectingDataType data);
    }    
}

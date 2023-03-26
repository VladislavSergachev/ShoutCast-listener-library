using System.Text;
using System.Xml;

namespace SCLL
{
    public class ContentInfo
    {
        protected MemoryStream data;
        
        public ContentInfo()
        {

        }
        
        public ContentInfo(MetadataPackage package) 
        {
            data = new MemoryStream((int)package.TotalPayloadSize);
            this.Read(package);
        }

        public void Read(MetadataPackage package)
        {
            for (uint i = 0; i < package.Span; i++)
                package[i].Payload.CopyTo(data);
        }

        public virtual bool Parse() => throw new NotImplementedException();
    }

    public class MinimalSongInfo : ContentInfo
    {
        protected string? titleValue;
        protected string titleTagName = "TIT2";
        protected XmlDocument xmlRoot;

        public MinimalSongInfo(MetadataPackage package) : base(package)
        {
            xmlRoot = new XmlDocument();
            xmlRoot.Load(data);
        }
        
        public override bool Parse()
        {
            try
            {
                titleValue = xmlRoot[titleTagName].Value;
            }
            catch (NullReferenceException e)
            {
                return false;
            }

            return true;
        }

        public string Title { get { return titleValue; } }
    }
}

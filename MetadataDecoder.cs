using System.Text;
using System.Xml;

namespace SCLL
{
    public class ContentInfo
    {
        protected MemoryStream data;
        
        public ContentInfo(MetadataPackage package) 
        {
            data = new MemoryStream((int)package.TotalPayloadSize);
            for (uint i = 1; i <= package.Span; i++)
                package[i].Payload.CopyTo(data);

            data.Position = 0;
        }

        public virtual void Parse() => throw new NotImplementedException();
    }

    public class XmlInfo : ContentInfo
    {
        protected string? titleValue;
        protected string titleTagName = "TIT2";
        protected readonly XmlDocument xmlRoot;
        protected readonly XmlElement metadataSection;

        public XmlInfo(MetadataPackage package) : base(package)
        {
            xmlRoot = new XmlDocument();
            xmlRoot.Load(data);

            metadataSection = xmlRoot["metadata"];

            data.Position = 0;
        }
        
        public override void Parse()
        {
            titleValue = metadataSection[titleTagName]?.InnerText;
        }

        public string Title { get { return titleValue; } }
    }
}

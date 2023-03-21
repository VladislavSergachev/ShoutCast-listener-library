using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SCLL
{
    public class ContentInfo
    {
        protected XmlDocument xmlSchema;
        
        public ContentInfo() 
        {
            xmlSchema = new XmlDocument();
        }

        public void Read(MetadataPackage package)
        {
            string xmlString = string.Empty;
            
            for (uint i = 0; i < package.Span; i++)
            {
                byte[] buffer = new byte[package[i].Payload.Length];
                package[i].Payload.Read(buffer);

                xmlString += Encoding.UTF8.GetString(buffer);
            }

            xmlSchema.Load(xmlString);
            this.Parse();
        }

        public virtual void Parse() => throw new NotImplementedException();
    }

    public class XmlSongInfo : ContentInfo
    {
        public override void Parse()
        {
            
        }
    }
}

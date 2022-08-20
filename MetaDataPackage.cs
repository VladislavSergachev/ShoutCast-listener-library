using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SCLL
{
    public class MetaDataPackage
    {
        public readonly List<Stream> Parts;
        
        public readonly UInt16 Id;
        public readonly UInt16 Span;

        public MetaDataPackage(UInt16 id, UInt16 span)
        {
            Id = id;
            Span = span;

            Parts = new List<Stream>();
        }


        public void Flush(MetaDataPackage package, UInt16 order) => 
            this.Parts.InsertRange(order, package.Parts);
    }
}

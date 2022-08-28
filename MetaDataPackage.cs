using System;
using System.Collections.Generic;
using System.IO;


namespace SCLL
{
    public class MetadataPackage
    {
        public readonly List<Stream> Parts;
        
        public readonly UInt16 Id;
        public readonly UInt16 Span;
        public readonly UInt16 Order;

        public DataType Type;

        
        public MetadataPackage(UInt16 id, UInt16 span, UInt16 order, DataType type)
        {
            Id = id;
            Span = span;
            Order = order;

            Parts = new List<Stream>();
        }


        public bool Merge(MetadataPackage package)
        {
            Parts.InsertRange(package.Order, package.Parts);
            return (Parts.Count == Span);
        }

        public Stream ToStream()
        {
            Stream result = new MemoryStream();
            foreach(Stream stream in Parts)
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer);
                result.Write(buffer);
            }

            return result;
        }
    }
}

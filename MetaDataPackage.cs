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

        public readonly DataType Type;

        
        public MetadataPackage()
        {
            Parts = new List<Stream>();
        }
        
        public MetadataPackage(UInt16 id, UInt16 span, UInt16 order, byte[] initialPayload, DataType type)
        {
            Id = id;
            Span = span;
            Order = order;

            Parts = new List<Stream>();
            Parts.Add(new MemoryStream(initialPayload));
        }

                                                                          // 0 - - - -
                                                                          // - - 0 0 0
        public bool Merge(MetadataPackage package)
        {
            uint elementsToAdd = (uint)Math.Abs((decimal)(package.Parts.Count - Parts.Count)) + package.Order;

            for (int i = 0; i < elementsToAdd; i++)
                Parts.Add(null);

            Parts.InsertRange( (package.Order > 0 ? (package.Order - 1) : 0), package.Parts);
            

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

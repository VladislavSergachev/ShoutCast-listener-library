using System;
using System.Collections.Generic;
using System.IO;


namespace SCLL
{
    public class MetadataPackage
    {
        public readonly List< Tuple<Stream, UInt16> > Parts;
        
        public readonly UInt16 Id;
        public readonly UInt16 Span;
        public readonly UInt16 Order;

        public DataType Type;

        public MetadataPackage(UInt16 id, UInt16 span, UInt16 order, byte[] initialData, DataType type)
        {
            Id = id;
            Span = span;
            Order = order;

            Parts = new List< Tuple<Stream, UInt16> >();
            Parts.Add(new Tuple<Stream, UInt16>(new MemoryStream(initialData), order));
        }

        public MetadataPackage(UInt16 id, UInt16 span, UInt16 order, DataType type)
        {
            Id = id;
            Span = span;
            Order = order;

            Parts = new List<Tuple<Stream, UInt16>>();
        }


        private int compareByOrderInt( Tuple<Stream, UInt16> firstPair, Tuple<Stream, UInt16> secondPair)
        {
            int result = 0;

            if (firstPair.Item2 > secondPair.Item2)
            {
                result = 1;
            }
            else if(firstPair.Item2 < secondPair.Item2)
            {
                result = -1;
            }

            return result;
        }

        public bool Append(MetadataPackage addition)
        {
            Parts.AddRange(addition.Parts);
            return (Parts.Count == this.Span);
        }

        public Stream GetAsSortedStream()
        {
            Stream result = new MemoryStream();
            Parts.Sort(compareByOrderInt);

            foreach( Tuple<Stream, UInt16> pair in Parts)
            {
                byte[] buffer = new byte[pair.Item1.Length];

                pair.Item1.Read(buffer);
                result.Write(buffer);
            }

            result.Position = 0;
            return result;
        }
    }
}

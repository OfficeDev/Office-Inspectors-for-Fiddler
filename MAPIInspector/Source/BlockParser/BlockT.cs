using System;

namespace BlockParser
{
    public class BlockT<T> : Block where T : struct
    {
        public T Data { get; set; }

        public BlockT() { }

        public BlockT(T _data, long size, long offset)
        {
            Parsed = true;
            Data = _data;
            Size = size;
            Offset = offset;
        }

        /// <summary>
        /// Read a block off our stream, but don't advance the stream position.
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        public static BlockT<T> TestParse(BinaryParser parser)
        {
            var offset = parser.Offset;
            var ret = new BlockT<T>
            {
                parser = parser
            };
            ret.EnsureParsed();
            parser.Offset = offset;
            return ret;
        }

        // Construct directly from a parser
        public BlockT(BinaryParser parser) => Parse(parser);

        protected override void Parse()
        {
            Parsed = false;
            Type type = typeof(T);
            if (type.IsEnum)
                type = Enum.GetUnderlyingType(type);
            int size = System.Runtime.InteropServices.Marshal.SizeOf(type);
            if (!parser.CheckSize(size)) return;

            Data = ReadStruct<T>(parser);
            Parsed = true;
        }

        protected override void ParseBlocks()
        {
            // No blocks to parse in BlockT
            // TODO: Consider if a default implementation should be provided
        }

        public static U ReadStruct<U>(BinaryParser parser) where U : struct
        {
            Type type = typeof(U);
            if (type.IsEnum)
                type = Enum.GetUnderlyingType(type);
            int size = System.Runtime.InteropServices.Marshal.SizeOf(type);
            byte[] bytes = parser.ReadBytes(size);
            var handle = System.Runtime.InteropServices.GCHandle.Alloc(bytes, System.Runtime.InteropServices.GCHandleType.Pinned);
            try
            {
                IntPtr ptr = handle.AddrOfPinnedObject();
                U convert = (U)System.Runtime.InteropServices.Marshal.PtrToStructure(ptr, type);
                return convert;
            }
            finally
            {
                handle.Free();
            }
        }

        public static BlockT<U> EmptyT<U>() where U : struct => new BlockT<U>();
    }
}

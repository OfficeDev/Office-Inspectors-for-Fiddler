using System;

namespace BlockParser
{
    public class BlockT<T> : Block where T : struct
    {
        public T Data { get; set; }

        public BlockT() { }

        public BlockT(T _data, int size, int offset)
        {
            Parsed = true;
            Data = _data;
            Size = size;
            Offset = offset;
        }

        public static BlockT<T> Create(T data, int size, int offset)
        {
            var ret = new BlockT<T>(data, size, offset)
            {
                Parsed = true
            };
            return ret;
        }

        public static BlockT<T> Parse(BinaryParser parser)
        {
            var ret = new BlockT<T>
            {
                parser = parser
            };
            ret.EnsureParsed();
            return ret;
        }

        // Construct directly from a parser
        public BlockT(BinaryParser parser) => Parse<T>(parser);

        // Build and return object of type T, reading from type U
        public static BlockT<T> Parse<U>(BinaryParser parser) where U : struct
        {
            Type type = typeof(U);
            if (type.IsEnum)
                type = Enum.GetUnderlyingType(type);
            if (!parser.CheckSize(System.Runtime.InteropServices.Marshal.SizeOf(type)))
                return new BlockT<T>();

            U uData = ReadStruct<U>(parser);
            int offset = parser.Offset;
            return Create((T)Convert.ChangeType(uData, typeof(T)), System.Runtime.InteropServices.Marshal.SizeOf(type), offset);
        }

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

        private static U ReadStruct<U>(BinaryParser parser) where U : struct
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

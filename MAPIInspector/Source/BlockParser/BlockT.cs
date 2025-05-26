using System;

namespace Parser
{
    public class BlockT<T> : Block where T : struct
    {
        private T data;

        public BlockT() { }

        public BlockT(T data, int size, int offset)
        {
            this.parsed = true;
            this.data = data;
            this.Size = size;
            this.SetOffset(offset);
        }

        public void SetData(T data) => this.data = data;
        public T GetData() => data;
        public ref T GetDataRef() => ref data;

        public T Data
        {
            get => data;
            set => data = value;
        }

        public static BlockT<T> Parse(BinaryParser parser)
        {
            var ret = new BlockT<T>();
            ret.parser = parser;
            ret.EnsureParsed();
            return ret;
        }

        // Construct directly from a parser
        public BlockT(BinaryParser parser) => Parse<T>(parser);

        // Build and return object of type T, reading from type U
        public static BlockT<T> Parse<U>(BinaryParser parser) where U : struct
        {
            if (!parser.CheckSize(System.Runtime.InteropServices.Marshal.SizeOf(typeof(U))))
                return new BlockT<T>();

            U uData = ReadStruct<U>(parser);
            int offset = parser.Offset;
            return Create((T)Convert.ChangeType(uData, typeof(T)), System.Runtime.InteropServices.Marshal.SizeOf(typeof(U)), offset);
        }

        public static BlockT<T> Create(T data, int size, int offset)
        {
            var ret = new BlockT<T>(data, size, offset);
            ret.parsed = true;
            return ret;
        }

        protected override void Parse()
        {
            this.parsed = false;
            int size = System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));
            if (!parser.CheckSize(size)) return;

            data = ReadStruct<T>(parser);
            this.parsed = true;
        }

        private static U ReadStruct<U>(BinaryParser parser) where U : struct
        {
            int size = System.Runtime.InteropServices.Marshal.SizeOf(typeof(U));
            byte[] bytes = parser.ReadBytes(size);
            var handle = System.Runtime.InteropServices.GCHandle.Alloc(bytes, System.Runtime.InteropServices.GCHandleType.Pinned);
            try
            {
                IntPtr ptr = handle.AddrOfPinnedObject();
                U convert = (U)System.Runtime.InteropServices.Marshal.PtrToStructure(ptr, typeof(U));
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

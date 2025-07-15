using System;

namespace BlockParser
{
    // Dummy interface to signal this is a BlockT type
    public interface IBlockT { }

    public class BlockT<T> : Block, IBlockT where T : struct
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

        // Construct directly from a parser
        public BlockT(BinaryParser parser) => Parse(parser);

        public static implicit operator string(BlockT<T> block) => block.ToString();
        public override string ToString()
        {
            var type = typeof(T);
            if (type == typeof(Guid))
            {
                // Consider Guid as a special case - we may wanna format it differently
                return Data.ToString();
            }
            else if (type.IsEnum)
            {
                return $"{Data} = 0x{Convert.ToUInt64(Data):X}";
            }
            else if (Data is IFormattable formattable)
            {
                return $"{Data} = 0x{formattable.ToString("X", null)}";
            }
            else
            {
                return Data.ToString();
            }
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

        protected override void ParseBlocks()
        {
            // No blocks to parse in BlockT
            // TODO: Consider if a default implementation should be provided
        }

        internal static U ReadStruct<U>(BinaryParser parser) where U : struct
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

        public static implicit operator T(BlockT<T> block)
        {
            return block != null ? block.Data : default;
        }
    }
}

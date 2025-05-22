namespace MAPIInspector.Parsers.block
{
	// Generic BlockT<T> class, mimicking the C++ template
	internal class BlockT<T> : Block
    {
		private T data;
		private bool parsed;

		public BlockT()
		{
			parsed = false;
		}

		// Copy constructor and assignment are not needed in C# (handled by default)

		public BlockT(T _data, int _size, int _offset)
		{
			parsed = true;
			data = _data;
			SetSize(_size);
			SetOffset(_offset);
		}

		// Mimic type T
		public void SetData(T _data) { data = _data; }
		public T GetData() { return data; }
		public ref T GetDataAddress() { return ref data; }
		public static implicit operator T(BlockT<T> block) => block.data;

		// Static parse method (single type)
		public static BlockT<T> Parse(BinaryParser parser)
		{
			var ret = new BlockT<T>();
			ret.parser = parser;
			ret.EnsureParsed();
			return ret;
		}

		// Static parse method (from type U)
		public static BlockT<T> Parse<U>(BinaryParser parser) where U : struct
		{
			if (!parser.CheckSize(System.Runtime.InteropServices.Marshal.SizeOf(typeof(U))))
				return new BlockT<T>();

			U _data = parser.ReadStruct<U>();
			int offset = parser.GetOffset();
			parser.Advance(System.Runtime.InteropServices.Marshal.SizeOf(typeof(U)));
			return Create((T)(object)_data, System.Runtime.InteropServices.Marshal.SizeOf(typeof(U)), offset);
		}

		public static BlockT<T> Create(T _data, int _size, int _offset)
		{
			var ret = new BlockT<T>(_data, _size, _offset);
			ret.parsed = true;
			return ret;
		}

		protected override void Parse()
		{
			parsed = false;
			int size = System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));
			if (!parser.CheckSize(size)) return;

			data = parser.ReadStruct<T>();
			parser.Advance(size);
			parsed = true;
		}

		// Construct directly from a parser
		public BlockT(BinaryParser parser)
		{
			Parse<T>(parser);
		}
	}

	// Helper to create an empty BlockT<T>
	internal static class BlockTHelper
	{
		public static BlockT<T> EmptyT<T>() => new BlockT<T>();
	}
}

using System;
using System.Text;

namespace MAPIInspector.Parsers.block
{
	// Assuming Block is a base class in your project
	internal class BlockStringW : Block
	{
		private string data = string.Empty;
		private int cchChar = -1;

		public BlockStringW()
		{
		}

		// Mimic std::wstring
		public static implicit operator string(BlockStringW block) => block.data;
		public string Value => data;
		public int Length => data.Length;
		public bool IsEmpty => string.IsNullOrEmpty(data);
		public string CStr() => data;

		public static BlockStringW Parse(string input, int size, int offset)
		{
			var ret = new BlockStringW();
			ret.Parsed = true;
			ret.EnableJunk = false;
			ret.data = input;
			ret.SetText(input);
			ret.SetSize(size);
			ret.SetOffset(offset);
			return ret;
		}

		public static BlockStringW Parse(BinaryParser parser, int cchChar = -1)
		{
			var ret = new BlockStringW();
			ret.Parser = parser;
			ret.EnableJunk = false;
			ret.cchChar = cchChar;
			ret.EnsureParsed();
			return ret;
		}

		protected override void Parse()
		{
			Parsed = false;
			if (cchChar == -1)
			{
				// Find null-terminated length (like wcsnlen_s)
				int maxChars = Parser.GetSize() / 2;
				cchChar = 0;
				for (int i = 0; i < maxChars; i++)
				{
					ushort ch = BitConverter.ToUInt16(Parser.GetAddress(), i * 2);
					if (ch == 0)
					{
						break;
					}
					cchChar++;
				}
				cchChar++; // include null terminator
			}

			if (cchChar > 0 && Parser.CheckSize(2 * cchChar))
			{
				// Read UTF-16LE string
				string raw = Encoding.Unicode.GetString(Parser.GetAddress(), 0, cchChar * 2);
				data = RemoveInvalidCharactersW(raw);
				Parser.Advance(2 * cchChar);
				SetText(data);
				Parsed = true;
			}
		}

		// Utility: Remove invalid characters (placeholder, implement as needed)
		private static string RemoveInvalidCharactersW(string input)
		{
			// Implement actual logic as needed
			return input;
		}

		public static BlockStringW EmptySW() => new BlockStringW();
	}
}

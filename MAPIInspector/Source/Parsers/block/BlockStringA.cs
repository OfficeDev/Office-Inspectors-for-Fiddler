using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAPIInspector.Parsers.block
{
	internal class BlockStringA : Block
	{
		private string _data = string.Empty;
		private int _cchChar = -1;

		private BlockStringA() { }

		// Mimic std::string conversion
		public override string ToString() => _data;
		public string Data => _data;
		public int Length => _data.Length;
		public bool IsEmpty => string.IsNullOrEmpty(_data);

		public static BlockStringA Parse(BinaryParser parser, int cchChar = -1)
		{
			var block = new BlockStringA();
			block._cchChar = cchChar;
			block.ParseInternal(parser);
			return block;
		}

		private void ParseInternal(BinaryParser parser)
		{
			if (_cchChar == -1)
			{
				// Find null-terminator in ANSI bytes
				int maxLen = parser.Remaining;
				int len = 0;
				while (len < maxLen && parser.PeekByte(len) != 0)
					len++;
				_cchChar = len + 1; // include null terminator
			}

			if (_cchChar > 0 && parser.Remaining >= _cchChar)
			{
				var bytes = parser.ReadBytes(_cchChar);
				// Remove invalid characters and null terminator
				int strLen = Array.IndexOf(bytes, (byte)0);
				if (strLen < 0) strLen = bytes.Length;
				_data = RemoveInvalidCharactersA(Encoding.ASCII.GetString(bytes, 0, strLen));
				SetText(_data);
			}
		}

		// Utility: Remove invalid ASCII characters
		private static string RemoveInvalidCharactersA(string input)
		{
			var sb = new StringBuilder(input.Length);
			foreach (char c in input)
			{
				if (c >= 32 && c <= 126) sb.Append(c);
			}
			return sb.ToString();
		}

		// Factory for empty BlockStringA
		public static BlockStringA Empty() => new BlockStringA();
	}
}

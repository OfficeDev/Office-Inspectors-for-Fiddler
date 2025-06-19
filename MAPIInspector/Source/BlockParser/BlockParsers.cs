
using System;
using System.IO;

namespace BlockParser
{
    public partial class Block
    {
        /// <summary>
        /// Static parse function returns a parsing block based on a stream at it's current position
        /// Advance the stream by the size of the block after parsing
        /// </summary>
        /// <typeparam name="T">Type of object inheriting from Block to be parsed</typeparam>
        /// <param name="stream">Stream to parse from</param>
        /// <param name="enableJunk">Indicates whether to enable junk data parsing</param>
        /// <returns>Parsed block of type T</returns>
        public static T Parse<T>(Stream stream, bool enableJunk = false) where T : Block, new()
        {
            var block = Parse<T>(new BinaryParser(stream, stream.Position), enableJunk);
            stream.Seek(block.Size, SeekOrigin.Current);
            return block;
        }

        /// <summary>
        /// Static parse function returns a parsing block based on a BinaryParser
        /// </summary>
        /// <typeparam name="T">Type of object inheriting from Block to be parsed</typeparam>
        /// <param name="parser">BinaryParser to parse from</param>
        /// <param name="enableJunk">Indicates whether to enable junk data parsing</param>
        /// <returns>Parsed block of type T</returns>
        public static T Parse<T>(BinaryParser parser, bool enableJunk = false) where T : Block, new()
        {
            return Parse<T>(parser, 0, enableJunk);
        }

        /// <summary>
        /// Static parse function returns a parsing block based on a BinaryParser
        /// </summary>
        /// <typeparam name="T">Type of object inheriting from Block to be parsed</typeparam>
        /// <param name="parser">BinaryParser to parse from</param>
        /// <param name="cbBin">Size of the binary data to parse</param>
        /// <param name="enableJunk">Indicates whether to enable junk data parsing</param>
        /// <returns>Parsed block of type T</returns>
        public static T Parse<T>(BinaryParser parser, int cbBin, bool enableJunk = false) where T : Block, new()
        {
            var ret = new T();
            ret.Parse(parser, cbBin, enableJunk);
            return ret;
        }

        /// <summary>
        /// Parses binary data using the specified parser and returns a <see cref="BlockT{T}"/> instance containing the
        /// parsed data.
        /// </summary>
        /// <typeparam name="T">The type of the data to parse. Must be a value type (<see langword="struct"/>).</typeparam>
        /// <param name="parser">The <see cref="BinaryParser"/> instance used to parse the binary data.</param>
        /// <returns>A <see cref="BlockT{T}"/> instance containing the parsed data.</returns>
        public static BlockT<T> ParseT<T>(BinaryParser parser) where T : struct
        {
            var ret = new BlockT<T>
            {
                parser = parser
            };
            ret.EnsureParsed();
            return ret;
        }

        /// <summary>
        /// Parses binary data of type <typeparamref name="U"/> from the provided <see cref="BinaryParser"/> and converts it into 
        /// a block of type <typeparamref name="T"/>.
        /// </summary>
        /// <remarks>This method reads binary data as type <typeparamref name="U"/> and converts it to
        /// type <typeparamref name="T"/>. If <typeparamref name="U"/> is an enum, its underlying type is used for size
        /// validation. The method ensures that the binary data size is sufficient before attempting to parse.</remarks>
        /// <typeparam name="U">The source data type to read from the binary stream. Must be a value type and can be an enum.</typeparam>
        /// <typeparam name="T">The target data type to convert the parsed binary data into. Must be a value type.</typeparam>
        /// <param name="parser">The <see cref="BinaryParser"/> instance used to read binary data.</param>
        /// <returns>A <see cref="BlockT{T}"/> containing the parsed and converted data of type <typeparamref name="T"/>. Returns
        /// an empty block if the binary data size is insufficient.</returns>
        public static BlockT<T> ParseAs<U, T>(BinaryParser parser) where U : struct where T : struct
        {
            Type type = typeof(U);
            if (type.IsEnum)
                type = Enum.GetUnderlyingType(type);
            if (!parser.CheckSize(System.Runtime.InteropServices.Marshal.SizeOf(type)))
                return new BlockT<T>();

            U uData = BlockT<U>.ReadStruct<U>(parser);
            int offset = parser.Offset;
            return CreateBlock((T)Convert.ChangeType(uData, typeof(T)), System.Runtime.InteropServices.Marshal.SizeOf(type), offset);
        }

        /// <summary>
        /// Read a block off our stream, but doesn't advance the stream position.
        /// </summary>
        /// <param name="parser"></param>
        /// <returns>A <see cref="BlockT{T}"/> instance containing the parsed data.</returns>
        public static BlockT<T> TestParse<T>(BinaryParser parser) where T : struct
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

        public BlockBytes ParseBytes(int cbBytes, int cbMaxBytes = -1) => ParseBytes(parser, cbBytes, cbMaxBytes);
        /// <summary>
        /// Parses a specified number of bytes from the binary parser and returns a <see cref="BlockBytes"/> instance.
        /// </summary>
        /// <param name="cbBytes">The number of bytes to parse.</param>
        /// <param name="cbMaxBytes">The maximum number of bytes to parse. If set to -1, no maximum is enforced.</param>
        /// <returns>A <see cref="BlockBytes"/> instance containing the parsed bytes.</returns>
        public static BlockBytes ParseBytes(BinaryParser parser, int cbBytes, int cbMaxBytes = -1)
        {
            var ret = new BlockBytes
            {
                parser = parser,
                EnableJunk = false,
                cbBytes = cbBytes,
                cbMaxBytes = cbMaxBytes
            };
            ret.EnsureParsed();
            return ret;
        }

        public BlockStringW ParseStringW(int cchChar = -1) => ParseStringW(parser, cchChar);
        /// <summary>
        /// Parses a Unicode (wide) string from the binary parser and returns a <see cref="BlockStringW"/> instance.
        /// </summary>
        /// <param name="parser">The <see cref="BinaryParser"/> instance used to parse the string.</param>
        /// <param name="cchChar">The number of characters to parse. If set to -1, parses until a null terminator or end of data.</param>
        /// <returns>A <see cref="BlockStringW"/> instance containing the parsed string.</returns>
        public static BlockStringW ParseStringW(BinaryParser parser, int cchChar = -1)
        {
            var ret = new BlockStringW
            {
                parser = parser,
                EnableJunk = false,
                cchChar = cchChar
            };
            ret.EnsureParsed();
            return ret;
        }

        public BlockStringA ParseStringA(int cchChar = -1) => ParseStringA(parser, cchChar);
        /// <summary>
        /// Parses an ANSI (narrow) string from the binary parser and returns a <see cref="BlockStringA"/> instance.
        /// </summary>
        /// <param name="parser">The <see cref="BinaryParser"/> instance used to parse the string.</param>
        /// <param name="cchChar">The number of characters to parse. If set to -1, parses until a null terminator or end of data.</param>
        /// <returns>A <see cref="BlockStringA"/> instance containing the parsed string.</returns>
        public static BlockStringA ParseStringA(BinaryParser parser, int cchChar = -1)
        {
            var ret = new BlockStringA
            {
                parser = parser,
                EnableJunk = false,
                cchChar = cchChar
            };
            ret.EnsureParsed();
            return ret;
        }
    }
}

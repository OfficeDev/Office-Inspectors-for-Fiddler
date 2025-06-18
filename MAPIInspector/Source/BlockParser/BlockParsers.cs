
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

        public static BlockT<T> ParseT<T>(BinaryParser parser) where T : struct
        {
            var ret = new BlockT<T>
            {
                parser = parser
            };
            ret.EnsureParsed();
            return ret;
        }

        // Build and return object of type T, reading from type U
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
    }
}

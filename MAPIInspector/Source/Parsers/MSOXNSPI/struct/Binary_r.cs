using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.3.1 Property Values
    /// 2.3.1.3 Binary_r
    /// A class indicates the Binary_r structure.
    /// </summary>
    public class Binary_r : BaseStructure
    {
        /// <summary>
        /// A variable value // TODO: Verify whether there is HasValue here
        /// </summary>
        public byte? HasValue;

        /// <summary>
        /// The number of uninterpreted bytes represented in structure. value MUST NOT exceed 2,097,152.
        /// </summary>
        public uint Cb;

        /// <summary>
        /// The uninterpreted bytes.
        /// </summary>
        public byte[] Lpb;

        /// <summary>
        /// Parse the Binary_r payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            byte temp = ReadByte();
            if (temp == 0xFF)
            {
                HasValue = temp;
            }
            else
            {
                s.Position -= 1;
            }

            Cb = ReadUint();
            Lpb = ReadBytes((int)Cb);
        }
    }
}

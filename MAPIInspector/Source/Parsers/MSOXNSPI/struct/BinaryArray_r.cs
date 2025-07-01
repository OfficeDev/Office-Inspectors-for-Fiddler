using System.Collections.Generic;
using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.3.1 Property Values
    /// 2.3.1.7 BinaryArray_r
    /// A class indicates the BinaryArray_r structure.
    /// </summary>
    public class BinaryArray_r : BaseStructure
    {
        /// <summary>
        /// A variable value
        /// </summary>
        public byte? HasValue;

        /// <summary>
        /// The number of Binary_r data structures represented in the BinaryArray_r structure. value MUST NOT exceed 100,000.
        /// </summary>
        public uint CValues;

        /// <summary>
        /// The Binary_r data structures.
        /// </summary>
        public Binary_r[] Lpbin;

        /// <summary>
        /// Parse the BinaryArray_r payload of session.
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

            CValues = ReadUint();
            List<Binary_r> temBytes = new List<Binary_r>();
            for (ulong i = 0; i < CValues; i++)
            {
                Binary_r br = new Binary_r();
                br.Parse(s);
                temBytes.Add(br);
            }

            Lpbin = temBytes.ToArray();
        }
    }
}

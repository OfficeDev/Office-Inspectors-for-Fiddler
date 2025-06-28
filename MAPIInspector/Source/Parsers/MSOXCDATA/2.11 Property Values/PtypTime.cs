using BlockParser;
using System;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.11.1 Property Data Types
    /// 8 bytes; a 64-bit integer representing the number of 100-nanosecond intervals since January 1, 1601.[MS-DTYP]: FILETIME.
    /// </summary>
    public class PtypTime : Block
    {
        /// <summary>
        /// 64-bit integer representing the number of 100-nanosecond intervals since January 1, 1601.[MS-DTYP]: FILETIME.
        /// </summary>
        private BlockT<ulong> Value;

        /// <summary>
        /// Parse the PtypTime structure.
        /// </summary>
        protected override void Parse()
        {
            Value = ParseT<ulong>();
        }

        protected override void ParseBlocks()
        {
            try
            {
                Text = new DateTime(1601, 1, 1).AddMilliseconds(Value / 10000).ToLocalTime().ToString();
            }
            catch
            {
                Text = $"{Value.Data:X}";
            }
        }
    }
}

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
        private DateTime dateTime;

        /// <summary>
        /// Parse the PtypTime structure.
        /// </summary>
        protected override void Parse()
        {
            Value = ParseT<ulong>();
            try
            {
                dateTime = new DateTime(1601, 1, 1).AddMilliseconds(Value.Data / 10000).ToLocalTime();
            }
            catch
            {
                dateTime = new DateTime();
            }
        }

        protected override void ParseBlocks()
        {
            Text = dateTime.ToString();
        }
    }
}

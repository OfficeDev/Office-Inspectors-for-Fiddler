namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System;

    /// <summary>
    /// 2.11.1 Property Data Types
    /// 16 bytes; a GUID with Data1, Data2, and Data3 fields in little-endian format.[MS-DTYP]: GUID.
    /// </summary>
    public class PtypGuid : Block
    {
        /// <summary>
        /// A GUID value.
        /// </summary>
        public BlockT<Guid> Value;

        /// <summary>
        /// Parse the PtypGuid structure.
        /// </summary>
        protected override void Parse()
        {
            Value = ParseT<Guid>();
        }

        protected override void ParseBlocks()
        {
            Text = $"{Value.Data}";
        }
    }
}

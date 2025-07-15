using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    // There is one variation in the width of count fields.In the context of ROP buffers,
    // such as the RopGetPropertiesSpecific ROP([MS-OXCROPS] section 2.2.8.3), byte counts
    // for PtypBinary property values are 16 bits wide and value counts for all PtypMultiple
    // property values are 32 bits wide.However, in the context of extended rules, as
    // specified in [MS - OXORULE] section 2.2.4, and in the context of the MAPI extensions
    // for HTTP, as specified in [MS - OXCMAPIHTTP] section 2.2.5, byte counts for PtypBinary
    // property values and value counts for PtypMultiple property values are 32 bits wide.
    // Such count fields have a width designation of COUNT, as specified in section 2.11.1.1,
    // rather than an explicit width, as throughout section 2.11.

    /// <summary>
    /// 2.11.1 Property Data Types
    /// Variable size; a COUNT field followed by that many PtypString values.
    /// </summary>
    public class PtypMultipleString : Block
    {
        /// <summary>
        /// Count values are typically used to specify the size of an associated field.
        /// </summary>
        public BlockT<uint> Count;

        /// <summary>
        /// The array of string value.
        /// </summary>
        public PtypString[] Value;

        /// <summary>
        /// Parse the PtypMultipleString structure.
        /// </summary>
        protected override void Parse()
        {
            Count = ParseT<uint>();

            var tempvalue = new List<PtypString>();
            for (int i = 0; i < Count; i++)
            {
                tempvalue.Add(Parse<PtypString>());
            }

            Value = tempvalue.ToArray();
        }

        protected override void ParseBlocks()
        {
            AddChildBlockT(Count, "Count");
            AddLabeledChildren(Value, "Value");
        }
    }
}

using BlockParser;
using System.Collections.Generic;
using System.Text;

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
    public class PtypMultipleString_AddressBook : Block
    {
        /// <summary>
        /// COUNT values are typically used to specify the size of an associated field.
        /// </summary>
        public BlockT<uint> Count;

        /// <summary>
        /// The array of string value.
        /// </summary>
        public MAPIStringAddressBook[] Value;

        /// <summary>
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// Initializes a new instance of the PtypMultipleString_AddressBook class
        /// </summary>
        /// <param name="wide">The Count wide size of PtypMultipleString type.</param>
        public PtypMultipleString_AddressBook(CountWideEnum wide)
        {
            countWide = wide;
        }

        /// <summary>
        /// Parse the PtypMultipleString_AddressBook structure.
        /// </summary>
        protected override void Parse()
        {
            switch (countWide)
            {
                case CountWideEnum.twoBytes:
                    Count = ParseAs<ushort,uint>();
                    break;
                default:
                case CountWideEnum.fourBytes:
                    Count = ParseT<uint>();
                    break;
            }
            var tempvalue = new List<MAPIStringAddressBook>();
            for (int i = 0; i < Count; i++)
            {
                var str = new MAPIStringAddressBook(Encoding.Unicode);
                str.Parse(parser);
                tempvalue.Add(str);
            }

            Value = tempvalue.ToArray();
        }

        protected override void ParseBlocks()
        {
            SetText("PtypMultipleString_AddressBook");
            AddChild(Count, $"Count:{Count}");
            AddLabeledChildren(Value, "Value");
        }
    }
}

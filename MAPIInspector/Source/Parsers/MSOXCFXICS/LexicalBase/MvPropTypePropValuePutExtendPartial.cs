using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// multi-valued property type PropValue
    /// </summary>
    public class MvPropTypePropValuePutExtendPartial : PropValue
    {
        /// <summary>
        /// This represent the length variable.
        /// </summary>
        public BlockT<int> Length;

        Block[] ValueArray;

        /// <summary>
        /// Length for partial
        /// </summary>
        private int Plength;

        protected override void Parse()
        {
            base.Parse();

            if (parser.Empty)
            {
                Partial.PartialPutExtendType = PropType;
                Partial.PartialPutExtendServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                Partial.PartialPutExtendProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                Partial.PartialPutExtendClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
            }
            else
            {
                if (Partial.PartialPutExtendType != 0 &&
                    Partial.PartialPutExtendServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath &&
                    Partial.PartialPutExtendProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess &&
                    Partial.PartialPutExtendClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                {
                    ptype = CreateBlock(Partial.PartialPutExtendType, 0, 0);

                    if (Partial.PartialPutExtendRemainSize != -1)
                    {
                        Plength = Partial.PartialPutExtendRemainSize;
                        Partial.PartialPutExtendRemainSize = -1;
                    }
                    else
                    {
                        Length = ParseT<int>();
                    }

                    // clear
                    Partial.PartialPutExtendType = 0;
                    Partial.PartialPutExtendId = 0;

                    if (Partial.PartialPutExtendRemainSize == -1 && Partial.PartialPutExtendSubRemainSize == -1)
                    {
                        Partial.PartialPutExtendServerUrl = string.Empty;
                        Partial.PartialPutExtendProcessName = string.Empty;
                        Partial.PartialPutExtendClientInfo = string.Empty;
                    }
                }
                else
                {
                    Length = ParseT<int>();
                }

                int blocksCount = Length != null ? Length : Plength;
                PropertyDataType typeValue = PropertyDataType.PtypUnspecified;
                if (PropType != null)
                {
                    typeValue = PropType;
                }
                else if (ptype != null)
                {
                    typeValue = ptype;
                }

                ValueArray = MvPropTypePropValue.ParseArray(parser, PropType, blocksCount);
            }
        }

        protected override void ParseBlocks()
        {
            base.ParseBlocks();
            AddChildBlockT(Length, "Length");
            AddLabeledChildren(ValueArray, "ValueArray");
        }
    }
}

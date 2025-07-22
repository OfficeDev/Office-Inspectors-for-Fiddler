using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// multi-valued property type PropValue
    /// </summary>
    public class MvPropTypePropValuePutPartial : PropValue
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

        private Block Comment;

        protected override void Parse()
        {
            base.Parse();

            if (parser.Empty)
            {
                Partial.PartialPutType = PropType;
                Partial.PartialPutServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                Partial.PartialPutProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                Partial.PartialPutClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
            }
            else
            {
                if (Partial.PartialPutType != 0 &&
                    Partial.PartialPutServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath &&
                    Partial.PartialPutProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess &&
                    Partial.PartialPutClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                {
                    Comment = Partial.CreatePartialComment();

                    ptype = CreateBlock(Partial.PartialPutType, 0, 0);

                    if (Partial.PartialPutRemainSize != -1)
                    {
                        Plength = Partial.PartialPutRemainSize;
                        Partial.PartialPutRemainSize = -1;
                    }
                    else
                    {
                        Length = ParseT<int>();
                    }

                    // clear
                    Partial.PartialPutType = 0;

                    if (Partial.PartialPutRemainSize == -1 && Partial.PartialPutSubRemainSize == -1)
                    {
                        Partial.PartialPutServerUrl = string.Empty;
                        Partial.PartialPutProcessName = string.Empty;
                        Partial.PartialPutClientInfo = string.Empty;
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
            AddChild(Comment);
            Text = "MvPropTypePropValuePutPartial";
            AddChildBlockT(Length, "Length");
            AddLabeledChildren(ValueArray, "ValueArray");
        }
    }
}

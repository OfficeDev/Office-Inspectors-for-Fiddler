using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// multi-valued property type PropValue
    /// </summary>
    public class MvPropTypePropValueGetPartial : PropValue
    {
        /// <summary>
        /// This represent the length variable.
        /// </summary>
        public BlockT<int> Length;

        Block[] ValueArray;

        /// <summary>
        /// Length value for partial split
        /// </summary>
        private int Plength;

        private Block Comment;

        protected override void Parse()
        {
            base.Parse();

            if (parser.Empty)
            {
                Partial.PartialGetType = PropType;
                Partial.PartialGetServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                Partial.PartialGetProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                Partial.PartialGetClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
            }
            else
            {
                if (Partial.PartialGetType != 0 && Partial.PartialGetServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath && Partial.PartialGetProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess
                    && Partial.PartialGetClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                {
                    Comment = Partial.CreatePartialComment();

                    ptype = CreateBlock(Partial.PartialGetType, 0, 0);

                    if (Partial.PartialGetRemainSize != -1)
                    {
                        Plength = Partial.PartialGetRemainSize;
                        Partial.PartialGetRemainSize = -1;
                    }
                    else
                    {
                        Length = ParseT<int>();
                    }

                    // clear
                    Partial.PartialGetType = 0;
                    if (Partial.PartialGetRemainSize == -1 && Partial.PartialGetSubRemainSize == -1)
                    {
                        Partial.PartialGetServerUrl = string.Empty;
                        Partial.PartialGetProcessName = string.Empty;
                        Partial.PartialGetClientInfo = string.Empty;
                    }
                }
                else
                {
                    Length = ParseT<int>();
                }

                PropertyDataType typeValue = PropType.Parsed ? PropType : ptype;
                int countValue = Length.Parsed ? Length : Plength;

                ValueArray = MvPropTypePropValue.ParseArray(parser, typeValue, countValue);
            }
        }

        protected override void ParseBlocks()
        {
            base.ParseBlocks();
            AddChild(Comment);
            AddChildBlockT(Length, "Length");
            AddLabeledChildren(ValueArray, "ValueArray");
        }
    }
}

using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Represent a fixedPropType PropValue.
    /// </summary>
    public class FixedPropTypePropValuePutPartial : PropValue
    {
        /// <summary>
        /// A fixed value.
        /// </summary>
        public Block FixedValue;

        private Block Comment;

        protected override void Parse()
        {
            base.Parse();

            if (parser.Empty)
            {
                Partial.PartialPutType = PropType;
                Partial.PartialPutId = PropInfo.PropID;
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
                    pid = CreateBlock(Partial.PartialPutId, 0, 0);

                    // clear
                    Partial.PartialPutType = 0;
                    Partial.PartialPutId = 0;
                    Partial.PartialPutServerUrl = string.Empty;
                    Partial.PartialPutProcessName = string.Empty;
                    Partial.PartialPutClientInfo = string.Empty;
                }

                PropertyDataType typeValue = PropertyDataType.PtypUnspecified;
                if (PropType != null)
                {
                    typeValue = PropType;
                }
                else if (ptype != null)
                {
                    typeValue = ptype;
                }

                PidTagPropertyEnum identifyValue = PropInfo != null ? PropInfo.PropID : pid;

                FixedValue = FixedPropTypePropValue.ParseFixedProp(parser, typeValue, identifyValue);
            }
        }

        protected override void ParseBlocks()
        {
            base.ParseBlocks();
            AddChild(Comment);
            SetText("FixedPropTypePropValuePutPartial");
            AddChild(FixedValue, $"FixedValue:{FixedValue}");
        }
    }
}

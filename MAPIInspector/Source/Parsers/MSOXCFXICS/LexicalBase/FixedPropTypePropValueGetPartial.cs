using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Represent a fixedPropType PropValue.
    /// </summary>
    public class FixedPropTypePropValueGetPartial : PropValue
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
                Partial.PartialGetType = PropType;
                Partial.PartialGetId = PropInfo.PropID;
                Partial.PartialGetServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                Partial.PartialGetProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                Partial.PartialGetClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
            }
            else
            {
                if (Partial.PartialGetType != 0 &&
                    Partial.PartialGetServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath &&
                    Partial.PartialGetProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess &&
                    Partial.PartialGetClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                {
                    Comment = Partial.CreatePartialComment();

                    ptype = CreateBlock(Partial.PartialGetType, 0, 0);
                    pid = CreateBlock(Partial.PartialGetId, 0, 0);

                    // clear
                    Partial.PartialGetType = 0;
                    Partial.PartialGetId = 0;
                    Partial.PartialGetServerUrl = string.Empty;
                    Partial.PartialGetProcessName = string.Empty;
                    Partial.PartialGetClientInfo = string.Empty;
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
            AddChild(FixedValue, $"FixedValue:{FixedValue}");
        }
    }
}

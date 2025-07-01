using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Represent a fixedPropType PropValue.
    /// </summary>
    public class FixedPropTypePropValuePutExtendPartial : PropValue
    {
        /// <summary>
        /// A fixed value.
        /// </summary>
        public Block FixedValue;

        protected override void Parse()
        {
            base.Parse();

            if (parser.Empty)
            {
                Partial.PartialPutExtendType = PropType;
                Partial.PartialPutExtendId = PropInfo.PropID;
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
                    pid = CreateBlock(Partial.PartialPutExtendId, 0, 0);

                    // clear
                    Partial.PartialPutExtendType = 0;
                    Partial.PartialPutExtendId = 0;
                    Partial.PartialPutExtendServerUrl = string.Empty;
                    Partial.PartialPutExtendProcessName = string.Empty;
                    Partial.PartialPutExtendClientInfo = string.Empty;
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
            AddChild(FixedValue, $"FixedValue:{FixedValue}");
        }
    }
}

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

        protected override void Parse()
        {
            base.Parse();

            if (parser.Empty)
            {
                MapiInspector.MAPIParser.PartialPutType = PropType;
                MapiInspector.MAPIParser.PartialPutId = PropInfo.PropID;
                MapiInspector.MAPIParser.PartialPutServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                MapiInspector.MAPIParser.PartialPutProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                MapiInspector.MAPIParser.PartialPutClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
            }
            else
            {
                if (MapiInspector.MAPIParser.PartialPutType != 0 && MapiInspector.MAPIParser.PartialPutServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIParser.PartialPutProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess
                    && MapiInspector.MAPIParser.PartialPutClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                {
                    ptype = CreateBlock(MapiInspector.MAPIParser.PartialPutType, 0, 0);
                    pid = CreateBlock(MapiInspector.MAPIParser.PartialPutId, 0, 0);

                    // clear
                    MapiInspector.MAPIParser.PartialPutType = 0;
                    MapiInspector.MAPIParser.PartialPutId = 0;
                    MapiInspector.MAPIParser.PartialPutServerUrl = string.Empty;
                    MapiInspector.MAPIParser.PartialPutProcessName = string.Empty;
                    MapiInspector.MAPIParser.PartialPutClientInfo = string.Empty;
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
            if (FixedValue != null) AddChild(FixedValue, $"FixedValue:{FixedValue}");
        }
    }
}

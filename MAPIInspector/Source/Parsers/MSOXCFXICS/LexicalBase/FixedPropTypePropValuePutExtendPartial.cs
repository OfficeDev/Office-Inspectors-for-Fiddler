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
                MapiInspector.MAPIParser.PartialPutExtendType = PropType.Data;
                MapiInspector.MAPIParser.PartialPutExtendId = PropInfo.PropID.Data;
                MapiInspector.MAPIParser.PartialPutExtendServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                MapiInspector.MAPIParser.PartialPutExtendProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                MapiInspector.MAPIParser.PartialPutExtendClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
            }
            else
            {
                if (MapiInspector.MAPIParser.PartialPutExtendType != 0 && MapiInspector.MAPIParser.PartialPutExtendServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIParser.PartialPutExtendProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess
                    && MapiInspector.MAPIParser.PartialPutExtendClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                {
                    ptype = CreateBlock(MapiInspector.MAPIParser.PartialPutExtendType, 0, 0);
                    pid = CreateBlock(MapiInspector.MAPIParser.PartialPutExtendId, 0, 0);

                    // clear
                    MapiInspector.MAPIParser.PartialPutExtendType = 0;
                    MapiInspector.MAPIParser.PartialPutExtendId = 0;
                    MapiInspector.MAPIParser.PartialPutExtendServerUrl = string.Empty;
                    MapiInspector.MAPIParser.PartialPutExtendProcessName = string.Empty;
                    MapiInspector.MAPIParser.PartialPutExtendClientInfo = string.Empty;
                }

                PropertyDataType typeValue = PropertyDataType.PtypUnspecified;
                if (PropType != null)
                {
                    typeValue = PropType.Data;
                }
                else if (ptype != null)
                {
                    typeValue = ptype.Data;
                }

                PidTagPropertyEnum identifyValue = PropInfo != null ? PropInfo.PropID.Data : pid.Data;

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

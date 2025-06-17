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

        protected override void Parse()
        {
            base.Parse();
            if (parser.Empty)
            {
                MapiInspector.MAPIParser.PartialGetType = PropType.Data;
                MapiInspector.MAPIParser.PartialGetId = PropInfo.PropID.Data;
                MapiInspector.MAPIParser.PartialGetServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                MapiInspector.MAPIParser.PartialGetProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                MapiInspector.MAPIParser.PartialGetClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
            }
            else
            {
                if (MapiInspector.MAPIParser.PartialGetType != 0 && MapiInspector.MAPIParser.PartialGetServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIParser.PartialGetProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess
                    && MapiInspector.MAPIParser.PartialGetClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                {
                    ptype = BlockT<PropertyDataType>.Create(MapiInspector.MAPIParser.PartialGetType, 0, 0);
                    pid = BlockT<PidTagPropertyEnum>.Create(MapiInspector.MAPIParser.PartialGetId, 0, 0);

                    // clear
                    MapiInspector.MAPIParser.PartialGetType = 0;
                    MapiInspector.MAPIParser.PartialGetId = 0;
                    MapiInspector.MAPIParser.PartialGetServerUrl = string.Empty;
                    MapiInspector.MAPIParser.PartialGetProcessName = string.Empty;
                    MapiInspector.MAPIParser.PartialGetClientInfo = string.Empty;
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

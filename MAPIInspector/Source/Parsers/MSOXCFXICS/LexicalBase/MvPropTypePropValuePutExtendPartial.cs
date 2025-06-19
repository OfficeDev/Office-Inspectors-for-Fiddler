namespace MAPIInspector.Parsers
{
    using BlockParser;

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
                MapiInspector.MAPIParser.PartialPutExtendType = PropType.Data;
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

                    if (MapiInspector.MAPIParser.PartialPutExtendRemainSize != -1)
                    {
                        Plength = MapiInspector.MAPIParser.PartialPutExtendRemainSize;
                        MapiInspector.MAPIParser.PartialPutExtendRemainSize = -1;
                    }
                    else
                    {
                        Length = ParseT<int>();
                    }

                    // clear
                    MapiInspector.MAPIParser.PartialPutExtendType = 0;
                    MapiInspector.MAPIParser.PartialPutExtendId = 0;

                    if (MapiInspector.MAPIParser.PartialPutExtendRemainSize == -1 && MapiInspector.MAPIParser.PartialPutExtendSubRemainSize == -1)
                    {
                        MapiInspector.MAPIParser.PartialPutExtendServerUrl = string.Empty;
                        MapiInspector.MAPIParser.PartialPutExtendProcessName = string.Empty;
                        MapiInspector.MAPIParser.PartialPutExtendClientInfo = string.Empty;
                    }
                }
                else
                {
                    Length = ParseT<int>();
                }

                int blocksLength = Length != null ? Length.Data : Plength;
                PropertyDataType typeValue = PropertyDataType.PtypUnspecified;
                if (PropType != null)
                {
                    typeValue = PropType.Data;
                }
                else if (ptype != null)
                {
                    typeValue = ptype.Data;
                }
                
                ValueArray = MvPropTypePropValue.ParseArray(parser, PropType.Data, blocksLength);
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

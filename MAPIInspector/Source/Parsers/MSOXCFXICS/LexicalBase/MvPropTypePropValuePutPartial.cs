namespace MAPIInspector.Parsers
{
    using BlockParser;

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

        protected override void Parse()
        {
            base.Parse();

            if (parser.Empty)
            {
                MapiInspector.MAPIParser.PartialPutType = PropType.Data;
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

                    if (MapiInspector.MAPIParser.PartialPutRemainSize != -1)
                    {
                        Plength = MapiInspector.MAPIParser.PartialPutRemainSize;
                        MapiInspector.MAPIParser.PartialPutRemainSize = -1;
                    }
                    else
                    {
                        Length = ParseT<int>();
                    }

                    // clear
                    MapiInspector.MAPIParser.PartialPutType = 0;
                    MapiInspector.MAPIParser.PartialPutId = 0;

                    if (MapiInspector.MAPIParser.PartialPutRemainSize == -1 && MapiInspector.MAPIParser.PartialPutSubRemainSize == -1)
                    {
                        MapiInspector.MAPIParser.PartialPutServerUrl = string.Empty;
                        MapiInspector.MAPIParser.PartialPutProcessName = string.Empty;
                        MapiInspector.MAPIParser.PartialPutClientInfo = string.Empty;
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
            SetText("MvPropTypePropValuePutPartial");
            AddChildBlockT(Length, "Length");
            AddLabeledChildren(ValueArray, "ValueArray");
        }
    }
}

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

        protected override void Parse()
        {
            base.Parse();

            if (parser.Empty)
            {
                MapiInspector.MAPIParser.PartialGetType = PropType;
                MapiInspector.MAPIParser.PartialGetServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                MapiInspector.MAPIParser.PartialGetProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                MapiInspector.MAPIParser.PartialGetClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
            }
            else
            {
                if (MapiInspector.MAPIParser.PartialGetType != 0 && MapiInspector.MAPIParser.PartialGetServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIParser.PartialGetProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess
                    && MapiInspector.MAPIParser.PartialGetClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                {
                    ptype = CreateBlock(MapiInspector.MAPIParser.PartialGetType, 0, 0);

                    if (MapiInspector.MAPIParser.PartialGetRemainSize != -1)
                    {
                        Plength = MapiInspector.MAPIParser.PartialGetRemainSize;
                        MapiInspector.MAPIParser.PartialGetRemainSize = -1;
                    }
                    else
                    {
                        Length = ParseT<int>();
                    }

                    // clear
                    MapiInspector.MAPIParser.PartialGetType = 0;
                    if (MapiInspector.MAPIParser.PartialGetRemainSize == -1 && MapiInspector.MAPIParser.PartialGetSubRemainSize == -1)
                    {
                        MapiInspector.MAPIParser.PartialGetServerUrl = string.Empty;
                        MapiInspector.MAPIParser.PartialGetProcessName = string.Empty;
                        MapiInspector.MAPIParser.PartialGetClientInfo = string.Empty;
                    }
                }
                else
                {
                    Length = ParseT<int>();
                }

                PropertyDataType typeValue = PropType.Parsed ? PropType : ptype;
                int lengthValue = Length.Parsed ? Length : Plength;

                ValueArray = MvPropTypePropValue.ParseArray(parser, typeValue, lengthValue);
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

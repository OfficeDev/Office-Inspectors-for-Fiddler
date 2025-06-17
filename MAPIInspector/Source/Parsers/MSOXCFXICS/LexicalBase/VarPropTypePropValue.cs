namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System.Collections.Generic;

    /// <summary>
    /// The VarPropTypePropValue class.
    /// </summary>
    public class VarPropTypePropValue : PropValue
    {
        /// <summary>
        /// The length of a variate type value.
        /// </summary>
        public BlockT<int> Length;

        /// <summary>
        /// The valueArray.
        /// </summary>
        public Block ValueArray;

        /// <summary>
        /// Verify that a stream's current position contains a serialized VarPropTypePropValue.
        /// </summary>
        /// <param name="parser">A BinaryParser.</param>
        /// <returns>If the stream's current position contains a serialized VarPropTypePropValue, return true, else false</returns>
        public static new bool Verify(BinaryParser parser)
        {
            var tmp = BlockT<PropertyDataType>.TestParse(parser);
            if (!tmp.Parsed) return false;
            return LexicalTypeHelper.IsVarType(tmp.Data)
                || IsMetaTagIdsetGiven(parser)
                || LexicalTypeHelper.IsCodePageType(tmp.Data);
        }

        protected override void Parse()
        {
            base.Parse();
            Length = BlockT<int>.Parse(parser);

            if (LexicalTypeHelper.IsCodePageType(PropType.Data))
            {
                var type = (CodePageType)PropType.Data;

                switch (type)
                {
                    case CodePageType.PtypCodePageUnicode:
                        ValueArray = Parse<PtypString>(parser);
                        break;
                    case CodePageType.PtypCodePageUnicodeBigendian:
                    case CodePageType.PtypCodePageWesternEuropean:
                        ValueArray = Parse<PtypString8>(parser);
                        break;
                    default:
                        ValueArray = Parse<PtypString8>(parser);
                        break;
                }
            }
            else
            {
                switch (PropType.Data)
                {
                    case PropertyDataType.PtypInteger32:
                    case PropertyDataType.PtypBinary:
                        if (PropInfo.PropID.Data == PidTagPropertyEnum.PidTagSourceKey ||
                            PropInfo.PropID.Data == PidTagPropertyEnum.PidTagParentSourceKey ||
                            PropInfo.PropID.Data == PidTagPropertyEnum.PidTagChangeKey)
                        {
                            if (Length.Data != 0)
                            {
                                var tmpXID = new XID(Length.Data);
                                tmpXID.Parse(parser);
                                ValueArray = tmpXID;
                            }
                        }
                        else if (PropInfo.PropID.Data == PidTagPropertyEnum.PidTagPredecessorChangeList)
                        {
                            var tmpPredecessorChangeList = new PredecessorChangeList(Length.Data);
                            tmpPredecessorChangeList.Parse(parser);
                            ValueArray = tmpPredecessorChangeList;
                        }
                        else if (
                            (ushort)PropInfo.PropID.Data == 0x402D ||
                            (ushort)PropInfo.PropID.Data == 0x402E ||
                            (ushort)PropInfo.PropID.Data == 0x67E5 ||
                            (ushort)PropInfo.PropID.Data == 0x4021 ||
                            (ushort)PropInfo.PropID.Data == 0x6793)
                        {
                            if (Length.Data != 0)
                            {
                                ValueArray.SetText("IDSET_REPLID list");
                                long EveLength = Length.Data;
                                var InterIDSET_REPLID = new List<IDSET_REPLID>();
                                while (EveLength > 0)
                                {
                                    var tmpIDSET_REPLID = Parse<IDSET_REPLID>(parser);
                                    ValueArray.AddChild(tmpIDSET_REPLID);
                                    InterIDSET_REPLID.Add(tmpIDSET_REPLID);
                                    EveLength -= tmpIDSET_REPLID.Size;
                                }
                            }
                        }
                        else if (
                            (ushort)PropInfo.PropID.Data == 0x4017 ||
                            (ushort)PropInfo.PropID.Data == 0x6796 ||
                            (ushort)PropInfo.PropID.Data == 0x67DA ||
                            (ushort)PropInfo.PropID.Data == 0x67D2)

                        {
                            if (Length.Data != 0)
                            {
                                ValueArray.SetText("IDSET_REPLGUID list");
                                long EveLength = Length.Data;
                                var InterIDSET_REPLGUID = new List<IDSET_REPLGUID>();
                                while (EveLength > 0)
                                {
                                    var tmpIDSET_REPLGUID = Parse<IDSET_REPLGUID>(parser);
                                    ValueArray.AddChild(tmpIDSET_REPLGUID);
                                    EveLength -= tmpIDSET_REPLGUID.Size;
                                }
                            }
                        }
                        else
                        {
                            ValueArray = BlockBytes.Parse(parser, Length.Data);
                        }

                        break;
                    case PropertyDataType.PtypString:
                        ValueArray = Parse<PtypString>(parser);
                        break;
                    case PropertyDataType.PtypString8:
                        ValueArray = Parse<PtypString8>(parser);
                        break;
                    case PropertyDataType.PtypServerId:
                        ValueArray = Parse<PtypServerId>(parser);
                        break;
                    case PropertyDataType.PtypObject_Or_PtypEmbeddedTable:
                        ValueArray = Parse<PtypObject_Or_PtypEmbeddedTable>(parser);
                        break;
                    default:
                        ValueArray = BlockBytes.Parse(parser, Length.Data);
                        break;
                }
            }
        }

        protected override void ParseBlocks()
        {
            base.ParseBlocks();
            SetText("VarPropTypePropValue");
            if (Length != null) AddChild(Length, $"Length: {Length.Data})");
            AddChild(ValueArray);
        }
    }
}
using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
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
            var tmp = TestParse<PropertyDataType>(parser);
            if (!tmp.Parsed) return false;
            return LexicalTypeHelper.IsVarType(tmp)
                || IsMetaTagIdsetGiven(parser)
                || LexicalTypeHelper.IsCodePageType(tmp);
        }

        protected override void Parse()
        {
            base.Parse();
            Length = ParseT<int>();

            if (LexicalTypeHelper.IsCodePageType(PropType))
            {
                var type = (CodePageType)PropType.Data;

                switch (type)
                {
                    case CodePageType.PtypCodePageUnicode:
                        ValueArray = Parse<PtypString>();
                        break;
                    case CodePageType.PtypCodePageUnicodeBigendian:
                    case CodePageType.PtypCodePageWesternEuropean:
                        ValueArray = Parse<PtypString8>();
                        break;
                    default:
                        ValueArray = Parse<PtypString8>();
                        break;
                }
            }
            else
            {
                switch (PropType.Data)
                {
                    case PropertyDataType.PtypInteger32:
                    case PropertyDataType.PtypBinary:
                        if (PropInfo.PropID == PidTagPropertyEnum.PidTagSourceKey ||
                            PropInfo.PropID == PidTagPropertyEnum.PidTagParentSourceKey ||
                            PropInfo.PropID == PidTagPropertyEnum.PidTagChangeKey)
                        {
                            if (Length != 0)
                            {
                                var tmpXID = new XID(Length);
                                tmpXID.Parse(parser);
                                ValueArray = tmpXID;
                            }
                        }
                        else if (PropInfo.PropID == PidTagPropertyEnum.PidTagPredecessorChangeList)
                        {
                            var tmpPredecessorChangeList = new PredecessorChangeList(Length);
                            tmpPredecessorChangeList.Parse(parser);
                            ValueArray = tmpPredecessorChangeList;
                        }
                        else if (
                            PropInfo.PropID == PidTagPropertyEnum.MetaTagIdsetRead ||
                            PropInfo.PropID == PidTagPropertyEnum.MetaTagIdsetUnread ||
                            PropInfo.PropID == PidTagPropertyEnum.MetaTagIdsetDeleted ||
                            PropInfo.PropID == PidTagPropertyEnum.MetaTagIdsetNoLongerInScope ||
                            PropInfo.PropID == PidTagPropertyEnum.MetaTagIdsetExpired)
                        {
                            if (Length != 0)
                            {
                                ValueArray.SetText("IDSET_REPLID list");
                                long EveLength = Length;
                                var InterIDSET_REPLID = new List<IDSET_REPLID>();
                                while (EveLength > 0)
                                {
                                    var tmpIDSET_REPLID = Parse<IDSET_REPLID>();
                                    ValueArray.AddChild(tmpIDSET_REPLID);
                                    InterIDSET_REPLID.Add(tmpIDSET_REPLID);
                                    EveLength -= tmpIDSET_REPLID.Size;
                                }
                            }
                        }
                        else if (
                            PropInfo.PropID == PidTagPropertyEnum.MetaTagIdsetGiven ||
                            PropInfo.PropID == PidTagPropertyEnum.MetaTagCnsetSeen ||
                            PropInfo.PropID == PidTagPropertyEnum.MetaTagCnsetSeenFAI ||
                            PropInfo.PropID == PidTagPropertyEnum.MetaTagCnsetRead)

                        {
                            if (Length != 0)
                            {
                                ValueArray.SetText("IDSET_REPLGUID list");
                                long EveLength = Length;
                                var InterIDSET_REPLGUID = new List<IDSET_REPLGUID>();
                                while (EveLength > 0)
                                {
                                    var tmpIDSET_REPLGUID = Parse<IDSET_REPLGUID>();
                                    ValueArray.AddChild(tmpIDSET_REPLGUID);
                                    EveLength -= tmpIDSET_REPLGUID.Size;
                                }
                            }
                        }
                        else
                        {
                            ValueArray = ParseBytes(Length);
                        }

                        break;
                    case PropertyDataType.PtypString:
                        ValueArray = Parse<PtypString>();
                        break;
                    case PropertyDataType.PtypString8:
                        ValueArray = Parse<PtypString8>();
                        break;
                    case PropertyDataType.PtypServerId:
                        ValueArray = Parse<PtypServerId>();
                        break;
                    case PropertyDataType.PtypObject_Or_PtypEmbeddedTable:
                        ValueArray = Parse<PtypObject_Or_PtypEmbeddedTable>();
                        break;
                    default:
                        ValueArray = ParseBytes(Length);
                        break;
                }
            }
        }

        protected override void ParseBlocks()
        {
            base.ParseBlocks();
            SetText("VarPropTypePropValue");
            AddChildBlockT(Length, "Length");
            AddChild(ValueArray);
        }
    }
}
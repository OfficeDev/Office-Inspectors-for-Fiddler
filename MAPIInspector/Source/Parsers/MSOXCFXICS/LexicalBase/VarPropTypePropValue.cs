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
        public int Length;

        /// <summary>
        /// The valueArray.
        /// </summary>
        public object ValueArray;

        /// <summary>
        /// Initializes a new instance of the VarPropTypePropValue class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public VarPropTypePropValue(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized VarPropTypePropValue.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized VarPropTypePropValue, return true, else false</returns>
        public static new bool Verify(FastTransferStream stream)
        {
            ushort tmp = stream.VerifyUInt16();
            return LexicalTypeHelper.IsVarType((PropertyDataType)tmp)
                || PropValue.IsMetaTagIdsetGiven(stream)
                || LexicalTypeHelper.IsCodePageType(tmp);
        }

        /// <summary>
        /// Parse a VarPropTypePropValue instance from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>A VarPropTypePropValue instance.</returns>
        public static new LexicalBase ParseFrom(FastTransferStream stream)
        {
            return new VarPropTypePropValue(stream);
        }

        /// <summary>
        /// Parse next object from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            base.Parse(stream);
            this.Length = stream.ReadInt32();

            if (LexicalTypeHelper.IsCodePageType((ushort)this.PropType))
            {
                CodePageType type = (CodePageType)this.PropType;

                switch (type)
                {
                    case CodePageType.PtypCodePageUnicode:
                        PtypString pstring = new PtypString();
                        pstring.Parse(stream);
                        this.ValueArray = pstring;
                        break;
                    case CodePageType.PtypCodePageUnicodeBigendian:
                    case CodePageType.PtypCodePageWesternEuropean:
                        PtypString8 pstring8 = new PtypString8();
                        pstring8.Parse(stream);
                        this.ValueArray = pstring8;
                        break;
                    default:
                        PtypString8 defaultstring8 = new PtypString8();
                        defaultstring8.Parse(stream);
                        break;
                }
            }
            else
            {
                PropertyDataType type = (PropertyDataType)this.PropType;

                switch (type)
                {
                    case PropertyDataType.PtypInteger32:
                    case PropertyDataType.PtypBinary:
                        // PidTagParentSourceKey, PidTagParentSourceKey, PidTagChangeKey
                        if ((ushort)this.PropInfo.PropID == 0x65E0 || (ushort)this.PropInfo.PropID == 0x65E1 || (ushort)this.PropInfo.PropID == 0x65E2)
                        {
                            if (this.Length != 0)
                            {
                                XID tmpXID = new XID(this.Length);
                                tmpXID.Parse(stream);
                                this.ValueArray = tmpXID;
                            }
                        }
                        else if ((ushort)this.PropInfo.PropID == 0x65E3) // PidTagPredecessorChangeList 
                        {
                            PredecessorChangeList tmpPredecessorChangeList = new PredecessorChangeList(this.Length);
                            tmpPredecessorChangeList.Parse(stream);
                            this.ValueArray = tmpPredecessorChangeList;
                        }
                        else if ((ushort)this.PropInfo.PropID == 0x402D || (ushort)this.PropInfo.PropID == 0x402E || (ushort)this.PropInfo.PropID == 0x67E5 || (ushort)this.PropInfo.PropID == 0x4021 || (ushort)this.PropInfo.PropID == 0x6793)
                        {
                            if (this.Length != 0)
                            {
                                int begionPosition = (int)stream.Position;
                                int EveLength = this.Length;
                                List<IDSET_REPLID> InterIDSET_REPLID = new List<IDSET_REPLID>();
                                while (EveLength > 0)
                                {
                                    IDSET_REPLID tmpIDSET_REPLID = new IDSET_REPLID();
                                    tmpIDSET_REPLID.Parse(stream);
                                    InterIDSET_REPLID.Add(tmpIDSET_REPLID);
                                    EveLength -= ((int)stream.Position - begionPosition);
                                }

                                this.ValueArray = InterIDSET_REPLID.ToArray();
                            }
                        }
                        else if ((ushort)this.PropInfo.PropID == 0x4017 || (ushort)this.PropInfo.PropID == 0x6796 || (ushort)this.PropInfo.PropID == 0x67DA || (ushort)this.PropInfo.PropID == 0x67D2)
                        {
                            if (this.Length != 0)
                            {
                                int begionPosition = (int)stream.Position;
                                int EveLength = this.Length;
                                List<IDSET_REPLGUID> InterIDSET_REPLGUID = new List<IDSET_REPLGUID>();
                                while (EveLength > 0)
                                {
                                    IDSET_REPLGUID tmpIDSET_REPLGUID = new IDSET_REPLGUID();
                                    tmpIDSET_REPLGUID.Parse(stream);
                                    InterIDSET_REPLGUID.Add(tmpIDSET_REPLGUID);
                                    EveLength -= ((int)stream.Position - begionPosition);
                                }

                                this.ValueArray = InterIDSET_REPLGUID.ToArray();
                            }
                        }
                        else
                        {
                            this.ValueArray = stream.ReadBlock(this.Length);
                        }

                        break;
                    case PropertyDataType.PtypString:
                        PtypString pstring = new PtypString();
                        pstring.Parse(stream);
                        this.ValueArray = pstring;
                        break;
                    case PropertyDataType.PtypString8:
                        PtypString8 pstring8 = new PtypString8();
                        pstring8.Parse(stream);
                        this.ValueArray = pstring8;
                        break;
                    case PropertyDataType.PtypServerId:
                        this.ValueArray = Block.Parse<PtypServerId>(stream);
                        break;
                    case PropertyDataType.PtypObject_Or_PtypEmbeddedTable:
                        this.ValueArray = stream.ReadBlock(this.Length);
                        break;
                    default:
                        this.ValueArray = stream.ReadBlock(this.Length);
                        break;
                }
            }
        }
    }
}

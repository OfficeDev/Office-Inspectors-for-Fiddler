namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The MetaPropValue represents identification information and the value of the Meta property.
    /// </summary>
    public class MetaPropValue : SyntacticalBase
    {
        /// <summary>
        /// The property type.
        /// </summary>
        public PropertyDataType PropType;

        /// <summary>
        /// The property id.
        /// </summary>
        public PidTagPropertyEnum PropID;

        /// <summary>
        /// The property value.
        /// </summary>
        public object PropValue;

        /// <summary>
        /// Initializes a new instance of the MetaPropValue class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public MetaPropValue(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized MetaPropValue.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized MetaPropValue, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            ushort tmpType = stream.VerifyUInt16();
            ushort tmpId = stream.VerifyUInt16();
            return !stream.IsEndOfStream && LexicalTypeHelper.IsMetaPropertyID(tmpId);
        }

        /// <summary>
        /// Parse MetaPropValue from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            this.PropType = (PropertyDataType)stream.ReadUInt16();
            this.PropID = (PidTagPropertyEnum)stream.ReadUInt16();

            if (this.PropID != PidTagPropertyEnum.MetaTagNewFXFolder && this.PropID != PidTagPropertyEnum.MetaTagDnPrefix)
            {
                this.PropValue = stream.ReadUInt32();
            }
            else
            {
                if (this.PropID != PidTagPropertyEnum.MetaTagNewFXFolder)
                {
                    FolderReplicaInfo folderReplicaInfo = new FolderReplicaInfo();
                    folderReplicaInfo.Parse(stream);
                    this.PropValue = folderReplicaInfo;
                }
                else
                {
                    PtypString8 pstring8 = new PtypString8();
                    pstring8.Parse(stream);
                    this.PropValue = pstring8;
                }
            }
        }
    }
}

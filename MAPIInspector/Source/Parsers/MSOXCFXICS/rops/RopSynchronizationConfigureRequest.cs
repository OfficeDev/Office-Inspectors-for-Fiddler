namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System.IO;

    /// <summary>
    ///  A class indicates the RopSynchronizationConfigure ROP Request Buffer.
    ///  2.2.3.2.1.1.1 RopSynchronizationConfigure ROP Request Buffer
    /// </summary>
    public class RopSynchronizationConfigureRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// An enumeration that controls the type of synchronization.
        /// </summary>
        public SynchronizationType SynchronizationType;

        /// <summary>
        /// A flags structure that contains flags that control the behavior of the operation.
        /// </summary>
        public SendOptions SendOptions;

        /// <summary>
        /// A flags structure that contains flags that control the behavior of the synchronization.
        /// </summary>
        public SynchronizationFlags SynchronizationFlags;

        /// <summary>
        /// An unsigned integer that specifies the length, in bytes, of the RestrictionData field.
        /// </summary>
        public ushort RestrictionDataSize;

        /// <summary>
        /// A restriction packet,that specifies the filter for this synchronization object.
        /// </summary>
        public RestrictionType RestrictionData;

        /// <summary>
        /// A flags structure that contains flags control the additional behavior of the synchronization. 
        /// </summary>
        public SynchronizationExtraFlags SynchronizationExtraFlags;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the PropertyTags field.
        /// </summary>
        public ushort PropertyTagCount;

        /// <summary>
        ///  An array of PropertyTag structures that specifies the properties to exclude during the copy.
        /// </summary>
        public PropertyTag[] PropertyTags;

        /// <summary>
        /// Parse the RopSynchronizationConfigureRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSynchronizationConfigureRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.SynchronizationType = (SynchronizationType)this.ReadByte();
            this.SendOptions = (SendOptions)this.ReadByte();
            this.SynchronizationFlags = (SynchronizationFlags)this.ReadUshort();
            this.RestrictionDataSize = this.ReadUshort();

            if (this.RestrictionDataSize > 0)
            {
                this.RestrictionData = new RestrictionType();
                this.RestrictionData.Parse(s);
            }

            this.SynchronizationExtraFlags = (SynchronizationExtraFlags)this.ReadUint();
            this.PropertyTagCount = this.ReadUshort();
            PropertyTag[] interTag = new PropertyTag[(int)this.PropertyTagCount];

            for (int i = 0; i < this.PropertyTagCount; i++)
            {
                interTag[i] = Block.Parse<PropertyTag>(s);
            }

            this.PropertyTags = interTag;
        }
    }
}

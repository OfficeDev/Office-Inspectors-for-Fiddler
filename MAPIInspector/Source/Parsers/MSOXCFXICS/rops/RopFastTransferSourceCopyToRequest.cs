namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System.IO;

    /// <summary>
    ///  A class indicates the RopFastTransferSourceCopyTo ROP Request Buffer.
    ///  2.2.3.1.1.1.1 RopFastTransferSourceCopyTo ROP Request Buffer
    /// </summary>
    public class RopFastTransferSourceCopyToRequest : BaseStructure
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
        /// An unsigned integer that specifies whether descendant subobjects are copied
        /// </summary>
        public byte Level;

        /// <summary>
        /// A flags structure that contains flags that control the type of operation. 
        /// </summary>
        public CopyFlags_CopyTo CopyFlags;

        /// <summary>
        ///  A flags structure that contains flags that control the behavior of the operation. 
        /// </summary>
        public SendOptions SendOptions;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the PropertyTags field.
        /// </summary>
        public ushort PropertyTagCount;

        /// <summary>
        /// An array of PropertyTag structures that specifies the properties to exclude during the copy.
        /// </summary>
        public PropertyTag[] PropertyTags;

        /// <summary>
        /// Parse the RopFastTransferSourceCopyToRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopFastTransferSourceCopyToRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.Level = this.ReadByte();
            this.CopyFlags = (CopyFlags_CopyTo)this.ReadUint();
            this.SendOptions = (SendOptions)this.ReadByte();
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

namespace MAPIInspector.Parsers
{
    using System.IO;
    #region 2.2.6.3 RopSaveChangesMessage ROP
    #endregion


    /// <summary>
    /// 2.2.6.4 RopRemoveAllRecipients ROP
    /// A class indicates the RopRemoveAllRecipients ROP request Buffer.
    /// </summary>
    public class RopRemoveAllRecipientsRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP. 
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the RopLogon associated with this operation.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// Reserved. The client SHOULD set this field to 0x00000000. 
        /// </summary>
        public uint Reserved;

        /// <summary>
        /// Parse the RopRemoveAllRecipientsRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopRemoveAllRecipientsRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.Reserved = this.ReadUint();
        }
    }
}

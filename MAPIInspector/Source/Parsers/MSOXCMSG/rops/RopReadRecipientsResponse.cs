namespace MAPIInspector.Parsers
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// 2.2.6.6 RopReadRecipients ROP
    /// A class indicates the RopReadRecipients ROP response Buffer.
    /// </summary>
    public class RopReadRecipientsResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the RecipientRows field.
        /// </summary>
        public byte? RowCount;

        /// <summary>
        /// A list of ReadRecipientRow structures. 
        /// </summary>
        public ReadRecipientRow[] RecipientRows;

        /// <summary>
        /// Parse the RopReadRecipientsResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopReadRecipientsResponse structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.RowCount = this.ReadByte();
                List<ReadRecipientRow> readRecipientRows = new List<ReadRecipientRow>();

                for (int i = 0; i < this.RowCount; i++)
                {
                    ReadRecipientRow readRecipientRow = new ReadRecipientRow();
                    readRecipientRow.Parse(s);
                    readRecipientRows.Add(readRecipientRow);
                }

                this.RecipientRows = readRecipientRows.ToArray();
            }
        }
    }
}

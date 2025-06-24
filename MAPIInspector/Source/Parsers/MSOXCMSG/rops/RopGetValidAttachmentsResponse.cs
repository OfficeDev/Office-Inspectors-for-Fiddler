namespace MAPIInspector.Parsers
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// 2.2.6.18 RopGetValidAttachments ROP
    /// A class indicates the RopGetValidAttachments ROP response Buffer.
    /// </summary>
    public class RopGetValidAttachmentsResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP. 
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the number of integers in the AttachmentIdArray field.
        /// </summary>
        public ushort? AttachmentIdCount;

        /// <summary>
        /// An array of 32-bit integers that represent the valid attachment identifiers of the message. 
        /// </summary>
        public int?[] AttachmentIdArray;

        /// <summary>
        /// Parse the RopGetValidAttachmentsResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetValidAttachmentsResponse structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            OutputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                AttachmentIdCount = ReadUshort();
                List<int> attachmentIdArrays = new List<int>();

                for (int i = 0; i < AttachmentIdCount; i++)
                {
                    attachmentIdArrays.Add(ReadINT32());
                }

                AttachmentIdArray = ConvertArray(attachmentIdArrays.ToArray());
            }
        }
    }
}

namespace MAPIInspector.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    ///  A class indicates the RopFastTransferDestinationPutBufferExtended ROP Request Buffer.
    ///  2.2.3.1.2.3.1 RopFastTransferDestinationPutBufferExtended ROP Request Buffer
    /// </summary>
    public class RopFastTransferDestinationPutBufferExtendedRequest : BaseStructure
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
        /// An unsigned integer that specifies the size of the TransferData field. 
        /// </summary>
        public ushort TransferDataSize;

        /// <summary>
        /// An array of bytes that contains the data to be uploaded to the destination fast transfer object.
        /// </summary>
        public object TransferData;

        /// <summary>
        /// Parse the RopFastTransferDestinationPutBufferExtendedRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopFastTransferDestinationPutBufferExtendedRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.TransferDataSize = this.ReadUshort();

            byte[] buffer = ReadBytes((int)this.TransferDataSize);
            FastTransferStream transferStream = new FastTransferStream(buffer, true);
            List<TransferPutBufferExtendElement> transferBufferList = new List<TransferPutBufferExtendElement>();
            long sposition = 0;

            while (!transferStream.IsEndOfStream)
            {
                sposition = transferStream.Position;
                TransferPutBufferExtendElement element = new TransferPutBufferExtendElement(transferStream);
                if (sposition == transferStream.Position)
                {
                    throw new Exception(string.Format("Error occurred in the {0} TransferElement", transferBufferList.Count));
                }
                else
                {
                    transferBufferList.Add(element);
                }
            }

            this.TransferData = transferBufferList.ToArray();
        }
    }
}

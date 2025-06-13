namespace MAPIInspector.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    ///  A class indicates the RopFastTransferSourceGetBuffer ROP Response Buffer.
    ///  2.2.3.1.1.5.2 RopFastTransferSourceGetBuffer ROP Response Buffer
    /// </summary>
    public class RopFastTransferSourceGetBufferResponse : BaseStructure
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
        /// An enumeration that specifies the current status of the transfer. 
        /// </summary>
        public TransferStatus? TransferStatus;

        /// <summary>
        /// An unsigned integer that specifies the number of steps that have been completed in the current operation.
        /// </summary>
        public ushort? InProgressCount;

        /// <summary>
        /// An unsigned integer that specifies the approximate number of steps to be completed in the current operation.
        /// </summary>
        public ushort? TotalStepCount;

        /// <summary>
        /// A reserved field
        /// </summary>
        public byte? Reserved;

        /// <summary>
        /// An unsigned integer that specifies the size of the TransferBuffer field.
        /// </summary>
        public ushort? TransferBufferSize;

        /// <summary>
        ///  An array of bytes that specifies FastTransferStream.
        /// </summary>
        public object TransferBuffer;

        /// <summary>
        /// An unsigned integer that specifies the number of milliseconds for the client to wait before trying this operation again
        /// </summary>
        public uint? BackoffTime;

        /// <summary>
        /// Parse the RopFastTransferSourceGetBufferResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopFastTransferSourceGetBufferResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.TransferStatus = (TransferStatus)this.ReadUshort();
                this.InProgressCount = this.ReadUshort();
                this.TotalStepCount = this.ReadUshort();
                this.Reserved = this.ReadByte();
                this.TransferBufferSize = this.ReadUshort();
                byte[] buffer = ReadBytes((int)this.TransferBufferSize);
                FastTransferStream transferStream = new FastTransferStream(new byte[0], true);
                long sposition = 0;

                if (this.TransferStatus.Value == Parsers.TransferStatus.Partial)
                {
                    transferStream = new FastTransferStream(buffer, true);
                    List<TransferGetBufferElement> transferBufferList = new List<TransferGetBufferElement>();

                    while (!transferStream.IsEndOfStream)
                    {
                        sposition = transferStream.Position;
                        TransferGetBufferElement element = new TransferGetBufferElement(transferStream);

                        if (sposition == transferStream.Position)
                        {
                            throw new Exception(string.Format("Error occurred in the {0} TransferElement", transferBufferList.Count));
                        }
                        else
                        {
                            transferBufferList.Add(element);
                        }
                    }

                    this.TransferBuffer = transferBufferList.ToArray();
                }
                else
                {
                    transferStream = new FastTransferStream(buffer, true);
                    List<TransferGetBufferElement> transferBufferList = new List<TransferGetBufferElement>();

                    while (transferStream.Position < transferStream.Length)
                    {
                        sposition = transferStream.Position;

                        TransferGetBufferElement element = new TransferGetBufferElement(transferStream);
                        if (sposition == transferStream.Position)
                        {
                            throw new Exception(string.Format("Error occurred in the {0} TransferElement", transferBufferList.Count));
                        }
                        else
                        {
                            transferBufferList.Add(element);
                        }
                    }

                    this.TransferBuffer = transferBufferList.ToArray();
                }
            }

            if ((AdditionalErrorCodes)this.ReturnValue == AdditionalErrorCodes.ServerBusy)
            {
                this.BackoffTime = this.ReadUint();
            }
        }
    }
}

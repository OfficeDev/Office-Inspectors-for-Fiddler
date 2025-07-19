using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the RopFastTransferSourceGetBuffer ROP Response Buffer.
    /// 2.2.3.1.1.5.2 RopFastTransferSourceGetBuffer ROP Response Buffer
    /// </summary>
    public class RopFastTransferSourceGetBufferResponse : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public BlockT<ErrorCodes> ReturnValue;

        /// <summary>
        /// An enumeration that specifies the current status of the transfer.
        /// </summary>
        public BlockT<TransferStatus> TransferStatus;

        /// <summary>
        /// An unsigned integer that specifies the number of steps that have been completed in the current operation.
        /// </summary>
        public BlockT<ushort> InProgressCount;

        /// <summary>
        /// An unsigned integer that specifies the approximate number of steps to be completed in the current operation.
        /// </summary>
        public BlockT<ushort> TotalStepCount;

        /// <summary>
        /// A reserved field
        /// </summary>
        public BlockT<byte> Reserved;

        /// <summary>
        /// An unsigned integer that specifies the size of the TransferBuffer field.
        /// </summary>
        public BlockT<ushort> TransferBufferSize;

        /// <summary>
        ///  An array of blocks that specifies FastTransferStream.
        /// </summary>
        public Block[] TransferBuffer;

        /// <summary>
        /// An unsigned integer that specifies the number of milliseconds for the client to wait before trying this operation again
        /// </summary>
        public BlockT<uint> BackoffTime;

        /// <summary>
        /// Parse the RopFastTransferSourceGetBufferResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();

            if (ReturnValue == ErrorCodes.Success)
            {
                TransferStatus = ParseT<TransferStatus>();
                InProgressCount = ParseT<ushort>();
                TotalStepCount = ParseT<ushort>();
                Reserved = ParseT<byte>();
                TransferBufferSize = ParseT<ushort>();

                parser.PushCap(TransferBufferSize);

                var transferBufferList = new List<Block>();

                while (!parser.Empty)
                {
                    var element = Parse<TransferGetBufferElement>();
                    if (!element.Parsed || element.Size == 0) break;
                    transferBufferList.Add(element);
                }

                if (!parser.Empty && parser.RemainingBytes > 0)
                {
                    // If there is still data left, grab it as a block
                    transferBufferList.Add(ParseJunk("Remaining Data"));
                }

                TransferBuffer = transferBufferList.ToArray();

                parser.PopCap();
            }
            else if ((AdditionalErrorCodes)ReturnValue.Data == AdditionalErrorCodes.ServerBusy)
            {
                BackoffTime = ParseT<uint>();
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopFastTransferSourceGetBufferResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            this.AddError(ReturnValue, "ReturnValue");
            AddChildBlockT(TransferStatus, "TransferStatus");
            AddChildBlockT(InProgressCount, "InProgressCount");
            AddChildBlockT(TotalStepCount, "TotalStepCount");
            AddChildBlockT(Reserved, "Reserved");
            AddChildBlockT(TransferBufferSize, "TransferBufferSize");
            AddLabeledChildren(TransferBuffer, "TransferBuffer");
            AddChildBlockT(BackoffTime, "BackoffTime");
        }
    }
}

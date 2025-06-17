namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System.Collections.Generic;

    /// <summary>
    ///  A class indicates the RopFastTransferSourceGetBuffer ROP Response Buffer.
    ///  2.2.3.1.1.5.2 RopFastTransferSourceGetBuffer ROP Response Buffer
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
            RopId = BlockT<RopIdType>.Parse(parser);
            InputHandleIndex = BlockT<byte>.Parse(parser);
            ReturnValue = BlockT<ErrorCodes>.Parse(parser);

            if (ReturnValue.Data == ErrorCodes.Success)
            {
                TransferStatus = BlockT<TransferStatus>.Parse(parser);
                InProgressCount = BlockT<ushort>.Parse(parser);
                TotalStepCount = BlockT<ushort>.Parse(parser);
                Reserved = BlockT<byte>.Parse(parser);
                TransferBufferSize = BlockT<ushort>.Parse(parser);

                parser.PushCap(TransferBufferSize.Data);
                if (TransferStatus.Data == Parsers.TransferStatus.Partial)
                {
                    var transferBufferList = new List<TransferGetBufferElement>();

                    while (!parser.Empty)
                    {
                        var element = Parse<TransferGetBufferElement>(parser);
                        if (!element.Parsed || element.Size == 0) break;
                        transferBufferList.Add(element);
                    }

                    TransferBuffer = transferBufferList.ToArray();
                }
                else
                {
                    var transferBufferList = new List<TransferGetBufferElement>();

                    while (!parser.Empty)
                    {
                        var element = Parse<TransferGetBufferElement>(parser);
                        if (!element.Parsed || element.Size == 0) break;
                        transferBufferList.Add(element);
                    }

                    TransferBuffer = transferBufferList.ToArray();
                }

                parser.PopCap();
            }
            else if ((AdditionalErrorCodes)ReturnValue.Data == AdditionalErrorCodes.ServerBusy)
            {
                BackoffTime = BlockT<uint>.Parse(parser);
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopFastTransferSourceGetBufferResponse");
            if (RopId != null) AddChild(RopId, $"RopId:{RopId.Data}");
            if (InputHandleIndex != null) AddChild(InputHandleIndex, $"InputHandleIndex:{InputHandleIndex.Data}");
            if (ReturnValue != null) AddChild(ReturnValue, $"ReturnValue:{ReturnValue.Data}");
            if (TransferStatus != null) AddChild(TransferStatus, $"TransferStatus:{TransferStatus.Data}");
            if (InProgressCount != null) AddChild(InProgressCount, $"InProgressCount:{InProgressCount.Data}");
            if (TotalStepCount != null) AddChild(TotalStepCount, $"TotalStepCount:{TotalStepCount.Data}");
            if (Reserved != null) AddChild(Reserved, $"Reserved:{Reserved.Data}");
            if (TransferBufferSize != null) AddChild(TransferBufferSize, $"TransferBufferSize:{TransferBufferSize.Data}");
            AddLabeledChildren(TransferBuffer, "TransferBuffer");
            if (BackoffTime != null) AddChild(BackoffTime, $"BackoffTime:{BackoffTime.Data}");
        }
    }
}

namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System.Collections.Generic;

    /// <summary>
    ///  A class indicates the RopFastTransferDestinationPutBufferExtended ROP Request Buffer.
    ///  2.2.3.1.2.3.1 RopFastTransferDestinationPutBufferExtended ROP Request Buffer
    /// </summary>
    public class RopFastTransferDestinationPutBufferExtendedRequest : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public BlockT<byte> LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the size of the TransferData field. 
        /// </summary>
        public BlockT<ushort> TransferDataSize;

        /// <summary>
        /// An array of blocks that contains the data to be uploaded to the destination fast transfer object.
        /// </summary>
        public Block[] TransferData;

        /// <summary>
        /// Parse the RopFastTransferDestinationPutBufferExtendedRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = BlockT<RopIdType>.Parse(parser);
            LogonId = BlockT<byte>.Parse(parser);
            InputHandleIndex = BlockT<byte>.Parse(parser);
            TransferDataSize = BlockT<ushort>.Parse(parser);

            parser.PushCap(TransferDataSize.Data);
            var transferBufferList = new List<TransferPutBufferExtendElement>();
            while (!parser.Empty)
            {
                var element = Parse<TransferPutBufferExtendElement>(parser);
                if (!element.Parsed)
                {
                    break;
                }

                transferBufferList.Add(element);
            }

            TransferData = transferBufferList.ToArray();

            parser.PopCap();
        }

        protected override void ParseBlocks()
        {
            SetText("RopFastTransferDestinationPutBufferExtendedRequest");
            if (RopId != null) AddChild(RopId, $"RopId:{RopId.Data}");
            if (LogonId != null) AddChild(LogonId, $"LogonId:{LogonId.Data}");
            if (InputHandleIndex != null) AddChild(InputHandleIndex, $"InputHandleIndex:{InputHandleIndex.Data}");
            if (TransferDataSize != null) AddChild(TransferDataSize, $"TransferDataSize:{TransferDataSize.Data}");
            if (TransferData != null)
            {
                foreach (var transferData in TransferData)
                {
                    AddLabeledChild(transferData, "TransferData");
                }
            }
        }
    }
}

namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System.Collections.Generic;

    /// <summary>
    ///  A class indicates the RopFastTransferDestinationPutBuffer ROP Request Buffer.
    ///  2.2.3.1.2.2.1 RopFastTransferDestinationPutBuffer ROP Request Buffer
    /// </summary>
    public class RopFastTransferDestinationPutBufferRequest : Block
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
        /// Parse the RopFastTransferDestinationPutBufferRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            TransferDataSize = ParseT<ushort>();

            parser.PushCap(TransferDataSize.Data);
            var transferBufferList = new List<TransferPutBufferElement>();
            while (!parser.Empty)
            {
                var element = Parse<TransferPutBufferElement>();

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
            SetText("RopFastTransferDestinationPutBufferRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(TransferDataSize, "TransferDataSize");
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

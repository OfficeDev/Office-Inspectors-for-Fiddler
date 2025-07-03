using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.15.2 RopBackoff
    /// A class indicates the RopBackoff ROP Response Buffer.
    /// </summary>
    public class RopBackoffResponse : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP. For this operation this field is set to 0x01.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer that specifies the RopLogon associated with this operation.
        /// </summary>
        public BlockT<byte> LogonId;

        /// <summary>
        /// An unsigned integer that specifies the number of milliseconds to apply a ROP BackOff.
        /// </summary>
        public BlockT<uint> Duration;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the BackoffRopData field.
        /// </summary>
        public BlockT<byte> BackoffRopCount;

        /// <summary>
        /// An array of BackoffRop structures.
        /// </summary>
        public BackoffRop[] BackoffRopData;

        /// <summary>
        /// An unsigned integer that specifies the size of the AdditionalData field.
        /// </summary>
        public BlockT<ushort> AdditionalDataSize;

        /// <summary>
        /// An array of bytes that specifies additional information about the ROP BackOff response.
        /// </summary>
        public BlockBytes AdditionalData;

        /// <summary>
        /// Parse the RopBackoffResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            Duration = ParseT<uint>();
            BackoffRopCount = ParseT<byte>();
            var backoffRopDataList = new List<BackoffRop>();

            for (int i = 0; i < BackoffRopCount; i++)
            {
                backoffRopDataList.Add(Parse<BackoffRop>());
            }

            BackoffRopData = backoffRopDataList.ToArray();
            AdditionalDataSize = ParseT<ushort>();
            AdditionalData = ParseBytes(AdditionalDataSize);
        }

        protected override void ParseBlocks()
        {
            SetText("RopBackoffResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(Duration, "Duration");
            AddChildBlockT(BackoffRopCount, "BackoffRopCount");
            AddLabeledChildren(BackoffRopData, "BackoffRopData");
            AddChildBlockT(AdditionalDataSize, "AdditionalDataSize");
            AddChildBytes(AdditionalData, "AdditionalData");
        }
    }
}

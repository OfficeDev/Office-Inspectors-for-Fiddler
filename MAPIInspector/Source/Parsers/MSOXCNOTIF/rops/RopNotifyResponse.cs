using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.14.2.1 RopNotify ROP Response Buffer
    /// A class indicates the RopNotify ROP Response Buffer.
    /// </summary>
    public class RopNotifyResponse : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP. For this operation this field is set to 0x2A.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// A Server object handle that specifies the notification Server object associated with this notification event.
        /// </summary>
        public BlockT<uint> NotificationHandle;

        /// <summary>
        /// An unsigned integer that specifies the RopLogon associated with this notification event.
        /// </summary>
        public BlockT<byte> LogonId;

        /// <summary>
        /// Various structures
        /// </summary>
        public NotificationData NotificationData;

        /// <summary>
        /// Parse the RopNotifyResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            NotificationHandle = ParseT<uint>();
            LogonId = ParseT<byte>();
            NotificationData = new NotificationData(NotificationHandle);
            NotificationData.Parse(parser);
        }

        protected override void ParseBlocks()
        {
            SetText("RopNotifyResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(NotificationHandle, "NotificationHandle");
            AddChildBlockT(LogonId, "LogonId");
            AddChild(NotificationData, "NotificationData");
        }
    }
}

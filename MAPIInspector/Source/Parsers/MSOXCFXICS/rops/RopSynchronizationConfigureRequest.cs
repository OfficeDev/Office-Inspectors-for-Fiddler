using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    ///  A class indicates the RopSynchronizationConfigure ROP Request Buffer.
    ///  2.2.3.2.1.1.1 RopSynchronizationConfigure ROP Request Buffer
    /// </summary>
    public class RopSynchronizationConfigureRequest : Block
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
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        /// </summary>
        public BlockT<byte> OutputHandleIndex;

        /// <summary>
        /// An enumeration that controls the type of synchronization.
        /// </summary>
        public BlockT<SynchronizationType> SynchronizationType;

        /// <summary>
        /// A flags structure that contains flags that control the behavior of the operation.
        /// </summary>
        public BlockT<SendOptions> SendOptions;

        /// <summary>
        /// A flags structure that contains flags that control the behavior of the synchronization.
        /// </summary>
        public BlockT<SynchronizationFlags> SynchronizationFlags;

        /// <summary>
        /// An unsigned integer that specifies the length, in bytes, of the RestrictionData field.
        /// </summary>
        public BlockT<ushort> RestrictionDataSize;

        /// <summary>
        /// A restriction packet,that specifies the filter for synchronization object.
        /// </summary>
        public RestrictionType RestrictionData;

        /// <summary>
        /// A flags structure that contains flags control the additional behavior of the synchronization. 
        /// </summary>
        public BlockT<SynchronizationExtraFlags> SynchronizationExtraFlags;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the PropertyTags field.
        /// </summary>
        public BlockT<ushort> PropertyTagCount;

        /// <summary>
        ///  An array of PropertyTag structures that specifies the properties to exclude during the copy.
        /// </summary>
        public PropertyTag[] PropertyTags;

        /// <summary>
        /// Parse the RopSynchronizationConfigureRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            OutputHandleIndex = ParseT<byte>();
            SynchronizationType = ParseT<SynchronizationType>();
            SendOptions = ParseT<SendOptions>();
            SynchronizationFlags = ParseT<SynchronizationFlags>();
            RestrictionDataSize = ParseT<ushort>();
            if (RestrictionDataSize.Data > 0)
            {
                parser.PushCap(RestrictionDataSize.Data);
                RestrictionData = new RestrictionType();
                RestrictionData.Parse(parser);
                parser.PopCap();
            }

            SynchronizationExtraFlags = ParseT<SynchronizationExtraFlags>();
            PropertyTagCount = ParseT<ushort>();

            var interTag = new List<PropertyTag>();
            for (int i = 0; i < PropertyTagCount.Data; i++)
            {
                interTag.Add(Parse<PropertyTag>());
            }

            PropertyTags = interTag.ToArray();
        }

        protected override void ParseBlocks()
        {
            SetText("RopSynchronizationConfigureRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(OutputHandleIndex, "OutputHandleIndex");
            AddChildBlockT(SynchronizationType, "SynchronizationType");
            AddChildBlockT(SendOptions, "SendOptions");
            AddChildBlockT(SynchronizationFlags, "SynchronizationFlags");
            AddChildBlockT(RestrictionDataSize, "RestrictionDataSize");
            AddChild(RestrictionData, "RestrictionData");
            AddChildBlockT(SynchronizationExtraFlags, "SynchronizationExtraFlags");
            AddChildBlockT(PropertyTagCount, "PropertyTagCount");
            AddLabeledChildren(PropertyTags, "PropertyTags");
        }
    }
}

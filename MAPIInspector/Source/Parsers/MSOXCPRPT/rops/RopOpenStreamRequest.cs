using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.14 RopOpenStream
    /// A class indicates the RopOpenStream ROP Request Buffer.
    /// </summary>
    public class RopOpenStreamRequest : Block
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
        /// A PropertyTag structure that specifies the property of the object to stream.
        /// </summary>
        public PropertyTag PropertyTag;

        /// <summary>
        /// A flags structure that contains flags that control how the stream is opened.
        /// </summary>
        public BlockT<OpenModeFlags >OpenModeFlags;

        /// <summary>
        /// Parse the RopOpenStreamRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            OutputHandleIndex = ParseT<byte>();
            PropertyTag = Parse<PropertyTag>();
            OpenModeFlags = ParseT<OpenModeFlags>();
        }

        protected override void ParseBlocks()
        {
            Text = "RopOpenStreamRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(OutputHandleIndex, "OutputHandleIndex");
            AddChild(PropertyTag, "PropertyTag");
            AddChildBlockT(OpenModeFlags, "OpenModeFlags");
        }
    }
}

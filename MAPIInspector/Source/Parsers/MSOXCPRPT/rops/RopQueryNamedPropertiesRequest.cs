using System;
using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.9 RopQueryNamedProperties
    /// A class indicates the RopQueryNamedProperties ROP Request Buffer.
    /// </summary>
    public class RopQueryNamedPropertiesRequest : Block
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
        /// A flags structure that contains flags control how this ROP behaves.
        /// </summary>
        public BlockT<byte> QueryFlags;

        /// <summary>
        /// A Boolean that specifies whether the PropertyGuid field is present.
        /// </summary>
        public BlockT<byte> HasGuid;

        /// <summary>
        /// A GUID that is present if HasGuid is nonzero and is not present if the value of the HasGuid field is zero.
        /// </summary>
        public BlockGuid PropertyGuid;

        /// <summary>
        /// Parse the RopQueryNamedPropertiesRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            QueryFlags = ParseT<byte>();
            HasGuid = ParseT<byte>();

            if (HasGuid != 0)
            {
                PropertyGuid = Parse<BlockGuid>();
            }
        }

        protected override void ParseBlocks()
        {
            Text = "RopQueryNamedPropertiesRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(QueryFlags, "QueryFlags");
            AddChildBlockT(HasGuid, "HasGuid");
            this.AddChildGuid(PropertyGuid, "PropertyGuid");
        }
    }
}

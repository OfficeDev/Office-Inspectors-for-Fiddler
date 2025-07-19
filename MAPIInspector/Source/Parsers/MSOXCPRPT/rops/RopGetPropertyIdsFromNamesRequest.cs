using BlockParser;
using System.Collections.Generic;
using System.Windows.Forms.Design;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.12 RopGetPropertyIdsFromNames
    /// A class indicates the RopGetPropertyIdsFromNames ROP Request Buffer.
    /// </summary>
    public class RopGetPropertyIdsFromNamesRequest : Block
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
        /// An integer that specifies whether to create a new entry.
        /// </summary>
        public BlockT<byte> Flags;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the PropertyNames field.
        /// </summary>
        public BlockT<ushort> PropertyNameCount;

        /// <summary>
        /// A list of PropertyName structures that specifies the property names requested.
        /// </summary>
        public PropertyName[] PropertyNames;

        /// <summary>
        /// Parse the RopGetPropertyIdsFromNamesRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            Flags = ParseT<byte>();
            PropertyNameCount = ParseT<ushort>();
            var tmpPropertyNames = new List<PropertyName>();
            for (int i = 0; i < PropertyNameCount; i++)
            {
                tmpPropertyNames.Add(Parse<PropertyName>());
            }
            PropertyNames = tmpPropertyNames.ToArray();
        }

        protected override void ParseBlocks()
        {
            Text = "RopGetPropertyIdsFromNamesRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(Flags, "Flags");
            AddChildBlockT(PropertyNameCount, "PropertyNameCount");
            AddLabeledChildren(PropertyNames, "PropertyNames");
        }
    }
}

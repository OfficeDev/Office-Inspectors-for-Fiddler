using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2 RopModifyPermissions ROP
    /// The RopModifyPermissions ROP ([MS-OXCROPS] section 2.2.10.1) creates, updates, or deletes entries in the permissions list on a folder.
    /// </summary>
    public class RopModifyPermissionsRequest : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP. For this operation this field is set to 0x40.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer that specifies the RopLogon associated with this operation.
        /// </summary>
        public BlockT<byte> LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// A flags structure that contains flags that control the behavior of this operation.
        /// </summary>
        public BlockT<ModifyFlags> ModifyFlags;

        /// <summary>
        /// An unsigned integer that specifies the number of structures serialized in the PermissionsData field.
        /// </summary>
        public BlockT<ushort> ModifyCount;

        /// <summary>
        /// A list of PermissionData structures. 
        /// </summary>
        public PermissionData[] PermissionsData;

        /// <summary>
        /// Parse the RopModifyPermissionsRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            ModifyFlags = ParseT<ModifyFlags>();
            ModifyCount = ParseT<ushort>();
            var listPermissionData = new List<PermissionData>();

            for (int i = 0; i < ModifyCount.Data; i++)
            {
                listPermissionData.Add(Parse<PermissionData>());
            }

            PermissionsData = listPermissionData.ToArray();
        }

        protected override void ParseBlocks()
        {
            SetText("RopModifyPermissionsRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(ModifyFlags, "ModifyFlags");
            AddChildBlockT(ModifyCount, "ModifyCount");
            AddLabeledChildren(PermissionsData, "PermissionsData");
        }
    }
}

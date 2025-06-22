namespace MAPIInspector.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// The enum value of RopGetPermissionsTable TableFlags
    /// </summary>
    [Flags]
    public enum TableFlagsRopGetPermissionsTable : byte
    {
        /// <summary>
        /// If this flag is set, the server MUST include the values of the FreeBusySimple and FreeBusyDetailed flags of the PidTagMemberRights property in the returned permissions list
        /// </summary>
        IncludeFreeBusy = 0x02
    }

    /// <summary>
    /// The enum value of Modify Flags
    /// </summary>
    [Flags]
    public enum ModifyFlags : byte
    {
        /// <summary>
        /// If this flag is set, the server MUST replace all existing entries except the default user entry in the current permissions list with the ones contained in the PermissionsData field
        /// </summary>
        ReplaceRows = 0x01,

        /// <summary>
        /// If this flag is set, the server MUST apply the settings of the FreeBusySimple and FreeBusyDetailed flags of the PidTagMemberRights property when modifying the permissions of the Calendar folder
        /// </summary>
        IncludeFreeBusy = 0x02
    }

    /// <summary>
    /// The enum value of Permission Data Flags
    /// </summary>
    [Flags]
    public enum PermissionDataFlags : byte
    {
        /// <summary>
        /// The user that is specified by the PidTagEntryId property (section 2.2.4) is added to the permissions list
        /// </summary>
        AddRow = 0x01,

        /// <summary>
        /// The existing permissions for the user that is identified by the PidTagMemberId property are modified
        /// </summary>
        ModifyRow = 0x02,

        /// <summary>
        /// The user that is identified by the PidTagMemberId property is deleted from the permissions list
        /// </summary>
        RemoveRow = 0x04
    }

    #region 2.2.1	RopGetPermissionsTable ROP
    /// <summary>
    /// The RopGetPermissionsTable ROP ([MS-OXCROPS] section 2.2.10.2) retrieves a Server object handle to a Table object, which is then used in other ROP requests to retrieve the current permissions list on a folder.
    /// </summary>
    public class RopGetPermissionsTableRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP. For this operation this field is set to 0x3E.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the RopLogon associated with this operation.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        /// </summary>     
        public byte OutputHandleIndex;

        /// <summary>
        /// A flags structure that contains flags that control the type of table. 
        /// </summary>
        public TableFlagsRopGetPermissionsTable TableFlags;

        /// <summary>
        /// Parse the RopGetPermissionsTableRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetPermissionsTableRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.TableFlags = (TableFlagsRopGetPermissionsTable)this.ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopGetPermissionsTable ROP Response Buffer.
    /// </summary>
    public class RopGetPermissionsTableResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP. For this operation this field is set to 0x3E.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// Parse the RopGetPermissionsTableResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetPermissionsTableResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)this.ReadUint());
        }
    }
    #endregion

    #region 2.2.2	RopModifyPermissions ROP
    /// <summary>
    /// The RopModifyPermissions ROP ([MS-OXCROPS] section 2.2.10.1) creates, updates, or deletes entries in the permissions list on a folder.
    /// </summary>
    public class RopModifyPermissionsRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP. For this operation this field is set to 0x40.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the RopLogon associated with this operation.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// A flags structure that contains flags that control the behavior of this operation.
        /// </summary>
        public ModifyFlags ModifyFlags;

        /// <summary>
        /// An unsigned integer that specifies the number of structures serialized in the PermissionsData field.
        /// </summary>
        public ushort ModifyCount;

        /// <summary>
        /// A list of PermissionData structures. 
        /// </summary>
        public PermissionData[] PermissionsData;

        /// <summary>
        /// Parse the RopModifyPermissionsRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopModifyPermissionsRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ModifyFlags = (ModifyFlags)this.ReadByte();
            this.ModifyCount = this.ReadUshort();
            List<PermissionData> listPermissionData = new List<PermissionData>();

            for (int i = 0; i < this.ModifyCount; i++)
            {
                PermissionData tempPermissionData = new PermissionData();
                tempPermissionData.Parse(s);
                listPermissionData.Add(tempPermissionData);
            }

            this.PermissionsData = listPermissionData.ToArray();
        }
    }

    /// <summary>
    ///  A class indicates the PermissionData.
    /// </summary>
    public class PermissionData : BaseStructure
    {
        /// <summary>
        /// A set of flags that specify the type of change to be made to the folder permissions.
        /// </summary>
        public PermissionDataFlags PermissionDataFlags;

        /// <summary>
        /// An integer that specifies the number of structures contained in the PropertyValues field.
        /// </summary>
        public ushort PropertyValueCount;

        /// <summary>
        /// An array of TaggedPropertyValue structures ([MS-OXCDATA] section 2.11.4).
        /// </summary>
        public TaggedPropertyValue[] PropertyValues;

        /// <summary>
        /// Parse the PermissionData structure.
        /// </summary>
        /// <param name="s">A stream containing PermissionData structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.PermissionDataFlags = (PermissionDataFlags)this.ReadByte();
            this.PropertyValueCount = this.ReadUshort();
            List<TaggedPropertyValue> listPropertyValues = new List<TaggedPropertyValue>();

            for (int i = 0; i < this.PropertyValueCount; i++)
            {
                TaggedPropertyValue tempPropertyValue = new TaggedPropertyValue();
                tempPropertyValue.Parse(s);
                listPropertyValues.Add(tempPropertyValue);
            }

            this.PropertyValues = listPropertyValues.ToArray();
        }
    }

    /// <summary>
    ///  A class indicates the RopModifyPermissions ROP Response Buffer.
    /// </summary>
    public class RopModifyPermissionsResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP. For this operation this field is set to 0x40.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// Parse the RopModifyPermissionsResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopModifyPermissionsResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)this.ReadUint());
        }
    }

    #endregion
}

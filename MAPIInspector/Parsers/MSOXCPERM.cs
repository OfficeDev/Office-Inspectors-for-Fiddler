using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The enum value of RopGetPermissionsTable TableFlags
    /// </summary>
    [Flags]
    public enum TableFlagsRopGetPermissionsTable : byte
    {
        IncludeFreeBusy = 0x02
    }

    /// <summary>
    /// The enum value of Modify Flags
    /// </summary>
    [Flags]
    public enum ModifyFlags : byte
    {
        ReplaceRows = 0x01,
        IncludeFreeBusy = 0x02
    }

    /// <summary>
    /// The enum value of Permission Data Flags
    /// </summary>
    [Flags]
    public enum PermissionDataFlags : byte
    {
        AddRow = 0x01,
        ModifyRow = 0x02,
        RemoveRow = 0x04
    }

    #region 2.2.1	RopGetPermissionsTable ROP
    /// <summary>
    /// The RopGetPermissionsTable ROP ([MS-OXCROPS] section 2.2.10.2) retrieves a Server object handle to a Table object, which is then used in other ROP requests to retrieve the current permissions list on a folder.
    /// </summary>
    public class RopGetPermissionsTableRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP. For this operation this field is set to 0x3E.
        public RopIdType RopId;

        // An unsigned integer that specifies the logon associated with this operation.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored. 
        public byte OutputHandleIndex;

        // A flags structure that contains flags that control the type of table. 
        public TableFlagsRopGetPermissionsTable TableFlags;

        /// <summary>
        /// Parse the RopGetPermissionsTableRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetPermissionsTableRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.OutputHandleIndex = ReadByte();
            this.TableFlags = (TableFlagsRopGetPermissionsTable)ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopGetPermissionsTable ROP Response Buffer.
    /// </summary>
    public class RopGetPermissionsTableResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP. For this operation this field is set to 0x3E.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        public byte OutputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        /// <summary>
        /// Parse the RopGetPermissionsTableResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetPermissionsTableResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.OutputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
        }
    }
    #endregion

    #region 2.2.2	RopModifyPermissions ROP
    /// <summary>
    /// The RopModifyPermissions ROP ([MS-OXCROPS] section 2.2.10.1) creates, updates, or deletes entries in the permissions list on a folder.
    /// </summary>
    public class RopModifyPermissionsRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP. For this operation this field is set to 0x40.
        public RopIdType RopId;

        // An unsigned integer that specifies the logon associated with this operation.
        public byte LogonId;

        //  An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        // A flags structure that contains flags that control the behavior of this operation. 
        public ModifyFlags ModifyFlags;

        // An unsigned integer that specifies specifies the number of structures serialized in the PermissionsData field.
        public ushort ModifyCount;

        // A list of PermissionData structures. 
        public PermissionData[] PermissionsData;

        /// <summary>
        /// Parse the RopModifyPermissionsRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopModifyPermissionsRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.ModifyFlags = (ModifyFlags)ReadByte();
            this.ModifyCount = ReadUshort();
            List<PermissionData> listPermissionData = new List<PermissionData>();
            for (int i = 0; i < ModifyCount; i++)
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
        // A set of flags that specify the type of change to be made to the folder permissions.
        public PermissionDataFlags PermissionDataFlags;

        // An integer that specifies the number of structures contained in the PropertyValues field.
        public ushort PropertyValueCount;

        // An array of TaggedPropertyValue structures ([MS-OXCDATA] section 2.11.4). 
        public TaggedPropertyValue[] PropertyValues;

        /// <summary>
        /// Parse the PermissionData structure.
        /// </summary>
        /// <param name="s">An stream containing PermissionData structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.PermissionDataFlags = (PermissionDataFlags)ReadByte();
            this.PropertyValueCount = ReadUshort();
            List<TaggedPropertyValue> listPropertyValues = new List<TaggedPropertyValue>();
            for (int i = 0; i < PropertyValueCount; i++)
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
        // An unsigned integer that specifies the type of ROP. For this operation this field is set to 0x40.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        public byte InputHandleIndex;

        //  An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        /// <summary>
        /// Parse the RopModifyPermissionsResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopModifyPermissionsResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
        }
    }
    #endregion

}

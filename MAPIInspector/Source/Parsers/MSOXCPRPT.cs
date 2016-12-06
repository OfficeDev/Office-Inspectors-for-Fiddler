using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace MAPIInspector.Parsers
{
    #region 2.2.2.2 RopGetPropertiesSpecific
    /// <summary>
    ///  A class indicates the RopGetPropertiesSpecific ROP Request Buffer.
    /// </summary>
    public class RopGetPropertiesSpecificRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the maximum size allowed for a property value returned.
        public ushort PropertySizeLimit;

        // A Boolean that specifies whether to return string properties in multibyte Unicode.
        public ushort WantUnicode;

        // An unsigned integer that specifies the number of tags present in the PropertyTags field.
        public ushort PropertyTagCount;

        // An array of PropertyTag structures that specifies the properties requested.
        public PropertyTag[] PropertyTags;

        /// <summary>
        /// Parse the RopGetPropertiesSpecificRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetPropertiesSpecificRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.PropertySizeLimit = ReadUshort();
            this.WantUnicode = ReadUshort();
            this.PropertyTagCount = ReadUshort();
            List<PropertyTag> tmpPropertyTags = new List<PropertyTag>();
            for (int i = 0; i < this.PropertyTagCount; i++)
            {
                PropertyTag tmppropertytag = new PropertyTag();
                tmppropertytag.Parse(s);
                tmpPropertyTags.Add(tmppropertytag);
            }
            this.PropertyTags = tmpPropertyTags.ToArray();
        }
    }

    /// <summary>
    ///  A class indicates the RopGetPropertiesSpecific ROP Response Buffer.
    /// </summary>
    public class RopGetPropertiesSpecificResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // A PropertyRow structure. 
        public PropertyRow RowData;

        /// <summary>
        /// Parse the RopGetPropertiesSpecificResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetPropertiesSpecificResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.RowData = new PropertyRow(DecodingContext.GetPropertiesSpec_propertyTags[MapiInspector.MAPIInspector.currentParsingSessionID][this.InputHandleIndex]);
                this.RowData.Parse(s);
            }
        }
    }
    #endregion

    #region 2.2.2.3 RopGetPropertiesAll
    /// <summary>
    ///  A class indicates the RopGetPropertiesAll ROP Request Buffer.
    /// </summary>
    public class RopGetPropertiesAllRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the maximum size allowed for a property value returned.
        public ushort PropertySizeLimit;

        // A Boolean that specifies whether to return string properties in multibyte Unicode.
        public ushort WantUnicode;

        /// <summary>
        /// Parse the RopGetPropertiesAllRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetPropertiesAllRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.PropertySizeLimit = ReadUshort();
            this.WantUnicode = ReadUshort();
        }
    }

    /// <summary>
    ///  A class indicates the RopGetPropertiesAll ROP Response Buffer.
    /// </summary>
    public class RopGetPropertiesAllResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An unsigned integer that specifies the number of structures present in the PropertyValues field.
        public ushort? PropertyValueCount;

        // An array of TaggedPropertyValue structures that are the properties defined on the object.
        public TaggedPropertyValue[] PropertyValues;

        /// <summary>
        /// Parse the RopGetPropertiesAllResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetPropertiesAllResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.PropertyValueCount = ReadUshort();
                TaggedPropertyValue[] InterValue = new TaggedPropertyValue[(int)this.PropertyValueCount];
                for (int i = 0; i < this.PropertyValueCount; i++)
                {
                    InterValue[i] = new TaggedPropertyValue();
                    InterValue[i].Parse(s);
                }
                this.PropertyValues = InterValue;
            }
        }
    }
    #endregion

    #region 2.2.2.4 RopGetPropertiesList
    /// <summary>
    ///  A class indicates the RopGetPropertiesList ROP Request Buffer.
    /// </summary>
    public class RopGetPropertiesListRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        /// <summary>
        /// Parse the RopGetPropertiesListRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetPropertiesListRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopGetPropertiesList ROP Response Buffer.
    /// </summary>
    public class RopGetPropertiesListResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An unsigned integer that specifies the number of property tags in the PropertyTags field.
        public ushort? PropertyTagCount;

        // An array of PropertyTag structures that lists the property tags on the object.
        public PropertyTag[] PropertyTags;

        /// <summary>
        /// Parse the RopGetPropertiesListResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetPropertiesListResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.PropertyTagCount = ReadUshort();
                PropertyTag[] InterTag = new PropertyTag[(int)this.PropertyTagCount];
                for (int i = 0; i < this.PropertyTagCount; i++)
                {
                    InterTag[i] = new PropertyTag();
                    InterTag[i].Parse(s);
                }
                this.PropertyTags = InterTag;
            }
        }
    }
    #endregion

    #region 2.2.2.5 RopSetProperties
    /// <summary>
    ///  A class indicates the RopSetProperties  ROP Request Buffer.
    /// </summary>
    public class RopSetPropertiesRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the number of bytes used for the PropertyValueCount field and the PropertyValues field.
        public ushort PropertyValueSize;

        // An unsigned integer that specifies the number of PropertyValue structures listed in the PropertyValues field.
        public ushort PropertyValueCount;

        // An array of TaggedPropertyValue structures that specifies the property values to be set on the object.
        public TaggedPropertyValue[] PropertyValues;

        /// <summary>
        /// Parse the RopSetPropertiesRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopSetPropertiesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.PropertyValueSize = ReadUshort();
            this.PropertyValueCount = ReadUshort();
            TaggedPropertyValue[] InterValue = new TaggedPropertyValue[(int)this.PropertyValueCount];
            for (int i = 0; i < this.PropertyValueCount; i++)
            {
                InterValue[i] = new TaggedPropertyValue();
                InterValue[i].Parse(s);
            }
            this.PropertyValues = InterValue;
        }
    }

    /// <summary>
    ///  A class indicates the RopSetProperties ROP Response Buffer.
    /// </summary>
    public class RopSetPropertiesResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An unsigned integer that specifies the number of PropertyProblem structures in the PropertyProblems field. 
        public ushort? PropertyProblemCount;

        // An array of PropertyProblem structures. The number of structures contained in this field is specified by the PropertyProblemCount field. 
        public PropertyProblem[] PropertyProblems;

        /// <summary>
        /// Parse the RopSetPropertiesResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopSetPropertiesResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            List<PropertyRow> TmpRows = new List<PropertyRow>();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.PropertyProblemCount = ReadUshort();
                PropertyProblem[] InterPropertyProblem = new PropertyProblem[(int)this.PropertyProblemCount];
                for (int i = 0; i < this.PropertyProblemCount; i++)
                {
                    InterPropertyProblem[i] = new PropertyProblem();
                    InterPropertyProblem[i].Parse(s);
                }
                this.PropertyProblems = InterPropertyProblem;
            }
        }
    }
    #endregion

    #region 2.2.2.6 RopSetPropertiesNoReplicate
    /// <summary>
    ///  A class indicates the RopSetPropertiesNoReplicate ROP Request Buffer.
    /// </summary>
    public class RopSetPropertiesNoReplicateRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the number of bytes used for the PropertyValueCount field and the PropertyValues field.
        public ushort PropertyValueSize;

        // An unsigned integer that specifies the number of structures listed in the PropertyValues field.
        public ushort PropertyValueCount;

        // PropertyValues (variable):  An array of TaggedPropertyValue structures that specifies the property values to be set on the object. 
        public TaggedPropertyValue[] PropertyValues;

        /// <summary>
        /// Parse the RopSetPropertiesNoReplicateRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopSetPropertiesNoReplicateRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.PropertyValueSize = ReadUshort();
            this.PropertyValueCount = ReadUshort();
            TaggedPropertyValue[] InterValue = new TaggedPropertyValue[(int)this.PropertyValueCount];
            for (int i = 0; i < this.PropertyValueCount; i++)
            {
                InterValue[i] = new TaggedPropertyValue();
                InterValue[i].Parse(s);
            }
            this.PropertyValues = InterValue;
        }
    }

    /// <summary>
    ///  A class indicates the RopSetPropertiesNoReplicate ROP Response Buffer.
    /// </summary>
    public class RopSetPropertiesNoReplicateResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An unsigned integer that specifies the number of PropertyProblem structures in the PropertyProblems field. 
        public ushort? PropertyProblemCount;

        // An array of PropertyProblem structures. The number of structures contained in this field is specified by the PropertyProblemCount field.
        public PropertyProblem[] PropertyProblems;

        /// <summary>
        /// Parse the RopSetPropertiesNoReplicateResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopSetPropertiesNoReplicateResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.PropertyProblemCount = ReadUshort();
                PropertyProblem[] InterPropertyProblem = new PropertyProblem[(int)this.PropertyProblemCount];
                for (int i = 0; i < this.PropertyProblemCount; i++)
                {
                    InterPropertyProblem[i] = new PropertyProblem();
                    InterPropertyProblem[i].Parse(s);
                }
                this.PropertyProblems = InterPropertyProblem;
            }
        }
    }
    #endregion

    #region 2.2.2.7 RopDeleteProperties
    /// <summary>
    ///  A class indicates the RopDeleteProperties ROP Request Buffer.
    /// </summary>
    public class RopDeletePropertiesRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the number of PropertyTag structures in the PropertyTags field. 
        public ushort PropertyTagCount;

        // An array of PropertyTag structures that specifies the property values to be deleted from the object. 
        public PropertyTag[] PropertyTags;

        /// <summary>
        /// Parse the RopDeletePropertiesRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopDeletePropertiesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.PropertyTagCount = ReadUshort();
            List<PropertyTag> tmpPropertyTags = new List<PropertyTag>();
            for (int i = 0; i < this.PropertyTagCount; i++)
            {
                PropertyTag tmppropertytag = new PropertyTag();
                tmppropertytag.Parse(s);
                tmpPropertyTags.Add(tmppropertytag);
            }
            this.PropertyTags = tmpPropertyTags.ToArray();
        }
    }

    /// <summary>
    ///  A class indicates the RopDeleteProperties ROP Response Buffer.
    /// </summary>
    public class RopDeletePropertiesResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An unsigned integer that specifies the number of PropertyProblem structures in the PropertyProblems field. 
        public ushort? PropertyProblemCount;

        // An array of PropertyProblem structures. The number of structures contained in this field is specified by the PropertyProblemCount field. 
        public PropertyProblem[] PropertyProblems;

        /// <summary>
        /// Parse the RopDeletePropertiesResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopDeletePropertiesResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.PropertyProblemCount = ReadUshort();
                PropertyProblem[] InterPropertyProblem = new PropertyProblem[(int)this.PropertyProblemCount];
                for (int i = 0; i < this.PropertyProblemCount; i++)
                {
                    InterPropertyProblem[i] = new PropertyProblem();
                    InterPropertyProblem[i].Parse(s);
                }
                this.PropertyProblems = InterPropertyProblem;
            }
        }
    }
    #endregion

    #region 2.2.2.8 RopDeletePropertiesNoReplicate
    /// <summary>
    ///  A class indicates the RopDeletePropertiesNoReplicate ROP Request Buffer.
    /// </summary>
    public class RopDeletePropertiesNoReplicateRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the number of PropertyTag structures in the PropertyTags field. 
        public ushort PropertyTagCount;

        // An array of PropertyTag structures that specifies the property values to be deleted from the object. 
        public PropertyTag[] PropertyTags;

        /// <summary>
        /// Parse the RopDeletePropertiesNoReplicateRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopDeletePropertiesNoReplicateRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.PropertyTagCount = ReadUshort();
            List<PropertyTag> tmpPropertyTags = new List<PropertyTag>();
            for (int i = 0; i < this.PropertyTagCount; i++)
            {
                PropertyTag tmppropertytag = new PropertyTag();
                tmppropertytag.Parse(s);
                tmpPropertyTags.Add(tmppropertytag);
            }
            this.PropertyTags = tmpPropertyTags.ToArray();
        }
    }

    /// <summary>
    ///  A class indicates the RopDeletePropertiesNoReplicate ROP Response Buffer.
    /// </summary>
    public class RopDeletePropertiesNoReplicateResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An unsigned integer that specifies the number of PropertyProblem structures in the PropertyProblems field. 
        public ushort? PropertyProblemCount;

        // An array of PropertyProblem structures. The number of structures contained in this field is specified by the PropertyProblemCount field. 
        public PropertyProblem[] PropertyProblems;

        /// <summary>
        /// Parse the RopDeletePropertiesNoReplicateResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopDeletePropertiesNoReplicateResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.PropertyProblemCount = ReadUshort();
                PropertyProblem[] InterPropertyProblem = new PropertyProblem[(int)this.PropertyProblemCount];
                for (int i = 0; i < this.PropertyProblemCount; i++)
                {
                    InterPropertyProblem[i] = new PropertyProblem();
                    InterPropertyProblem[i].Parse(s);
                }
                this.PropertyProblems = InterPropertyProblem;
            }
        }
    }
    #endregion

    #region 2.2.2.9 RopQueryNamedProperties
    /// <summary>
    ///  A class indicates the RopQueryNamedProperties ROP Request Buffer.
    /// </summary>
    public class RopQueryNamedPropertiesRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // A flags structure that contains flags control how this ROP behaves.
        public byte QueryFlags;

        // A Boolean that specifies whether the PropertyGuid field is present.
        public byte HasGuid;

        // A GUID that is present if HasGuid is nonzero and is not present if the value of the HasGuid field is zero.
        public Guid? PropertyGuid;

        /// <summary>
        /// Parse the RopQueryNamedPropertiesRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopQueryNamedPropertiesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.QueryFlags = ReadByte();
            this.HasGuid = ReadByte();
            if (this.HasGuid != 0)
            {
                this.PropertyGuid = ReadGuid();
            }
        }
    }

    /// <summary>
    ///  A class indicates the RopQueryNamedProperties ROP Response Buffer.
    /// </summary>
    public class RopQueryNamedPropertiesResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An unsigned integer that specifies the number of elements contained in the PropertyIds and PropertyNames fields.
        public ushort? IdCount;

        // An array of unsigned 16-bit integers. Each integer in the array is the property ID associated with a property name.
        public ushort?[] PropertyIds;

        // A list of PropertyName structures that specifies the property names for the property IDs specified in the PropertyIds field. 
        public PropertyName[] PropertyNames;

        /// <summary>
        /// Parse the RopQueryNamedPropertiesResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopQueryNamedPropertiesResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.IdCount = ReadUshort();
                this.PropertyIds = ConvertArray(new ushort[(int)this.IdCount]);
                this.PropertyNames = new PropertyName[(int)this.IdCount];
                for (int i = 0; i < this.IdCount; i++)
                {
                    PropertyIds[i] = ReadUshort();
                }
                for (int i = 0; i < this.IdCount; i++)
                {
                    PropertyNames[i] = new PropertyName();
                    PropertyNames[i].Parse(s);
                }
            }
        }
    }
    #endregion

    #region 2.2.2.10 RopCopyProperties
    /// <summary>
    ///  A class indicates the RopCopyProperties ROP Request Buffer.
    /// </summary>
    public class RopCopyPropertiesRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the source Server object is stored.
        public byte SourceHandleIndex;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the destination Server object is stored.
        public byte DestHandleIndex;

        // A Boolean that specifies whether the operation is to be processed asynchronously with status reported via the RopProgress ROP 
        public bool WantAsynchronous;

        // A flags structure that contains flags that control the operation behavior.
        public CopyFlags CopyFlags;

        // An unsigned integer that specifies how many tags are present in the PropertyTags field.
        public ushort PropertyTagCount;

        // An array of PropertyTag structures that specifies the properties to copy.
        public PropertyTag[] PropertyTags;

        /// <summary>
        /// Parse the RopCopyPropertiesRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopCopyPropertiesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.SourceHandleIndex = ReadByte();
            this.DestHandleIndex = ReadByte();
            this.WantAsynchronous = ReadBoolean();
            this.CopyFlags = (CopyFlags)ReadByte();
            this.PropertyTagCount = ReadUshort();
            this.PropertyTags = new PropertyTag[(int)this.PropertyTagCount];
            for (int i = 0; i < this.PropertyTagCount; i++)
            {
                PropertyTags[i] = new PropertyTag();
                PropertyTags[i].Parse(s);
            }
        }
    }

    /// <summary>
    ///  A class indicates the RopCopyProperties ROP Response Buffer.
    /// </summary>
    public class RopCopyPropertiesResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the SourceHandleIndex field specified in the request.
        public byte SourceHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An unsigned integer that specifies the number of PropertyProblem structures in the PropertyProblems field. 
        public ushort? PropertyProblemCount;

        // An array of PropertyProblem structures. 
        public PropertyProblem[] PropertyProblems;

        // An unsigned integer index that MUST be set to the value specified in the DestHandleIndex field in the request.
        public uint? DestHandleIndex;

        /// <summary>
        /// Parse the RopCopyPropertiesResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopCopyPropertiesResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.SourceHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.PropertyProblemCount = ReadUshort();
                PropertyProblem[] InterPropertyProblem = new PropertyProblem[(int)this.PropertyProblemCount];
                for (int i = 0; i < this.PropertyProblemCount; i++)
                {
                    InterPropertyProblem[i] = new PropertyProblem();
                    InterPropertyProblem[i].Parse(s);
                }
                this.PropertyProblems = InterPropertyProblem;
            }
            else if ((AdditionalErrorCodes)this.ReturnValue == AdditionalErrorCodes.NullDestinationObject)
            {
                this.DestHandleIndex = ReadUint();
            }
        }
    }
    #endregion

    #region 2.2.2.11 RopCopyTo
    /// <summary>
    ///  A class indicates the RopCopyTo ROP Request Buffer.
    /// </summary>
    public class RopCopyToRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the source Server object is stored.
        public byte SourceHandleIndex;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the destination Server object is stored.
        public byte DestHandleIndex;

        // A Boolean that specifies whether the operation is to be processed asynchronously with status reported via the RopProgress ROP 
        public bool WantAsynchronous;

        // A Boolean that specifies whether to copy subobjects.
        public bool WantSubObjects;

        // A flags structure that contains flags that control the operation behavior.
        public CopyFlags CopyFlags;

        // An unsigned integer that specifies how many tags are present in the ExcludedTags field.
        public ushort ExcludedTagCount;

        // An array of PropertyTag structures that specifies the properties to exclude from the copy. 
        public PropertyTag[] ExcludedTags;

        /// <summary>
        /// Parse the RopCopyToRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopCopyToRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.SourceHandleIndex = ReadByte();
            this.DestHandleIndex = ReadByte();
            this.WantAsynchronous = ReadBoolean();
            this.WantSubObjects = ReadBoolean();
            this.CopyFlags = (CopyFlags)ReadByte();
            this.ExcludedTagCount = ReadUshort();
            this.ExcludedTags = new PropertyTag[(int)this.ExcludedTagCount];
            for (int i = 0; i < this.ExcludedTagCount; i++)
            {
                ExcludedTags[i] = new PropertyTag();
                ExcludedTags[i].Parse(s);
            }
        }
    }

    /// <summary>
    ///  A class indicates the RopCopyTo ROP Response Buffer.
    /// </summary>
    public class RopCopyToResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the SourceHandleIndex field specified in the request.
        public byte SourceHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An unsigned integer that specifies the number of PropertyProblem structures in the PropertyProblems field. 
        public ushort? PropertyProblemCount;

        // An array of PropertyProblem structures. 
        public PropertyProblem[] PropertyProblems;

        // An unsigned integer index that MUST be set to the value specified in the DestHandleIndex field in the request.
        public uint? DestHandleIndex;

        /// <summary>
        /// Parse the RopCopyToResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopCopyToResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.SourceHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.PropertyProblemCount = ReadUshort();
                PropertyProblem[] InterPropertyProblem = new PropertyProblem[(int)this.PropertyProblemCount];
                for (int i = 0; i < this.PropertyProblemCount; i++)
                {
                    InterPropertyProblem[i] = new PropertyProblem();
                    InterPropertyProblem[i].Parse(s);
                }
                this.PropertyProblems = InterPropertyProblem;
            }
            else if ((AdditionalErrorCodes)this.ReturnValue == AdditionalErrorCodes.NullDestinationObject)
            {
                this.DestHandleIndex = ReadUint();
            }
        }
    }
    #endregion

    #region 2.2.2.12 RopGetPropertyIdsFromNames
    /// <summary>
    ///  A class indicates the RopGetPropertyIdsFromNames ROP Request Buffer.
    /// </summary>
    public class RopGetPropertyIdsFromNamesRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An integer that specifies whether to create a new entry.
        public byte Flags;

        // An unsigned integer that specifies the number of structures in the PropertyNames field.
        public ushort PropertyNameCount;

        // A list of PropertyName structures that specifies the property names requested.
        public PropertyName[] PropertyNames;

        /// <summary>
        /// Parse the RopGetPropertyIdsFromNamesRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetPropertyIdsFromNamesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.Flags = ReadByte();
            this.PropertyNameCount = ReadUshort();
            this.PropertyNames = new PropertyName[(int)this.PropertyNameCount];
            for (int i = 0; i < this.PropertyNameCount; i++)
            {
                PropertyNames[i] = new PropertyName();
                PropertyNames[i].Parse(s);
            }
        }
    }

    /// <summary>
    ///  A class indicates the RopGetPropertyIdsFromNames ROP Response Buffer.
    /// </summary>
    public class RopGetPropertyIdsFromNamesResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An unsigned integer that specifies the number of integers contained in the PropertyIds field.
        public ushort? PropertyIdCount;

        // An array of unsigned 16-bit integers. Each integer in the array is the property ID associated with a property name
        public ushort?[] PropertyIds;

        /// <summary>
        /// Parse the RopGetPropertyIdsFromNamesResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetPropertyIdsFromNamesResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.PropertyIdCount = ReadUshort();
                this.PropertyIds = ConvertArray(new ushort[(int)this.PropertyIdCount]);
                for (int i = 0; i < this.PropertyIdCount; i++)
                {
                    PropertyIds[i] = ReadUshort();
                }
            }
        }
    }
    #endregion

    #region 2.2.2.13 RopGetNamesFromPropertyIds
    /// <summary>
    ///  A class indicates the RopGetNamesFromPropertyIds ROP Request Buffer.
    /// </summary>
    public class RopGetNamesFromPropertyIdsRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the number of integers contained in the PropertyIds field.
        public ushort PropertyIdCount;

        // An array of unsigned 16-bit integers.
        public ushort[] PropertyIds;

        /// <summary>
        /// Parse the RopGetNamesFromPropertyIdsRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetNamesFromPropertyIdsRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.PropertyIdCount = ReadUshort();
            this.PropertyIds = new ushort[(int)this.PropertyIdCount];
            for (int i = 0; i < this.PropertyIdCount; i++)
            {
                PropertyIds[i] = ReadUshort();
            }
        }
    }

    /// <summary>
    ///  A class indicates the RopGetNamesFromPropertyIds ROP Response Buffer.
    /// </summary>
    public class RopGetNamesFromPropertyIdsResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An unsigned integer that specifies the number of structures in the PropertyNames field.
        public ushort? PropertyNameCount;

        // A list of PropertyName structures that specifies the property names requested.
        public PropertyName[] PropertyNames;


        /// <summary>
        /// Parse the RopGetNamesFromPropertyIdsResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetNamesFromPropertyIdsResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.PropertyNameCount = ReadUshort();
                this.PropertyNames = new PropertyName[(int)this.PropertyNameCount];
                for (int i = 0; i < this.PropertyNameCount; i++)
                {
                    PropertyNames[i] = new PropertyName();
                    PropertyNames[i].Parse(s);
                }
            }
        }
    }
    #endregion

    #region 2.2.2.14 RopOpenStream
    /// <summary>
    ///  A class indicates the RopOpenStream ROP Request Buffer.
    /// </summary>
    public class RopOpenStreamRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // n unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        public byte OutputHandleIndex;

        // A PropertyTag structure that specifies the property of the object to stream. 
        public PropertyTag PropertyTag;

        // A flags structure that contains flags that control how the stream is opened. 
        public OpenModeFlags OpenModeFlags;

        /// <summary>
        /// Parse the RopOpenStreamRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopOpenStreamRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.OutputHandleIndex = ReadByte();
            this.PropertyTag = new PropertyTag();
            this.PropertyTag.Parse(s);
            this.OpenModeFlags = (OpenModeFlags)ReadByte();

        }
    }

    /// <summary>
    ///  A class indicates the RopOpenStream ROP Response Buffer.
    /// </summary>
    public class RopOpenStreamResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An unsigned integer that indicates the size of the stream opened.
        public uint? StreamSize;

        /// <summary>
        /// Parse the RopOpenStreamResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopOpenStreamResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.StreamSize = ReadUint();
            }
        }
    }
    #endregion

    #region 2.2.2.15 RopReadStream
    /// <summary>
    ///  A class indicates the RopReadStream ROP Request Buffer.
    /// </summary>
    public class RopReadStreamRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the maximum number of bytes to read if the value is not equal to 0xBABE.
        public ushort ByteCount;

        // An unsigned integer that specifies the maximum number of bytes to read if the value of the ByteCount field is equal to 0xBABE.
        public uint MaximumByteCount;

        /// <summary>
        /// Parse the RopReadStreamRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopReadStreamRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.ByteCount = ReadUshort();
            if (this.ByteCount == 0xBABE)
            {
                this.MaximumByteCount = ReadUint();
            }
        }
    }

    /// <summary>
    ///  A class indicates the RopReadStream ROP Response Buffer.
    /// </summary>
    public class RopReadStreamResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An unsigned integer that specifies the size, in bytes, of the Data field.
        public ushort DataSize;

        // An array of bytes that are the bytes read from the stream.
        public byte[] Data;

        /// <summary>
        /// Parse the RopReadStreamResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopReadStreamResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            this.DataSize = ReadUshort();
            this.Data = ReadBytes((int)this.DataSize);
        }
    }
    #endregion

    #region 2.2.2.16 RopWriteStream
    /// <summary>
    ///  A class indicates the RopWriteStream ROP Request Buffer.
    /// </summary>
    public class RopWriteStreamRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the size of the Data field.
        public ushort DataSize;

        // An array of bytes that specifies the bytes to be written to the stream. The size of this field, in bytes, is specified by the DataSize field.
        public byte[] Data;

        /// <summary>
        /// Parse the RopWriteStreamRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopWriteStreamRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.DataSize = ReadUshort();
            this.Data = ReadBytes((int)this.DataSize);
        }
    }

    /// <summary>
    ///  A class indicates the RopWriteStream ROP Response Buffer.
    /// </summary>
    public class RopWriteStreamResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An unsigned integer that specifies the number of bytes actually written.
        public ushort WrittenSize;

        /// <summary>
        /// Parse the RopWriteStreamResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopWriteStreamResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            this.WrittenSize = ReadUshort();
        }
    }
    #endregion

    #region 2.2.2.17 RopCommitStream
    /// <summary>
    ///  A class indicates the RopCommitStream ROP Request Buffer.
    /// </summary>
    public class RopCommitStreamRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        /// <summary>
        /// Parse the RopCommitStreamRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopCommitStreamRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopCommitStream ROP Response Buffer.
    /// </summary>
    public class RopCommitStreamResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        /// <summary>
        /// Parse the RopCommitStreamResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopCommitStreamResponse structure.</param>
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

    #region 2.2.2.18 RopGetStreamSize
    /// <summary>
    ///  A class indicates the RopGetStreamSize ROP Request Buffer.
    /// </summary>
    public class RopGetStreamSizeRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        /// <summary>
        /// Parse the RopGetStreamSizeRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetStreamSizeRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopGetStreamSize ROP Response Buffer.
    /// </summary>
    public class RopGetStreamSizeResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An unsigned integer that is the current size of the stream.
        public uint StreamSize;

        /// <summary>
        /// Parse the RopGetStreamSizeResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetStreamSizeResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.StreamSize = ReadUint();
            }
        }
    }
    #endregion

    #region 2.2.2.19 RopSetStreamSize
    /// <summary>
    ///  A class indicates the RopSetStreamSize ROP Request Buffer.
    /// </summary>
    public class RopSetStreamSizeRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the size of the stream.
        public ulong StreamSize;

        /// <summary>
        /// Parse the RopSetStreamSizeRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopSetStreamSizeRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.StreamSize = ReadUlong();
        }
    }

    /// <summary>
    ///  A class indicates the RopSetStreamSize ROP Response Buffer.
    /// </summary>
    public class RopSetStreamSizeResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        /// <summary>
        /// Parse the RopSetStreamSizeResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopSetStreamSizeResponse structure.</param>
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

    #region 2.2.2.20 RopSeekStream
    /// <summary>
    ///  A class indicates the RopSeekStream ROP Request Buffer.
    /// </summary>
    public class RopSeekStreamRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An enumeration that specifies the origin location for the seek operation.
        public Origin Origin;

        //  An unsigned integer that specifies the seek offset.
        public ulong Offset;

        /// <summary>
        /// Parse the RopSeekStreamRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopSeekStreamRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.Origin = (Origin)ReadByte();
            this.Offset = ReadUlong();
        }
    }

    /// <summary>
    ///  A class indicates the RopSeekStream ROP Response Buffer.
    /// </summary>
    public class RopSeekStreamResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An unsigned integer that represents the new position in the stream after the operation.
        public ulong? NewPosition;

        /// <summary>
        /// Parse the RopSeekStreamResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopSeekStreamResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.NewPosition = ReadUlong();
            }
        }
    }
    #endregion

    #region 2.2.2.21 RopCopyToStream
    /// <summary>
    ///  A class indicates the RopCopyToStream ROP Request Buffer.
    /// </summary>
    public class RopCopyToStreamRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the source Server object is stored.
        public byte SourceHandleIndex;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the destination Server object is stored.
        public byte DestHandleIndex;

        // An unsigned integer that specifies the number of bytes to be copied.
        public ulong ByteCount;

        /// <summary>
        /// Parse the RopCopyToStreamRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopCopyToStreamRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.SourceHandleIndex = ReadByte();
            this.DestHandleIndex = ReadByte();
            this.ByteCount = ReadUlong();
        }
    }

    /// <summary>
    ///  A class indicates the RopCopyToStream ROP Response Buffer.
    /// </summary>
    public class RopCopyToStreamResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the SourceHandleIndex field in the request.
        public byte SourceHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An unsigned integer index that MUST be set to the value specified in the DestHandleIndex field in the request.
        public uint? DestHandleIndex;

        // An unsigned integer that specifies the number of bytes read from the source object.
        public ulong ReadByteCount;

        // An unsigned integer that specifies the number of bytes written to the destination object.
        public ulong WrittenByteCount;

        /// <summary>
        /// Parse the RopCopyToStreamResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopCopyToStreamResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.SourceHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((AdditionalErrorCodes)ReturnValue == AdditionalErrorCodes.NullDestinationObject)
            {
                this.DestHandleIndex = ReadUint();
            }
            this.ReadByteCount = ReadUlong();
            this.WrittenByteCount = ReadUlong();
        }
    }
    #endregion

    #region 2.2.2.22 RopProgress
    /// <summary>
    ///  A class indicates the RopProgress ROP Request Buffer.
    /// </summary>
    public class RopProgressRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // A Boolean that specifies whether to cancel the operation.
        public bool WantCancel;

        /// <summary>
        /// Parse the RopProgressRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopProgressRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.WantCancel = ReadBoolean();
        }
    }

    /// <summary>
    ///  A class indicates the RopProgress ROP Response Buffer.
    /// </summary>
    public class RopProgressResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An unsigned integer that specifies the logon associated with this operation.
        public byte? LogonId;

        // An unsigned integer that specifies the number of tasks completed.
        public uint? CompletedTaskCount;

        // An unsigned integer that specifies the total number of tasks.
        public uint? TotalTaskCount;

        /// <summary>
        /// Parse the RopProgressResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopProgressResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.LogonId = ReadByte();
                this.CompletedTaskCount = ReadUint();
                this.TotalTaskCount = ReadUint();
            }
        }
    }
    #endregion

    #region 2.2.2.23 RopLockRegionStream
    /// <summary>
    ///  A class indicates the RopLockRegionStream ROP Request Buffer.
    /// </summary>
    public class RopLockRegionStreamRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the byte location in the stream where the region begins.
        public ulong RegionOffset;

        // An unsigned integer that specifies the size of the region, in bytes.
        public ulong RegionSize;

        // A flags structure that contains flags specifying the behavior of the lock operation. 
        public uint LockFlags;

        /// <summary>
        /// Parse the RopLockRegionStreamRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopLockRegionStreamRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.RegionOffset = ReadUlong();
            this.RegionSize = ReadUlong();
            this.LockFlags = ReadUint();
        }
    }

    /// <summary>
    ///  A class indicates the RopLockRegionStream ROP Response Buffer.
    /// </summary>
    public class RopLockRegionStreamResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        /// <summary>
        /// Parse the RopLockRegionStreamResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopLockRegionStreamResponse structure.</param>
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

    #region 2.2.2.24 RopUnlockRegionStream
    /// <summary>
    ///  A class indicates the RopUnlockRegionStream ROP Request Buffer.
    /// </summary>
    public class RopUnlockRegionStreamRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the byte location in the stream where the region begins.
        public ulong RegionOffset;

        // An unsigned integer that specifies the size of the region, in bytes.
        public ulong RegionSize;

        // A flags structure that contains flags specifying the behavior of the lock operation. 
        public uint LockFlags;

        /// <summary>
        /// Parse the RopUnlockRegionStreamRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopUnlockRegionStreamRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.RegionOffset = ReadUlong();
            this.RegionSize = ReadUlong();
            this.LockFlags = ReadUint();
        }
    }

    /// <summary>
    ///  A class indicates the RopUnlockRegionStream ROP Response Buffer.
    /// </summary>
    public class RopUnlockRegionStreamResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        /// <summary>
        /// Parse the RopUnlockRegionStreamResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopUnlockRegionStreamResponse structure.</param>
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

    #region 2.2.2.25 RopWriteAndCommitStream
    /// <summary>
    ///  A class indicates the RopWriteAndCommitStream ROP Request Buffer.
    /// </summary>
    public class RopWriteAndCommitStreamRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the size of the Data field.
        public ushort DataSize;

        // An array of bytes that specifies the bytes to be written to the stream. The size of this field, in bytes, is specified by the DataSize field.
        public byte[] Data;

        /// <summary>
        /// Parse the RopWriteAndCommitStreamRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopWriteAndCommitStreamRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.DataSize = ReadUshort();
            this.Data = ReadBytes((int)this.DataSize);
        }
    }

    /// <summary>
    ///  A class indicates the RopWriteAndCommitStream ROP Response Buffer.
    /// </summary>
    public class RopWriteAndCommitStreamResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An unsigned integer that specifies the number of bytes actually written.
        public ushort WrittenSize;

        /// <summary>
        /// Parse the RopWriteAndCommitStreamResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopWriteAndCommitStreamResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            this.WrittenSize = ReadUshort();
        }
    }
    #endregion

    #region 2.2.2.26 RopCloneStream
    /// <summary>
    ///  A class indicates the RopCloneStream ROP Request Buffer.
    /// </summary>
    public class RopCloneStreamRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        public byte OutputHandleIndex;

        /// <summary>
        /// Parse the RopCloneStreamRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopCloneStreamRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.OutputHandleIndex = ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopCloneStream ROP Response Buffer.
    /// </summary>
    public class RopCloneStreamResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        /// <summary>
        /// Parse the RopCloneStreamResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopCloneStreamResponse structure.</param>
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

    # region Enum
    /// <summary>
    ///  A flags structure that contains flags that control options for moving or copying properties.
    /// </summary>
    [Flags]
    public enum CopyFlags : byte
    {
        Move = 0x01,
        NoOverwrite = 0x02,
    }

    /// <summary>
    /// A flags structure that contains flags that control how the stream is opened.
    /// </summary>
    public enum OpenModeFlags : byte
    {
        ReadOnly = 0x00,
        ReadWrite = 0x01,
        Create = 0x02
    }

    /// <summary>
    /// An enumeration that specifies the origin location for the seek operation.
    /// </summary>
    public enum Origin : byte
    {
        Beginning = 0x00,
        Current = 0x01,
        End = 0x02
    }
    #endregion
}

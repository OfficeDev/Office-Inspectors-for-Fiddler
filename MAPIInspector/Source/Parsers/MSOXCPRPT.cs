namespace MAPIInspector.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    #region Enum
    /// <summary>
    ///  A flags structure that contains flags that control options for moving or copying properties.
    /// </summary>
    [Flags]
    public enum CopyFlags : byte
    {
        /// <summary>
        /// If this bit is set, properties are moved; otherwise, properties are copied
        /// </summary>
        Move = 0x01,

        /// <summary>
        /// Properties that already have a value on the destination object will not be overwritten
        /// </summary>
        NoOverwrite = 0x02,
    }

    /// <summary>
    /// A flags structure that contains flags that control how the stream is opened.
    /// </summary>
    public enum OpenModeFlags : byte
    {
        /// <summary>
        /// Open the stream for read-only access.
        /// </summary>
        ReadOnly = 0x00,

        /// <summary>
        /// Open the stream for read/write access.
        /// </summary>
        ReadWrite = 0x01,

        /// <summary>
        /// Open a new stream. This mode will delete the current property value and open the stream for read/write access
        /// </summary>
        Create = 0x02
    }

    /// <summary>
    /// An enumeration that specifies the origin location for the seek operation.
    /// </summary>
    public enum Origin : byte
    {
        /// <summary>
        /// The point of origin is the beginning of the stream.
        /// </summary>
        Beginning = 0x00,

        /// <summary>
        /// The point of origin is the location of the current seek pointer.
        /// </summary>
        Current = 0x01,

        /// <summary>
        /// The point of origin is the end of the stream.
        /// </summary>
        End = 0x02
    }
    #endregion

    #region 2.2.2.2 RopGetPropertiesSpecific
    /// <summary>
    ///  A class indicates the RopGetPropertiesSpecific ROP Request Buffer.
    /// </summary>
    public class RopGetPropertiesSpecificRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the maximum size allowed for a property value returned.
        /// </summary>
        public ushort PropertySizeLimit;

        /// <summary>
        /// A Boolean that specifies whether to return string properties in multibyte Unicode.
        /// </summary>
        public ushort WantUnicode;

        /// <summary>
        /// An unsigned integer that specifies the number of tags present in the PropertyTags field.
        /// </summary>
        public ushort PropertyTagCount;

        /// <summary>
        /// An array of PropertyTag structures that specifies the properties requested.
        /// </summary>
        public PropertyTag[] PropertyTags;

        /// <summary>
        /// Parse the RopGetPropertiesSpecificRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetPropertiesSpecificRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.PropertySizeLimit = this.ReadUshort();
            this.WantUnicode = this.ReadUshort();
            this.PropertyTagCount = this.ReadUshort();
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
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
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
        /// A PropertyRow structure. 
        /// </summary>
        public PropertyRow RowData;

        /// <summary>
        /// Parse the RopGetPropertiesSpecificResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetPropertiesSpecificResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                PropertyTag[] proTags = new PropertyTag[0];
                if (!MapiInspector.MAPIParser.IsFromFiddlerCore(MapiInspector.MAPIParser.ParsingSession))
                {
                    proTags = DecodingContext.GetPropertiesSpec_propertyTags[MapiInspector.MAPIParser.ParsingSession.id][this.InputHandleIndex].Dequeue();
                }
                else
                {
                    proTags = DecodingContext.GetPropertiesSpec_propertyTags[int.Parse(MapiInspector.MAPIParser.ParsingSession["VirtualID"])][this.InputHandleIndex].Dequeue();
                }
                this.RowData = new PropertyRow(proTags);
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
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the maximum size allowed for a property value returned.
        /// </summary>
        public ushort PropertySizeLimit;

        /// <summary>
        /// A Boolean that specifies whether to return string properties in multibyte Unicode.
        /// </summary>
        public ushort WantUnicode;

        /// <summary>
        /// Parse the RopGetPropertiesAllRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetPropertiesAllRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.PropertySizeLimit = this.ReadUshort();
            this.WantUnicode = this.ReadUshort();
        }
    }

    /// <summary>
    ///  A class indicates the RopGetPropertiesAll ROP Response Buffer.
    /// </summary>
    public class RopGetPropertiesAllResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the number of structures present in the PropertyValues field.
        /// </summary>
        public ushort? PropertyValueCount;

        /// <summary>
        /// An array of TaggedPropertyValue structures that are the properties defined on the object.
        /// </summary>
        public TaggedPropertyValue[] PropertyValues;

        /// <summary>
        /// Parse the RopGetPropertiesAllResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetPropertiesAllResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.PropertyValueCount = this.ReadUshort();
                TaggedPropertyValue[] interValue = new TaggedPropertyValue[(int)this.PropertyValueCount];

                for (int i = 0; i < this.PropertyValueCount; i++)
                {
                    interValue[i] = new TaggedPropertyValue();
                    interValue[i].Parse(s);
                }

                this.PropertyValues = interValue;
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
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// Parse the RopGetPropertiesListRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetPropertiesListRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopGetPropertiesList ROP Response Buffer.
    /// </summary>
    public class RopGetPropertiesListResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the number of property tags in the PropertyTags field.
        /// </summary>
        public ushort? PropertyTagCount;

        /// <summary>
        /// An array of PropertyTag structures that lists the property tags on the object.
        /// </summary>
        public PropertyTag[] PropertyTags;

        /// <summary>
        /// Parse the RopGetPropertiesListResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetPropertiesListResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.PropertyTagCount = this.ReadUshort();
                PropertyTag[] interTag = new PropertyTag[(int)this.PropertyTagCount];

                for (int i = 0; i < this.PropertyTagCount; i++)
                {
                    interTag[i] = new PropertyTag();
                    interTag[i].Parse(s);
                }

                this.PropertyTags = interTag;
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
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the number of bytes used for the PropertyValueCount field and the PropertyValues field.
        /// </summary>
        public ushort PropertyValueSize;

        /// <summary>
        /// An unsigned integer that specifies the number of PropertyValue structures listed in the PropertyValues field.
        /// </summary>
        public ushort PropertyValueCount;

        /// <summary>
        /// An array of TaggedPropertyValue structures that specifies the property values to be set on the object.
        /// </summary>
        public TaggedPropertyValue[] PropertyValues;

        /// <summary>
        /// Parse the RopSetPropertiesRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSetPropertiesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.PropertyValueSize = this.ReadUshort();
            this.PropertyValueCount = this.ReadUshort();
            TaggedPropertyValue[] interValue = new TaggedPropertyValue[(int)this.PropertyValueCount];

            for (int i = 0; i < this.PropertyValueCount; i++)
            {
                interValue[i] = new TaggedPropertyValue();
                interValue[i].Parse(s);
            }

            this.PropertyValues = interValue;
        }
    }

    /// <summary>
    ///  A class indicates the RopSetProperties ROP Response Buffer.
    /// </summary>
    public class RopSetPropertiesResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the number of PropertyProblem structures in the PropertyProblems field. 
        /// </summary>
        public ushort? PropertyProblemCount;

        /// <summary>
        /// An array of PropertyProblem structures. The number of structures contained in this field is specified by the PropertyProblemCount field. 
        /// </summary>
        public PropertyProblem[] PropertyProblems;

        /// <summary>
        /// Parse the RopSetPropertiesResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSetPropertiesResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            List<PropertyRow> tmpRows = new List<PropertyRow>();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.PropertyProblemCount = this.ReadUshort();
                PropertyProblem[] interPropertyProblem = new PropertyProblem[(int)this.PropertyProblemCount];

                for (int i = 0; i < this.PropertyProblemCount; i++)
                {
                    interPropertyProblem[i] = new PropertyProblem();
                    interPropertyProblem[i].Parse(s);
                }

                this.PropertyProblems = interPropertyProblem;
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
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the number of bytes used for the PropertyValueCount field and the PropertyValues field.
        /// </summary>
        public ushort PropertyValueSize;

        /// <summary>
        /// An unsigned integer that specifies the number of structures listed in the PropertyValues field.
        /// </summary>
        public ushort PropertyValueCount;

        /// <summary>
        /// PropertyValues (variable):  An array of TaggedPropertyValue structures that specifies the property values to be set on the object. 
        /// </summary>
        public TaggedPropertyValue[] PropertyValues;

        /// <summary>
        /// Parse the RopSetPropertiesNoReplicateRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSetPropertiesNoReplicateRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.PropertyValueSize = this.ReadUshort();
            this.PropertyValueCount = this.ReadUshort();
            TaggedPropertyValue[] interValue = new TaggedPropertyValue[(int)this.PropertyValueCount];

            for (int i = 0; i < this.PropertyValueCount; i++)
            {
                interValue[i] = new TaggedPropertyValue();
                interValue[i].Parse(s);
            }

            this.PropertyValues = interValue;
        }
    }

    /// <summary>
    ///  A class indicates the RopSetPropertiesNoReplicate ROP Response Buffer.
    /// </summary>
    public class RopSetPropertiesNoReplicateResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the number of PropertyProblem structures in the PropertyProblems field. 
        /// </summary>
        public ushort? PropertyProblemCount;

        /// <summary>
        /// An array of PropertyProblem structures. The number of structures contained in this field is specified by the PropertyProblemCount field.
        /// </summary>
        public PropertyProblem[] PropertyProblems;

        /// <summary>
        /// Parse the RopSetPropertiesNoReplicateResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSetPropertiesNoReplicateResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.PropertyProblemCount = this.ReadUshort();
                PropertyProblem[] interPropertyProblem = new PropertyProblem[(int)this.PropertyProblemCount];

                for (int i = 0; i < this.PropertyProblemCount; i++)
                {
                    interPropertyProblem[i] = new PropertyProblem();
                    interPropertyProblem[i].Parse(s);
                }

                this.PropertyProblems = interPropertyProblem;
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
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the number of PropertyTag structures in the PropertyTags field. 
        /// </summary>
        public ushort PropertyTagCount;

        /// <summary>
        /// An array of PropertyTag structures that specifies the property values to be deleted from the object. 
        /// </summary>
        public PropertyTag[] PropertyTags;

        /// <summary>
        /// Parse the RopDeletePropertiesRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopDeletePropertiesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.PropertyTagCount = this.ReadUshort();
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
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the number of PropertyProblem structures in the PropertyProblems field. 
        /// </summary>
        public ushort? PropertyProblemCount;

        /// <summary>
        /// An array of PropertyProblem structures. The number of structures contained in this field is specified by the PropertyProblemCount field. 
        /// </summary>
        public PropertyProblem[] PropertyProblems;

        /// <summary>
        /// Parse the RopDeletePropertiesResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopDeletePropertiesResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.PropertyProblemCount = this.ReadUshort();
                PropertyProblem[] interPropertyProblem = new PropertyProblem[(int)this.PropertyProblemCount];

                for (int i = 0; i < this.PropertyProblemCount; i++)
                {
                    interPropertyProblem[i] = new PropertyProblem();
                    interPropertyProblem[i].Parse(s);
                }

                this.PropertyProblems = interPropertyProblem;
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
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the number of PropertyTag structures in the PropertyTags field. 
        /// </summary>
        public ushort PropertyTagCount;

        /// <summary>
        /// An array of PropertyTag structures that specifies the property values to be deleted from the object. 
        /// </summary>
        public PropertyTag[] PropertyTags;

        /// <summary>
        /// Parse the RopDeletePropertiesNoReplicateRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopDeletePropertiesNoReplicateRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.PropertyTagCount = this.ReadUshort();
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
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the number of PropertyProblem structures in the PropertyProblems field. 
        /// </summary>
        public ushort? PropertyProblemCount;

        /// <summary>
        /// An array of PropertyProblem structures. The number of structures contained in this field is specified by the PropertyProblemCount field. 
        /// </summary>
        public PropertyProblem[] PropertyProblems;

        /// <summary>
        /// Parse the RopDeletePropertiesNoReplicateResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopDeletePropertiesNoReplicateResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.PropertyProblemCount = this.ReadUshort();
                PropertyProblem[] interPropertyProblem = new PropertyProblem[(int)this.PropertyProblemCount];

                for (int i = 0; i < this.PropertyProblemCount; i++)
                {
                    interPropertyProblem[i] = new PropertyProblem();
                    interPropertyProblem[i].Parse(s);
                }

                this.PropertyProblems = interPropertyProblem;
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
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// A flags structure that contains flags control how this ROP behaves.
        /// </summary>
        public byte QueryFlags;

        /// <summary>
        /// A Boolean that specifies whether the PropertyGuid field is present.
        /// </summary>
        public byte HasGuid;

        /// <summary>
        /// A GUID that is present if HasGuid is nonzero and is not present if the value of the HasGuid field is zero.
        /// </summary>
        public Guid? PropertyGuid;

        /// <summary>
        /// Parse the RopQueryNamedPropertiesRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopQueryNamedPropertiesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.QueryFlags = this.ReadByte();
            this.HasGuid = this.ReadByte();

            if (this.HasGuid != 0)
            {
                this.PropertyGuid = this.ReadGuid();
            }
        }
    }

    /// <summary>
    ///  A class indicates the RopQueryNamedProperties ROP Response Buffer.
    /// </summary>
    public class RopQueryNamedPropertiesResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the number of elements contained in the PropertyIds and PropertyNames fields.
        /// </summary>
        public ushort? IdCount;

        /// <summary>
        /// An array of unsigned 16-bit integers. Each integer in the array is the property ID associated with a property name.
        /// </summary>
        public ushort?[] PropertyIds;

        /// <summary>
        /// A list of PropertyName structures that specifies the property names for the property IDs specified in the PropertyIds field. 
        /// </summary>
        public PropertyName[] PropertyNames;

        /// <summary>
        /// Parse the RopQueryNamedPropertiesResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopQueryNamedPropertiesResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.IdCount = this.ReadUshort();
                this.PropertyIds = this.ConvertArray(new ushort[(int)this.IdCount]);
                this.PropertyNames = new PropertyName[(int)this.IdCount];

                for (int i = 0; i < this.IdCount; i++)
                {
                    this.PropertyIds[i] = this.ReadUshort();
                }

                for (int i = 0; i < this.IdCount; i++)
                {
                    this.PropertyNames[i] = new PropertyName();
                    this.PropertyNames[i].Parse(s);
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
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the source Server object is stored.
        /// </summary>
        public byte SourceHandleIndex;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the destination Server object is stored.
        /// </summary>
        public byte DestHandleIndex;

        /// <summary>
        /// A Boolean that specifies whether the operation is to be processed asynchronously with status reported via the RopProgress ROP 
        /// </summary>
        public bool WantAsynchronous;

        /// <summary>
        /// A flags structure that contains flags that control the operation behavior.
        /// </summary>
        public CopyFlags CopyFlags;

        /// <summary>
        /// An unsigned integer that specifies how many tags are present in the PropertyTags field.
        /// </summary>
        public ushort PropertyTagCount;

        /// <summary>
        /// An array of PropertyTag structures that specifies the properties to copy.
        /// </summary>
        public PropertyTag[] PropertyTags;

        /// <summary>
        /// Parse the RopCopyPropertiesRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopCopyPropertiesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.SourceHandleIndex = this.ReadByte();
            this.DestHandleIndex = this.ReadByte();
            this.WantAsynchronous = this.ReadBoolean();
            this.CopyFlags = (CopyFlags)this.ReadByte();
            this.PropertyTagCount = this.ReadUshort();
            this.PropertyTags = new PropertyTag[(int)this.PropertyTagCount];

            for (int i = 0; i < this.PropertyTagCount; i++)
            {
                this.PropertyTags[i] = new PropertyTag();
                this.PropertyTags[i].Parse(s);
            }
        }
    }

    /// <summary>
    ///  A class indicates the RopCopyProperties ROP Response Buffer.
    /// </summary>
    public class RopCopyPropertiesResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the SourceHandleIndex field specified in the request.
        /// </summary>
        public byte SourceHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the number of PropertyProblem structures in the PropertyProblems field. 
        /// </summary>
        public ushort? PropertyProblemCount;

        /// <summary>
        /// An array of PropertyProblem structures. 
        /// </summary>
        public PropertyProblem[] PropertyProblems;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the DestHandleIndex field in the request.
        /// </summary>
        public uint? DestHandleIndex;

        /// <summary>
        /// Parse the RopCopyPropertiesResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopCopyPropertiesResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.SourceHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.PropertyProblemCount = this.ReadUshort();
                PropertyProblem[] interPropertyProblem = new PropertyProblem[(int)this.PropertyProblemCount];

                for (int i = 0; i < this.PropertyProblemCount; i++)
                {
                    interPropertyProblem[i] = new PropertyProblem();
                    interPropertyProblem[i].Parse(s);
                }

                this.PropertyProblems = interPropertyProblem;
            }
            else if ((AdditionalErrorCodes)this.ReturnValue == AdditionalErrorCodes.NullDestinationObject)
            {
                this.DestHandleIndex = this.ReadUint();
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
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the source Server object is stored.
        /// </summary>
        public byte SourceHandleIndex;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the destination Server object is stored.
        /// </summary>
        public byte DestHandleIndex;

        /// <summary>
        /// A Boolean that specifies whether the operation is to be processed asynchronously with status reported via the RopProgress ROP
        /// </summary>
        public bool WantAsynchronous;

        /// <summary>
        /// A Boolean that specifies whether to copy subobjects.
        /// </summary>
        public bool WantSubObjects;

        /// <summary>
        /// A flags structure that contains flags that control the operation behavior.
        /// </summary>
        public CopyFlags CopyFlags;

        /// <summary>
        /// An unsigned integer that specifies how many tags are present in the ExcludedTags field.
        /// </summary>
        public ushort ExcludedTagCount;

        /// <summary>
        /// An array of PropertyTag structures that specifies the properties to exclude from the copy. 
        /// </summary>
        public PropertyTag[] ExcludedTags;

        /// <summary>
        /// Parse the RopCopyToRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopCopyToRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.SourceHandleIndex = this.ReadByte();
            this.DestHandleIndex = this.ReadByte();
            this.WantAsynchronous = this.ReadBoolean();
            this.WantSubObjects = this.ReadBoolean();
            this.CopyFlags = (CopyFlags)this.ReadByte();
            this.ExcludedTagCount = this.ReadUshort();
            this.ExcludedTags = new PropertyTag[(int)this.ExcludedTagCount];

            for (int i = 0; i < this.ExcludedTagCount; i++)
            {
                this.ExcludedTags[i] = new PropertyTag();
                this.ExcludedTags[i].Parse(s);
            }
        }
    }

    /// <summary>
    ///  A class indicates the RopCopyTo ROP Response Buffer.
    /// </summary>
    public class RopCopyToResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the SourceHandleIndex field specified in the request.
        /// </summary>
        public byte SourceHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the number of PropertyProblem structures in the PropertyProblems field. 
        /// </summary>
        public ushort? PropertyProblemCount;

        /// <summary>
        /// An array of PropertyProblem structures. 
        /// </summary>
        public PropertyProblem[] PropertyProblems;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the DestHandleIndex field in the request.
        /// </summary>
        public uint? DestHandleIndex;

        /// <summary>
        /// Parse the RopCopyToResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopCopyToResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.SourceHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.PropertyProblemCount = this.ReadUshort();
                PropertyProblem[] interPropertyProblem = new PropertyProblem[(int)this.PropertyProblemCount];

                for (int i = 0; i < this.PropertyProblemCount; i++)
                {
                    interPropertyProblem[i] = new PropertyProblem();
                    interPropertyProblem[i].Parse(s);
                }

                this.PropertyProblems = interPropertyProblem;
            }
            else if ((AdditionalErrorCodes)this.ReturnValue == AdditionalErrorCodes.NullDestinationObject)
            {
                this.DestHandleIndex = this.ReadUint();
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
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An integer that specifies whether to create a new entry.
        /// </summary>
        public byte Flags;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the PropertyNames field.
        /// </summary>
        public ushort PropertyNameCount;

        /// <summary>
        /// A list of PropertyName structures that specifies the property names requested.
        /// </summary>
        public PropertyName[] PropertyNames;

        /// <summary>
        /// Parse the RopGetPropertyIdsFromNamesRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetPropertyIdsFromNamesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.Flags = this.ReadByte();
            this.PropertyNameCount = this.ReadUshort();
            this.PropertyNames = new PropertyName[(int)this.PropertyNameCount];

            for (int i = 0; i < this.PropertyNameCount; i++)
            {
                this.PropertyNames[i] = new PropertyName();
                this.PropertyNames[i].Parse(s);
            }
        }
    }

    /// <summary>
    ///  A class indicates the RopGetPropertyIdsFromNames ROP Response Buffer.
    /// </summary>
    public class RopGetPropertyIdsFromNamesResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the number of integers contained in the PropertyIds field.
        /// </summary>
        public ushort? PropertyIdCount;

        /// <summary>
        /// An array of unsigned 16-bit integers. Each integer in the array is the property ID associated with a property name
        /// </summary>
        public ushort?[] PropertyIds;

        /// <summary>
        /// Parse the RopGetPropertyIdsFromNamesResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetPropertyIdsFromNamesResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.PropertyIdCount = this.ReadUshort();
                this.PropertyIds = this.ConvertArray(new ushort[(int)this.PropertyIdCount]);

                for (int i = 0; i < this.PropertyIdCount; i++)
                {
                    this.PropertyIds[i] = this.ReadUshort();
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
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the number of integers contained in the PropertyIds field.
        /// </summary>
        public ushort PropertyIdCount;

        /// <summary>
        /// An array of unsigned 16-bit integers.
        /// </summary>
        public ushort[] PropertyIds;

        /// <summary>
        /// Parse the RopGetNamesFromPropertyIdsRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetNamesFromPropertyIdsRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.PropertyIdCount = this.ReadUshort();
            this.PropertyIds = new ushort[(int)this.PropertyIdCount];

            for (int i = 0; i < this.PropertyIdCount; i++)
            {
                this.PropertyIds[i] = this.ReadUshort();
            }
        }
    }

    /// <summary>
    ///  A class indicates the RopGetNamesFromPropertyIds ROP Response Buffer.
    /// </summary>
    public class RopGetNamesFromPropertyIdsResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the PropertyNames field.
        /// </summary>
        public ushort? PropertyNameCount;

        /// <summary>
        /// A list of PropertyName structures that specifies the property names requested.
        /// </summary>
        public PropertyName[] PropertyNames;

        /// <summary>
        /// Parse the RopGetNamesFromPropertyIdsResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetNamesFromPropertyIdsResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.PropertyNameCount = this.ReadUshort();
                this.PropertyNames = new PropertyName[(int)this.PropertyNameCount];

                for (int i = 0; i < this.PropertyNameCount; i++)
                {
                    this.PropertyNames[i] = new PropertyName();
                    this.PropertyNames[i].Parse(s);
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
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
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
        /// A PropertyTag structure that specifies the property of the object to stream. 
        /// </summary>
        public PropertyTag PropertyTag;

        /// <summary>
        /// A flags structure that contains flags that control how the stream is opened. 
        /// </summary>
        public OpenModeFlags OpenModeFlags;

        /// <summary>
        /// Parse the RopOpenStreamRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopOpenStreamRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.PropertyTag = new PropertyTag();
            this.PropertyTag.Parse(s);
            this.OpenModeFlags = (OpenModeFlags)this.ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopOpenStream ROP Response Buffer.
    /// </summary>
    public class RopOpenStreamResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
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
        /// An unsigned integer that indicates the size of the stream opened.
        /// </summary>
        public uint? StreamSize;

        /// <summary>
        /// Parse the RopOpenStreamResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopOpenStreamResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.StreamSize = this.ReadUint();
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
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the maximum number of bytes to read if the value is not equal to 0xBABE.
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// An unsigned integer that specifies the maximum number of bytes to read if the value of the ByteCount field is equal to 0xBABE.
        /// </summary>
        public uint MaximumByteCount;

        /// <summary>
        /// Parse the RopReadStreamRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopReadStreamRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ByteCount = this.ReadUshort();

            if (this.ByteCount == 0xBABE)
            {
                this.MaximumByteCount = this.ReadUint();
            }
        }
    }

    /// <summary>
    ///  A class indicates the RopReadStream ROP Response Buffer.
    /// </summary>
    public class RopReadStreamResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the Data field.
        /// </summary>
        public ushort DataSize;

        /// <summary>
        /// An array of bytes that are the bytes read from the stream.
        /// </summary>
        public byte[] Data;

        /// <summary>
        /// Parse the RopReadStreamResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopReadStreamResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
            this.DataSize = this.ReadUshort();
            this.Data = this.ReadBytes((int)this.DataSize);
        }
    }
    #endregion

    #region 2.2.2.16 RopWriteStream
    /// <summary>
    ///  A class indicates the RopWriteStream ROP Request Buffer.
    /// </summary>
    public class RopWriteStreamRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the size of the Data field.
        /// </summary>
        public ushort DataSize;

        /// <summary>
        /// An array of bytes that specifies the bytes to be written to the stream. The size of this field, in bytes, is specified by the DataSize field.
        /// </summary>
        public byte[] Data;

        /// <summary>
        /// Parse the RopWriteStreamRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopWriteStreamRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.DataSize = this.ReadUshort();
            this.Data = this.ReadBytes((int)this.DataSize);
        }
    }

    /// <summary>
    ///  A class indicates the RopWriteStream ROP Response Buffer.
    /// </summary>
    public class RopWriteStreamResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the number of bytes actually written.
        /// </summary>
        public ushort WrittenSize;

        /// <summary>
        /// Parse the RopWriteStreamResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopWriteStreamResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
            this.WrittenSize = this.ReadUshort();
        }
    }
    #endregion

    #region 2.2.2.17 RopWriteStreamExtended
    /// <summary>
    ///  A class indicates the RopWriteStreamExtended ROP Request Buffer.
    /// </summary>
    public class RopWriteStreamExtendedRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the size of the Data field.
        /// </summary>
        public ushort DataSize;

        /// <summary>
        /// An array of bytes that specifies the bytes to be written to the stream. The size of this field, in bytes, is specified by the DataSize field.
        /// </summary>
        public byte[] Data;

        /// <summary>
        /// Parse the RopWriteStreamExtendedRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopWriteStreamExtendedRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.DataSize = this.ReadUshort();
            this.Data = this.ReadBytes((int)this.DataSize);
        }
    }

    /// <summary>
    ///  A class indicates the RopWriteStreamExtended ROP Response Buffer.
    /// </summary>
    public class RopWriteStreamExtendedResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the number of bytes actually written.
        /// </summary>
        public uint WrittenSize;

        /// <summary>
        /// Parse the RopWriteStreamExtendedResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopWriteStreamExtendedResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
            this.WrittenSize = this.ReadUint();
        }
    }
    #endregion

    #region 2.2.2.18 RopCommitStream
    /// <summary>
    ///  A class indicates the RopCommitStream ROP Request Buffer.
    /// </summary>
    public class RopCommitStreamRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// Parse the RopCommitStreamRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopCommitStreamRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopCommitStream ROP Response Buffer.
    /// </summary>
    public class RopCommitStreamResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// Parse the RopCommitStreamResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopCommitStreamResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
        }
    }
    #endregion

    #region 2.2.2.19 RopGetStreamSize
    /// <summary>
    ///  A class indicates the RopGetStreamSize ROP Request Buffer.
    /// </summary>
    public class RopGetStreamSizeRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// Parse the RopGetStreamSizeRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetStreamSizeRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopGetStreamSize ROP Response Buffer.
    /// </summary>
    public class RopGetStreamSizeResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer that is the current size of the stream.
        /// </summary>
        public uint StreamSize;

        /// <summary>
        /// Parse the RopGetStreamSizeResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetStreamSizeResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.StreamSize = this.ReadUint();
            }
        }
    }
    #endregion

    #region 2.2.2.20 RopSetStreamSize
    /// <summary>
    ///  A class indicates the RopSetStreamSize ROP Request Buffer.
    /// </summary>
    public class RopSetStreamSizeRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the size of the stream.
        /// </summary>
        public ulong StreamSize;

        /// <summary>
        /// Parse the RopSetStreamSizeRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSetStreamSizeRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.StreamSize = this.ReadUlong();
        }
    }

    /// <summary>
    ///  A class indicates the RopSetStreamSize ROP Response Buffer.
    /// </summary>
    public class RopSetStreamSizeResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// Parse the RopSetStreamSizeResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSetStreamSizeResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
        }
    }
    #endregion

    #region 2.2.2.21 RopSeekStream
    /// <summary>
    ///  A class indicates the RopSeekStream ROP Request Buffer.
    /// </summary>
    public class RopSeekStreamRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An enumeration that specifies the origin location for the seek operation.
        /// </summary>
        public Origin Origin;

        /// <summary>
        /// An unsigned integer that specifies the seek offset.
        /// </summary>
        public ulong Offset;

        /// <summary>
        /// Parse the RopSeekStreamRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSeekStreamRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.Origin = (Origin)this.ReadByte();
            this.Offset = this.ReadUlong();
        }
    }

    /// <summary>
    ///  A class indicates the RopSeekStream ROP Response Buffer.
    /// </summary>
    public class RopSeekStreamResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer that represents the new position in the stream after the operation.
        /// </summary>
        public ulong? NewPosition;

        /// <summary>
        /// Parse the RopSeekStreamResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSeekStreamResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.NewPosition = this.ReadUlong();
            }
        }
    }
    #endregion

    #region 2.2.2.22 RopCopyToStream
    /// <summary>
    ///  A class indicates the RopCopyToStream ROP Request Buffer.
    /// </summary>
    public class RopCopyToStreamRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the source Server object is stored.
        /// </summary>
        public byte SourceHandleIndex;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the destination Server object is stored.
        /// </summary>
        public byte DestHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the number of bytes to be copied.
        /// </summary>
        public ulong ByteCount;

        /// <summary>
        /// Parse the RopCopyToStreamRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopCopyToStreamRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.SourceHandleIndex = this.ReadByte();
            this.DestHandleIndex = this.ReadByte();
            this.ByteCount = this.ReadUlong();
        }
    }

    /// <summary>
    ///  A class indicates the RopCopyToStream ROP Response Buffer.
    /// </summary>
    public class RopCopyToStreamResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the SourceHandleIndex field in the request.
        /// </summary>
        public byte SourceHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the DestHandleIndex field in the request.
        /// </summary>
        public uint? DestHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the number of bytes read from the source object.
        /// </summary>
        public ulong ReadByteCount;

        /// <summary>
        /// An unsigned integer that specifies the number of bytes written to the destination object.
        /// </summary>
        public ulong WrittenByteCount;

        /// <summary>
        /// Parse the RopCopyToStreamResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopCopyToStreamResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.SourceHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());

            if ((AdditionalErrorCodes)this.ReturnValue == AdditionalErrorCodes.NullDestinationObject)
            {
                this.DestHandleIndex = this.ReadUint();
            }

            this.ReadByteCount = this.ReadUlong();
            this.WrittenByteCount = this.ReadUlong();
        }
    }
    #endregion

    #region 2.2.2.23 RopProgress
    /// <summary>
    ///  A class indicates the RopProgress ROP Request Buffer.
    /// </summary>
    public class RopProgressRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// A Boolean that specifies whether to cancel the operation.
        /// </summary>
        public bool WantCancel;

        /// <summary>
        /// Parse the RopProgressRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopProgressRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.WantCancel = this.ReadBoolean();
        }
    }

    /// <summary>
    ///  A class indicates the RopProgress ROP Response Buffer.
    /// </summary>
    public class RopProgressResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the RopLogon associated with this operation.
        /// </summary>
        public byte? LogonId;

        /// <summary>
        /// An unsigned integer that specifies the number of tasks completed.
        /// </summary>
        public uint? CompletedTaskCount;

        /// <summary>
        /// An unsigned integer that specifies the total number of tasks.
        /// </summary>
        public uint? TotalTaskCount;

        /// <summary>
        /// Parse the RopProgressResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopProgressResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.LogonId = this.ReadByte();
                this.CompletedTaskCount = this.ReadUint();
                this.TotalTaskCount = this.ReadUint();
            }
        }
    }
    #endregion

    #region 2.2.2.24 RopLockRegionStream
    /// <summary>
    ///  A class indicates the RopLockRegionStream ROP Request Buffer.
    /// </summary>
    public class RopLockRegionStreamRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the byte location in the stream where the region begins.
        /// </summary>
        public ulong RegionOffset;

        /// <summary>
        /// An unsigned integer that specifies the size of the region, in bytes.
        /// </summary>
        public ulong RegionSize;

        /// <summary>
        /// A flags structure that contains flags specifying the behavior of the lock operation. 
        /// </summary>
        public uint LockFlags;

        /// <summary>
        /// Parse the RopLockRegionStreamRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopLockRegionStreamRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.RegionOffset = this.ReadUlong();
            this.RegionSize = this.ReadUlong();
            this.LockFlags = this.ReadUint();
        }
    }

    /// <summary>
    ///  A class indicates the RopLockRegionStream ROP Response Buffer.
    /// </summary>
    public class RopLockRegionStreamResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// Parse the RopLockRegionStreamResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopLockRegionStreamResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
        }
    }
    #endregion

    #region 2.2.2.25 RopUnlockRegionStream
    /// <summary>
    ///  A class indicates the RopUnlockRegionStream ROP Request Buffer.
    /// </summary>
    public class RopUnlockRegionStreamRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the byte location in the stream where the region begins.
        /// </summary>
        public ulong RegionOffset;

        /// <summary>
        /// An unsigned integer that specifies the size of the region, in bytes.
        /// </summary>
        public ulong RegionSize;

        /// <summary>
        /// A flags structure that contains flags specifying the behavior of the lock operation. 
        /// </summary>
        public uint LockFlags;

        /// <summary>
        /// Parse the RopUnlockRegionStreamRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopUnlockRegionStreamRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.RegionOffset = this.ReadUlong();
            this.RegionSize = this.ReadUlong();
            this.LockFlags = this.ReadUint();
        }
    }

    /// <summary>
    ///  A class indicates the RopUnlockRegionStream ROP Response Buffer.
    /// </summary>
    public class RopUnlockRegionStreamResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// Parse the RopUnlockRegionStreamResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopUnlockRegionStreamResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
        }
    }
    #endregion

    #region 2.2.2.26 RopWriteAndCommitStream
    /// <summary>
    ///  A class indicates the RopWriteAndCommitStream ROP Request Buffer.
    /// </summary>
    public class RopWriteAndCommitStreamRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the size of the Data field.
        /// </summary>
        public ushort DataSize;

        /// <summary>
        /// An array of bytes that specifies the bytes to be written to the stream. The size of this field, in bytes, is specified by the DataSize field.
        /// </summary>
        public byte[] Data;

        /// <summary>
        /// Parse the RopWriteAndCommitStreamRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopWriteAndCommitStreamRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.DataSize = this.ReadUshort();
            this.Data = this.ReadBytes((int)this.DataSize);
        }
    }

    /// <summary>
    ///  A class indicates the RopWriteAndCommitStream ROP Response Buffer.
    /// </summary>
    public class RopWriteAndCommitStreamResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the number of bytes actually written.
        /// </summary>
        public ushort WrittenSize;

        /// <summary>
        /// Parse the RopWriteAndCommitStreamResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopWriteAndCommitStreamResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
            this.WrittenSize = this.ReadUshort();
        }
    }
    #endregion

    #region 2.2.2.27 RopCloneStream
    /// <summary>
    ///  A class indicates the RopCloneStream ROP Request Buffer.
    /// </summary>
    public class RopCloneStreamRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
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
        /// Parse the RopCloneStreamRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopCloneStreamRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopCloneStream ROP Response Buffer.
    /// </summary>
    public class RopCloneStreamResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// Parse the RopCloneStreamResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopCloneStreamResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
        }
    }
    #endregion
}

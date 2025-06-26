using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MAPIInspector.Parsers
{
    #region Enums values

    #region 2.2.3 Display Type Values

    /// <summary>
    /// The DisplayTypeValues enum type 
    /// </summary>
    public enum DisplayTypeValues : uint
    {
        /// <summary>
        /// A typical messaging user.
        /// </summary>
        DT_MAILUSER = 0x00000000,

        /// <summary>
        /// A distribution list.
        /// </summary>
        DT_DISTLIST = 0x00000001,

        /// <summary>
        /// A forum, such as a bulletin board service or a public or shared folder.
        /// </summary>
        DT_FORUM = 0x00000002,

        /// <summary>
        /// An automated agent, such as Quote-Of-The-Day or a weather chart display
        /// </summary>
        DT_AGENT = 0x00000003,

        /// <summary>
        /// An Address Book object defined for a large group
        /// </summary>
        DT_ORGANIZATION = 0x00000004,

        /// <summary>
        /// A private, personally administered distribution list.
        /// </summary>
        DT_PRIVATE_DISTLIST = 0x00000005,

        /// <summary>
        /// An Address Book object known to be from a foreign or remote messaging system
        /// </summary>
        DT_REMOTE_MAILUSER = 0x00000006,

        /// <summary>
        /// An address book hierarchy table container.
        /// </summary>
        DT_CONTAINER = 0x00000100,

        /// <summary>
        /// A display template object. An Exchange NSPI server MUST NOT return this display type.
        /// </summary>
        DT_TEMPLATE = 0x00000101,

        /// <summary>
        /// An address creation template. 
        /// </summary>
        DT_ADDRESS_TEMPLATE = 0x00000102,

        /// <summary>
        /// A search template
        /// </summary>
        DT_SEARCH = 0x00000200
    }

    #endregion

    #region 2.2.8   Positioning Minimal Entry IDs
    /// <summary>
    /// The PositioningMinimalEntryIDs enum type 
    /// </summary>
    public enum PositioningMinimalEntryIDs : uint
    {
        /// <summary>
        /// Specifies the position before the first row in the current address book container.
        /// </summary>
        MID_BEGINNING_OF_TABLE = 0x00000000,

        /// <summary>
        /// Specifies the position after the last row in the current address book container
        /// </summary>
        MID_END_OF_TABLE = 0x00000002,

        /// <summary>
        /// Specifies the current position in a table.
        /// </summary>
        MID_CURRENT = 0x00000001
    }

    #endregion

    #region 2.2.10   Table Sort Order
    /// <summary>
    /// The TableSortOrders enum type 
    /// </summary>
    public enum TableSortOrders : uint
    {
        /// <summary>
        /// The table is sorted ascending on the PidTagDisplayName property
        /// </summary>
        SortTypeDisplayName = 0x00000000,

        /// <summary>
        /// The table is sorted ascending on the PidTagAddressBookPhoneticDisplayName property
        /// </summary>
        SortTypePhoneticDisplayName = 0x00000003,

        /// <summary>
        /// The table is sorted ascending on the PidTagDisplayName property
        /// </summary>
        SortTypeDisplayName_RO = 0x000003E8,

        /// <summary>
        /// The table is sorted ascending on the PidTagDisplayName property
        /// </summary>
        SortTypeDisplayName_W = 0x000003E9
    }
    #endregion
    #endregion
    
    #region 2.2.8	STAT
    /// <summary>
    /// A class indicates the STAT structure.
    /// </summary>
    public class STAT : BaseStructure
    {
        /// <summary>
        /// A DWORD [MS-DTYP] value that specifies a sort order.
        /// </summary>
        public uint SortType;

        /// <summary>
        /// A DWORD value that specifies the Minimal Entry ID of the address book container that this STAT structure represents. 
        /// </summary>
        public uint ContainerID;

        /// <summary>
        /// A DWORD value that specifies a beginning position in the table for the start of an NSPI method. 
        /// </summary>
        public uint CurrentRec;

        /// <summary>
        /// A long value that specifies an offset from the beginning position in the table for the start of an NSPI method. 
        /// </summary>
        public uint Delta;

        /// <summary>
        /// A DWORD value that specifies a position in the table. 
        /// </summary>
        public uint NumPos;

        /// <summary>
        /// A DWORD value that specifies the number of rows in the table. 
        /// </summary>
        public uint TotalRecs;

        /// <summary>
        /// A DWORD value that represents a code page. 
        /// </summary>
        public uint CodePage;

        /// <summary>
        /// A DWORD value that represents a language code identifier (LCID). 
        /// </summary>
        public uint TemplateLocale;

        /// <summary>
        /// A DWORD value that represents an LCID. 
        /// </summary>
        public uint SortLocale;

        /// <summary>
        /// Parse the STAT payload of session.
        /// </summary>
        /// <param name="s">The stream containing STAT structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.SortType = this.ReadUint();
            this.ContainerID = this.ReadUint();
            this.CurrentRec = this.ReadUint();
            this.Delta = this.ReadUint();
            this.NumPos = this.ReadUint();
            this.TotalRecs = this.ReadUint();
            this.CodePage = this.ReadUint();
            this.TemplateLocale = this.ReadUint();
            this.SortLocale = this.ReadUint();
        }
    }
    #endregion

    #region 2.2.9	EntryIDs

    #region 2.2.9.1   MinimalEntryID
    /// <summary>
    /// A class indicates the MinimalEntryID structure.
    /// </summary>
    public class MinimalEntryID : BaseStructure
    {
        /// <summary>
        /// A Minimal Entry ID is a single DWORD value that identifies a specific object in the address book. 
        /// </summary>
        public uint MinEntryID;

        /// <summary>
        /// Parse the MinimalEntryID payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.MinEntryID = this.ReadUint();
        }
    }
    #endregion

    #region 2.2.9.2   EphemeralEntryID

    /// <summary>
    /// A class indicates the EphemeralEntryID structure.
    /// </summary>
    public class EphemeralEntryID : BaseStructure
    {
        /// <summary>
        /// The type of this ID.
        /// </summary>
        public byte Type;

        /// <summary>
        /// Reserved, generally this value is a constant 0x00.
        /// </summary>
        public byte R1;

        /// <summary>
        /// Reserved, generally this value is a constant 0x00.
        /// </summary>
        public byte R2;

        /// <summary>
        /// Reserved, generally this value is a constant 0x00.
        /// </summary>
        public byte R3;

        /// <summary>
        /// A FlatUID_r value contains the GUID of the server that issued this Ephemeral Entry ID.
        /// </summary>
        public Guid ProviderUID;

        /// <summary>
        /// Reserved, generally this value is a constant 0x00000001.
        /// </summary>
        public uint R4;

        /// <summary>
        /// The display type of the object specified by this Ephemeral Entry ID. 
        /// </summary>
        public DisplayTypeValues DisplayType;

        /// <summary>
        /// The Minimal Entry ID of this object. 
        /// </summary>
        public MinimalEntryID Mid;

        /// <summary>
        /// Parse the EphemeralEntryID payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Type = this.ReadByte();
            this.R1 = this.ReadByte();
            this.R2 = this.ReadByte();
            this.R3 = this.ReadByte();
            this.ProviderUID = this.ReadGuid();
            this.R4 = this.ReadUint();
            this.DisplayType = (DisplayTypeValues)this.ReadUint();
            this.Mid = new MinimalEntryID();
            this.Mid.Parse(s);
        }
    }

    #endregion

    #region 2.2.9.3   PermanentEntryID

    /// <summary>
    /// A class indicates the PermanentEntryID structure.
    /// </summary>
    public class PermanentEntryID : BaseStructure
    {
        /// <summary>
        /// The type of this ID. 
        /// </summary>
        public byte IDType;

        /// <summary>
        /// Reserved. All clients and servers MUST set this value to the constant 0x00.
        /// </summary>
        public byte R1;

        /// <summary>
        /// Reserved. All clients and servers MUST set this value to the constant 0x00.
        /// </summary>
        public byte R2;

        /// <summary>
        /// Reserved. All clients and servers MUST set this value to the constant 0x00.
        /// </summary>
        public byte R3;

        /// <summary>
        /// A FlatUID_r value that contains the constant GUID specified in Permanent Entry ID GUID, 
        /// </summary>
        public Guid ProviderUID;

        /// <summary>
        /// Reserved. All clients and servers MUST set this value to the constant 0x00000001.
        /// </summary>
        public uint R4;

        /// <summary>
        /// The display type of the object specified by this Permanent Entry ID. 
        /// </summary>
        public DisplayTypeValues DisplayTypeString;

        /// <summary>
        /// The DN (1) of the object specified by this Permanent Entry ID. 
        /// </summary>
        public MAPIString DistinguishedName;

        /// <summary>
        /// Parse the PermanentEntryID payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.IDType = this.ReadByte();
            this.R1 = this.ReadByte();
            this.R2 = this.ReadByte();
            this.R3 = this.ReadByte();
            this.ProviderUID = this.ReadGuid();
            this.R4 = this.ReadUint();
            this.DisplayTypeString = (DisplayTypeValues)this.ReadUint();
            this.DistinguishedName = new MAPIString(Encoding.ASCII);
            this.DistinguishedName.Parse(s);
        }
    }

    #endregion

    #endregion

    #region 2.2.2	Property Values

    /// <summary>
    /// A class indicates the FlatUID_r structure.
    /// </summary>
    public class FlatUID_r : BaseStructure
    {
        /// <summary>
        /// Encodes the ordered bytes of the FlatUID data structure.
        /// </summary>
        public Guid Ab;

        /// <summary>
        /// Parse the FlatUID_r payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Ab = this.ReadGuid();
        }
    }

    /// <summary>
    /// A class indicates the PropertyValue_r structure.
    /// </summary>
    public class PropertyValue_r : BaseStructure
    {
        /// <summary>
        /// Encodes the PropTag of the property whose value is represented by the PropertyValue_r data structure.
        /// </summary>
        public uint UlPropTag;

        /// <summary>
        /// Reserved. All clients and servers MUST set this value to the constant 0x00000000.
        /// </summary>
        public uint Reserved;

        /// <summary>
        /// Encodes the actual value of the property represented by the PropertyValue_r data structure. 
        /// </summary>
        public PROP_VAL_UNION Value;

        /// <summary>
        /// Parse the PropertyValue_r payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.UlPropTag = this.ReadUint();
            this.Reserved = this.ReadUint();
            this.Value = new PROP_VAL_UNION((int)this.UlPropTag & 0XFFFF);
            this.Value.Parse(s);
        }
    }

    /// <summary>
    /// A class indicates the PROP_VAL_UNION structure.
    /// </summary>
    public class PROP_VAL_UNION : BaseStructure
    {
        /// <summary>
        /// PROP_VAL_UNION contains an encoding of the value of a property that can contain a single 16-bit integer value.
        /// </summary>
        public short? I;

        /// <summary>
        /// PROP_VAL_UNION contains an encoding of the value of a property that can contain a single 32-bit integer value.
        /// </summary>
        public int? L;

        /// <summary>
        /// PROP_VAL_UNION contains an encoding of the value of a property that can contain a single Boolean value. 
        /// </summary>
        public ushort? B;

        /// <summary>
        /// PROP_VAL_UNION contains an encoding of the value of a property that can contain a single 8-bit character string value. 
        /// </summary>
        public String_r LpszA;

        /// <summary>
        /// PROP_VAL_UNION contains an encoding of the value of a property that can contain a single binary data value. 
        /// </summary>
        public Binary_r Bin;

        /// <summary>
        /// PROP_VAL_UNION contains an encoding of the value of a property that can contain a single Unicode string value.
        /// </summary>
        public WString_r LpszW;

        /// <summary>
        /// PROP_VAL_UNION contains an encoding of the value of a property that can contain a single GUID value. 
        /// </summary>
        public FlatUID_r Lpguid;

        /// <summary>
        /// PROP_VAL_UNION contains an encoding of the value of a property that can contain a single 64-bit integer value. 
        /// </summary>
        public PtypTime Ft;

        /// <summary>
        /// PROP_VAL_UNION contains an encoding of the value of a property that can contain a single PtypErrorCode value.
        /// </summary>
        public int? Err;

        /// <summary>
        /// PROP_VAL_UNION contains an encoding of the values of a property that can contain multiple 16-bit integer values. 
        /// </summary>
        public ShortArray_r MVi;

        /// <summary>
        /// PROP_VAL_UNION contains an encoding of the values of a property that can contain multiple 32-bit integer values. 
        /// </summary>
        public LongArray_r MVl;

        /// <summary>
        /// PROP_VAL_UNION contains an encoding of the values of a property that can contain multiple 8-bit character string values. 
        /// </summary>
        public StringArray_r MVszA;

        /// <summary>
        /// PROP_VAL_UNION contains an encoding of the values of a property that can contain multiple binary data values. 
        /// </summary>
        public BinaryArray_r MVbin;

        /// <summary>
        /// PROP_VAL_UNION contains an encoding of the values of a property that can contain multiple GUID values. 
        /// </summary>
        public FlatUIDArray_r MVguid;

        /// <summary>
        /// PROP_VAL_UNION contains an encoding of the values of a property that can contain multiple Unicode string values. 
        /// </summary>
        public WStringArray_r MVszW;

        /// <summary>
        /// PROP_VAL_UNION contains an encoding of the value of a property that can contain multiple 64-bit integer values. 
        /// </summary>
        public DateTimeArray_r MVft;

        /// <summary>
        /// Reserved. All clients and servers MUST set this value to the constant 0x00000000.
        /// </summary>
        public int? LReserved;

        /// <summary>
        /// Int value to initialize PROP_VAL_UNION constructed function.
        /// </summary>
        private int tag;

        /// <summary>
        /// Initializes a new instance of the PROP_VAL_UNION class.
        /// </summary>
        /// <param name="tag">The int value to initialize the function.</param>
        public PROP_VAL_UNION(int tag)
        {
            this.tag = tag;
        }

        /// <summary>
        /// Parse the PROP_VAL_UNION payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            switch (this.tag)
            {
                case 0x00000002:
                    this.I = this.ReadINT16();
                    break;
                case 0x00000003:
                    this.L = this.ReadINT32();
                    break;
                case 0x0000000B:
                    this.B = this.ReadUshort();
                    break;
                case 0x0000001E:
                    this.LpszA = new String_r();
                    this.LpszA.Parse(s);
                    break;
                case 0x00000102:
                    this.Bin = new Binary_r();
                    this.Bin.Parse(s);
                    break;
                case 0x0000001F:
                    this.LpszW = new WString_r();
                    this.LpszW.Parse(s);
                    break;
                case 0x00000048:
                    this.Lpguid = new FlatUID_r();
                    this.Lpguid.Parse(s);
                    break;
                case 0x00000040:
                    this.Ft = new PtypTime();
                    this.Ft.Parse(s);
                    break;
                case 0x0000000A:
                    this.Err = this.ReadINT32();
                    break;
                case 0x00001002:
                    this.MVi = new ShortArray_r();
                    this.MVi.Parse(s);
                    break;
                case 0x00001003:
                    this.MVl = new LongArray_r();
                    this.MVl.Parse(s);
                    break;
                case 0x0000101E:
                    this.MVszA = new StringArray_r();
                    this.MVszA.Parse(s);
                    break;
                case 0x00001102:
                    this.MVbin = new BinaryArray_r();
                    this.MVbin.Parse(s);
                    break;
                case 0x00001048:
                    this.MVguid = new FlatUIDArray_r();
                    this.MVguid.Parse(s);
                    break;
                case 0x0000101F:
                    this.MVszW = new WStringArray_r();
                    this.MVszW.Parse(s);
                    break;
                case 0x00001040:
                    this.MVft = new DateTimeArray_r();
                    this.MVft.Parse(s);
                    break;
                case 0x00000001:
                case 0x0000000D:
                    this.LReserved = this.ReadINT32();
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// A class indicates the String_r structure.
    /// </summary>
    public class String_r : BaseStructure
    {
        /// <summary>
        /// A variable value
        /// </summary>
        public byte? HasValue;

        /// <summary>
        /// A single 8-bit character string value. This value is NULL-terminated.
        /// </summary>
        public MAPIString Value;

        /// <summary>
        /// Parse the String_r payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            byte temp = ReadByte();
            if (temp == 0xFF)
            {
                this.HasValue = temp;
            }
            else
            {
                s.Position -= 1;
            }

            this.Value = new MAPIString(Encoding.ASCII);
            this.Value.Parse(s);
        }
    }

    /// <summary>
    /// A class indicates the WString_r structure.
    /// </summary>
    public class WString_r : BaseStructure
    {
        /// <summary>
        /// A variable value
        /// </summary>
        public byte? HasValue;

        /// <summary>
        /// A single Unicode string value. This value is NULL-terminated.
        /// </summary>
        public MAPIString Value;

        /// <summary>
        /// Parse the WString_r payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            byte temp = ReadByte();
            if (temp == 0xFF)
            {
                this.HasValue= temp;
            }
            else
            {
                s.Position -= 1;
            }

            this.Value = new MAPIString(Encoding.Unicode);
            this.Value.Parse(s);
        }
    }

    /// <summary>
    /// A class indicates the Binary_r structure.
    /// </summary>
    public class Binary_r : BaseStructure
    {
        /// <summary>
        /// A variable value // TODO: Verify whether there is HasValue here
        /// </summary>
        public byte? HasValue;

        /// <summary>
        /// The number of uninterpreted bytes represented in this structure. This value MUST NOT exceed 2,097,152.
        /// </summary>
        public uint Cb;

        /// <summary>
        /// The uninterpreted bytes.
        /// </summary>
        public byte[] Lpb;

        /// <summary>
        /// Parse the Binary_r payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            byte temp = this.ReadByte();
            if (temp == 0xFF)
            {
                this.HasValue= temp;
            }
            else
            {
                s.Position -= 1;
            }

            this.Cb = this.ReadUint();
            this.Lpb = this.ReadBytes((int)this.Cb);
        }
    }

    /// <summary>
    /// A class indicates the ShortArray_r structure.
    /// </summary>
    public class ShortArray_r : BaseStructure
    {
        /// <summary>
        /// The number of 16-bit integer values represented in the ShortArray_r structure. This value MUST NOT exceed 100,000.
        /// </summary>
        public uint CValues;

        /// <summary>
        /// The 16-bit integer values.
        /// </summary>
        public short[] Lpi;

        /// <summary>
        /// Parse the ShortArray_r payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.CValues = this.ReadUint();
            List<short> tempList = new List<short>();
            for (ulong i = 0; i < this.CValues; i++)
            {
                tempList.Add(this.ReadINT16());
            }

            this.Lpi = tempList.ToArray();
        }
    }

    /// <summary>
    /// A class indicates the LongArray_r structure.
    /// </summary>
    public class LongArray_r : BaseStructure
    {
        /// <summary>
        /// The number of 32-bit integers represented in this structure. This value MUST NOT exceed 100,000.
        /// </summary>
        public uint CValues;

        /// <summary>
        /// The 32-bit integer values.
        /// </summary>
        public int[] Lpl;

        /// <summary>
        /// Parse the LongArray_r payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.CValues = this.ReadUint();
            List<int> tempList = new List<int>();
            for (int i = 0; i < this.CValues; i++)
            {
                tempList.Add(this.ReadINT32());
            }

            this.Lpl = tempList.ToArray();
        }
    }

    /// <summary>
    /// A class indicates the StringArray_r structure.
    /// </summary>
    public class StringArray_r : BaseStructure
    {
        /// <summary>
        /// A variable value
        /// </summary>
        public byte? HasValue;

        /// <summary>
        /// The number of 8-bit character string references represented in the StringArray_r structure. This value MUST NOT exceed 100,000.
        /// </summary>
        public uint CValues;

        /// <summary>
        /// The 8-bit character string references. The strings referred to are NULL-terminated.
        /// </summary>
        public MAPIString[] LppszA;

        /// <summary>
        /// Parse the StringArray_r payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            byte temp = this.ReadByte();
            if (temp == 0xFF)
            {
                this.HasValue = temp;
            }
            else
            {
                s.Position -= 1;
            }

            this.CValues = this.ReadUint();
            List<MAPIString> temBytes = new List<MAPIString>();
            for (ulong i = 0; i < this.CValues; i++)
            {
                MAPIString tempByte = new MAPIString(Encoding.ASCII);
                tempByte.Parse(s);
                temBytes.Add(tempByte);
            }

            this.LppszA = temBytes.ToArray();
        }
    }

    /// <summary>
    /// A class indicates the BinaryArray_r structure.
    /// </summary>
    public class BinaryArray_r : BaseStructure
    {
        /// <summary>
        /// A variable value
        /// </summary>
        public byte? HasValue;

        /// <summary>
        /// The number of Binary_r data structures represented in the BinaryArray_r structure. This value MUST NOT exceed 100,000.
        /// </summary>
        public uint CValues;

        /// <summary>
        /// The Binary_r data structures.
        /// </summary>
        public Binary_r[] Lpbin;

        /// <summary>
        /// Parse the BinaryArray_r payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            byte temp = this.ReadByte();
            if (temp == 0xFF)
            {
                this.HasValue = temp;
            }
            else
            {
                s.Position -= 1;
            }

            this.CValues = this.ReadUint();
            List<Binary_r> temBytes = new List<Binary_r>();
            for (ulong i = 0; i < this.CValues; i++)
            {
                Binary_r br = new Binary_r();
                br.Parse(s);
                temBytes.Add(br);
            }

            this.Lpbin = temBytes.ToArray();
        }
    }

    /// <summary>
    /// A class indicates the FlatUIDArray_r structure.
    /// </summary>
    public class FlatUIDArray_r : BaseStructure
    {
        /// <summary>
        /// The number of FlatUID_r structures represented in the FlatUIDArray_r structure. This value MUST NOT exceed 100,000.
        /// </summary>
        public uint CValues;

        /// <summary>
        /// The FlatUID_r data structures.
        /// </summary>
        public FlatUID_r[] Lpguid;

        /// <summary>
        /// Parse the FlatUIDArray_r payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.CValues = this.ReadUint();
            List<FlatUID_r> temBytes = new List<FlatUID_r>();
            for (ulong i = 0; i < this.CValues; i++)
            {
                FlatUID_r br = new FlatUID_r();
                br.Parse(s);
                temBytes.Add(br);
            }

            this.Lpguid = temBytes.ToArray();
        }
    }

    /// <summary>
    /// A class indicates the WStringArray_r structure.
    /// </summary>
    public class WStringArray_r : BaseStructure
    {
        /// <summary>
        /// A variable value
        /// </summary>
        public byte? HasValue;

        /// <summary>
        /// The number of Unicode character string references represented in the WStringArray_r structure. This value MUST NOT exceed 100,000.
        /// </summary>
        public uint CValues;

        /// <summary>
        /// The Unicode character string references. The strings referred to are NULL-terminated.
        /// </summary>
        public MAPIString[] LppszW;

        /// <summary>
        /// Parse the WStringArray_r payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            byte temp = this.ReadByte();
            if (temp == 0xFF)
            {
                this.HasValue = temp;
            }
            else
            {
                s.Position -= 1;
            }

            this.CValues = this.ReadUint();
            List<MAPIString> temBytes = new List<MAPIString>();
            for (ulong i = 0; i < this.CValues; i++)
            {
                MAPIString tempByte = new MAPIString(Encoding.Unicode);
                tempByte.Parse(s);
                temBytes.Add(tempByte);
            }

            this.LppszW = temBytes.ToArray();
        }
    }

    /// <summary>
    /// A class indicates the DateTimeArray_r structure.
    /// </summary>
    public class DateTimeArray_r : BaseStructure
    {
        /// <summary>
        /// The number of FILETIME data structures represented in the DateTimeArray_r structure. This value MUST NOT exceed 100,000.
        /// </summary>
        public uint CValues;

        /// <summary>
        /// The FILETIME data structures.
        /// </summary>
        public PtypTime[] Lpft;

        /// <summary>
        /// Parse the DateTimeArray_r payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.CValues = this.ReadUint();
            List<PtypTime> temBytes = new List<PtypTime>();
            for (ulong i = 0; i < this.CValues; i++)
            {
                PtypTime pt = new PtypTime();
                pt.Parse(s);
                temBytes.Add(pt);
            }

            this.Lpft = temBytes.ToArray();
        }
    }

    /// <summary>
    /// A class indicates the StringsArray_r structure.
    /// </summary>
    public class StringsArray_r : BaseStructure
    {
        /// <summary>
        /// A variable value
        /// </summary>
        public byte? HasValue;

        /// <summary>
        /// The number of character string structures in this aggregation. The value MUST NOT exceed 100,000.
        /// </summary>
        public uint Count;

        /// <summary>
        /// The list of character type strings in this aggregation. The strings in this list are NULL-terminated.
        /// </summary>
        public MAPIString[] Strings;

        /// <summary>
        /// Parse the StringsArray_r payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            byte temp = this.ReadByte();
            if (temp == 0xFF)
            {
                this.HasValue = temp;
            }
            else
            {
                s.Position -= 1;
            }

            this.Count = this.ReadUint();
            List<MAPIString> temBytes = new List<MAPIString>();
            for (ulong i = 0; i < this.Count; i++)
            {
                MAPIString tempByte = new MAPIString(Encoding.ASCII);
                tempByte.Parse(s);
                temBytes.Add(tempByte);
            }

            this.Strings = temBytes.ToArray();
        }
    }

    /// <summary>
    /// A class indicates the WStringsArray_r structure.
    /// </summary>
    public class WStringsArray_r : BaseStructure
    {
        /// <summary>
        /// A variable value
        /// </summary>
        public byte? HasValue;

        /// <summary>
        /// The number of character strings structures in this aggregation. The value MUST NOT exceed 100,000.
        /// </summary>
        public uint Count;

        /// <summary>
        /// The list of wchar_t type strings in this aggregation. The strings in this list are NULL-terminated.
        /// </summary>
        public MAPIString[] Strings;

        /// <summary>
        /// Parse the WStringsArray_r payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            byte temp = this.ReadByte();
            if (temp == 0xFF)
            {
                this.HasValue = temp;
            }
            else
            {
                s.Position -= 1;
            }

            this.Count = this.ReadUint();
            List<MAPIString> temBytes = new List<MAPIString>();
            for (ulong i = 0; i < this.Count; i++)
            {
                MAPIString tempByte = new MAPIString(Encoding.Unicode);
                tempByte.Parse(s);
                temBytes.Add(tempByte);
            }

            this.Strings = temBytes.ToArray();
        }
    }

    #endregion
}

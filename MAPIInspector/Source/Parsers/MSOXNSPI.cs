using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MAPIInspector.Parsers
{
    #region 2.2.8	STAT
    /// <summary>
    /// A class indicates the STAT structure.
    /// </summary>
    public class STAT : BaseStructure
    {
        // A DWORD [MS-DTYP] value that specifies a sort order.
        public uint SortType;

        // A DWORD value that specifies the Minimal Entry ID of the address book container that this STAT structure represents. 
        public uint ContainerID;

        // A DWORD value that specifies a beginning position in the table for the start of an NSPI method. 
        public uint CurrentRec;

        // A long value that specifies an offset from the beginning position in the table for the start of an NSPI method. 
        public int Delta;

        // A DWORD value that specifies a position in the table. 
        public uint NumPos;

        // A DWORD value that specifies the number of rows in the table. 
        public uint TotalRecs;

        // A DWORD value that represents a code page. 
        public uint CodePage;

        // A DWORD value that represents a language code identifier (LCID). 
        public uint TemplateLocale;

        // A DWORD value that represents an LCID. 
        public uint SortLocale;

        /// <summary>
        /// Parse the STAT payload of session.
        /// </summary>
        /// <param name="s">The stream containing STAT structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.SortType = ReadUint();
            this.ContainerID = ReadUint();
            this.CurrentRec = ReadUint();
            this.Delta = ReadINT32();
            this.NumPos = ReadUint();
            this.TotalRecs = ReadUint();
            this.CodePage = ReadUint();
            this.TemplateLocale = ReadUint();
            this.SortLocale = ReadUint();
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
        // A Minimal Entry ID is a single DWORD value that identifies a specific object in the address book. 
        public uint MinEntryID;

        /// <summary>
        /// Parse the MinimalEntryID payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.MinEntryID = ReadUint();
        }
    }
    #endregion

    #region 2.2.9.2   EphemeralEntryID

    /// <summary>
    /// A class indicates the EphemeralEntryID structure.
    /// </summary>
    public class EphemeralEntryID : BaseStructure
    {
        // The type of this ID.
        public byte Type;

        // Reserved, generally this value is a constant 0x00.
        public byte R1;

        // Reserved, generally this value is a constant 0x00.
        public byte R2;

        // Reserved, generally this value is a constant 0x00.
        public byte R3;

        // A FlatUID_r value contains the GUID of the server that issued this Ephemeral Entry ID.
        public Guid ProviderUID;

        // Reserved, generally this value is a constant 0x00000001.
        public uint R4;

        // The display type of the object specified by this Ephemeral Entry ID. 
        public DisplayTypeValues DisplayType;

        // The Minimal Entry ID of this object. 
        public MinimalEntryID Mid;

        /// <summary>
        /// Parse the EphemeralEntryID payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Type = ReadByte();
            this.R1 = ReadByte();
            this.R2 = ReadByte();
            this.R3 = ReadByte();
            this.ProviderUID = ReadGuid();
            this.R4 = ReadUint();
            this.DisplayType = (DisplayTypeValues)ReadUint();
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
        // The type of this ID. 
        public byte IDType;

        // Reserved. All clients and servers MUST set this value to the constant 0x00.
        public byte R1;

        // Reserved. All clients and servers MUST set this value to the constant 0x00.
        public byte R2;

        // Reserved. All clients and servers MUST set this value to the constant 0x00.
        public byte R3;

        // A FlatUID_r value that contains the constant GUID specified in Permanent Entry ID GUID, 
        public Guid ProviderUID;

        // Reserved. All clients and servers MUST set this value to the constant 0x00000001.
        public uint R4;

        // The display type of the object specified by this Permanent Entry ID. 
        public DisplayTypeValues DisplayTypeString;

        // The DN (1) of the object specified by this Permanent Entry ID. 
        public MAPIString DistinguishedName;

        /// <summary>
        /// Parse the PermanentEntryID payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.IDType = ReadByte();
            this.R1 = ReadByte();
            this.R2 = ReadByte();
            this.R3 = ReadByte();
            this.ProviderUID = ReadGuid();
            this.R4 = ReadUint();
            this.DisplayTypeString = (DisplayTypeValues)ReadUint();
            this.DistinguishedName = new MAPIString(Encoding.ASCII);
            this.DistinguishedName.Parse(s);
        }
    }

    #endregion

    #endregion

    #region 2.2.2	Property Values
    /// <summary>
    /// 2.2.2.1	FlatUID_r Structure
    /// </summary>

    /// <summary>
    /// A class indicates the FlatUID_r structure.
    /// </summary>
    public class FlatUID_r : BaseStructure
    {
        // Encodes the ordered bytes of the FlatUID data structure.
        public Guid ab;

        /// <summary>
        /// Parse the FlatUID_r payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.ab = ReadGuid();
        }
    }

    /// <summary>
    /// A class indicates the PropertyValue_r structure.
    /// </summary>
    public class PropertyValue_r : BaseStructure
    {
        // Encodes the proptag of the property whose value is represented by the PropertyValue_r data structure.
        public uint ulPropTag;

        // Reserved. All clients and servers MUST set this value to the constant 0x00000000.
        public uint Reserved;

        // Encodes the actual value of the property represented by the PropertyValue_r data structure. 
        public PROP_VAL_UNION value;

        /// <summary>
        /// Parse the PropertyValue_r payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.ulPropTag = ReadUint();
            this.Reserved = ReadUint();
            this.value = new PROP_VAL_UNION((int)this.ulPropTag & 0XFFFF);
            this.value.Parse(s);
        }

    }

    /// <summary>
    /// A class indicates the PROP_VAL_UNION structure.
    /// </summary>
    public class PROP_VAL_UNION : BaseStructure
    {
        // PROP_VAL_UNION contains an encoding of the value of a property that can contain a single 16-bit integer value.
        public short? i;

        // PROP_VAL_UNION contains an encoding of the value of a property that can contain a single 32-bit integer value.
        public int? l;

        // PROP_VAL_UNION contains an encoding of the value of a property that can contain a single Boolean value. 
        public ushort? b;

        // PROP_VAL_UNION contains an encoding of the value of a property that can contain a single 8-bit character string value. 
        public String_r lpszA;

        // PROP_VAL_UNION contains an encoding of the value of a property that can contain a single binary data value. 
        public Binary_r bin;

        // PROP_VAL_UNION contains an encoding of the value of a property that can contain a single Unicode string value. 
        public WString_r lpszW;

        // PROP_VAL_UNION contains an encoding of the value of a property that can contain a single GUID value. 
        public FlatUID_r lpguid;

        // PROP_VAL_UNION contains an encoding of the value of a property that can contain a single 64-bit integer value. 
        public PtypTime ft;

        // PROP_VAL_UNION contains an encoding of the value of a property that can contain a single PtypErrorCode value.
        public int? err;

        // PROP_VAL_UNION contains an encoding of the values of a property that can contain multiple 16-bit integer values. 
        public ShortArray_r MVi;

        // PROP_VAL_UNION contains an encoding of the values of a property that can contain multiple 32-bit integer values. 
        public LongArray_r MVl;

        // PROP_VAL_UNION contains an encoding of the values of a property that can contain multiple 8-bit character string values. 
        public StringArray_r MVszA;

        // PROP_VAL_UNION contains an encoding of the values of a property that can contain multiple binary data values. 
        public BinaryArray_r MVbin;

        // PROP_VAL_UNION contains an encoding of the values of a property that can contain multiple GUID values. 
        public FlatUIDArray_r MVguid;

        // PROP_VAL_UNION contains an encoding of the values of a property that can contain multiple Unicode string values. 
        public WStringArray_r MVszW;

        // PROP_VAL_UNION contains an encoding of the value of a property that can contain multiple 64-bit integer values. 
        public DateTimeArray_r MVft;

        // Reserved. All clients and servers MUST set this value to the constant 0x00000000.
        public int? lReserved;

        // Int value to initialize PROP_VAL_UNION constructed function.
        private int tag;

        /// <summary>
        /// The constructed function.
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
                    this.i = ReadINT16();
                    break;
                case 0x00000003:
                    this.l = ReadINT32();
                    break;
                case 0x0000000B:
                    this.b = ReadUshort();
                    break;
                case 0x0000001E:
                    this.lpszA = new String_r();
                    this.lpszA.Parse(s);
                    break;
                case 0x00000102:
                    this.bin = new Binary_r();
                    this.bin.Parse(s);
                    break;
                case 0x0000001F:
                    this.lpszW = new WString_r();
                    this.lpszW.Parse(s);
                    break;
                case 0x00000048:
                    this.lpguid = new FlatUID_r();
                    this.lpguid.Parse(s);
                    break;
                case 0x00000040:
                    this.ft = new PtypTime();
                    this.ft.Parse(s);
                    break;
                case 0x0000000A:
                    this.err = ReadINT32();
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
                    this.lReserved = ReadINT32();
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
        // A variable value
        public byte? MagicNumber;

        // A single 8-bit character string value. This value is NULL-terminated.
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
                this.MagicNumber = temp;
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
        // A variable value
        public byte? MagicNumber;

        // A single Unicode string value. This value is NULL-terminated.
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
                this.MagicNumber = temp;
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
        // A variable value // TODO: Verify whether there is MagicNumber here
        public byte? MagicNumber;

        // The number of uninterpreted bytes represented in this structure. This value MUST NOT exceed 2,097,152.
        public uint cb;

        // The uninterpreted bytes.
        public byte[] lpb;

        /// <summary>
        /// Parse the Binary_r payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            byte temp = ReadByte();
            if (temp == 0xFF)
            {
                this.MagicNumber = temp;
            }
            else
            {
                s.Position -= 1;
            }
            this.cb = ReadUint();
            this.lpb = ReadBytes((int)this.cb);
        }
    }

    /// <summary>
    /// A class indicates the ShortArray_r structure.
    /// </summary>
    public class ShortArray_r : BaseStructure
    {
        // The number of 16-bit integer values represented in the ShortArray_r structure. This value MUST NOT exceed 100,000.
        public uint cValues;

        // The 16-bit integer values.
        public short[] lpi;

        /// <summary>
        /// Parse the ShortArray_r payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.cValues = ReadUint();
            List<short> tempList = new List<short>();
            for (ulong i = 0; i < this.cValues; i++)
            {
                tempList.Add(ReadINT16());
            }
            this.lpi = tempList.ToArray();
        }
    }

    /// <summary>
    /// A class indicates the LongArray_r structure.
    /// </summary>
    public class LongArray_r : BaseStructure
    {
        // The number of 32-bit integers represented in this structure. This value MUST NOT exceed 100,000.
        public uint cValues;

        // The 32-bit integer values.
        public int[] lpl;

        /// <summary>
        /// Parse the LongArray_r payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.cValues = ReadUint();
            List<int> tempList = new List<int>();
            for (int i = 0; i < this.cValues; i++)
            {
                tempList.Add(ReadINT32());
            }
            this.lpl = tempList.ToArray();
        }
    }

    /// <summary>
    /// A class indicates the StringArray_r structure.
    /// </summary>
    public class StringArray_r : BaseStructure
    {
        // A variable value
        public byte? MagicNumber;

        // The number of 8-bit character string references represented in the StringArray_r structure. This value MUST NOT exceed 100,000.
        public uint cValues;

        // The 8-bit character string references. The strings referred to are NULL-terminated.
        public MAPIString[] lppszA;

        /// <summary>
        /// Parse the StringArray_r payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            byte temp = ReadByte();
            if (temp == 0xFF)
            {
                this.MagicNumber = temp;
            }
            else
            {
                s.Position -= 1;
            }
            this.cValues = ReadUint();
            List<MAPIString> temBytes = new List<MAPIString>();
            for (ulong i = 0; i < this.cValues; i++)
            {
                MAPIString tempByte = new MAPIString(Encoding.ASCII);
                tempByte.Parse(s);
                temBytes.Add(tempByte);
            }
            this.lppszA = temBytes.ToArray();
        }

    }

    /// <summary>
    /// A class indicates the BinaryArray_r structure.
    /// </summary>
    public class BinaryArray_r : BaseStructure
    {
        // A variable value
        public byte? MagicNumber;

        // The number of Binary_r data structures represented in the BinaryArray_r structure. This value MUST NOT exceed 100,000.
        public uint cValues;

        // The Binary_r data structures.
        public Binary_r[] lpbin;

        /// <summary>
        /// Parse the BinaryArray_r payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            byte temp = ReadByte();
            if (temp == 0xFF)
            {
                this.MagicNumber = temp;
            }
            else
            {
                s.Position -= 1;
            }
            this.cValues = ReadUint();
            List<Binary_r> temBytes = new List<Binary_r>();
            for (ulong i = 0; i < this.cValues; i++)
            {
                Binary_r br = new Binary_r();
                br.Parse(s);
                temBytes.Add(br);
            }
            this.lpbin = temBytes.ToArray();
        }
    }

    /// <summary>
    /// A class indicates the FlatUIDArray_r structure.
    /// </summary>
    public class FlatUIDArray_r : BaseStructure
    {
        // The number of FlatUID_r structures represented in the FlatUIDArray_r structure. This value MUST NOT exceed 100,000.
        public uint cValues;

        // The FlatUID_r data structures.
        public FlatUID_r[] lpguid;

        /// <summary>
        /// Parse the FlatUIDArray_r payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.cValues = ReadUint();
            List<FlatUID_r> temBytes = new List<FlatUID_r>();
            for (ulong i = 0; i < this.cValues; i++)
            {
                FlatUID_r br = new FlatUID_r();
                br.Parse(s);
                temBytes.Add(br);
            }
            this.lpguid = temBytes.ToArray();
        }
    }

    /// <summary>
    /// A class indicates the WStringArray_r structure.
    /// </summary>
    public class WStringArray_r : BaseStructure
    {
        // A variable value
        public byte? MagicNumber;

        // The number of Unicode character string references represented in the WStringArray_r structure. This value MUST NOT exceed 100,000.
        public uint cValues;

        // The Unicode character string references. The strings referred to are NULL-terminated.
        public MAPIString[] lppszW;

        /// <summary>
        /// Parse the WStringArray_r payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            byte temp = ReadByte();
            if (temp == 0xFF)
            {
                this.MagicNumber = temp;
            }
            else
            {
                s.Position -= 1;
            }
            this.cValues = ReadUint();
            List<MAPIString> temBytes = new List<MAPIString>();
            for (ulong i = 0; i < this.cValues; i++)
            {
                MAPIString tempByte = new MAPIString(Encoding.Unicode);
                tempByte.Parse(s);
                temBytes.Add(tempByte);
            }
            this.lppszW = temBytes.ToArray();
        }
    }

    /// <summary>
    /// A class indicates the DateTimeArray_r structure.
    /// </summary>
    public class DateTimeArray_r : BaseStructure
    {
        // The number of FILETIME data structures represented in the DateTimeArray_r structure. This value MUST NOT exceed 100,000.
        public uint cValues;

        // The FILETIME data structures.
        public PtypTime[] lpft;

        /// <summary>
        /// Parse the DateTimeArray_r payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.cValues = ReadUint();
            List<PtypTime> temBytes = new List<PtypTime>();
            for (ulong i = 0; i < this.cValues; i++)
            {
                PtypTime pt = new PtypTime();
                pt.Parse(s);
                temBytes.Add(pt);
            }
            this.lpft = temBytes.ToArray();
        }
    }

    /// <summary>
    /// A class indicates the StringsArray_r structure.
    /// </summary>
    public class StringsArray_r : BaseStructure
    {
        // A variable value
        public byte? MagicNumber;

        // The number of character string structures in this aggregation. The value MUST NOT exceed 100,000.
        public uint Count;

        // The list of character type strings in this aggregation. The strings in this list are NULL-terminated.
        public MAPIString[] Strings;

        /// <summary>
        /// Parse the StringsArray_r payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            byte temp = ReadByte();
            if (temp == 0xFF)
            {
                this.MagicNumber = temp;
            }
            else
            {
                s.Position -= 1;
            }
            this.Count = ReadUint();
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
        // A variable value
        public byte? MagicNumber;

        // The number of character strings structures in this aggregation. The value MUST NOT exceed 100,000.
        public uint Count;

        // The list of wchar_t type strings in this aggregation. The strings in this list are NULL-terminated.
        public MAPIString[] Strings;

        /// <summary>
        /// Parse the WStringsArray_r payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            byte temp = ReadByte();
            if (temp == 0xFF)
            {
                this.MagicNumber = temp;
            }
            else
            {
                s.Position -= 1;
            }
            this.Count = ReadUint();
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

    #region Enums values

    #region 2.2.3 Display Type Values

    /// <summary>
    /// The DisplayTypeValues enum type 
    /// </summary>
    public enum DisplayTypeValues : uint
    {
        DT_MAILUSER = 0x00000000,
        DT_DISTLIST = 0x00000001,
        DT_FORUM = 0x00000002,
        DT_AGENT = 0x00000003,
        DT_ORGANIZATION = 0x00000004,
        DT_PRIVATE_DISTLIST = 0x00000005,
        DT_REMOTE_MAILUSER = 0x00000006,
        DT_CONTAINER = 0x00000100,
        DT_TEMPLATE = 0x00000101,
        DT_ADDRESS_TEMPLATE = 0x00000102,
        DT_SEARCH = 0x00000200
    };

    #endregion

    #region 2.2.8   Positioning Minimal Entry IDs
    /// <summary>
    /// The PositioningMinimalEntryIDs enum type 
    /// </summary>
    public enum PositioningMinimalEntryIDs : uint
    {
        MID_BEGINNING_OF_TABLE = 0x00000000,
        MID_END_OF_TABLE = 0x00000002,
        MID_CURRENT = 0x00000001
    };

    #endregion

    #region 2.2.10   Table Sort Order
    /// <summary>
    /// The TableSortOrders enum type 
    /// </summary>
    public enum TableSortOrders : uint
    {
        SortTypeDisplayName = 0x00000000,
        SortTypePhoneticDisplayName = 0x00000003,
        SortTypeDisplayName_RO = 0x000003E8,
        SortTypeDisplayName_W = 0x000003E9
    };
    #endregion
    #endregion
}

using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.3.1 Property Values
    /// 2.3.1.11 PROP_VAL_UNION
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
        /// Reserved. All clients and servers MUST set value to the constant 0x00000000.
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
            switch (tag)
            {
                case 0x00000002:
                    I = ReadINT16();
                    break;
                case 0x00000003:
                    L = ReadINT32();
                    break;
                case 0x0000000B:
                    B = ReadUshort();
                    break;
                case 0x0000001E:
                    LpszA = new String_r();
                    LpszA.Parse(s);
                    break;
                case 0x00000102:
                    Bin = new Binary_r();
                    Bin.Parse(s);
                    break;
                case 0x0000001F:
                    LpszW = new WString_r();
                    LpszW.Parse(s);
                    break;
                case 0x00000048:
                    Lpguid = new FlatUID_r();
                    Lpguid.Parse(s);
                    break;
                case 0x00000040:
                    Ft = new PtypTime();
                    Ft.Parse(s);
                    break;
                case 0x0000000A:
                    Err = ReadINT32();
                    break;
                case 0x00001002:
                    MVi = new ShortArray_r();
                    MVi.Parse(s);
                    break;
                case 0x00001003:
                    MVl = new LongArray_r();
                    MVl.Parse(s);
                    break;
                case 0x0000101E:
                    MVszA = new StringArray_r();
                    MVszA.Parse(s);
                    break;
                case 0x00001102:
                    MVbin = new BinaryArray_r();
                    MVbin.Parse(s);
                    break;
                case 0x00001048:
                    MVguid = new FlatUIDArray_r();
                    MVguid.Parse(s);
                    break;
                case 0x0000101F:
                    MVszW = new WStringArray_r();
                    MVszW.Parse(s);
                    break;
                case 0x00001040:
                    MVft = new DateTimeArray_r();
                    MVft.Parse(s);
                    break;
                case 0x00000001:
                case 0x0000000D:
                    LReserved = ReadINT32();
                    break;
                default:
                    break;
            }
        }
    }
}

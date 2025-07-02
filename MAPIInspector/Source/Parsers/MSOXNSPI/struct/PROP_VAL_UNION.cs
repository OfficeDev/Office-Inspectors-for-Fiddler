using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.3.1 Property Values
    /// 2.3.1.11 PROP_VAL_UNION
    /// A class indicates the PROP_VAL_UNION structure.
    /// </summary>
    public class PROP_VAL_UNION : Block
    {
        /// <summary>
        /// PROP_VAL_UNION contains an encoding of the value of a property that can contain a single 16-bit integer value.
        /// </summary>
        public BlockT<short> I;

        /// <summary>
        /// PROP_VAL_UNION contains an encoding of the value of a property that can contain a single 32-bit integer value.
        /// </summary>
        public BlockT<int> L;

        /// <summary>
        /// PROP_VAL_UNION contains an encoding of the value of a property that can contain a single Boolean value.
        /// </summary>
        public BlockT<ushort> B;

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
        public BlockT<int> Err;

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
        public BlockT<int> LReserved;

        /// <summary>
        /// Int value to initialize PROP_VAL_UNION constructed function.
        /// </summary>
        private PropertyDataType propType;

        /// <summary>
        /// Initializes a new instance of the PROP_VAL_UNION class.
        /// </summary>
        /// <param name="type">The PropertyDataType value to initialize the function.</param>
        public PROP_VAL_UNION(PropertyDataType type)
        {
            propType = type;
        }

        /// <summary>
        /// Parse the PROP_VAL_UNION payload of session.
        /// </summary>
        protected override void Parse()
        {
            switch (propType)
            {
                case PropertyDataType.PtypInteger16:
                    I = ParseT<short>();
                    break;
                case PropertyDataType.PtypInteger32:
                    L = ParseT<int>();
                    break;
                case PropertyDataType.PtypBoolean:
                    B = ParseT<ushort>();
                    break;
                case PropertyDataType.PtypString8:
                    LpszA = Parse<String_r>();
                    break;
                case PropertyDataType.PtypBinary:
                    Bin = Parse<Binary_r>();
                    break;
                case PropertyDataType.PtypString:
                    LpszW = Parse<WString_r>();
                    break;
                case PropertyDataType.PtypGuid:
                    Lpguid = Parse<FlatUID_r>();
                    break;
                case PropertyDataType.PtypTime:
                    Ft = Parse<PtypTime>();
                    break;
                case PropertyDataType.PtypErrorCode:
                    Err = ParseT<int>();
                    break;
                case PropertyDataType.PtypMultipleInteger16:
                    MVi = Parse<ShortArray_r>();
                    break;
                case PropertyDataType.PtypMultipleInteger32:
                    MVl = Parse<LongArray_r>();
                    break;
                case PropertyDataType.PtypMultipleString8:
                    MVszA = Parse<StringArray_r>();
                    break;
                case PropertyDataType.PtypMultipleBinary:
                    MVbin = Parse<BinaryArray_r>();
                    break;
                case PropertyDataType.PtypMultipleGuid:
                    MVguid = Parse<FlatUIDArray_r>();
                    break;
                case PropertyDataType.PtypMultipleString:
                    MVszW = Parse<WStringArray_r>();
                    break;
                case PropertyDataType.PtypMultipleTime:
                    MVft = Parse<DateTimeArray_r>();
                    break;
                case PropertyDataType.PtypNull:
                case PropertyDataType.PtypObject_Or_PtypEmbeddedTable:
                    LReserved = ParseT<int>();
                    break;
                default:
                    break;
            }
        }

        protected override void ParseBlocks()
        {
            SetText("PROP_VAL_UNION");
            AddChildBlockT(I, "I");
            AddChildBlockT(L, "L");
            AddChildBlockT(B, "B");
            AddChild(LpszA, "LpszA");
            AddChild(Bin, "Bin");
            AddChild(LpszW, "LpszW");
            AddChild(Lpguid, "Lpguid");
            AddChild(Ft, "Ft");
            AddChildBlockT(Err, "Err");
            AddChild(MVi, "MVi");
            AddChild(MVl, "MVl");
            AddChild(MVszA, "MVszA");
            AddChild(MVbin, "MVbin");
            AddChild(MVguid, "MVguid");
            AddChild(MVszW, "MVszW");
            AddChild(MVft, "MVft");
            AddChildBlockT(LReserved, "LReserved");
        }
    }
}

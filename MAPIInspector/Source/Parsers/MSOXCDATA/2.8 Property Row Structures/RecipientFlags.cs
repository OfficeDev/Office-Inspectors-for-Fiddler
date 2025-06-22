namespace MAPIInspector.Parsers
{
    using BlockParser;

    /// <summary>
    /// 2.8.3.1 RecipientFlags Field
    /// </summary>
    public class RecipientFlags : Block
    {
        private BlockT<byte> Byte0;

        /// <summary>
        /// If this flag is b'1', a different transport is responsible for delivery to this recipient (1).
        /// </summary>
        public BlockT<bool> R;

        /// <summary>
        /// If this flag is b'1', the value of the TransmittableDisplayName field is the same as the value of the DisplayName field.
        /// </summary>
        public BlockT<bool> S;

        /// <summary>
        /// If this flag is b'1', the TransmittableDisplayName (section 2.8.3.2) field is included.
        /// </summary>
        public BlockT<bool> T;

        /// <summary>
        /// If this flag is b'1', the DisplayName (section 2.8.3.2) field is included.
        /// </summary>
        public BlockT<bool> D;

        /// <summary>
        /// If this flag is b'1', the EmailAddress (section 2.8.3.2) field is included.
        /// </summary>
        public BlockT<bool> E;

        /// <summary>
        /// This enumeration specifies the type of address.
        /// </summary>
        public BlockT<AddressTypeEnum> Type;

        private BlockT<byte> Byte1;

        /// <summary>
        /// If this flag is b'1', this recipient (1) has a non-standard address type and the AddressType field is included.
        /// </summary>
        public BlockT<bool> O;

        /// <summary>
        /// The server MUST set this to b'0000'.
        /// </summary>
        public BlockT<byte> Reserved;

        /// <summary>
        /// If this flag is b'1', the SimpleDisplayName field is included.
        /// </summary>
        [BitAttribute(1)]
        public BlockT<bool> I;

        /// <summary>
        /// If this flag is b'1', the associated string properties are in Unicode with a 2-byte terminating null character; if this flag is b'0', string properties are MBCS with a single terminating null character.
        /// </summary>
        public BlockT<bool> U;

        /// <summary>
        /// If b'1', this flag specifies that the recipient (1) does not support receiving rich text messages.
        /// </summary>
        public BlockT<bool> N;

        /// <summary>
        /// Parse the RecipientFlags structure.
        /// </summary>
        protected override void Parse()
        {
            Byte0 = ParseT<byte>();
            int index = 0;
            R = CreateBlock(1 == BaseStructure.GetBits(Byte0.Data, index, 1), Byte0.Size, Byte0.Offset);
            index = index + 1;
            S = CreateBlock(1 == BaseStructure.GetBits(Byte0.Data, index, 1), Byte0.Size, Byte0.Offset);
            index = index + 1;
            T = CreateBlock(1 == BaseStructure.GetBits(Byte0.Data, index, 1), Byte0.Size, Byte0.Offset);
            index = index + 1;
            D = CreateBlock(1 == BaseStructure.GetBits(Byte0.Data, index, 1), Byte0.Size, Byte0.Offset);
            index = index + 1;
            E = CreateBlock(1 == BaseStructure.GetBits(Byte0.Data, index, 1), Byte0.Size, Byte0.Offset);
            index = index + 1;
            Type = CreateBlock((AddressTypeEnum)BaseStructure.GetBits(Byte0.Data, index, 3), Byte0.Size, Byte0.Offset);

            Byte1 = ParseT<byte>();
            index = 0;
            O = CreateBlock(1 == BaseStructure.GetBits(Byte1.Data, index, 1), Byte1.Size, Byte1.Offset);
            index = index + 1;
            Reserved = CreateBlock(BaseStructure.GetBits(Byte1.Data, index, 4), Byte1.Size, Byte1.Offset);
            index = index + 4;
            I = CreateBlock(1 == BaseStructure.GetBits(Byte1.Data, index, 1), Byte1.Size, Byte1.Offset);
            index = index + 1;
            U = CreateBlock(1 == BaseStructure.GetBits(Byte1.Data, index, 1), Byte1.Size, Byte1.Offset);
            index = index + 1;
            N = CreateBlock(1 == BaseStructure.GetBits(Byte1.Data, index, 1), Byte1.Size, Byte1.Offset);
        }

        protected override void ParseBlocks()
        {
            SetText("RecipientFlags");
            AddChildBlockT(R, "R");
            AddChildBlockT(S, "S");
            AddChildBlockT(T, "T");
            AddChildBlockT(D, "D");
            AddChildBlockT(E, "E");
            AddChildBlockT(Type, "Type");
            AddChildBlockT(O, "O");
            AddChildBlockT(Reserved, "Reserved");
            AddChildBlockT(I, "I");
            AddChildBlockT(U, "U");
            AddChildBlockT(N, "N");
        }
    }
}

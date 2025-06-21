namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    /// 2.8.3.1 RecipientFlags Field
    /// </summary>
    public class RecipientFlags : BaseStructure
    {
        /// <summary>
        /// If this flag is b'1', a different transport is responsible for delivery to this recipient (1).
        /// </summary>
        [BitAttribute(1)]
        public byte R;

        /// <summary>
        /// If this flag is b'1', the value of the TransmittableDisplayName field is the same as the value of the DisplayName field.
        /// </summary>
        [BitAttribute(1)]
        public byte S;

        /// <summary>
        /// If this flag is b'1', the TransmittableDisplayName (section 2.8.3.2) field is included.
        /// </summary>
        [BitAttribute(1)]
        public byte T;

        /// <summary>
        /// If this flag is b'1', the DisplayName (section 2.8.3.2) field is included.
        /// </summary>
        [BitAttribute(1)]
        public byte D;

        /// <summary>
        /// If this flag is b'1', the EmailAddress (section 2.8.3.2) field is included.
        /// </summary>
        [BitAttribute(1)]
        public byte E;

        /// <summary>
        /// This enumeration specifies the type of address.
        /// </summary>
        [BitAttribute(3)]
        public AddressTypeEnum Type;

        /// <summary>
        /// If this flag is b'1', this recipient (1) has a non-standard address type and the AddressType field is included.
        /// </summary>
        [BitAttribute(1)]
        public byte O;

        /// <summary>
        /// The server MUST set this to b'0000'.
        /// </summary>
        [BitAttribute(4)]
        public byte Reserved;

        /// <summary>
        /// If this flag is b'1', the SimpleDisplayName field is included.
        /// </summary>
        [BitAttribute(1)]
        public byte I;

        /// <summary>
        /// If this flag is b'1', the associated string properties are in Unicode with a 2-byte terminating null character; if this flag is b'0', string properties are MBCS with a single terminating null character.
        /// </summary>
        [BitAttribute(1)]
        public byte U;

        /// <summary>
        /// If b'1', this flag specifies that the recipient (1) does not support receiving rich text messages.
        /// </summary>
        [BitAttribute(1)]
        public byte N;

        /// <summary>
        /// Parse the RecipientFlags structure.
        /// </summary>
        /// <param name="s">A stream containing the RecipientFlags structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            byte tempByte = ReadByte();
            int index = 0;
            R = BaseStructure.GetBits(tempByte, index, 1);
            index = index + 1;
            S = BaseStructure.GetBits(tempByte, index, 1);
            index = index + 1;
            T = BaseStructure.GetBits(tempByte, index, 1);
            index = index + 1;
            D = BaseStructure.GetBits(tempByte, index, 1);
            index = index + 1;
            E = BaseStructure.GetBits(tempByte, index, 1);
            index = index + 1;
            Type = (AddressTypeEnum)GetBits(tempByte, index, 3);

            tempByte = ReadByte();
            index = 0;
            O = BaseStructure.GetBits(tempByte, index, 1);
            index = index + 1;
            Reserved = BaseStructure.GetBits(tempByte, index, 4);
            index = index + 4;
            I = BaseStructure.GetBits(tempByte, index, 1);
            index = index + 1;
            U = BaseStructure.GetBits(tempByte, index, 1);
            index = index + 1;
            N = BaseStructure.GetBits(tempByte, index, 1);
        }
    }
}

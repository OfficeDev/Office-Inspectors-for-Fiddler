namespace MAPIInspector.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    /// <summary>
    /// 2.8.3.2 RecipientRow Structure
    /// </summary>
    public class RecipientRow : BaseStructure
    {
        /// <summary>
        /// A RecipientFlags structure, as specified in section 2.8.3.1.
        /// </summary>
        public RecipientFlags RecipientFlags;

        /// <summary>
        /// Unsigned integer. This field MUST be present when the Type field of the RecipientFlags field is set to X500DN (0x1) and MUST NOT be present otherwise.
        /// </summary>
        public byte? AddressPrefixUsed;

        /// <summary>
        /// An enumeration. This field MUST be present when the Type field of the RecipientFlags field is set to X500DN (0x1) and MUST NOT be present otherwise.
        /// </summary>
        public DisplayType? DisplayType;

        /// <summary>
        /// A null-terminated ASCII string.
        /// </summary>
        public MAPIString X500DN;

        /// <summary>
        /// An unsigned integer. This field MUST be present when the Type field of the RecipientFlags field is set to PersonalDistributionList1 (0x6) or PersonalDistributionList2 (0x7).
        /// </summary>
        public ushort? EntryIdSize;

        /// <summary>
        /// An array of bytes. This field MUST be present when the Type field of the RecipientFlags field is set to PersonalDistributionList1 (0x6) or PersonalDistributionList2 (0x7).
        /// </summary>
        public AddressBookEntryID EntryID;

        /// <summary>
        /// This value specifies the size of the SearchKey field.
        /// </summary>
        public ushort? SearchKeySize;

        /// <summary>
        /// This array specifies the search key of the distribution list.
        /// </summary>
        public byte?[] SearchKey;

        /// <summary>
        /// This string specifies the address type of the recipient (1).
        /// </summary>
        public MAPIString AddressType;

        /// <summary>
        /// This string specifies the email address of the recipient (1).
        /// </summary>
        public MAPIString EmailAddress;

        /// <summary>
        /// This string specifies the display name of the recipient (1).
        /// </summary>
        public MAPIString DisplayName;

        /// <summary>
        /// This string specifies the simple display name of the recipient (1).
        /// </summary>
        public MAPIString SimpleDisplayName;

        /// <summary>
        /// This string specifies the transmittable display name of the recipient (1).
        /// </summary>
        public MAPIString TransmittableDisplayName;

        /// <summary>
        /// This value specifies the number of columns from the RecipientColumns field([MS-OXCROPS] section 2.2.6.16.2) that are included in the RecipientProperties field.
        /// </summary>
        public ushort? RecipientColumnCount;

        /// <summary>
        /// The columns used for this row are those specified in RecipientProperties.
        /// </summary>
        public PropertyRow RecipientProperties;

        /// <summary>
        /// The array of property tag.
        /// </summary>
        private PropertyTag[] propTags;

        /// <summary>
        /// Initializes a new instance of the RecipientRow class
        /// </summary>
        /// <param name="propTags">The property Tags</param>
        public RecipientRow(PropertyTag[] propTags)
        {
            propTags = propTags;
        }

        /// <summary>
        /// Parse the RecipientRow structure.
        /// </summary>
        /// <param name="s">A stream containing the RecipientRow structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RecipientFlags = new RecipientFlags();
            RecipientFlags.Parse(s);
            if (RecipientFlags.Type == AddressTypeEnum.X500DN)
            {
                AddressPrefixUsed = ReadByte();
                DisplayType = (DisplayType)ReadByte();
                X500DN = new MAPIString(Encoding.ASCII);
                X500DN.Parse(s);
            }
            else if (RecipientFlags.Type == AddressTypeEnum.PersonalDistributionList1 || RecipientFlags.Type == AddressTypeEnum.PersonalDistributionList2)
            {
                EntryIdSize = ReadUshort();
                EntryID = new AddressBookEntryID();
                EntryID.Parse(s);
                SearchKeySize = ReadUshort();
                SearchKey = ConvertArray(ReadBytes((int)SearchKeySize));
            }
            else if (RecipientFlags.Type == AddressTypeEnum.NoType && RecipientFlags.O == 0x1)
            {
                AddressType = new MAPIString(Encoding.ASCII);
                AddressType.Parse(s);
            }

            if (RecipientFlags.E == 0x1)
            {
                EmailAddress = new MAPIString((RecipientFlags.U == 0x1) ? Encoding.Unicode : Encoding.ASCII);
                EmailAddress.Parse(s);
            }

            if (RecipientFlags.D == 0x1)
            {
                DisplayName = new MAPIString((RecipientFlags.U == 0x1) ? Encoding.Unicode : Encoding.ASCII);
                DisplayName.Parse(s);
            }

            if (RecipientFlags.I == 0x1)
            {
                SimpleDisplayName = new MAPIString((RecipientFlags.U == 0x1) ? Encoding.Unicode : Encoding.ASCII);
                SimpleDisplayName.Parse(s);
            }

            if (RecipientFlags.T == 0x1)
            {
                TransmittableDisplayName = new MAPIString((RecipientFlags.U == 0x1) ? Encoding.Unicode : Encoding.ASCII);
                TransmittableDisplayName.Parse(s);
            }

            RecipientColumnCount = ReadUshort();
            List<PropertyTag> propTagsActually = new List<PropertyTag>();
            if (propTags.Length >= RecipientColumnCount)
            {
                for (int i = 0; i < RecipientColumnCount; i++)
                {
                    propTagsActually.Add(propTags[i]);
                }
            }
            else
            {
                throw new Exception(string.Format("Request format error: the RecipientColumnCount {0} should be less than RecipientColumns count {1}", RecipientColumnCount, propTags.Length));
            }

            PropertyRow tempPropertyRow = new PropertyRow(propTagsActually.ToArray());
            RecipientProperties = tempPropertyRow;
            RecipientProperties.Parse(s);
        }
    }
}

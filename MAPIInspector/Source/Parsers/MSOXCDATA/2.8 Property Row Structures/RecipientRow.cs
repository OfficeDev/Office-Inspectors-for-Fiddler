namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 2.8.3.2 RecipientRow Structure
    /// </summary>
    public class RecipientRow : Block
    {
        /// <summary>
        /// A RecipientFlags structure, as specified in section 2.8.3.1.
        /// </summary>
        public RecipientFlags RecipientFlags;

        /// <summary>
        /// Unsigned integer. This field MUST be present when the Type field of the RecipientFlags field is set to X500DN (0x1) and MUST NOT be present otherwise.
        /// </summary>
        public BlockT<byte> AddressPrefixUsed;

        /// <summary>
        /// An enumeration. This field MUST be present when the Type field of the RecipientFlags field is set to X500DN (0x1) and MUST NOT be present otherwise.
        /// </summary>
        public BlockT<DisplayType> DisplayType;

        /// <summary>
        /// A null-terminated ASCII string.
        /// </summary>
        public BlockStringA X500DN;

        /// <summary>
        /// An unsigned integer. This field MUST be present when the Type field of the RecipientFlags field is set to PersonalDistributionList1 (0x6) or PersonalDistributionList2 (0x7).
        /// </summary>
        public BlockT<ushort> EntryIdSize;

        /// <summary>
        /// An array of bytes. This field MUST be present when the Type field of the RecipientFlags field is set to PersonalDistributionList1 (0x6) or PersonalDistributionList2 (0x7).
        /// </summary>
        public AddressBookEntryID EntryID;

        /// <summary>
        /// This value specifies the size of the SearchKey field.
        /// </summary>
        public BlockT<ushort> SearchKeySize;

        /// <summary>
        /// This array specifies the search key of the distribution list.
        /// </summary>
        public BlockBytes SearchKey;

        /// <summary>
        /// This string specifies the address type of the recipient (1).
        /// </summary>
        public BlockStringA AddressType;

        /// <summary>
        /// This string specifies the email address of the recipient (1).
        /// </summary>
        public BlockString EmailAddress;

        /// <summary>
        /// This string specifies the display name of the recipient (1).
        /// </summary>
        public BlockString DisplayName;

        /// <summary>
        /// This string specifies the simple display name of the recipient (1).
        /// </summary>
        public BlockString SimpleDisplayName;

        /// <summary>
        /// This string specifies the transmittable display name of the recipient (1).
        /// </summary>
        public BlockString TransmittableDisplayName;

        /// <summary>
        /// This value specifies the number of columns from the RecipientColumns field([MS-OXCROPS] section 2.2.6.16.2) that are included in the RecipientProperties field.
        /// </summary>
        public BlockT<ushort> RecipientColumnCount;

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
        /// <param name="_propTags">The property Tags</param>
        public RecipientRow(PropertyTag[] _propTags)
        {
            propTags = _propTags;
        }

        /// <summary>
        /// Parse the RecipientRow structure.
        /// </summary>
        protected override void Parse()
        {
            RecipientFlags = Parse<RecipientFlags>();
            if (RecipientFlags.Type.Data == AddressTypeEnum.X500DN)
            {
                AddressPrefixUsed = ParseT<byte>();
                DisplayType = ParseT<DisplayType>();
                X500DN = ParseStringA();
            }
            else if (RecipientFlags.Type.Data == AddressTypeEnum.PersonalDistributionList1 || RecipientFlags.Type.Data == AddressTypeEnum.PersonalDistributionList2)
            {
                EntryIdSize = ParseT<ushort>();
                EntryID = Parse<AddressBookEntryID>();
                SearchKeySize = ParseT<ushort>();
                SearchKey = ParseBytes(SearchKeySize.Data);
            }
            else if (RecipientFlags.Type.Data == AddressTypeEnum.NoType && RecipientFlags.O.Data)
            {
                AddressType = ParseStringA();
            }

            if (RecipientFlags.E.Data)
            {
                if (RecipientFlags.U.Data)
                {
                    EmailAddress = ParseStringW();
                }
                else
                {
                    EmailAddress = ParseStringA();
                }
            }

            if (RecipientFlags.D.Data)
            {
                if (RecipientFlags.U.Data)
                {
                    DisplayName = ParseStringW();
                }
                else
                {
                    DisplayName = ParseStringA();
                }
            }

            if (RecipientFlags.I.Data)
            {
                if (RecipientFlags.U.Data)
                {
                    SimpleDisplayName = ParseStringW();
                }
                else
                {
                    SimpleDisplayName = ParseStringA();
                }
            }

            if (RecipientFlags.T.Data)
            {
                if (RecipientFlags.U.Data)
                {
                    TransmittableDisplayName = ParseStringW();
                }
                else
                {
                    TransmittableDisplayName = ParseStringA();
                }
            }

            RecipientColumnCount = ParseT<ushort>();
            var propTagsActually = new List<PropertyTag>();
            if (propTags.Length >= RecipientColumnCount.Data)
            {
                for (int i = 0; i < RecipientColumnCount.Data; i++)
                {
                    propTagsActually.Add(propTags[i]);
                }
            }
            else
            {
                throw new Exception(string.Format("Request format error: the RecipientColumnCount {0} should be less than RecipientColumns count {1}", RecipientColumnCount, propTags.Length));
            }

            RecipientProperties = new PropertyRow(propTagsActually.ToArray());
            RecipientProperties.Parse(parser);
        }

        protected override void ParseBlocks()
        {
            SetText("RecipientRow");
            AddChild(RecipientFlags, "RecipientFlags");
            AddChildBlockT(AddressPrefixUsed, "AddressPrefixUsed");
            AddChildBlockT(DisplayType, "DisplayType");
            AddChild(X500DN, $"X500DN:{X500DN}");
            AddChildBlockT(EntryIdSize, "EntryIdSize");
            AddChild(EntryID, $"EntryID:{EntryID}");
            AddChild(EntryID, $"SearchKeySize:{SearchKeySize}");
            if (SearchKey != null) AddChild(SearchKey, $"SearchKey:{SearchKey.ToHexString()}");
            AddChild(AddressType, $"AddressType:{AddressType}");
            AddChild(EmailAddress, $"EmailAddress:{EmailAddress}");
            AddChild(DisplayName, $"DisplayName:{DisplayName}");
            AddChild(SimpleDisplayName, $"SimpleDisplayName:{SimpleDisplayName}");
            AddChild(TransmittableDisplayName, $"TransmittableDisplayName:{TransmittableDisplayName}");
            AddChildBlockT(RecipientColumnCount, "RecipientColumnCount");
            AddChild(RecipientProperties, "RecipientProperties");
        }
    }
}

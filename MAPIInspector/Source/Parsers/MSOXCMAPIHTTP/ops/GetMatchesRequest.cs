using System;
using System.Collections.Generic;
using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the GetMatchesRequest structure.
    /// 2.2.5.5 GetMatches
    /// </summary>
    public class GetMatchesRequest : BaseStructure
    {
        /// <summary>
        /// Reserved. The client MUST set this field to 0x00000000 and the server MUST ignore this field.
        /// </summary>
        public uint Reserved;

        /// <summary>
        /// A Boolean value that specifies whether the State field is present.
        /// </summary>
        public bool HasState;

        /// <summary>
        /// A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container.
        /// </summary>
        public STAT State;

        /// <summary>
        /// A Boolean value that specifies whether the MinimalIdCount and MinimalIds fields are present.
        /// </summary>
        public bool HasMinimalIds;

        /// <summary>
        /// An unsigned integer that specifies the number of structures present in the MinimalIds field.
        /// </summary>
        public uint? MinimalIdCount;

        /// <summary>
        /// An array of MinimalEntryID structures ([MS-OXNSPI] section 2.2.9.1) that constitute an Explicit Table.
        /// </summary>
        public MinimalEntryID[] MinimalIds;

        /// <summary>
        /// Reserved. The client MUST set this field to 0x00000000 and the server MUST ignore this field.
        /// </summary>
        public uint InterfaceOptionFlags;

        /// <summary>
        /// A Boolean value that specifies whether the Filter field is present.
        /// </summary>
        public bool HasFilter;

        /// <summary>
        /// A restriction, as specified in [MS-OXCDATA] section 2.12, that is to be applied to the rows in the address book container.
        /// </summary>
        public RestrictionType Filter;

        /// <summary>
        /// A Boolean value that specifies whether the PropertyNameGuid and PropertyNameId fields are present.
        /// </summary>
        public bool HasPropertyName;

        /// <summary>
        /// The GUID of the property to be opened.
        /// </summary>
        public Guid? PropertyNameGuid;

        /// <summary>
        /// A 4-byte value that specifies the ID of the property to be opened.
        /// </summary>
        public uint? PropertyNameId;

        /// <summary>
        /// An unsigned integer that specifies the number of rows the client is requesting.
        /// </summary>
        public uint RowCount;

        /// <summary>
        /// A Boolean value that specifies whether the Columns field is present.
        /// </summary>
        public bool HasColumns;

        /// <summary>
        /// A LargePropertyTagArray structure (section 2.2.1.8) that specifies the columns that the client is requesting.
        /// </summary>
        public LargePropertyTagArray Columns;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client.
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetMatchesRequest structure.
        /// </summary>
        /// <param name="s">A stream containing GetMatchesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            Reserved = ReadUint();
            HasState = ReadBoolean();

            if (HasState)
            {
                STAT stat = new STAT();
                stat.Parse(s);
                State = stat;
            }

            HasMinimalIds = ReadBoolean();

            if (HasMinimalIds)
            {
                MinimalIdCount = ReadUint();
                List<MinimalEntryID> me = new List<MinimalEntryID>();

                for (int i = 0; i < MinimalIdCount; i++)
                {
                    MinimalEntryID minimalEntryId = new MinimalEntryID();
                    minimalEntryId.Parse(s);
                    me.Add(minimalEntryId);
                }

                MinimalIds = me.ToArray();
            }

            InterfaceOptionFlags = ReadUint();
            HasFilter = ReadBoolean();

            if (HasFilter)
            {
                RestrictionType restriction = new RestrictionType(CountWideEnum.fourBytes);
                restriction.Parse(s);
                Filter = restriction;
            }

            HasPropertyName = ReadBoolean();

            if (HasPropertyName)
            {
                PropertyNameGuid = ReadGuid();
                PropertyNameId = ReadUint();
            }

            RowCount = ReadUint();
            HasColumns = ReadBoolean();

            if (HasColumns)
            {
                LargePropertyTagArray largePTA = new LargePropertyTagArray();
                largePTA.Parse(s);
                Columns = largePTA;
            }

            AuxiliaryBufferSize = ReadUint();

            if (AuxiliaryBufferSize > 0)
            {
                AuxiliaryBuffer = new ExtendedBuffer();
                AuxiliaryBuffer.Parse(s);
            }
            else
            {
                AuxiliaryBuffer = null;
            }
        }
    }
}
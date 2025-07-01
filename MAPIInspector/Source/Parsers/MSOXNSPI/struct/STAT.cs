using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.8 STAT
    /// A class indicates the STAT structure.
    /// </summary>
    public class STAT : BaseStructure
    {
        /// <summary>
        /// A DWORD [MS-DTYP] value that specifies a sort order.
        /// </summary>
        public uint SortType;

        /// <summary>
        /// A DWORD value that specifies the Minimal Entry ID of the address book container that STAT structure represents.
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
            SortType = ReadUint();
            ContainerID = ReadUint();
            CurrentRec = ReadUint();
            Delta = ReadUint();
            NumPos = ReadUint();
            TotalRecs = ReadUint();
            CodePage = ReadUint();
            TemplateLocale = ReadUint();
            SortLocale = ReadUint();
        }
    }
}

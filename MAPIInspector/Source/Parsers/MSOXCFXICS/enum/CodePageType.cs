namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Code page property types are used to transmit string properties using the code page format of the string as stored on the server,
    /// </summary>
    public enum CodePageType : ushort
    {
        /// <summary>
        /// PtypCodePage Unicode 51
        /// </summary>
        PtypCodePageUnicode = 0x84B0,

        /// <summary>
        /// PtypCodePage Unicode big end
        /// </summary>
        PtypCodePageUnicodeBigendian = 0x84B1,

        /// <summary>
        /// PtypCodePage western European
        /// </summary>
        PtypCodePageWesternEuropean = 0x84E4,

        /// <summary>
        /// ptypCodePag eUnicode 52
        /// </summary>
        ptypCodePageUnicode52 = 0x94B0
    }
}

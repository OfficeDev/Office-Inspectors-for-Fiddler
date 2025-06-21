namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System.IO;

    /// <summary>
    /// 2.7 PropertyProblem Structure
    /// </summary>
    public class PropertyProblem : BaseStructure
    {
        /// <summary>
        /// An unsigned integer. This value specifies an index into an array of property tags.
        /// </summary>
        public ushort Index;

        /// <summary>
        /// A PropertyTag structure, as specified in section 2.9.
        /// </summary>
        public PropertyTag PropertyTag;

        /// <summary>
        /// An unsigned integer. This value specifies the error that occurred when processing this property.
        /// </summary>
        public PropertyErrorCodes ErrorCode;

        /// <summary>
        /// Parse the PropertyProblem structure.
        /// </summary>
        /// <param name="s">A stream containing the PropertyProblem structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            Index = ReadUshort();
            PropertyTag = Block.Parse<PropertyTag>(s);
            ErrorCode = (PropertyErrorCodes)ReadUint();
        }
    }
}

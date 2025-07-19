using BlockParser;
using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.7 PropertyProblem Structure
    /// </summary>
    public class PropertyProblem : Block
    {
        /// <summary>
        /// An unsigned integer. This value specifies an index into an array of property tags.
        /// </summary>
        public BlockT<ushort> Index;

        /// <summary>
        /// A PropertyTag structure, as specified in section 2.9.
        /// </summary>
        public PropertyTag PropertyTag;

        /// <summary>
        /// An unsigned integer. This value specifies the error that occurred when processing this property.
        /// </summary>
        public BlockT<PropertyErrorCodes> ErrorCode;

        /// <summary>
        /// Parse the PropertyProblem structure.
        /// </summary>
        protected override void Parse()
        {
            Index = ParseT<ushort>();
            PropertyTag = Parse<PropertyTag>();
            ErrorCode = ParseT<PropertyErrorCodes>();
        }

        protected override void ParseBlocks()
        {
            Text = "PropertyProblem";
            AddChildBlockT(Index, "Index");
            AddChild(PropertyTag, "PropertyTag");
            AddChildBlockT(ErrorCode, "ErrorCode");
        }
    }
}

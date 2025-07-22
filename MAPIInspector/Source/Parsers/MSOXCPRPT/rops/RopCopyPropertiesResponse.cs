using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.10 RopCopyProperties
    /// A class indicates the RopCopyProperties ROP Response Buffer.
    /// </summary>
    public class RopCopyPropertiesResponse : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the SourceHandleIndex field specified in the request.
        /// </summary>
        public BlockT<byte> SourceHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public BlockT<ErrorCodes> ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the number of PropertyProblem structures in the PropertyProblems field.
        /// </summary>
        public BlockT<ushort> PropertyProblemCount;

        /// <summary>
        /// An array of PropertyProblem structures.
        /// </summary>
        public PropertyProblem[] PropertyProblems;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the DestHandleIndex field in the request.
        /// </summary>
        public BlockT<uint> DestHandleIndex;

        /// <summary>
        /// Parse the RopCopyPropertiesResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            SourceHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();

            if (ReturnValue == ErrorCodes.Success)
            {
                PropertyProblemCount = ParseT<ushort>();
                var interPropertyProblem = new List<PropertyProblem>();

                for (int i = 0; i < PropertyProblemCount; i++)
                {
                    interPropertyProblem.Add(Parse<PropertyProblem>());
                }

                PropertyProblems = interPropertyProblem.ToArray();
            }
            else if ((AdditionalErrorCodes)ReturnValue.Data == AdditionalErrorCodes.NullDestinationObject)
            {
                DestHandleIndex = ParseT<uint>();
            }
        }

        protected override void ParseBlocks()
        {
            Text = "RopCopyPropertiesResponse";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(SourceHandleIndex, "SourceHandleIndex");
            this.AddError(ReturnValue, "ReturnValue");
            AddChildBlockT(PropertyProblemCount, "PropertyProblemCount");
            AddLabeledChildren(PropertyProblems, "PropertyProblems");
            AddChildBlockT(DestHandleIndex, "DestHandleIndex");
        }
    }
}

using System.Collections.Generic;
using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.5 RopSetProperties
    /// A class indicates the RopSetProperties ROP Response Buffer.
    /// </summary>
    public class RopSetPropertiesResponse : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public BlockT<ErrorCodes> ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the number of PropertyProblem structures in the PropertyProblems field.
        /// </summary>
        public BlockT<ushort> PropertyProblemCount;

        /// <summary>
        /// An array of PropertyProblem structures. The number of structures contained in this field is specified by the PropertyProblemCount field.
        /// </summary>
        public PropertyProblem[] PropertyProblems;

        /// <summary>
        /// Parse the RopSetPropertiesResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            List<PropertyRow> tmpRows = new List<PropertyRow>();
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
        }

        protected override void ParseBlocks()
        {
            SetText("RopSetPropertiesResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            this.AddError(ReturnValue, "ReturnValue");
            AddChildBlockT(PropertyProblemCount, "PropertyProblemCount");
            AddLabeledChildren(PropertyProblems, "PropertyProblems");
        }
    }
}

namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System.Collections.Generic;

    /// <summary>
    ///  2.2.2.6 RopSetPropertiesNoReplicate
    ///  A class indicates the RopSetPropertiesNoReplicate ROP Response Buffer.
    /// </summary>
    public class RopSetPropertiesNoReplicateResponse : Block
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
        /// Parse the RopSetPropertiesNoReplicateResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();

            if (ReturnValue.Data == ErrorCodes.Success)
            {
                PropertyProblemCount = ParseT<ushort>();
                var interPropertyProblem = new List<PropertyProblem>();

                for (int i = 0; i < PropertyProblemCount.Data; i++)
                {
                    interPropertyProblem.Add(Parse<PropertyProblem>());
                }

                PropertyProblems = interPropertyProblem.ToArray();
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopSetPropertiesNoReplicateResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            if (ReturnValue.Data != 0) AddChild(ReturnValue, $"ReturnValue:{ReturnValue.Data.FormatErrorCode()}");
            AddChildBlockT(PropertyProblemCount, "PropertyProblemCount");
            AddLabeledChildren(PropertyProblems, "PropertyProblems");
        }
    }
}

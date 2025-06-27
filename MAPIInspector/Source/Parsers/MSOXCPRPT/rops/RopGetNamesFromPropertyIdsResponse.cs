using BlockParser;
using System.Collections.Generic;
using System.Windows.Forms.Design;

namespace MAPIInspector.Parsers
{
    /// <summary>
    ///  2.2.2.13 RopGetNamesFromPropertyIds
    ///  A class indicates the RopGetNamesFromPropertyIds ROP Response Buffer.
    /// </summary>
    public class RopGetNamesFromPropertyIdsResponse : Block
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
        /// An unsigned integer that specifies the number of structures in the PropertyNames field.
        /// </summary>
        public BlockT<ushort> PropertyNameCount;

        /// <summary>
        /// A list of PropertyName structures that specifies the property names requested.
        /// </summary>
        public PropertyName[] PropertyNames;

        /// <summary>
        /// Parse the RopGetNamesFromPropertyIdsResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();

            if (ReturnValue == ErrorCodes.Success)
            {
                PropertyNameCount = ParseT<ushort>();
                var tmpPropertyNames = new List<PropertyName>();
                for (int i = 0; i < PropertyNameCount; i++)
                {
                    tmpPropertyNames.Add(Parse<PropertyName>());
                }
                PropertyNames = tmpPropertyNames.ToArray();
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopGetNamesFromPropertyIdsResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            if (ReturnValue != null) AddChild(ReturnValue, $"ReturnValue:{ReturnValue.Data.FormatErrorCode()}");
            AddChildBlockT(PropertyNameCount, "PropertyNameCount");
            AddLabeledChildren(PropertyNames, "PropertyNames");
        }
    }
}

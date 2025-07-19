using BlockParser;
using System.Collections.Generic;
using System.Text;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.7.3 RopGetAddressTypes
    /// A class indicates the RopGetAddressTypes ROP Response Buffer.
    /// </summary>
    public class RopGetAddressTypesResponse : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public BlockT<ErrorCodes> ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the number of strings in the AddressTypes field.
        /// </summary>
        public BlockT<ushort> AddressTypeCount;

        /// <summary>
        /// An unsigned integer that specifies the length of the AddressTypes field.
        /// </summary>
        public BlockT<ushort> AddressTypeSize;

        /// <summary>
        /// A list of null-terminated ASCII strings.
        /// </summary>
        public BlockString[] AddressTypes;

        /// <summary>
        /// Parse the RopGetAddressTypesResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();

            if (ReturnValue == ErrorCodes.Success)
            {
                AddressTypeCount = ParseT<ushort>();
                AddressTypeSize = ParseT<ushort>();
                var listAddressTypes = new List<BlockString>();

                for (int i = 0; i < AddressTypeCount; i++)
                {
                    listAddressTypes.Add(ParseStringA());
                }

                AddressTypes = listAddressTypes.ToArray();
            }
        }

        protected override void ParseBlocks()
        {
            Text = "RopGetAddressTypesResponse";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            this.AddError(ReturnValue, "ReturnValue");
            AddChildBlockT(AddressTypeCount, "AddressTypeCount");
            AddChildBlockT(AddressTypeSize, "AddressTypeSize");
            AddLabeledChildren(AddressTypes, "AddressTypes");
        }
    }
}

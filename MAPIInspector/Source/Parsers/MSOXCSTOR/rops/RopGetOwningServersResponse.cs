namespace MAPIInspector.Parsers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    /// <summary>
    ///  2.2.1.6 RopGetOwningServers
    ///  A class indicates the RopGetOwningServers ROP Response Buffer.
    /// </summary>
    public class RopGetOwningServersResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the number of strings in the OwningServers field.
        /// </summary>
        public ushort? OwningServersCount;

        /// <summary>
        /// An unsigned integer that specifies the number of strings in the OwningServers field that refer to lowest-cost servers.
        /// </summary>
        public ushort? CheapServersCount;

        /// <summary>
        /// A list of null-terminated ASCII strings that specify which servers have replicas (1) of this folder.
        /// </summary>
        public MAPIString[] OwningServers;

        /// <summary>
        /// Parse the RopGetOwningServersResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetOwningServersResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            InputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                OwningServersCount = ReadUshort();
                CheapServersCount = ReadUshort();

                List<MAPIString> tmpOwning = new List<MAPIString>();
                for (int i = 0; i < OwningServersCount; i++)
                {
                    MAPIString subOwing = new MAPIString(Encoding.ASCII);
                    subOwing.Parse(s);
                    tmpOwning.Add(subOwing);
                }

                OwningServers = tmpOwning.ToArray();
            }
        }
    }
}

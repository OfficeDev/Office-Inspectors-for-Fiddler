namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System.IO;

    /// <summary>
    ///  2.2.2.14 RopOpenStream
    ///  A class indicates the RopOpenStream ROP Request Buffer.
    /// </summary>
    public class RopOpenStreamRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// A PropertyTag structure that specifies the property of the object to stream. 
        /// </summary>
        public PropertyTag PropertyTag;

        /// <summary>
        /// A flags structure that contains flags that control how the stream is opened. 
        /// </summary>
        public OpenModeFlags OpenModeFlags;

        /// <summary>
        /// Parse the RopOpenStreamRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopOpenStreamRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            OutputHandleIndex = ReadByte();
            PropertyTag = Block.Parse<PropertyTag>(s);
            OpenModeFlags = (OpenModeFlags)ReadByte();
        }
    }
}

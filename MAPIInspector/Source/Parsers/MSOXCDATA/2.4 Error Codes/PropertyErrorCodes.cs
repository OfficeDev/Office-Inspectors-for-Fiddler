namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCDATA] 2.4.2 Property Error Codes
    /// </summary>
    public enum PropertyErrorCodes : uint
    {
        /// <summary>
        /// On get, indicates that the property or column value is too large to be retrieved by the request, and the property value needs to be accessed with the RopOpenStream ROP ([MS-OXCROPS] section 2.2.9.1).
        /// </summary>
        NotEnoughMemory = 0x8007000E,

        /// <summary>
        /// On get, indicates that the property or column has no value for this object.
        /// </summary>
        NotFound = 0x8004010F,

        /// <summary>
        /// On set, indicates that the property value is not acceptable to the server.
        /// </summary>
        BadValue = 0x80040301,

        /// <summary>
        /// On get or set, indicates that the data type passed with the property or column is undefined.
        /// </summary>
        InvalidType = 0x80040302,

        /// <summary>
        /// On get or set, indicates that the data type passed with the property or column is not acceptable to the server.
        /// </summary>
        UnsupportedType = 0x80040303,

        /// <summary>
        /// On get or set, indicates that the data type passed with the property or column is not the type expected by the server.
        /// </summary>
        UnexpectedType = 0x80040304,

        /// <summary>
        /// Indicates that the result set of the operation is too big for the server to return.
        /// </summary>
        TooBig = 0x80040305,

        /// <summary>
        /// On a copy operation, indicates that the server cannot copy the object, possibly because the source and destination are on different types of servers, and the server will delegate the copying to client code.
        /// </summary>
        DeclineCopy = 0x80040306,

        /// <summary>
        /// On get or set, indicates that the server does not support property IDs in this range, usually the named property ID range (from 0x8000 through 0xFFFF).
        /// </summary>
        UnexpectedId = 0x80040307
    }
}

using Fiddler;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Information for FastertransferStream Partial
    /// </summary>
    public class PartialContextInformation
    {
        /// <summary>
        /// Initializes a new instance of the PartialContextInformation class
        /// </summary>
        /// <param name="type">The property type</param>
        /// <param name="id">The property id</param>
        /// <param name="remainSize">The property value remain size</param>
        /// <param name="subRemainSize">The property value sub remain size for multiple type data</param>
        /// <param name="isGet">Boolean value indicates if this is about RopGetBuffer ROP</param>
        /// <param name="session">The session that contains this</param>
        /// <param name="payLoadCompresssedXOR">The payload value about this</param>
        public PartialContextInformation(PropertyDataType type = 0, PidTagPropertyEnum id = 0, int remainSize = -1, int subRemainSize = -1, bool isGet = true, Session session = null, List<byte[]> payLoadCompresssedXOR = null)
        {
            Type = type;
            ID = id;
            RemainSize = remainSize;
            SubRemainSize = subRemainSize;
            IsGet = isGet;
            PayLoadCompresssedXOR = payLoadCompresssedXOR;
            Session = session;
        }

        /// <summary>
        /// Gets or sets the property type
        /// </summary>
        public PropertyDataType Type { get; set; }

        /// <summary>
        /// Gets or sets the property ID
        /// </summary>
        public PidTagPropertyEnum ID { get; set; }

        /// <summary>
        /// Gets or sets the property value remain size
        /// </summary>
        public int RemainSize { get; set; }

        /// <summary>
        /// Gets or sets the property value sub remain size for multiple type data
        /// </summary>
        public int SubRemainSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is about RopGetBuffer ROP
        /// </summary>
        public bool IsGet { get; set; }

        /// <summary>
        /// Gets or sets the payload value about this
        /// </summary>
        public List<byte[]> PayLoadCompresssedXOR { get; set; }

        /// <summary>
        /// Gets or sets the session that contains this
        /// </summary>
        public Session Session { get; set; }
    }
}

using System;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A flags structure that contains flags that control options for moving or copying properties.
    /// </summary>
    [Flags]
    public enum CopyFlags : byte
    {
        /// <summary>
        /// If this bit is set, properties are moved; otherwise, properties are copied
        /// </summary>
        Move = 0x01,

        /// <summary>
        /// Properties that already have a value on the destination object will not be overwritten
        /// </summary>
        NoOverwrite = 0x02,
    }
}

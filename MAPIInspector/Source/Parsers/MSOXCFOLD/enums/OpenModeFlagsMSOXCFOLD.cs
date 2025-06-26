using System;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Section 2.2.1.1.1   RopOpenFolder ROP Request Buffer
    /// </summary>
    [Flags]
    public enum OpenModeFlagsMSOXCFOLD : byte
    {
        /// <summary>
        /// The operation opens either an existing folder or a soft-deleted folder
        /// </summary>
        OpenSoftDeleted = 0x04
    }
}
using System;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCMSG] 2.2.3.10.1 RopSetReadFlags ROP Request Buffer
    /// The enum specifies the flags to set.
    /// </summary>
    [Flags]
    public enum ReadFlags : byte
    {
        /// <summary>
        /// The server sets the read flag and sends the receipt.
        /// </summary>
        rfDefault = 0x00,

        /// <summary>
        /// The user requests that any pending read receipt be canceled; the server sets the mfRead bit
        /// </summary>
        rfSuppressReceipt = 0x01,

        /// <summary>
        /// Ignored by the server
        /// </summary>
        rfReserved = 0x0A,

        /// <summary>
        /// Server clears the mfRead bit; the client MUST include the rfSuppressReceipt bit with this flag
        /// </summary>
        rfClearReadFlag = 0x04,

        /// <summary>
        /// The server sends a read receipt if one is pending, but does not change the mfRead bit
        /// </summary>
        rfGenerateReceiptOnly = 0x10,

        /// <summary>
        /// The server clears the mfNotifyRead bit but does not send a read receipt
        /// </summary>
        rfClearNotifyRead = 0x20,

        /// <summary>
        /// The server clears the mfNotifyUnread bit but does not send a nonread receipt
        /// </summary>
        rfClearNotifyUnread = 0x40
    }
}

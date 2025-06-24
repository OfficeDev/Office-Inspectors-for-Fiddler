namespace MAPIInspector.Parsers
{
    using System;

    /// <summary>
    /// 2.2.3.1.2 RopOpenMessage ROP Response Buffer
    /// An enumeration that specifies the flag of RecipientType.
    /// </summary>
    [Flags]
    public enum RecipientTypeFlag : byte
    {
        /// <summary>
        /// This flag indicates that this recipient (1) did not successfully receive the message on the previous attempt
        /// </summary>
        FailToReceiveTheMessageOnThePreviousAttempt = 0x10,

        /// <summary>
        /// This flag indicates that this recipient (1) did successfully receive the message on the previous attempt
        /// </summary>
        SuccessfullyReceiveTheMessageOnThePreviousAttempt = 0x80
    }
}

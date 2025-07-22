namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXORULE] 2.2.5.1.2.5 OP_BOUNCE ActionData Structure
    /// The enum value of Bounce Code.
    /// </summary>
    public enum BounceCodeEnum : uint
    {
        /// <summary>
        /// The message was rejected because it was too large
        /// </summary>
        RejectedMessageTooLarge = 0x0000000D,

        /// <summary>
        /// The message was rejected because it cannot be displayed to the user
        /// </summary>
        RejectedMessageNotDisplayed = 0x0000001F,

        /// <summary>
        /// The message delivery was denied for other reasons
        /// </summary>
        DeliveryMessageDenied = 0x00000026
    }
}

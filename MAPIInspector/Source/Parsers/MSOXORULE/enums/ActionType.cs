namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.5.1 ActionBlock Structure
    /// The enum value of ActionType.
    /// </summary>
    public enum ActionType : byte
    {
        /// <summary>
        /// Moves the message to a folder. MUST NOT be used in a public folder rule
        /// </summary>
        OP_MOVE = 0x01,

        /// <summary>
        /// Copies the message to a folder. MUST NOT be used in a public folder rule
        /// </summary>
        OP_COPY = 0x02,

        /// <summary>
        /// Replies to the message
        /// </summary>
        OP_REPLY = 0x03,

        /// <summary>
        /// Sends an OOF reply to the message
        /// </summary>
        OP_OOF_REPLY = 0x04,

        /// <summary>
        /// Used for actions that cannot be executed by the server
        /// </summary>
        OP_DEFER_ACTION = 0x05,

        /// <summary>
        /// Rejects the message back to the sender.
        /// </summary>
        OP_BOUNCE = 0x06,

        /// <summary>
        /// Forwards the message to a recipient (2) address
        /// </summary>
        OP_FORWARD = 0x07,

        /// <summary>
        /// Resends the message to another recipient (2), who acts as a delegate
        /// </summary>
        OP_DELEGATE = 0x08,

        /// <summary>
        /// Adds or changes a property on the message
        /// </summary>
        OP_TAG = 0x09,

        /// <summary>
        /// Deletes the message.
        /// </summary>
        OP_DELETE = 0x0A,

        /// <summary>
        /// Sets the MSGFLAG_READ flag in the PidTagMessageFlags property ([MS-OXCMSG] section 2.2.1.6) on the message
        /// </summary>
        OP_MARK_AS_READ = 0x0B
    }
}

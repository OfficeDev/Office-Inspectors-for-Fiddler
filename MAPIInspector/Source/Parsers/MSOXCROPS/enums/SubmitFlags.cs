namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The enum type for flags indicates how the message is to be delivered.
    /// </summary>
    public enum SubmitFlags : byte
    {
        /// <summary>
        /// No special behavior is specified
        /// </summary>
        None = 0x00,

        /// <summary>
        /// The message needs to be preprocessed by the server.
        /// </summary>
        PreProcess = 0x01,

        /// <summary>
        /// The message is to be processed by a client spooler.
        /// </summary>
        NeedsSpooler = 0x02
    }
}

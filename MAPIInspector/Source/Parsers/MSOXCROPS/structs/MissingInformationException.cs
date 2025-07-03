using System;

namespace MAPIInspector.Parsers
{
    #region Helper method for Decoding

    /// <summary>
    /// The MissingInformationException is used to define the exception, which are caused by missing context information.
    /// </summary>
    public class MissingInformationException : Exception
    {
        /// <summary>
        /// The exception message thrown
        /// </summary>
        public string ErrorMessage;

        /// <summary>
        /// The ROP ID needs context information
        /// </summary>
        public ushort RopID;

        /// <summary>
        /// The source ROP parameters to pass
        /// </summary>
        public uint[] Parameters;

        /// <summary>
        /// Initializes a new instance of the MissingInformationException class
        /// </summary>
        /// <param name="message">Exception error messge</param>
        /// <param name="ropID">ROP id</param>
        /// <param name="parameter">parameters for this missing information exception</param>
        public MissingInformationException(string message, ushort ropID, uint[] parameter = null)
        {
            ErrorMessage = message;
            RopID = ropID;
            Parameters = parameter;
        }
    }
    #endregion
}

using BlockParser;
using System;

namespace MAPIInspector.Parsers
{
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
        public RopIdType RopID;

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
        private MissingInformationException(string message, RopIdType ropID, uint[] parameter = null)
        {
            ErrorMessage = message;
            RopID = ropID;
            Parameters = parameter;
        }

        /// <summary>
        /// Attempts to throw a MissingInformationException or returns a BlockException if we've 
        /// already thrown and are in a safe handle context.
        /// </summary>
        /// <param name="message">Exception error message.</param>
        /// <param name="ropID">ROP id that needs context information.</param>
        /// <param name="parameter">Parameters for this missing information exception.</param>
        /// <returns>
        /// A <see cref="Block"/> representing the exception if in a safe handle context; otherwise, throws a <see cref="MissingInformationException"/>.
        /// </returns>
        public static Block MaybeThrow(string message, RopIdType ropID, uint[] parameter = null)
        {
            if (MapiInspector.MAPIParser.inSafeHandleContextInformation)
            {
                var ex = new MissingInformationException(message, ropID, parameter);
                return BlockException.Create("Failed locating missing information", ex, 0);
            }

            throw new MissingInformationException(message, ropID, parameter);
        }
    }
}

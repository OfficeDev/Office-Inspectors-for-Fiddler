using System;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The MissingPartialInformationException is used to define the exception, which are caused by missing context information for partial.
    /// </summary>
    public class MissingPartialInformationException : Exception
    {
        /// <summary>
        /// The ROP ID needs context information
        /// </summary>
        public RopIdType RopID;

        /// <summary>
        /// The source ROP parameters to pass
        /// </summary>
        public uint Parameter;

        /// <summary>
        /// Initializes a new instance of the MissingPartialInformationException class
        /// </summary>
        /// <param name="ropID">ROP id</param>
        /// <param name="parameter">parameters for this missing partial information exception</param>
        public MissingPartialInformationException(RopIdType ropID, uint parameter)
        {
            RopID = ropID;
            Parameter = parameter;
        }
    }
}

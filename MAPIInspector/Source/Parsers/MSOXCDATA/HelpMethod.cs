namespace MAPIInspector.Parsers
{
    using System;
    using System.IO;

    /// <summary>
    /// The help method to read the Count of Ptyp data type
    /// </summary>
    public class HelpMethod : BaseStructure
    {
        /// <summary>
        /// The method to read the Count of Ptyp type.
        /// </summary>
        /// <param name="countWide">The count wide.</param>
        /// <param name="s">The stream contain the COUNT</param>
        /// <returns>The COUNT value.</returns>
        public object ReadCount(CountWideEnum countWide, Stream s)
        {
            Parse(s);

            switch (countWide)
            {
                case CountWideEnum.twoBytes:
                    {
                        return ReadUshort();
                    }

                case CountWideEnum.fourBytes:
                    {
                        return ReadUint();
                    }

                default:
                    return ReadUshort();
            }
        }

        /// <summary>
        /// Format the error codes.
        /// </summary>
        /// <param name="errorCodeUint">The UInt error code</param>
        /// <returns>The enum error code name.</returns>
        public static object FormatErrorCode(uint errorCodeUint)
        {
            object errorCode = null;
            if (Enum.IsDefined(typeof(ErrorCodes), errorCodeUint))
            {
                errorCode = (ErrorCodes)errorCodeUint;
            }
            else if (Enum.IsDefined(typeof(AdditionalErrorCodes), errorCodeUint))
            {
                errorCode = (AdditionalErrorCodes)errorCodeUint;
            }
            else if (Enum.IsDefined(typeof(WarningCodes), errorCodeUint))
            {
                errorCode = (WarningCodes)errorCodeUint;
            }
            else
            {
                errorCode = errorCodeUint;
            }

            return errorCode;
        }

        /// <summary>
        /// Override parse method.
        /// </summary>
        /// <param name="s">Stream used to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
        }
    }
}
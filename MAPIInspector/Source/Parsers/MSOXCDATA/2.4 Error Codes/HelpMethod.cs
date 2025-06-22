namespace MAPIInspector.Parsers
{
    using System;

    public static class HelpMethod
    {
        // TODO: Make this a string later
        public static object FormatErrorCode(this ErrorCodes errorCodeUint)
        {
            object errorCode = null;
            if (Enum.IsDefined(typeof(ErrorCodes), errorCodeUint))
            {
                errorCode = errorCodeUint;
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
    }
}
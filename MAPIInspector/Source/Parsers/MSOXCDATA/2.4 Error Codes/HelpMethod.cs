using System;

namespace MAPIInspector.Parsers
{
    public static class HelpMethod
    {
        // TODO: Make this a string later
        public static object FormatErrorCode(this ErrorCodes errorCodeUint)
        {
            object errorCode = null;
            if (Enum.IsDefined(typeof(ErrorCodes), (uint)errorCodeUint))
            {
                errorCode = errorCodeUint;
            }
            else if (Enum.IsDefined(typeof(AdditionalErrorCodes), (uint)errorCodeUint))
            {
                errorCode = (AdditionalErrorCodes)errorCodeUint;
            }
            else if (Enum.IsDefined(typeof(WarningCodes), (uint)errorCodeUint))
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
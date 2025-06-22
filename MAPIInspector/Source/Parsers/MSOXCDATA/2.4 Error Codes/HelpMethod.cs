namespace MAPIInspector.Parsers
{
    using System;

    public static class HelpMethod
    {
        public static string FormatErrorCode(this ErrorCodes errorCodeUint)
        {
            string errorCode = null;
            if (Enum.IsDefined(typeof(ErrorCodes), errorCodeUint))
            {
                errorCode = $"{errorCodeUint}";
            }
            else if (Enum.IsDefined(typeof(AdditionalErrorCodes), errorCodeUint))
            {
                errorCode = $"{(AdditionalErrorCodes)errorCodeUint}";
            }
            else if (Enum.IsDefined(typeof(WarningCodes), errorCodeUint))
            {
                errorCode = $"{(WarningCodes)errorCodeUint}";
            }
            else
            {
                errorCode = $"{errorCodeUint:X}";
            }

            return errorCode;
        }
    }
}
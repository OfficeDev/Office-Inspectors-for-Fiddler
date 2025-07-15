using System;

namespace MAPIInspector.Parsers
{
    public static class HelpMethod
    {
        public static string FormatErrorCode(this ErrorCodes errorCodeUint)
        {
            string errorCodeString = string.Empty;
            if (Enum.IsDefined(typeof(ErrorCodes), (uint)errorCodeUint))
            {
                errorCodeString = errorCodeUint.ToString();
            }
            else if (Enum.IsDefined(typeof(AdditionalErrorCodes), (uint)errorCodeUint))
            {
                errorCodeString = ((AdditionalErrorCodes)errorCodeUint).ToString();
            }
            else if (Enum.IsDefined(typeof(WarningCodes), (uint)errorCodeUint))
            {
                errorCodeString = ((WarningCodes)errorCodeUint).ToString();
            }
            else
            {
                errorCodeString = errorCodeUint.ToString();
            }

            return $"{errorCodeString} = 0x{errorCodeUint:X}";
        }
    }
}
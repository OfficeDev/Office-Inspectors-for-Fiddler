using BlockParser;
using System;

namespace MAPIInspector.Parsers
{
    public static class HelpMethod
    {
        public static void AddError(this Block parent, BlockT<ErrorCodes> error, string label)
        {
            if (error != null) parent.AddChild(error, $"{label}: {error.Data.FormatErrorCode()}");
        }

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

            return string.IsNullOrEmpty(errorCodeString) ? "0x{errorCodeUint:X}" : $"{errorCodeString} = 0x{errorCodeUint:X}";
        }
    }
}

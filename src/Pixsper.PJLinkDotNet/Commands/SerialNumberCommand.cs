using System;
using System.Linq;

namespace Pixsper.PJLinkDotNet.Commands
{
    public class CommandGetSerialNumber : CommandGetBase
    {
        public CommandGetSerialNumber()
            : base(PJLinkClassValue.Class2, CommandBodyValue.SerialNumber)
        {

        }
    }
    

    public class CommandGetSerialNumberResponse : CommandGetResponseBase
    {
        public const int SerialNumberMaxLength = 32;
        public const char SerialNumberCharacterRangeMin = (char)0x20;
        public const char SerialNumberCharacterRangeMax = (char)0x7E;

        public static CommandGetSerialNumberResponse? FromParameterString(string str)
        {
            // TODO
            return null;
        }

        public CommandGetSerialNumberResponse(string serialNumber)
            : base(PJLinkClassValue.Class2, CommandBodyValue.SerialNumber)
        {
            if (serialNumber.Length > SerialNumberMaxLength)
                throw new ArgumentOutOfRangeException(nameof(serialNumber), $"Over maximum length of {SerialNumberMaxLength}");

            if (serialNumber.Any(c => c < SerialNumberCharacterRangeMin || c > SerialNumberCharacterRangeMax))
                throw new ArgumentOutOfRangeException(nameof(serialNumber), $"Contains forbidden characters (ASCII only, no control characters)");

            SerialNumber = serialNumber;
        }

        public CommandGetSerialNumberResponse(CommandResponseErrorCode errorCode)
            : base(PJLinkClassValue.Class2, CommandBodyValue.SerialNumber, errorCode)
        {

        }

        public override string? ResultParameter => SerialNumber;

        public string? SerialNumber { get; }
    }
}

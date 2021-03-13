using System;
using System.Linq;

namespace Pixsper.PJLinkDotNet.Commands
{
    public class CommandGetOtherInformation : CommandGetBase
    {
        public CommandGetOtherInformation()
            : base(PJLinkClassValue.Class1, CommandBodyValue.OtherInformation)
        {

        }
    }
    

    public class CommandGetOtherInformationResponse : CommandGetResponseBase
    {
        public const int InformationMaxLength = 32;
        public const char InformationCharacterRangeMin = (char)0x20;
        public const char InformationCharacterRangeMax = (char)0x7E;

        public static CommandGetOtherInformationResponse? FromParameterString(string str)
        {
            // TODO
            return null;
        }

        public CommandGetOtherInformationResponse(string information)
            : base(PJLinkClassValue.Class1, CommandBodyValue.OtherInformation)
        {
            if (information.Length > InformationMaxLength)
                throw new ArgumentOutOfRangeException(nameof(information), $"Over maximum length of {InformationMaxLength}");

            if (information.Any(c => c < InformationCharacterRangeMin || c > InformationCharacterRangeMax))
                throw new ArgumentOutOfRangeException(nameof(information), $"Contains forbidden characters (ASCII only, no control characters)");

            Information = information;
        }

        public CommandGetOtherInformationResponse(CommandResponseErrorCode errorCode)
            : base(PJLinkClassValue.Class1, CommandBodyValue.OtherInformation, errorCode)
        {

        }

        public override string? ResultParameter => Information;

        public string? Information { get; }
    }
}

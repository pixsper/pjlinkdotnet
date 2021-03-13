using System;
using System.Linq;

namespace Pixsper.PJLinkDotNet.Commands
{
    public class CommandGetSoftwareVersion : CommandGetBase
    {
        public CommandGetSoftwareVersion()
            : base(PJLinkClassValue.Class2, CommandBodyValue.SoftwareVersion)
        {

        }
    }
    

    public class CommandGetSoftwareVersionResponse : CommandGetResponseBase
    {
        public const int SoftwareVersionMaxLength = 32;
        public const char SoftwareVersionCharacterRangeMin = (char)0x20;
        public const char SoftwareVersionCharacterRangeMax = (char)0x7E;

        public static CommandGetSoftwareVersionResponse? FromParameterString(string str)
        {
            // TODO
            return null;
        }

        public CommandGetSoftwareVersionResponse(string softwareVersion)
            : base(PJLinkClassValue.Class2, CommandBodyValue.SoftwareVersion)
        { 
            if (softwareVersion.Length > SoftwareVersionMaxLength)
                throw new ArgumentOutOfRangeException(nameof(softwareVersion), $"Over maximum length of {SoftwareVersionMaxLength}");

            if (softwareVersion.Any(c => c < SoftwareVersionCharacterRangeMin || c > SoftwareVersionCharacterRangeMax))
                throw new ArgumentOutOfRangeException(nameof(softwareVersion), $"Contains forbidden characters (ASCII only, no control characters)");

            SoftwareVersion = softwareVersion;
        }

        public CommandGetSoftwareVersionResponse(CommandResponseErrorCode errorCode)
            : base(PJLinkClassValue.Class2, CommandBodyValue.SoftwareVersion, errorCode)
        {

        }

        public override string? ResultParameter => SoftwareVersion;

        public string? SoftwareVersion { get; }
    }
}

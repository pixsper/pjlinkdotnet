using System;
using System.Linq;

namespace Pixsper.PJLinkDotNet.Commands
{
    public class CommandGetManufacturerName : CommandGetBase
    {
        public CommandGetManufacturerName()
            : base(PJLinkClassValue.Class1, CommandBodyValue.ManufacturerName)
        {

        }
    }
    

    public class CommandGetManufacturerNameResponse : CommandGetResponseBase
    {
        public const int NameMaxLength = 32;
        public const char NameCharacterRangeMin = (char)0x20;
        public const char NameCharacterRangeMax = (char)0x7E;

        public static CommandGetManufacturerNameResponse? FromParameterString(string str)
        {
            // TODO
            return null;
        }

        public CommandGetManufacturerNameResponse(string name)
            : base(PJLinkClassValue.Class1, CommandBodyValue.ManufacturerName)
        {
            if (name.Length > NameMaxLength)
                throw new ArgumentOutOfRangeException(nameof(name), $"Over maximum length of {NameMaxLength}");

            if (name.Any(c => c < NameCharacterRangeMin || c > NameCharacterRangeMax))
                throw new ArgumentOutOfRangeException(nameof(name), $"Contains forbidden characters (ASCII only, no control characters)");

            Name = name;
        }

        public CommandGetManufacturerNameResponse(CommandResponseErrorCode errorCode)
            : base(PJLinkClassValue.Class1, CommandBodyValue.ManufacturerName, errorCode)
        {

        }

        public override string? ResultParameter => Name;

        public string? Name { get; }
    }
}

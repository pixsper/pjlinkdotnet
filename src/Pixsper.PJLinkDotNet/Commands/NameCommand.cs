using System;
using System.Linq;

namespace Pixsper.PJLinkDotNet.Commands
{
    public class CommandGetProjectorName : CommandGetBase
    {
        public CommandGetProjectorName()
            : base(PJLinkClassValue.Class1, CommandBodyValue.Name)
        {

        }
    }
    

    public class CommandGetProjectorNameResponse : CommandGetResponseBase
    {
        public const int NameMaxLength = 64;
        public const char NameCharacterRangeMin = (char)0x20;
        public const char NameCharacterRangeMax = (char)0xFF;

        public static CommandGetProjectorNameResponse? FromParameterString(string str)
        {
            // TODO
            return null;
        }

        public CommandGetProjectorNameResponse(string name)
            : base(PJLinkClassValue.Class1, CommandBodyValue.Name)
        {
            if (name.Length > NameMaxLength)
                throw new ArgumentOutOfRangeException(nameof(name), $"Over maximum length of {NameMaxLength}");

            if (name.Any(c => c < NameCharacterRangeMin || c > NameCharacterRangeMax))
                throw new ArgumentOutOfRangeException(nameof(name), $"Contains forbidden characters (UTF-8 only, no control characters)");

            Name = name;
        }

        public CommandGetProjectorNameResponse(CommandResponseErrorCode errorCode)
            : base(PJLinkClassValue.Class1, CommandBodyValue.Name, errorCode)
        {

        }

        public override string? ResultParameter => Name;

        public string? Name { get; }
    }
}

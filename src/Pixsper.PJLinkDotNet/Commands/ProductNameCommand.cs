using System;
using System.Linq;

namespace Pixsper.PJLinkDotNet.Commands
{
    public class CommandGetProductName : CommandGetBase
    {
        public CommandGetProductName()
            : base(PJLinkClassValue.Class1, CommandBodyValue.ProductName)
        {

        }
    }
    

    public class CommandGetProductNameResponse : CommandGetResponseBase
    {
        public const int NameMaxLength = 32;
        public const char NameCharacterRangeMin = (char)0x20;
        public const char NameCharacterRangeMax = (char)0x7E;

        public static CommandGetProductNameResponse? FromParameterString(string str)
        {
            // TODO
            return null;
        }

        public CommandGetProductNameResponse(string name)
            : base(PJLinkClassValue.Class1, CommandBodyValue.ProductName)
        {
            if (name.Length > NameMaxLength)
                throw new ArgumentOutOfRangeException(nameof(name), $"Over maximum length of {NameMaxLength}");

            if (name.Any(c => c < NameCharacterRangeMin || c > NameCharacterRangeMax))
                throw new ArgumentOutOfRangeException(nameof(name), $"Contains forbidden characters (ASCII only, no control characters)");

            Name = name;
        }

        public CommandGetProductNameResponse(CommandResponseErrorCode errorCode)
            : base(PJLinkClassValue.Class1, CommandBodyValue.ProductName, errorCode)
        {

        }

        public override string? ResultParameter => Name;

        public string? Name { get; }
    }
}

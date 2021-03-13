using System;

namespace Pixsper.PJLinkDotNet.Commands
{
    public class CommandGetInput : CommandGetBase
    {
        public CommandGetInput(PJLinkClassValue pjLinkClass = PJLinkClassValue.Class1)
            : base(pjLinkClass, CommandBodyValue.Input)
        {

        }
    }
    

    public class CommandGetInputResponse : CommandGetResponseBase
    {
        public static CommandGetInputResponse? FromParameterString(PJLinkClassValue pjLinkClass, string str)
        {
            if (!InputId.TryParse(str, out var value))
                return null;

            if (value.PJLinkClass == PJLinkClassValue.Class2 && pjLinkClass == PJLinkClassValue.Class1)
                return null;

            return new CommandGetInputResponse(value, pjLinkClass);
        }

        public CommandGetInputResponse(InputId input, PJLinkClassValue pjLinkClass = PJLinkClassValue.Class1)
            : base(pjLinkClass, CommandBodyValue.Input)
        {
            Input = input;
        }

        public CommandGetInputResponse(CommandResponseErrorCode errorCode, PJLinkClassValue pjLinkClass = PJLinkClassValue.Class1)
            : base(pjLinkClass, CommandBodyValue.Input, errorCode)
        {

        }

        public override string? ResultParameter => Input;

        public InputId? Input { get; }
    }
    

    public class CommandSetInput : CommandSetBase
    {
        public static CommandSetInput? FromParameterString(PJLinkClassValue pjLinkClass, string str)
        {
            if (!InputId.TryParse(str, out var value))
                return null;

            if (value.PJLinkClass == PJLinkClassValue.Class2 && pjLinkClass == PJLinkClassValue.Class1)
                return null;

            return new CommandSetInput(value, pjLinkClass);
        }

        public CommandSetInput(InputId input, PJLinkClassValue pjLinkClass = PJLinkClassValue.Class1)
            : base(pjLinkClass, CommandBodyValue.Input)
        {
            Input = input;

            if (input.PJLinkClass == PJLinkClassValue.Class2 && PJLinkClass == PJLinkClassValue.Class1)
                throw new ArgumentException("Input id value not supported in PJLink Class 1 message");
        }

        public override string RawParameter => Input;

        public InputId Input { get; }
    }
    

    public class CommandSetInputResponse : CommandSetResponseBase
    {
        public CommandSetInputResponse(CommandResponseErrorCode errorCode, PJLinkClassValue pjLinkClass = PJLinkClassValue.Class1)
            : base(pjLinkClass, CommandBodyValue.Input, errorCode)
        {

        }
    }
}
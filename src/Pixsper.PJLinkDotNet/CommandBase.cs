using System;

namespace Pixsper.PJLinkDotNet
{
    public abstract partial class CommandBase
    {
        protected static string ParameterFromValue(bool value) => value ? "1" : "0";
        protected static string ParameterFromValue(int value) => value.ToString("D");
        protected static string ParameterFromValue(Enum value) => value.ToString("D");


        protected CommandBase(CommandKind kind, PJLinkClassValue pjLinkClass, CommandBodyValue body)
        {
            Kind = kind;
            PJLinkClass = pjLinkClass;
            Body = body;
        }

        public CommandKind Kind { get; }

        public PJLinkClassValue PJLinkClass { get; }

        public CommandBodyValue Body { get; }

        public abstract string RawParameter { get; }
    }
}

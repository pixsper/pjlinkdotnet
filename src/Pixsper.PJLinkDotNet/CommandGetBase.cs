namespace Pixsper.PJLinkDotNet
{
    public abstract class CommandGetBase : CommandBase
    {
        protected CommandGetBase(PJLinkClassValue pjLinkClass, CommandBodyValue body)
            : base(CommandKind.Get, pjLinkClass, body)
        {

        }

        public sealed override string RawParameter { get; } = QueryParameterValue;
    }


    public abstract class CommandGetResponseBase : CommandResponseBase
    {
        protected CommandGetResponseBase(PJLinkClassValue pjLinkClass, CommandBodyValue body, CommandResponseErrorCode errorCode = CommandResponseErrorCode.Ok)
            : base(CommandKind.GetResponse, pjLinkClass, body, errorCode)
        {

        }

        public sealed override string RawParameter => ResultParameter ?? ErrorCode.ToString();

        public abstract string? ResultParameter { get; }
    }
}

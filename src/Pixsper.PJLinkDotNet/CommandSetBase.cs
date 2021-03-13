namespace Pixsper.PJLinkDotNet
{
    public abstract class CommandSetBase : CommandBase
    {
        protected CommandSetBase(PJLinkClassValue pjLinkClass, CommandBodyValue body)
            : base(CommandKind.Set, pjLinkClass, body)
        {

        }
    }


    public abstract class CommandSetResponseBase : CommandResponseBase
    {
        protected CommandSetResponseBase(PJLinkClassValue pjLinkClass, CommandBodyValue body, CommandResponseErrorCode errorCode)
            : base(CommandKind.SetResponse, pjLinkClass, body, errorCode)
        {

        }

        public sealed override string RawParameter => ErrorCode.ToString();
    }
}
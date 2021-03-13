using System;

namespace Pixsper.PJLinkDotNet
{
    public abstract class CommandResponseBase : CommandBase
    {
        protected CommandResponseBase(CommandKind kind, PJLinkClassValue pjLinkClass, CommandBodyValue body, CommandResponseErrorCode errorCode)
            : base(kind, pjLinkClass, body)
        {
            ErrorCode = errorCode;
        }

        public CommandResponseErrorCode ErrorCode { get; }

        public virtual string ResponseOkMessage { get; } = "OK";
        public virtual string ResponseErr1Message { get; } = "Undefined/unsupported command";
        public virtual string ResponseErr2Message { get; } = "Invalid parameter";
        public virtual string ResponseErr3Message { get; } = "Operation unavailable at this time";
        public virtual string ResponseErr4Message { get; } = "Projector/display failure";


        public string ExecutionResponseMessage => ErrorCode switch
        {
            CommandResponseErrorCode.Ok => ResponseOkMessage,
            CommandResponseErrorCode.Err1 => ResponseErr1Message,
            CommandResponseErrorCode.Err2 => ResponseErr2Message,
            CommandResponseErrorCode.Err3 => ResponseErr3Message,
            CommandResponseErrorCode.Err4 => ResponseErr4Message,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
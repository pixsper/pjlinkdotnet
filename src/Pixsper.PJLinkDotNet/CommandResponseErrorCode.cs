using System;

namespace Pixsper.PJLinkDotNet
{
    public enum CommandResponseErrorCode
    {
        Ok,
        Err1,
        Err2,
        Err3,
        Err4
    }

    internal static class CommandResponseTypeExtensions
    {
        public const string ErrorCodeOk = "OK";
        public const string ErrorCode1 = "ERR1";
        public const string ErrorCode2 = "ERR2";
        public const string ErrorCode3 = "ERR3";
        public const string ErrorCode4 = "ERR4";

        public static string ToErrorCodeString(this CommandResponseErrorCode errorCode)
        {
            switch (errorCode)
            {
                case CommandResponseErrorCode.Ok:
                    return ErrorCodeOk;
                case CommandResponseErrorCode.Err1:
                    return ErrorCode1;
                case CommandResponseErrorCode.Err2:
                    return ErrorCode2;
                case CommandResponseErrorCode.Err3:
                    return ErrorCode3;
                case CommandResponseErrorCode.Err4:
                    return ErrorCode4;
                default:
                    throw new ArgumentOutOfRangeException(nameof(errorCode), errorCode, null);
            }
        }

        public static CommandResponseErrorCode? ToCommandResponseErrorCode(this string str)
        {
            switch (str)
            {
                case ErrorCodeOk:
                    return CommandResponseErrorCode.Ok;
                case ErrorCode1:
                    return CommandResponseErrorCode.Err1;
                case ErrorCode2:
                    return CommandResponseErrorCode.Err2;
                case ErrorCode3:
                    return CommandResponseErrorCode.Err3;
                case ErrorCode4:
                    return CommandResponseErrorCode.Err4;
                default:
                    return null;
            }
        }
    }
}
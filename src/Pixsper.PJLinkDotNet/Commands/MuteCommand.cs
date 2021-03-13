namespace Pixsper.PJLinkDotNet.Commands
{
    public class CommandGetMute : CommandGetBase
    {
        public CommandGetMute()
            : base(PJLinkClassValue.Class1, CommandBodyValue.Mute)
        {

        }
    }
    

    public class CommandGetMuteResponse : CommandGetResponseBase
    {
        public static CommandGetMuteResponse? FromParameterString(string str)
        {
            if (!int.TryParse(str, out var statusInt))
                return null;

            var status = (MuteStatus)statusInt;
            if (status != MuteStatus.VideoMuteOn
                && status != MuteStatus.AudioMuteOn
                && status != MuteStatus.VideoAndAudioMuteOn
                && status != MuteStatus.VideoAndAudioMuteOff)
            {
                return null;
            }

            return new CommandGetMuteResponse(status);
        }

        public CommandGetMuteResponse(MuteStatus status)
            : base(PJLinkClassValue.Class1, CommandBodyValue.Mute)
        {
            Status = status;
        }

        public CommandGetMuteResponse(CommandResponseErrorCode errorCode)
            : base(PJLinkClassValue.Class1, CommandBodyValue.Mute, errorCode)
        {

        }

        public override string? ResultParameter => Status.HasValue ? ParameterFromValue((int)Status) : null;

        public MuteStatus? Status { get; }


        public enum MuteStatus
        {
            VideoMuteOn = 11,
            AudioMuteOn = 21,
            VideoAndAudioMuteOn = 31,
            VideoAndAudioMuteOff = 30
        }
    }
    

    public class CommandSetMute : CommandSetBase
    {
        public static CommandSetMute? FromParameterString(string str)
        {
            if (str.Length != 2)
                return null;

            if (!int.TryParse(str, out var operationInt))
                return null;

            var operation = (MuteCommandOperation)operationInt;
            if (operation != MuteCommandOperation.VideoMuteOn
                && operation != MuteCommandOperation.VideoMuteOff
                && operation != MuteCommandOperation.AudioMuteOn
                && operation != MuteCommandOperation.AudioMuteOff
                && operation != MuteCommandOperation.VideoAndAudioMuteOn
                && operation != MuteCommandOperation.VideoAndAudioMuteOff)
            {
                return null;
            }

            return new CommandSetMute(operation);
        }

        public CommandSetMute(MuteCommandOperation operation)
            : base(PJLinkClassValue.Class1, CommandBodyValue.Mute)
        {
            Operation = operation;
        }

        public override string RawParameter => ParameterFromValue(Operation);

        public MuteCommandOperation Operation { get; }

        public enum MuteCommandOperation
        {
            VideoMuteOn = 11,
            VideoMuteOff = 10,
            AudioMuteOn = 21,
            AudioMuteOff = 20,
            VideoAndAudioMuteOn = 31,
            VideoAndAudioMuteOff = 30
        }
    }
    

    public class CommandSetMuteResponse : CommandSetResponseBase
    {
        public CommandSetMuteResponse(CommandResponseErrorCode errorCode)
            : base(PJLinkClassValue.Class1, CommandBodyValue.Mute, errorCode)
        {

        }
    }
}

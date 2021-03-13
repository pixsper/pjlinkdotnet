namespace Pixsper.PJLinkDotNet.Commands
{
    public class CommandGetErrorStatus : CommandGetBase
    {
        public CommandGetErrorStatus()
            : base(PJLinkClassValue.Class1, CommandBodyValue.ErrorStatus)
        {

        }
    }
    

    public class CommandGetErrorStatusResponse : CommandGetResponseBase
    {
        public static CommandGetErrorStatusResponse? FromParameterString(string str)
        {
            var statuses = new ErrorStatus[6];

            for (int i = 0; i < statuses.Length; ++i)
            {
                if (!int.TryParse(str[i].ToString(), out var statusInt))
                    return null;

                var status = (ErrorStatus)statusInt;
                if (status != ErrorStatus.Ok && status != ErrorStatus.Warning && status != ErrorStatus.Error)
                    return null;

                statuses[i] = status;
            }

            return new CommandGetErrorStatusResponse(statuses[0], statuses[1],
                statuses[2], statuses[3],
                statuses[4], statuses[5]);
        }

        public CommandGetErrorStatusResponse(ErrorStatus fanErrorStatus = ErrorStatus.Ok, ErrorStatus lampErrorStatus = ErrorStatus.Ok,
            ErrorStatus temperatureErrorStatus = ErrorStatus.Ok, ErrorStatus coverOpenErrorStatus = ErrorStatus.Ok,
            ErrorStatus filterErrorStatus = ErrorStatus.Ok, ErrorStatus otherErrorStatus = ErrorStatus.Ok)
            : base(PJLinkClassValue.Class1, CommandBodyValue.ErrorStatus)
        {
            FanErrorStatus = fanErrorStatus;
            LampErrorStatus = lampErrorStatus;
            TemperatureErrorStatus = temperatureErrorStatus;
            CoverOpenErrorStatus = coverOpenErrorStatus;
            FilterErrorStatus = filterErrorStatus;
            OtherErrorStatus = otherErrorStatus;
        }

        public CommandGetErrorStatusResponse(CommandResponseErrorCode errorCode)
            : base(PJLinkClassValue.Class1, CommandBodyValue.ErrorStatus, errorCode)
        {

        }

        public override string? ResultParameter
        {
            get
            {
                if (!FanErrorStatus.HasValue
                    || !LampErrorStatus.HasValue
                    || !TemperatureErrorStatus.HasValue
                    || !CoverOpenErrorStatus.HasValue
                    || !FilterErrorStatus.HasValue
                    || !OtherErrorStatus.HasValue)
                {
                    return null;
                }

                return
                    $"{FanErrorStatus:D}{LampErrorStatus:D}{TemperatureErrorStatus:D}{CoverOpenErrorStatus:D}{FilterErrorStatus:D}{OtherErrorStatus:D}";
            }
        }

        public ErrorStatus? FanErrorStatus { get; }
        public ErrorStatus? LampErrorStatus { get; }
        public ErrorStatus? TemperatureErrorStatus { get; }
        public ErrorStatus? CoverOpenErrorStatus { get; }
        public ErrorStatus? FilterErrorStatus { get; }
        public ErrorStatus? OtherErrorStatus { get; }


        public enum ErrorStatus
        {
            Ok = 0,
            Warning = 1,
            Error = 2
        }
    }
}

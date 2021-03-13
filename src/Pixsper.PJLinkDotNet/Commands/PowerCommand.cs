namespace Pixsper.PJLinkDotNet.Commands
{
    public class CommandGetPower : CommandGetBase
    {
        public CommandGetPower()
            : base(PJLinkClassValue.Class1, CommandBodyValue.Power)
        {

        }
    }
    

    public class CommandGetPowerResponse : CommandGetResponseBase
    {
        public static CommandGetPowerResponse? FromParameterString(string str)
        {
            if (!int.TryParse(str[0].ToString(), out var powerStatusInt))
                return null;

            var powerStatus = (PowerStatus)powerStatusInt;
            if (powerStatus != PowerStatus.PowerOff
                && powerStatus != PowerStatus.PowerOn
                && powerStatus != PowerStatus.Cooling
                && powerStatus != PowerStatus.WarmUp)
            {
                return null;
            }

            return new CommandGetPowerResponse(powerStatus);
        }

        public CommandGetPowerResponse(PowerStatus status)
            : base(PJLinkClassValue.Class1, CommandBodyValue.Power)
        {
            Status = status;
        }

        public CommandGetPowerResponse(CommandResponseErrorCode errorCode)
            : base(PJLinkClassValue.Class1, CommandBodyValue.Power, errorCode)
        {

        }

        public override string? ResultParameter => Status.HasValue ? ParameterFromValue((int)Status) : null;

        public PowerStatus? Status { get; }

        public enum PowerStatus
        {
            PowerOff = 0,
            PowerOn = 1,
            Cooling = 2,
            WarmUp = 3
        }
    }
    

    public class CommandSetPower : CommandSetBase
    {
        public static CommandSetPower? FromParameterString(string str)
        {
            if (str.Length != 1)
                return null;

            switch (str[0])
            {
                case '0':
                    return new CommandSetPower(false);
                case '1':
                    return new CommandSetPower(true);
                default:
                    return null;
            }
        }

        public CommandSetPower(bool isPoweredOn)
            : base(PJLinkClassValue.Class1, CommandBodyValue.Power)
        {
            IsPoweredOn = isPoweredOn;
        }

        public override string RawParameter => ParameterFromValue(IsPoweredOn);

        public bool IsPoweredOn { get; }
    }
    

    public class CommandSetPowerResponse : CommandSetResponseBase
    {
        public CommandSetPowerResponse(CommandResponseErrorCode errorCode)
            : base(PJLinkClassValue.Class1, CommandBodyValue.Power, errorCode)
        {

        }
    }
}

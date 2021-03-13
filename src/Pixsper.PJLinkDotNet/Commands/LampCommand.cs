using System;
using System.Collections.Generic;
using System.Linq;

namespace Pixsper.PJLinkDotNet.Commands
{
    public class CommandGetLampCountUsageTime : CommandGetBase
    {
        public CommandGetLampCountUsageTime()
            : base(PJLinkClassValue.Class1, CommandBodyValue.Lamp)
        {

        }
    }
    

    public class CommandGetLampCountUsageTimeResponse : CommandGetResponseBase
    {
        public static CommandGetLampCountUsageTimeResponse? FromParameterString(string str)
        {
            if (str.Length < 3)
                return null;
            
            // TODO

            return null;
        }


        public CommandGetLampCountUsageTimeResponse(IEnumerable<LampInfo> lamps)
            : base(PJLinkClassValue.Class1, CommandBodyValue.Lamp)
        {
            Lamps = lamps.ToList();
        }

        public CommandGetLampCountUsageTimeResponse(params LampInfo[] lamps)
            : this((IEnumerable<LampInfo>)lamps)
        {

        }

        public CommandGetLampCountUsageTimeResponse(CommandResponseErrorCode errorCode)
            : base(PJLinkClassValue.Class1, CommandBodyValue.Lamp, errorCode)
        {

        }

        public override string? ResultParameter => Lamps != null ? string.Join(" ", Lamps) : null;

        public IReadOnlyList<LampInfo>? Lamps { get; }


        public readonly struct LampInfo : IEquatable<LampInfo>
        {
            public LampInfo(int usageHours, bool isOn)
            {
                UsageHours = usageHours;
                IsOn = isOn;
            }
            
            public int UsageHours { get; }
            public bool IsOn { get; }

            public override string ToString() => $"{UsageHours} {ParameterFromValue(IsOn)}";

            public bool Equals(LampInfo other) => UsageHours == other.UsageHours && IsOn == other.IsOn;

            public override bool Equals(object? obj) => obj is LampInfo other && Equals(other);

            public override int GetHashCode()
            {
                unchecked
                {
                    return (UsageHours * 397) ^ IsOn.GetHashCode();
                }
            }

            public static bool operator ==(LampInfo left, LampInfo right) => left.Equals(right);

            public static bool operator !=(LampInfo left, LampInfo right) => !left.Equals(right);
        }
    }
}

using System;
using System.Text;
using Pixsper.PJLinkDotNet.Commands;

namespace Pixsper.PJLinkDotNet
{
    public abstract partial class CommandBase
    {
        public const int CommandMinLength = 9;
        public const int CommandMaxLength = ParameterMaxLength + 8;
        public const int ParameterMaxLength = 128;

        public const char IdentifierChar = '%';
        public const char CommandSeparatorChar = ' ';
        public const char ResponseSeparatorChar = '=';
        public const char TerminatorChar = '\r';

        public const string QueryParameterValue = "?";

        public override string ToString()
        {
            var parameter = RawParameter;

            if (string.IsNullOrEmpty(parameter) || parameter?.Length > 128)
                parameter = string.Empty;

            string separator = parameter?.Length > 0 
                ? (Kind == CommandKind.GetResponse || Kind == CommandKind.SetResponse ? ResponseSeparatorChar : CommandSeparatorChar).ToString() 
                : string.Empty;

            return $"{IdentifierChar}{PJLinkClass:D}{Body}{separator}{parameter}{TerminatorChar}";
        }

        public byte[] ToByteArray()
        {
            return Encoding.UTF8.GetBytes(ToString());
        }

        public static CommandBase? FromByteArray(byte[] data, CommandKind? isInResponseTo = null)
        {
            if (data.Length < CommandMinLength || data.Length > CommandMaxLength)
                return null;

            return TryParse(Encoding.UTF8.GetString(data), isInResponseTo);
        }

        public static CommandBase? TryParse(string str, CommandKind? isInResponseTo = null)
        {
            if (str.Length < CommandMinLength || str.Length > CommandMaxLength)
                return null;

            if (str[0] != IdentifierChar || str[str.Length - 1] != TerminatorChar)
                return null;

            if (!int.TryParse(str[1].ToString(), out var pjLinkClassInt))
                return null;

            var pjLinkClass = (PJLinkClassValue)pjLinkClassInt;
            if (pjLinkClass != PJLinkClassValue.Class1 && pjLinkClass != PJLinkClassValue.Class2)
                return null;

            var body = str.Substring(2, 4).ToCommandBodyValue();
            if (!body.HasValue)
                return null;

            CommandKind kind;

            string parameter = str.Substring(7, str.Length - 8);

            switch (str[6])
            {
                case CommandSeparatorChar:
                    kind = parameter == QueryParameterValue ? CommandKind.Get : CommandKind.Set;
                    break;

                case ResponseSeparatorChar:
                    kind = isInResponseTo switch
                    {
                        CommandKind.Set => CommandKind.SetResponse,
                        CommandKind.Get => CommandKind.GetResponse,
                        _ => throw new ArgumentOutOfRangeException(
                            $"Cannot determine response message type without context from '{nameof(isInResponseTo)}' parameter")
                    };

                    break;

                default:
                    return null;
            }

            var metadata = CommandBodyExtensions.CommandBodies[body.Value];
            if (!metadata.SupportsClass(pjLinkClass) || !metadata.SupportsKind(kind)) 
                return null;

            switch (kind)
            {
                case CommandKind.Get:
                    return deserializeGetCommand(pjLinkClass, body.Value);
                case CommandKind.GetResponse:
                    return deserializeGetCommandResponse(pjLinkClass, body.Value, parameter);
                case CommandKind.Set:
                    return deserializeSetCommand(pjLinkClass, body.Value, parameter);
                case CommandKind.SetResponse:
                    return deserializeSetCommandResponse(pjLinkClass, body.Value, parameter);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static CommandGetBase? deserializeGetCommand(PJLinkClassValue pjLinkClass, CommandBodyValue body)
        {
            return body switch
            {
                CommandBodyValue.Power => new CommandGetPower(),
                CommandBodyValue.Input => new CommandGetInput(pjLinkClass),
                CommandBodyValue.Mute => new CommandGetMute(),
                CommandBodyValue.ErrorStatus => new CommandGetErrorStatus(),
                CommandBodyValue.Lamp => new CommandGetLampCountUsageTime(),
                CommandBodyValue.InputTogglingList => new CommandGetInputTogglingList(pjLinkClass),
                CommandBodyValue.Name => new CommandGetProjectorName(),
                CommandBodyValue.ManufacturerName => new CommandGetManufacturerName(),
                CommandBodyValue.ProductName => new CommandGetProductName(),
                CommandBodyValue.OtherInformation => new CommandGetOtherInformation(),
                CommandBodyValue.ClassInformation => new CommandGetClassInformation(),
                CommandBodyValue.SerialNumber => new CommandGetSerialNumber(),
                CommandBodyValue.SoftwareVersion => new CommandGetSoftwareVersion(),
                CommandBodyValue.InputTerminalName => null,
                CommandBodyValue.InputResolution => null,
                CommandBodyValue.RecommendedResolution => null,
                CommandBodyValue.FilterUsageTime => null,
                CommandBodyValue.LampReplacementModelNumber => null,
                CommandBodyValue.FilterReplacementModelNumber => null,
                CommandBodyValue.Freeze => null,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static CommandGetResponseBase? deserializeGetCommandResponse(PJLinkClassValue pjLinkClass, CommandBodyValue body, string parameter)
        {
            var errorCode = parameter.ToCommandResponseErrorCode();
            if (errorCode.HasValue)
            {
                return body switch
                {
                    CommandBodyValue.Power => new CommandGetPowerResponse(errorCode.Value),
                    CommandBodyValue.Input => new CommandGetInputResponse(errorCode.Value, pjLinkClass),
                    CommandBodyValue.Mute => new CommandGetMuteResponse(errorCode.Value),
                    CommandBodyValue.ErrorStatus => new CommandGetErrorStatusResponse(errorCode.Value),
                    CommandBodyValue.Lamp => new CommandGetLampCountUsageTimeResponse(errorCode.Value),
                    CommandBodyValue.InputTogglingList => new CommandGetInputTogglingListResponse(errorCode.Value, pjLinkClass),
                    CommandBodyValue.Name => new CommandGetProjectorNameResponse(errorCode.Value),
                    CommandBodyValue.ManufacturerName => new CommandGetManufacturerNameResponse(errorCode.Value),
                    CommandBodyValue.ProductName => new CommandGetProductNameResponse(errorCode.Value),
                    CommandBodyValue.OtherInformation => new CommandGetOtherInformationResponse(errorCode.Value),
                    CommandBodyValue.ClassInformation => new CommandGetClassInformationResponse(errorCode.Value),
                    CommandBodyValue.SerialNumber => new CommandGetSerialNumberResponse(errorCode.Value),
                    CommandBodyValue.SoftwareVersion => new CommandGetSoftwareVersionResponse(errorCode.Value),
                    CommandBodyValue.InputTerminalName => null,
                    CommandBodyValue.InputResolution => null,
                    CommandBodyValue.RecommendedResolution => null,
                    CommandBodyValue.FilterUsageTime => null,
                    CommandBodyValue.LampReplacementModelNumber => null,
                    CommandBodyValue.FilterReplacementModelNumber => null,
                    CommandBodyValue.Freeze => null,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
            else
            {
                return body switch
                {
                    CommandBodyValue.Power => CommandGetPowerResponse.FromParameterString(parameter),
                    CommandBodyValue.Input => CommandGetInputResponse.FromParameterString(pjLinkClass, parameter),
                    CommandBodyValue.Mute => CommandGetMuteResponse.FromParameterString(parameter),
                    CommandBodyValue.ErrorStatus => CommandGetErrorStatusResponse.FromParameterString(parameter),
                    CommandBodyValue.Lamp => CommandGetLampCountUsageTimeResponse.FromParameterString(parameter),
                    CommandBodyValue.InputTogglingList => CommandGetInputTogglingListResponse.FromParameterString(pjLinkClass, parameter),
                    CommandBodyValue.Name => CommandGetProjectorNameResponse.FromParameterString(parameter),
                    CommandBodyValue.ManufacturerName => CommandGetManufacturerNameResponse.FromParameterString(parameter),
                    CommandBodyValue.ProductName => CommandGetProductNameResponse.FromParameterString(parameter),
                    CommandBodyValue.OtherInformation => CommandGetOtherInformationResponse.FromParameterString(parameter),
                    CommandBodyValue.ClassInformation => CommandGetClassInformationResponse.FromParameterString(parameter),
                    CommandBodyValue.SerialNumber => CommandGetSerialNumberResponse.FromParameterString(parameter),
                    CommandBodyValue.SoftwareVersion => CommandGetSoftwareVersionResponse.FromParameterString(parameter),
                    CommandBodyValue.InputTerminalName => null,
                    CommandBodyValue.InputResolution => null,
                    CommandBodyValue.RecommendedResolution => null,
                    CommandBodyValue.FilterUsageTime => null,
                    CommandBodyValue.LampReplacementModelNumber => null,
                    CommandBodyValue.FilterReplacementModelNumber => null,
                    CommandBodyValue.Freeze => null,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        private static CommandSetBase? deserializeSetCommand(PJLinkClassValue pjLinkClass, CommandBodyValue body, string parameter)
        {
            return body switch
            {
                CommandBodyValue.Power => CommandSetPower.FromParameterString(parameter),
                CommandBodyValue.Input => CommandSetInput.FromParameterString(pjLinkClass, parameter),
                CommandBodyValue.Mute => CommandSetMute.FromParameterString(parameter),
                CommandBodyValue.SpeakerVolume => null,
                CommandBodyValue.MicrophoneVolume => null,
                CommandBodyValue.Freeze => null,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static CommandSetResponseBase? deserializeSetCommandResponse(PJLinkClassValue pjLinkClass, CommandBodyValue body, string parameter)
        {
            var errorCode = parameter.ToCommandResponseErrorCode();
            if (!errorCode.HasValue)
                return null;

            return body switch
            {
                CommandBodyValue.Power => new CommandSetPowerResponse(errorCode.Value),
                CommandBodyValue.Input => new CommandSetInputResponse(errorCode.Value, pjLinkClass),
                CommandBodyValue.Mute => new CommandSetMuteResponse(errorCode.Value),
                CommandBodyValue.SpeakerVolume => null,
                CommandBodyValue.MicrophoneVolume => null,
                CommandBodyValue.Freeze => null,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        
    }
}

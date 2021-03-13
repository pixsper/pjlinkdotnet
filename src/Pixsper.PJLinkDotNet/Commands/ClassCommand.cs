namespace Pixsper.PJLinkDotNet.Commands
{
    public class CommandGetClassInformation : CommandGetBase
    {
        public CommandGetClassInformation()
            : base(PJLinkClassValue.Class1, CommandBodyValue.ClassInformation)
        {

        }
    }
    

    public class CommandGetClassInformationResponse : CommandGetResponseBase
    {
        public static CommandGetClassInformationResponse? FromParameterString(string str)
        {
            // TODO
            return null;
        }

        public CommandGetClassInformationResponse(PJLinkClassValue projectorPJLinkClass)
            : base(PJLinkClassValue.Class1, CommandBodyValue.ClassInformation)
        {
            ProjectorPJLinkClass = projectorPJLinkClass;
        }

        public CommandGetClassInformationResponse(CommandResponseErrorCode errorCode)
            : base(PJLinkClassValue.Class1, CommandBodyValue.ClassInformation, errorCode)
        {

        }

        public override string? ResultParameter => ProjectorPJLinkClass.HasValue ? ParameterFromValue(ProjectorPJLinkClass) : null;

        public PJLinkClassValue? ProjectorPJLinkClass { get; }
    }
}

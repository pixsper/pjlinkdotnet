using System;
using System.Collections.Generic;
using System.Linq;

namespace Pixsper.PJLinkDotNet.Commands
{
    public class CommandGetInputTogglingList : CommandGetBase
    {
        public CommandGetInputTogglingList(PJLinkClassValue pjLinkClass = PJLinkClassValue.Class1)
            : base(pjLinkClass, CommandBodyValue.InputTogglingList)
        {

        }
    }
    

    public class CommandGetInputTogglingListResponse : CommandGetResponseBase
    {
        public const int MaximumInputsCount = 50;

        public static CommandGetInputTogglingListResponse? FromParameterString(PJLinkClassValue pjLinkClass, string str)
        {
            // TODO

            return null;
        }

        public CommandGetInputTogglingListResponse(IEnumerable<InputId> inputs, PJLinkClassValue pjLinkClass = PJLinkClassValue.Class1)
            : base(pjLinkClass, CommandBodyValue.InputTogglingList)
        {
            var inputsList = inputs.ToList();

            if (inputsList.Count > MaximumInputsCount)
                throw new ArgumentException($"Inputs list count over maximum length of {MaximumInputsCount}", nameof(inputs));

            if (inputsList.Any(i => i.PJLinkClass != PJLinkClass))
                throw new ArgumentException($"PJLink class of all inputs must match message type", nameof(inputs));

            if (inputsList.Select(i => i.ToString()).Distinct().Count() != inputsList.Count)
                throw new ArgumentException($"Inputs list cannot contain duplicate values within same category", nameof(inputs));

            Inputs = inputsList.OrderBy(i => i.ToString()).ToList();
        }


        public CommandGetInputTogglingListResponse(CommandResponseErrorCode errorCode, PJLinkClassValue pjLinkClass = PJLinkClassValue.Class1)
            : base(pjLinkClass, CommandBodyValue.InputTogglingList, errorCode)
        {

        }

        public override string? ResultParameter => Inputs != null ? string.Join(" ", Inputs) : null;

        public IReadOnlyList<InputId>? Inputs { get; }
    }
}

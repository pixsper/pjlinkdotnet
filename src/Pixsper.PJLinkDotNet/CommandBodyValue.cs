using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Pixsper.PJLinkDotNet
{
    public enum CommandBodyValue
    {
        [CommandBodyMetadata(@"POWR", "Power", CommandBodyFlags.SupportsClass1 | CommandBodyFlags.SupportsGetAndSet)]
        Power,

        [CommandBodyMetadata(@"INPT", "Input", CommandBodyFlags.SupportsClass1And2 | CommandBodyFlags.SupportsGetAndSet)]
        Input,

        [CommandBodyMetadata(@"AVMT", "Mute", CommandBodyFlags.SupportsClass1 | CommandBodyFlags.SupportsGetAndSet)]
        Mute,

        [CommandBodyMetadata(@"ERST", "Error Status", CommandBodyFlags.SupportsClass1 | CommandBodyFlags.SupportsGet)]
        ErrorStatus,

        [CommandBodyMetadata(@"LAMP", "Lamp", CommandBodyFlags.SupportsClass1 | CommandBodyFlags.SupportsGet)]
        Lamp,

        [CommandBodyMetadata(@"INST", "Input Toggling List", CommandBodyFlags.SupportsClass1And2 | CommandBodyFlags.SupportsGet)]
        InputTogglingList,

        [CommandBodyMetadata(@"NAME", "Name", CommandBodyFlags.SupportsClass1 | CommandBodyFlags.SupportsGet)]
        Name,

        [CommandBodyMetadata(@"INF1", "Manufacturer Name", CommandBodyFlags.SupportsClass1 | CommandBodyFlags.SupportsGet)]
        ManufacturerName,

        [CommandBodyMetadata(@"INF2", "Product Name", CommandBodyFlags.SupportsClass1 | CommandBodyFlags.SupportsGet)]
        ProductName,

        [CommandBodyMetadata(@"INFO", "Other Information", CommandBodyFlags.SupportsClass1 | CommandBodyFlags.SupportsGet)]
        OtherInformation,

        [CommandBodyMetadata(@"CLSS", "Class Information", CommandBodyFlags.SupportsClass1 | CommandBodyFlags.SupportsGet)]
        ClassInformation,

        [CommandBodyMetadata(@"SNUM", "Serial Number", CommandBodyFlags.SupportsClass2 | CommandBodyFlags.SupportsGet)]
        SerialNumber,

        [CommandBodyMetadata(@"SVER", "Software Version", CommandBodyFlags.SupportsClass2 | CommandBodyFlags.SupportsGet)]
        SoftwareVersion,

        [CommandBodyMetadata(@"INNM", "Input Terminal Name", CommandBodyFlags.SupportsClass2 | CommandBodyFlags.SupportsGet)]
        InputTerminalName,

        [CommandBodyMetadata(@"IRES", "Input Resolution", CommandBodyFlags.SupportsClass2 | CommandBodyFlags.SupportsGet)]
        InputResolution,

        [CommandBodyMetadata(@"RRES", "Recommended Resolution", CommandBodyFlags.SupportsClass2 | CommandBodyFlags.SupportsGet)]
        RecommendedResolution,

        [CommandBodyMetadata(@"FILT", "Filter Usage Time", CommandBodyFlags.SupportsClass2 | CommandBodyFlags.SupportsGet)]
        FilterUsageTime,

        [CommandBodyMetadata(@"RLMP", "Lamp Replacement Model Number", CommandBodyFlags.SupportsClass2 | CommandBodyFlags.SupportsGet)]
        LampReplacementModelNumber,

        [CommandBodyMetadata(@"RFIL", "Filter Replacement Model Number", CommandBodyFlags.SupportsClass2 | CommandBodyFlags.SupportsGet)]
        FilterReplacementModelNumber,

        [CommandBodyMetadata(@"SVOL", "Speaker Volume", CommandBodyFlags.SupportsClass2 | CommandBodyFlags.SupportsSet)]
        SpeakerVolume,

        [CommandBodyMetadata(@"MVOL", "Microphone Volume", CommandBodyFlags.SupportsClass2 | CommandBodyFlags.SupportsSet)]
        MicrophoneVolume,

        [CommandBodyMetadata(@"FREZ", "Freeze", CommandBodyFlags.SupportsClass2 | CommandBodyFlags.SupportsGetAndSet)]
        Freeze
    }


    internal class CommandBodyMetadataAttribute : Attribute
    {
        public const int ValueLength = 4;

        public CommandBodyMetadataAttribute(string value, string displayName, CommandBodyFlags flags)
        {
            if (value.Length != 4 || !value.All(c => c >= 'A' && c <= 'Z' || c >= '0' && c <= '9'))
                throw new ArgumentException(
                    $"Must be {ValueLength} characters in length and is restricted to uppercase letters and digits", nameof(value));

            Value = value;

            if (string.IsNullOrWhiteSpace(displayName))
                throw new ArgumentException("Must not be empty or whitespace", nameof(displayName));

            DisplayName = displayName;

            if (!flags.HasFlag(CommandBodyFlags.SupportsClass1) && !flags.HasFlag(CommandBodyFlags.SupportsClass2))
                throw new ArgumentException("Command must support either class 1, class 2 or both");

            if (!flags.HasFlag(CommandBodyFlags.SupportsGet) && !flags.HasFlag(CommandBodyFlags.SupportsSet))
                throw new ArgumentException("Command must support either get, set or both");

            Flags = flags;
        }

        public string Value { get; }
        public string DisplayName { get; }
        public CommandBodyFlags Flags { get; }

        public bool SupportsClass(PJLinkClassValue pjLinkClass)
        {
            switch (pjLinkClass)
            {
                case PJLinkClassValue.Class1:
                    return Flags.HasFlag(CommandBodyFlags.SupportsClass1);
                case PJLinkClassValue.Class2:
                    return Flags.HasFlag(CommandBodyFlags.SupportsClass2);
                default:
                    throw new ArgumentOutOfRangeException(nameof(pjLinkClass), pjLinkClass, null);
            }
        }

        public bool SupportsKind(CommandKind kind)
        {
            switch (kind)
            {
                case CommandKind.Get:
                case CommandKind.GetResponse:
                    return Flags.HasFlag(CommandBodyFlags.SupportsGet);
                case CommandKind.Set:
                case CommandKind.SetResponse:
                    return Flags.HasFlag(CommandBodyFlags.SupportsSet);
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }
    }


    [Flags]
    internal enum CommandBodyFlags
    {
        None = 0b0000,
        SupportsClass1 = 0b0001,
        SupportsClass2 = 0b0010,
        SupportsClass1And2 = SupportsClass1 | SupportsClass2,
        SupportsGet = 0b0100,
        SupportsSet = 0b1000,
        SupportsGetAndSet = SupportsGet | SupportsSet
    }


    internal static class CommandBodyExtensions
    {
        private static readonly IReadOnlyDictionary<CommandBodyValue, string> ValueToString;
        private static readonly IReadOnlyDictionary<string, CommandBodyValue> StringToValue;

        static CommandBodyExtensions()
        {
            CommandBodies = Enum.GetValues(typeof(CommandBodyValue)).Cast<CommandBodyValue>()
                .Select(v => new
                {
                    v = v,
                    m = typeof(CommandBodyValue).GetTypeInfo().DeclaredMembers.First(m => m.Name == v.ToString())
                })
                .ToDictionary(a => a.v,
                    a => a.m.GetCustomAttribute<CommandBodyMetadataAttribute>());

            ValueToString = CommandBodies.ToDictionary(p => p.Key, 
                p => p.Value.Value);

            StringToValue = CommandBodies.ToDictionary(p => p.Value.Value,
                p => p.Key);
        }

        public static IReadOnlyDictionary<CommandBodyValue, CommandBodyMetadataAttribute> CommandBodies { get; }

        public static string ToCommandBodyString(this CommandBodyValue value) => ValueToString[value];

        public static CommandBodyValue? ToCommandBodyValue(this string str) => StringToValue.TryGetValue(str, out var value) ? (CommandBodyValue?)value : null;
    }
}
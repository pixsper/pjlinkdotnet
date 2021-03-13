using System;

namespace Pixsper.PJLinkDotNet
{
    public readonly struct InputId : IEquatable<InputId>, IComparable<InputId>, IComparable
    {
        public static bool TryParse(string str, out InputId value)
        {
            value = new InputId();

            if (str.Length != 2)
                return false;

            if (!int.TryParse(str[0].ToString(), out var categoryInt))
                return false;

            var category = (InputCategory)categoryInt;

            if (category != InputCategory.Rgb
                && category != InputCategory.Video
                && category != InputCategory.Digital
                && category != InputCategory.Storage
                && category != InputCategory.Network
                && category != InputCategory.Internal)
            {
                return false;
            }

            char inputValue = str[1];

            if ((inputValue < '0' || inputValue > '9') && (inputValue < 'A' || inputValue > 'Z'))
                return false;

            value = new InputId(category, inputValue);
            return true;
        }

        public InputId(InputCategory category, char value)
        {
            Category = category;

            var valueUpper = char.ToUpperInvariant(value);
            if ((valueUpper < '0' || valueUpper > '9') && (valueUpper < 'A' || valueUpper > 'Z'))
                throw new ArgumentOutOfRangeException(nameof(value), "Must be between 0-9 (class 1) or 0-9/A-Z (class 2)");

            Value = valueUpper;
        }

        public PJLinkClassValue PJLinkClass => Value >= 'A' && Value <= 'Z' ? PJLinkClassValue.Class2 : PJLinkClassValue.Class1;

        public InputCategory Category { get; }

        public char Value { get; }


        public static implicit operator string(InputId inputId) => inputId.ToString();

        public override string ToString() => $"{(int)Category}{Value}";


        public bool Equals(InputId other)
        {
            return Category == other.Category && Value == other.Value;
        }

        public override bool Equals(object? obj)
        {
            return obj is InputId other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int)Category * 397) ^ Value.GetHashCode();
            }
        }

        public static bool operator ==(InputId left, InputId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(InputId left, InputId right)
        {
            return !left.Equals(right);
        }

        public int CompareTo(InputId other)
        {
            var categoryComparison = Category.CompareTo(other.Category);
            if (categoryComparison != 0) return categoryComparison;
            return Value.CompareTo(other.Value);
        }

        public int CompareTo(object? obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            return obj is InputId other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(InputId)}");
        }

        public static bool operator <(InputId left, InputId right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator >(InputId left, InputId right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator <=(InputId left, InputId right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >=(InputId left, InputId right)
        {
            return left.CompareTo(right) >= 0;
        }
    }
}

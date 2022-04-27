using System;
using System.Collections.Generic;
using System.Text;

namespace PreFinals_Project.Models
{
    public class Variable : IComparable<Variable>
    {
        public char Character { get; private set; }
        public int Exponent { get; internal set; }
        public bool NotAdded { get; internal set; }

        public Variable(char character, int exponent)
        {
            Character = character;
            Exponent = exponent;
            NotAdded = true;
            if (Exponent < 0)
            {
                throw new InvalidOperationException("Exponent cannot be a negative number.");
            }
        }

        public int CompareTo(Variable other)
        {
            IComparer<char> comparer = Comparer<char>.Default;
            return comparer.Compare(Character, other.Character);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (Exponent == 1) return $"{Character}";
            if (Exponent > 9)
            {
                sb.Append($"{Character}");
                sb.Append("^{");
                sb.Append($"{Exponent.ToString()}");
                sb.Append("}");
                return sb.ToString();
            }
            return $"{Character}^{Exponent.ToString()}";
        }
    }
}
using System;

namespace Interpreter
{
    public struct SourceLocation : IEquatable<SourceLocation>
    {
        private readonly int column;
        private readonly int index;
        private readonly int line;

        public int Column => column;

        public int Index => index;

        public int Line => line;

        public SourceLocation(int index, int line, int column)
        {
            this.index = index;
            this.line = line;
            this.column = column;
        }

        public static bool operator !=(SourceLocation left, SourceLocation right)
        {
            return !left.Equals(right);
        }

        public static bool operator ==(SourceLocation left, SourceLocation right)
        {
            return left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            if (obj is SourceLocation)
            {
                return Equals((SourceLocation)obj);
            }
            return base.Equals(obj);
        }

        public bool Equals(SourceLocation other)
        {
            return other.GetHashCode() == GetHashCode();
        }

        public override int GetHashCode()
        {
            return 0xB1679EE ^ Index ^ Line ^ Column;
        }
    }
}

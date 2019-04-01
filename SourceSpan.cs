using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    public struct SourceSpan : IEquatable<SourceSpan>
    {
        private readonly SourceLocation end;
        private readonly SourceLocation start;

        private int priority;

        public int Priority => priority;

        public SourceLocation End => end;

        public int Length => end.Index - start.Index;

        public SourceLocation Start => start;

        public SourceSpan(SourceLocation start, SourceLocation end, int priority = 0)
        {
            this.start = start;
            this.end = end;
            this.priority = priority;
        }

        public static bool operator !=(SourceSpan left, SourceSpan right)
        {
            return !left.Equals(right);
        }

        public static bool operator ==(SourceSpan left, SourceSpan right)
        {
            return left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            if (obj is SourceSpan)
            {
                return Equals((SourceSpan)obj);
            }
            return base.Equals(obj);
        }

        public bool Equals(SourceSpan other)
        {
            return other.Start == Start && other.End == End;
        }

        public override int GetHashCode()
        {
            return 0x509CE ^ Start.GetHashCode() ^ End.GetHashCode();
        }

        public override string ToString()
        {
            return $"start - line: {start.Line} column: {start.Column}" +
                   $"    end - line: {end.Line} column: {end.Column}";
        }
    }
}

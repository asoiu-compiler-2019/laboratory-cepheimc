using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    public sealed class ErrorEntry
    {
        public string[] Lines { get; }

        public string Message { get; }

        public Severity Severity { get; }

        public SourceSpan Span { get; }

        public ErrorEntry(string message, string[] lines, Severity severity, SourceSpan span)
        {
            Message = message;
            Lines = lines;
            Span = span;
            Severity = severity;
        }
    }

    public class ErrorSink : IEnumerable<ErrorEntry>
    {
        private List<ErrorEntry> errors;

        public IEnumerable<ErrorEntry> Errors => errors.AsReadOnly();

        public bool HasErrors => errors.Count > 0;

        public ErrorSink()
        {
            errors = new List<ErrorEntry>();
        }

        public void AddError(string message, SourceCode sourceCode, Severity severity, SourceSpan span)
        {
            errors.Add(new ErrorEntry(message, sourceCode.GetLines(span.Start.Line, span.End.Line), severity, span));
        }

        public void Clear()
        {
            errors.Clear();
        }

        public IEnumerator<ErrorEntry> GetEnumerator()
        {
            return errors.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return errors.GetEnumerator();
        }
    }

    public enum Severity
    {
        None,
        Message,
        Warning,
        Error,
        Fatal
    }
}

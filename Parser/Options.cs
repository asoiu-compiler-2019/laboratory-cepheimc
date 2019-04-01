
namespace Interpreter.Parser
{
    public sealed class Options
    {
        public static readonly Options Default = new Options();
        public static readonly Options OptionalSemicolons = new Options { EnforceSemicolons = false }; 

        public bool AllowRootStatements { get; set; }

        public bool EnforceSemicolons { get; set; }  //end with ;

        public Options()
        {
            EnforceSemicolons = true;
            AllowRootStatements = true;
        }
    }
}

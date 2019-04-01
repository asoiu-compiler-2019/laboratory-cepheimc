
namespace Interpreter
{
    public enum TokenKind
    {
        EndOfFile,
        Error,

        #region Constants

        IntegerLiteral,
        StringLiteral,
        FloatLiteral,

        #endregion Constants

        #region WhiteSpace

        WhiteSpace,
        NewLine,

        #endregion WhiteSpace
       
        #region Identifiers

        Identifier,
        Keyword,

        #endregion Identifiers

        #region Groupings

        LeftBracket, // {
        RightBracket, // }
       // RightBrace, // ]
       // LeftBrace, // [
        LeftParenthesis, // (
        RightParenthesis, // )

        #endregion Groupings

        #region Operators

        //BitwiseAnd, // &
        //BitwiseOr, // |
        //BitwiseAndEqual, // &=
        //BitwiseOrEqual, // |=

        //BitwiseXorEqual, // ^=
        //BitwiseXor, // ^

        //BitShiftLeft, // <<
        //BitShiftRight, // >>

        GreaterThanOrEqual, // >=
        GreaterThan, // >

        LessThan, // <
        LessThanOrEqual, // <=

        Assignment, // =

        NotEqual, // !=
        Equal, // ==

        BooleanAnd, // &&
        BooleanOr, // ||
        
        #endregion Operators

        #region Punctuation

        Dot,
        Comma,
        Semicolon,
        Colon,

        #endregion Punctuation
    }
}

namespace XScriptLib
{
    /// <summary>
    /// Syntax words/symbols repository
    /// </summary>
    class XSyntax
    {
        #region Words

        public static string DeclareVarWord = "dec";
        public static string DeclareAndSetVarWord = "dset";
        public static string SetVarWord = "set";
        public static string BeginWord = "begin";
        public static string EndWord = "end";
        public static string IfWord = "if";
        public static string ElseWord = "else";
        public static string ElseIfWord = "elseif";
        public static string WhileWord = "while";
        public static string ForWord = "for";
        public static string FunctionWord = "func";
        public static string ReturnWord = "return";
        public static string ParamsWord = "params";
        public static string DeleteArray = "del";
        public static string DeleteAll = "delall";
        public static string ArrayLength = "len";
        public static string ArrayLevel = "lev";
        public static string SumArrayWord = "tsum";
        public static string DoWord = "do";

        #endregion

        #region Comments

        public static string OpenMultilineComment = "/*";
        public static string CloseMultilineComment = "*/";
        public static string SinglelineComment = "//";

        #endregion

        #region Arithmetic Operators

        public static char AddOp = '+';
        public static char SubOp = '-';
        public static char MulOp = '*';
        public static char DivOp = '/';
        public static char ModOp = '%';
        public static char PowOp = '^';

        #endregion

        #region Other Math Operators

        public static char Dot = '.';
        public static char Comma = ',';
        public static char OpenRoundBracket = '(';
        public static char CloseRoundBracket = ')';
        public static char OpenSquareBracket = '[';
        public static char CloseSquareBracket = ']';
        public static char OpenTriangleBracket = '<';
        public static char CloseTriangleBracket = '>';
        public static char PlacementEqual = '=';
        public static char AtSign = '@';
        public static char PoundSign = '#';
        public static char DollarSign = '$';
        public static string Arrow = ".";

        #endregion

        #region Logic Operators

        public static char LogicEqual = '=';
        public static char LogicNot = '!';
        public static char LogicAnd = '&';
        public static char LogicOr = '|';
        public static char LogicSmaller = '<';
        public static char LogicLarger = '>';
        public static string TrueWord = "true";
        public static string FalseWord = "false";

        #endregion
    }
}
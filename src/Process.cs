using System.Collections.Generic;

namespace XScriptLib
{
    /// <summary>
    /// Represents a process in a different scope (code level) like condition/loop/function
    /// </summary>
    public class Process
    {
        private string name;

        public string Name
        {
            get { return name; }
        }

        public Process(string Name)
        {
            name = Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code">String of code</param>
        /// <param name="arrs">Predetermined arrays</param>
        /// <param name="parser">Instance of script parser</param>
        /// <param name="boolean">The condition/expression of the process</param>
        /// <param name="codeLevel">The code level that the process is in</param>
        /// <returns>List of arrays</returns>
        public List<XArray> BeginProcess(string code, List<XArray> arrs, XSParser parser, string boolean, int codeLevel)
        {
            string[] cons = Helper.SplitOutOfBrackets(boolean, XSyntax.Comma, XSyntax.OpenRoundBracket, XSyntax.CloseRoundBracket);
            return OverrideProcess(code, arrs, parser, cons, codeLevel);
        }

        // A function that every process has to override
        protected virtual List<XArray> OverrideProcess(string code, List<XArray> arrs, XSParser parser, string[] cons, int codeLevel)
        {
            return new List<XArray>();
        }
    }
}

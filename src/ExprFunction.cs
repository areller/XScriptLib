using System;
using System.Collections.Generic;
using System.Text;

namespace XScriptLib
{
    public class ExprFunction
    {
        private string name;

        public string Name
        {
            get { return name; }
        }

        public ExprFunction(string Name)
        {
            name = Name;
        }

        public double ProccessFunction(string expr, ref int index, XMParser xmParser, List<XArray> arrs)
        {
            string[] parameters;
            index += name.Length + 1;
            int ti = index;
            for (int i = 1; i > 0; index++)
            {
                if (expr[index] == XSyntax.OpenRoundBracket)
                    i++;
                else if (expr[index] == XSyntax.CloseRoundBracket)
                    i--;
            }
            string s = expr.Substring(ti, index - ti - 1);
            if (s != "")
                parameters = Helper.SplitOutOfBrackets(s, XSyntax.Comma, XSyntax.OpenRoundBracket, XSyntax.CloseRoundBracket);
            else
                parameters = new string[0];
            return OverrideFunction(parameters, xmParser, arrs);
        }

        protected virtual double OverrideFunction(string[] parameters, XMParser xmParser, List<XArray> arrs)
        {
            return 0;
        }
    }
}

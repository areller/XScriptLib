using System;
using System.Collections.Generic;
using System.Text;
using XScriptLib.Exceptions;

namespace XScriptLib.Processes
{
    class FunctionProcess : Process
    {
        public FunctionProcess() : base(XSyntax.FunctionWord) { }

        protected override List<XArray> OverrideProcess(string code, List<XArray> arrs, XSParser parser, string[] cons, int codeLevel)
        {
            string funcName = cons[0];
            string[] arrsNames = new string[cons.Length - 1];
            bool p = false;
            if (cons[1].Replace(" ", "").StartsWith(XSyntax.ParamsWord))
            {
                if (cons.Length != 2)
                    throw new ParseException(Messages.ParamArray);
                for (int j = XSyntax.ParamsWord.Length + 1; j < cons[1].Length; j++)
                {
                    if (cons[1][j] == ' ')
                        continue;
                    else
                    {
                        arrsNames[0] = cons[1].Substring(j);
                        p = true;
                        break;
                    }
                }
            }
            else
            {
                for (int i = 1; i < cons.Length; i++)
                    arrsNames[i - 1] = cons[i];
            }
            parser.xmParser.mathFunctions.Add(new MathFunction(funcName, code, p, arrsNames, parser));
            return arrs;
        }
    }
}

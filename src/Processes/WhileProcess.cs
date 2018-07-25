using System;
using System.Collections.Generic;
using System.Text;
using XScriptLib.Exceptions;

namespace XScriptLib.Processes
{
    class WhileProcess : Process
    {
        public WhileProcess() : base(XSyntax.WhileWord) { }

        protected override List<XArray> OverrideProcess(string code, List<XArray> arrs, XSParser parser, string[] cons, int codeLevel)
        {
            if (cons.Length != 1)
                throw new XLogicException(Messages.InvalidNumCons);
            for (int i = 0; Helper.BooleanExpression(cons[0], arrs, parser); i++)
            {
                if (i > 1000000)
                    throw new EndlessException(Messages.LoopStuck);
                arrs = Helper.OnlyBeneathLevel(parser.ParseReturn(code, codeLevel + 1, arrs), codeLevel);
            }
            return arrs;
        }
    }
}
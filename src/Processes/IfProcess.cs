using System;
using System.Collections.Generic;
using System.Text;
using XScriptLib.Exceptions;

namespace XScriptLib.Processes
{
    class IfProcess : Process
    {
        public IfProcess() : base(XSyntax.IfWord) { }

        protected override List<XArray> OverrideProcess(string code, List<XArray> arrs, XSParser parser, string[] cons, int codeLevel)
        {
            if (cons.Length != 1)
                throw new XLogicException(Messages.InvalidNumCons);
            if (Helper.BooleanExpression(cons[0], arrs, parser))
                arrs = Helper.OnlyBeneathLevel(parser.ParseReturn(code, codeLevel + 1, arrs), codeLevel);
            return arrs;
        }
    }
}

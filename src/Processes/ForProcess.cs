using System;
using System.Collections.Generic;
using System.Text;
using XScriptLib.Exceptions;

namespace XScriptLib.Processes
{
    class ForProcess : Process
    {
        public ForProcess() : base(XSyntax.ForWord) { }

        protected override List<XArray> OverrideProcess(string code, List<XArray> arrs, XSParser parser, string[] cons, int codeLevel)
        {
            if (cons.Length != 2)
                throw new XLogicException(Messages.InvalidNumCons);
            int from = (int)parser.ParseMathExpr(cons[0], arrs);
            int to = (int)parser.ParseMathExpr(cons[1], arrs);
            List<XArray> t = new List<XArray>();
            t.Add(new XArray("loop." + codeLevel + ".from", 1));
            t.Add(new XArray("loop." + codeLevel + ".to", 1));
            t[0].vars[0] = from;
            t[1].vars[0] = to;
            t.Add(new XArray("loop." + codeLevel + ".current", 1));
            t.Add(new XArray("loop.this.from", 1));
            t.Add(new XArray("loop.this.to", 1));
            t[3].vars[0] = from;
            t[4].vars[0] = to;
            t.Add(new XArray("loop.this.current", 1));
            for (int i = from; i <= to; i++)
            {
                if (i > 1000000)
                    throw new EndlessException(Messages.LoopStuck);
                t[2].vars[0] = i;
                t[5].vars[0] = i;
                arrs = Helper.OnlyBeneathLevel(parser.ParseReturn(code, codeLevel + 1, XArray.ConnectLists(arrs, t)), codeLevel);
            }
            return arrs;
        }
    }
}
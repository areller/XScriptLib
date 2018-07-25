using System;
using System.Collections.Generic;
using System.Text;
using XScriptLib.Exceptions;

namespace XScriptLib.Commands
{
    class SumArray : Command
    {
        public SumArray() : base(XSyntax.SumArrayWord) { }

        protected override List<XArray> OverrideCommand(string[] parameters, XSParser parser, List<XArray> arrs)
        {
            int varIndex = Helper.ArrayIndex(arrs, parameters[1].Replace(" ", ""));
            int arrIndex = Helper.ArrayIndex(arrs, parameters[0].Replace(" ", ""));
            if (arrs[varIndex].vars.Length > 1)
                throw new ParseException(Messages.FaieldToSumArr);
            arrs[varIndex].vars[0] = 0;
            for (int i = 0; i < arrs[arrIndex].vars.Length; i++)
                arrs[varIndex].vars[0] += arrs[arrIndex].vars[i];
            return arrs;
        }
    }
}

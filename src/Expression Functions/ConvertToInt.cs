using System;
using System.Collections.Generic;
using System.Text;

namespace XScriptLib.Expression_Functions
{
    class ConvertToInt : ExprFunction
    {
        public ConvertToInt() : base("cti") { }

        protected override double OverrideFunction(string[] parameters, XMParser xmParser, List<XArray> arrs)
        {
            return (int)xmParser.GetDouble(parameters[0], 0, arrs);
        }
    }
}

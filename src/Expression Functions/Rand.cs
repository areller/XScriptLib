using System;
using System.Collections.Generic;
using System.Text;

namespace XScriptLib.Expression_Functions
{
    class Rand : ExprFunction
    {
        public Rand() : base("rand") { }

        protected override double OverrideFunction(string[] parameters, XMParser xmParser, List<XArray> arrs)
        {
            Random r = new Random();
            if (parameters.Length == 1)
            {
                int len = (int)xmParser.GetDouble(parameters[0], 0, arrs);
                return (double)r.Next(len);
            }
            else if (parameters.Length == 2)
            {
                int a = (int)xmParser.GetDouble(parameters[0], 0, arrs);
                int b = (int)xmParser.GetDouble(parameters[1], 0, arrs);
                return (double)r.Next(a, b);
            }
            return r.NextDouble();
        }
    }
}

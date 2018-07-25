using System;
using System.Collections.Generic;
using System.Text;

namespace XScriptLib.Expression_Functions
{
    class SumSeries : ExprFunction
    {
        public SumSeries() : base("sseries") { }

        protected override double OverrideFunction(string[] parameters, XMParser xmParser, List<XArray> arrs)
        {
            double sum = 0;
            int s = (int)xmParser.GetDouble(parameters[1], 0, arrs);
            int e = (int)xmParser.GetDouble(parameters[2], 0, arrs);
            List<XArray> arrList = new List<XArray>();
            arrList.Add(new XArray("n", 1));
            for (int i = s; i <= e; i++)
            {
                arrList[0].vars[0] = i;
                sum += xmParser.GetDouble(parameters[0], 0, XArray.ConnectLists(arrs, arrList));
            }
            return sum;
        }
    }
}
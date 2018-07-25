using System;
using System.Collections.Generic;
using System.Text;

namespace XScriptLib
{
    public class MathFunction : ExprFunction
    {
        public MathFunction(string Name, string Code, bool Param, string[] Arrays, XSParser XsParser)
            : base(Name)
        {
            code = Code;
            arrs = Arrays;
            xsParser = XsParser;
            param = Param;
        }

        public string code;
        public string[] arrs;
        public XSParser xsParser;
        public bool param;

        protected override double OverrideFunction(string[] parameters, XMParser xmParser, List<XArray> xarrs)
        {
            if (param)
            {
                double[] mparameters = new double[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                    mparameters[i] = xmParser.GetDouble(parameters[i], 0, xarrs);
                XArray array = new XArray(arrs[0], mparameters.Length);
                array.vars = mparameters;
                array.level = 1;
                List<XArray> arrList = new List<XArray>();
                arrList.Add(array);
                return xsParser.ParseReturnVariable(code, 1, arrList);
            }
            else
            {
                List<XArray> arrays = new List<XArray>();
                for (int i = 0; i < parameters.Length; i++)
                {
                    int k = -1;
                    for (int j = 0; j < xarrs.Count; j++)
                    {
                        if (xarrs[j].name == parameters[i])
                        {
                            k = j;
                            break;
                        }
                    }
                    if (k != -1)
                    {
                        arrays.Add(new XArray(arrs[i], xarrs[k].vars.Length));
                        arrays[arrays.Count - 1].vars = xarrs[k].vars;
                        arrays[arrays.Count - 1].level = 1;
                    }
                }
                return xsParser.ParseReturnVariable(code, 1, arrays);
            }
        }
    }
}
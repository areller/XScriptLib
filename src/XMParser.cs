using System;
using System.Collections.Generic;
using XScriptLib.Exceptions;
using XScriptLib.ArrayProperties;
using XScriptLib.Expression_Functions;

namespace XScriptLib
{
    /// <summary>
    /// Math expressions parser
    /// </summary>
    public class XMParser
    {
        public List<ExprFunction> exprFunctions;
        public List<MathFunction> mathFunctions;
        public List<ArrayProperty> arrProperties;

        /// <summary>
        /// Initializes functions and properties
        /// </summary>
        public XMParser()
        {
            exprFunctions = new List<ExprFunction>();
            mathFunctions = new List<MathFunction>();
            arrProperties = new List<ArrayProperty>();
            arrProperties.Add(new ArrayLength());
            arrProperties.Add(new ArrayLevel());
            exprFunctions.Add(new SumSeries());
            exprFunctions.Add(new Rand());
            exprFunctions.Add(new ConvertToInt());
        }

        /// <summary>
        /// Parses an expressions string with predetermined variables and returns a number as a  result
        /// </summary>
        /// <param name="expr">Expressions string</param>
        /// <param name="vars">Names of predetermined variables</param>
        /// <param name="values">Values of predetermined variables</param>
        /// <returns>Result of parsing</returns>
        public double GetDouble(string expr, string[] vars, double[] values)
        {
            List<XArray> arrs = new List<XArray>();
            for (int i = 0; i < vars.Length; i++)
            {
                arrs.Add(new XArray(vars[i], 1));
                arrs[i].vars[0] = values[i];
            }
            return GetDouble(expr, 0, arrs);
        }

        /// <summary>
        /// Parses an expression string starting from a certain index with predetermined arrays and returns a number as a result
        /// </summary>
        /// <param name="expr">Expression string</param>
        /// <param name="index">Some index on the expressiosn string</param>
        /// <param name="arrs">Predetermined arrays</param>
        /// <returns>Result of parsing</returns>
        public double GetDouble(string expr, int index, List<XArray> arrs)
        {
            expr = expr.Replace(" ", "");

            // A minus sign can appear in the beginning of an expression
            // In that case, a zero is added infront of it to turn the minus sign into a subtraction between two numbers
            if (expr[0] == XSyntax.SubOp)
                expr = "0" + expr;

            return getSums(expr, ref index, arrs);
        }

        /// <summary>
        /// Iterates over sums/subtractions in the expression 
        /// </summary>
        /// <param name="expr">Expression string</param>
        /// <param name="index">Current index on the expression string</param>
        /// <param name="arrs">Predetermined arrays</param>
        /// <returns>Numeric result of summations/subtractions</returns>
        private double getSums(string expr, ref int index, List<XArray> arrs)
        {
            double y;
            char op;
            double x = getFactors(expr, ref index, arrs);
            while (index < expr.Length)
            {
                op = expr[index];
                if (op != XSyntax.AddOp && op != XSyntax.SubOp)
                    return x;
                index++;
                y = getFactors(expr, ref index, arrs);
                if (op == XSyntax.AddOp)
                    x += y;
                else
                    x -= y;
            }
            return x;
        }

        /// <summary>
        /// Iterates over factors/fractions in the expressions
        /// </summary>
        /// <param name="expr">Expression string</param>
        /// <param name="index">Current index on the expression string</param>
        /// <param name="arrs">Predetermined arrays</param>
        /// <returns>Numeric result of factors/fractions</returns>
        private double getFactors(string expr, ref int index, List<XArray> arrs)
        {
            double y;
            char op;
            double x = getPowers(expr, ref index, arrs);
            while (index < expr.Length)
            {
                op = expr[index];
                if (op != XSyntax.MulOp && op != XSyntax.DivOp && op != XSyntax.ModOp)
                    return x;
                index++;
                y = getPowers(expr, ref index, arrs);
                if (op == XSyntax.MulOp)
                    x *= y;
                else if (op == XSyntax.DivOp)
                    x /= y;
                else
                    x %= y; // Also iterates over modulo's
            }
            return x;
        }

        /// <summary>
        /// Iterates over powers in the expressions
        /// </summary>
        /// <param name="expr">Expression string</param>
        /// <param name="index">Current index on the expression string</param>
        /// <param name="arrs">Predetermined arrays</param>
        /// <returns>Numeric result of powers</returns>
        private double getPowers(string expr, ref int index, List<XArray> arrs)
        {
            double y;
            char op;
            double x = getArrs(expr, ref index, arrs);
            while (index < expr.Length)
            {
                op = expr[index];
                if (op != XSyntax.PowOp)
                    return x;
                index++;
                y = getArrs(expr, ref index, arrs);
                x = Math.Pow(x, y);
            }
            return x;
        }

        /// <summary>
        /// Parses array indices, extracts array values from predetermined list
        /// </summary>
        /// <param name="expr">Expression string</param>
        /// <param name="index">Current index on the expression string</param>
        /// <param name="arrs">Predetermined arrays</param>
        /// <returns>Numeric result of parsed array indices</returns>
        private double getArrs(string expr, ref int index, List<XArray> arrs)
        {
            for (int i = 0; i < arrs.Count; i++)
            {
                if (expr.Substring(index).StartsWith(arrs[i].name) &&
                    (index == expr.Length - arrs[i].name.Length ||
                    expr[index + arrs[i].name.Length] == XSyntax.CloseRoundBracket ||
                    expr[index + arrs[i].name.Length] == XSyntax.AddOp ||
                    expr[index + arrs[i].name.Length] == XSyntax.SubOp ||
                    expr[index + arrs[i].name.Length] == XSyntax.MulOp ||
                    expr[index + arrs[i].name.Length] == XSyntax.DivOp ||
                    expr[index + arrs[i].name.Length] == XSyntax.ModOp ||
                    expr[index + arrs[i].name.Length] == XSyntax.PowOp ||
                    expr[index + arrs[i].name.Length] == XSyntax.CloseSquareBracket ||
                    expr[index + arrs[i].name.Length] == XSyntax.OpenSquareBracket))
                {
                    if (index == expr.Length - arrs[i].name.Length || expr[index + arrs[i].name.Length] != XSyntax.OpenSquareBracket)
                    {
                        if (arrs[i].vars.Length > 1)
                            throw new XMParseException(Messages.ArrayIndexAfterNameErr);
                        index += arrs[i].name.Length;
                        return arrs[i].vars[0];
                    }
                    else
                    {
                        index = expr.IndexOf(XSyntax.OpenSquareBracket, index);
                        index++;
                        int ti = index;
                        for (int j = 1; j > 0; index++)
                        {
                            if (expr[index] == XSyntax.OpenSquareBracket)
                                j++;
                            else if (expr[index] == XSyntax.CloseSquareBracket)
                                j--;
                        }
                        return arrs[i].vars[(int)GetDouble(expr.Substring(ti, index - ti - 1), 0, arrs)];
                    }
                }
                else
                {
                    for (int j = 0; j < arrProperties.Count; j++)
                    {
                        if (expr.Substring(index).StartsWith(arrs[i].name + XSyntax.Arrow + arrProperties[j].Name) &&
   (index == expr.Length - (arrs[i].name.Length + XSyntax.Arrow.Length + arrProperties[j].Name.Length) ||
   expr[index + arrs[i].name.Length + XSyntax.Arrow.Length + arrProperties[j].Name.Length] == XSyntax.CloseRoundBracket ||
   expr[index + arrs[i].name.Length + XSyntax.Arrow.Length + arrProperties[j].Name.Length] == XSyntax.AddOp ||
   expr[index + arrs[i].name.Length + XSyntax.Arrow.Length + arrProperties[j].Name.Length] == XSyntax.SubOp ||
   expr[index + arrs[i].name.Length + XSyntax.Arrow.Length + arrProperties[j].Name.Length] == XSyntax.MulOp ||
   expr[index + arrs[i].name.Length + XSyntax.Arrow.Length + arrProperties[j].Name.Length] == XSyntax.DivOp ||
   expr[index + arrs[i].name.Length + XSyntax.Arrow.Length + arrProperties[j].Name.Length] == XSyntax.ModOp ||
   expr[index + arrs[i].name.Length + XSyntax.Arrow.Length + arrProperties[j].Name.Length] == XSyntax.PowOp ||
   expr[index + arrs[i].name.Length + XSyntax.Arrow.Length + arrProperties[j].Name.Length] == XSyntax.CloseSquareBracket))
                            return arrProperties[j].ProccessProperty(arrs[i], ref index);
                    }
                }
            }
            return getBrackets(expr, ref index, arrs);
        }

        /// <summary>
        /// Parses brackets by evaluating them recursivly
        /// </summary>
        /// <param name="expr">Expression string</param>
        /// <param name="index">Current index on the expression string</param>
        /// <param name="arrs">Predetermined arrays</param>
        /// <returns>Numeric result of parsed brackets</returns>
        private double getBrackets(string expr, ref int index, List<XArray> arrs)
        {
            if (expr[index] == XSyntax.OpenRoundBracket)
            {
                index++;
                int tempIndex = index;
                for (int i = 1; i > 0; index++)
                {
                    if (expr[index] == XSyntax.OpenRoundBracket)
                        i++;
                    else if (expr[index] == XSyntax.CloseRoundBracket)
                        i--;
                }
                string s = expr.Substring(tempIndex, index - tempIndex);
                return GetDouble(s, 0, arrs);
            }
            return getFunctions(expr, ref index, arrs);
        }

        /// <summary>
        /// Parses functions by evaluating the arguments recursivly
        /// </summary>
        /// <param name="expr">Expression string</param>
        /// <param name="index">Current index on expression string</param>
        /// <param name="arrs">Predetermined arrays</param>
        /// <returns>Numeric result of parsed functions</returns>
        private double getFunctions(string expr, ref int index, List<XArray> arrs)
        {
            if (char.IsDigit(expr[index]))
                return getNumber(expr, ref index);
            if (expr[index] == XSyntax.AtSign)
                return getExpressionFunctions(expr, ref index, arrs);
            else
                return getMathFunctions(expr, ref index, arrs);
        }

        private double getExpressionFunctions(string expr, ref int index, List<XArray> arrs)
        {
            for (int i = 0; i < exprFunctions.Count; i++)
            {
                if (expr.Substring(index).StartsWith(XSyntax.AtSign + exprFunctions[i].Name + XSyntax.OpenRoundBracket))
                {
                    index++;
                    return exprFunctions[i].ProccessFunction(expr, ref index, this, arrs);
                }
            }
            throw new XMParseException(Messages.FunctionWasNotFound);
        }

        private double getMathFunctions(string expr, ref int index, List<XArray> arrs)
        {
            for (int i = 0; i < mathFunctions.Count; i++)
            {
                if (expr.Substring(index).StartsWith(mathFunctions[i].Name + XSyntax.OpenRoundBracket))
                    return mathFunctions[i].ProccessFunction(expr, ref index, this, arrs);
            }
            throw new XMParseException(Messages.FunctionWasNotFound);
        }

        /// <summary>
        /// Parses numbers
        /// </summary>
        /// <param name="expr">Expression string</param>
        /// <param name="index">Current index on expression string</param>
        /// <returns>Numeric result of a string of digits</returns>
        private double getNumber(string expr, ref int index)
        {
            string num = "";
            while (char.IsDigit(expr[index]) || expr[index] == '.')
            {
                num += expr[index];
                index++;
                if (index == expr.Length)
                {
                    index--;
                    break;
                }
            }
            return double.Parse(num);
        }
    }
}
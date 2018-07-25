using System;
using System.Collections.Generic;
using System.Text;
using XScriptLib.Exceptions;

namespace XScriptLib
{
    class Helper
    {
        public static string[] SplitOutOfBrackets(string expr, char seperator, char openBracket, char closeBracket)
        {
            List<string> args = new List<string>();
            int brackets = 0;
            int ti = 0;
            for (int i = 0; i < expr.Length; i++)
            {
                if (expr[i] == openBracket)
                    brackets++;
                else if (expr[i] == closeBracket)
                    brackets--;
                if (expr[i] == seperator && brackets == 0)
                {
                    args.Add(expr.Substring(ti, i - ti));
                    ti = i + 1;
                }
            }
            args.Add(expr.Substring(ti));
            return args.ToArray();
        }

        public static List<XArray> OnlyBeneathLevel(List<XArray> arrays, int codeLevel)
        {
            List<XArray> tempArrays = new List<XArray>();
            for (int i = 0; i < arrays.Count; i++)
            {
                if (arrays[i].level <= codeLevel && arrays[i].level != -1 && arrays[i].level != -2)
                {
                    tempArrays.Add(new XArray(arrays[i].name, arrays[i].vars.Length));
                    tempArrays[tempArrays.Count - 1].vars = arrays[i].vars;
                    tempArrays[tempArrays.Count - 1].level = arrays[i].level;
                }
            }
            return tempArrays;
        }

        public static bool BooleanExpression(string expr, List<XArray> arrays, XSParser parser)
        {
            string b = "";
            int ti = 0;
            string eexpr = "";
            for (int i = 0; i < expr.Length; i++)
            {
                if (expr[i] == XSyntax.LogicAnd || expr[i] == XSyntax.LogicOr)
                {
                    eexpr = expr.Substring(ti, i - ti);
                    if (SingleBooleanExpression(eexpr, arrays, parser))
                        b += XSyntax.TrueWord;
                    else
                        b += XSyntax.FalseWord;
                    b += expr[i];
                    ti = i + 1;
                }
            }
            eexpr = expr.Substring(ti, expr.Length - ti);
            if (SingleBooleanExpression(eexpr, arrays, parser))
                b += XSyntax.TrueWord;
            else
                b += XSyntax.FalseWord;
            while (!(b == XSyntax.TrueWord || b == XSyntax.FalseWord))
            {
                b = b.Replace(XSyntax.TrueWord + XSyntax.LogicAnd + XSyntax.TrueWord, XSyntax.TrueWord);
                b = b.Replace(XSyntax.FalseWord + XSyntax.LogicAnd + XSyntax.FalseWord, XSyntax.FalseWord);
                b = b.Replace(XSyntax.TrueWord + XSyntax.LogicAnd + XSyntax.FalseWord, XSyntax.FalseWord);
                b = b.Replace(XSyntax.FalseWord + XSyntax.LogicAnd + XSyntax.TrueWord, XSyntax.FalseWord);
                b = b.Replace(XSyntax.TrueWord + XSyntax.LogicOr + XSyntax.TrueWord, XSyntax.TrueWord);
                b = b.Replace(XSyntax.FalseWord + XSyntax.LogicOr + XSyntax.FalseWord, XSyntax.FalseWord);
                b = b.Replace(XSyntax.TrueWord + XSyntax.LogicOr + XSyntax.FalseWord, XSyntax.TrueWord);
                b = b.Replace(XSyntax.FalseWord + XSyntax.LogicOr + XSyntax.TrueWord, XSyntax.TrueWord);
            }
            if (b == XSyntax.TrueWord)
                return true;
            return false;
        }

        public static bool SingleBooleanExpression(string expr, List<XArray> arrays, XSParser parser)
        {
            char op = ' ';
            double left = 0f;
            double right = 0f;
            for (int i = 0; i < expr.Length; i++)
            {
                if (expr[i] == XSyntax.LogicEqual || expr[i] == XSyntax.LogicLarger || expr[i] == XSyntax.LogicSmaller || expr[i] == XSyntax.LogicNot)
                {
                    op = expr[i];
                    left = parser.ParseMathExpr(expr.Substring(0, i), arrays);
                    right = parser.ParseMathExpr(expr.Substring(i + 1), arrays);
                    break;
                }
            }

            if (op == XSyntax.LogicEqual)
            {
                if (left == right)
                    return true;
                return false;
            }
            else if (op == XSyntax.LogicSmaller)
            {
                if (left < right)
                    return true;
                return false;
            }
            else if (op == XSyntax.LogicLarger)
            {
                if (left > right)
                    return true;
                return false;
            }
            else if(op == XSyntax.LogicNot)
            {
                if(left != right)
                    return true;
                return false;
            }
            throw new XLogicException("Unknown operator.");
        }

        public static int ArrayIndex(List<XArray> arrs, string name)
        {
            for (int i = 0; i < arrs.Count; i++)
            {
                if (arrs[i].name == name)
                    return i;
            }
            return -1;
        }
    }
}
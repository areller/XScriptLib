using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using XScriptLib.Processes;
using XScriptLib.Commands;
using XScriptLib.Exceptions;

namespace XScriptLib
{
    /// <summary>
    /// Script Parser
    /// </summary>
    public class XSParser
    {
        public XMParser xmParser;
        public List<Process> processes;
        public List<Command> commands;
        public bool removeSpaces = true;

        /// <summary>
        /// Initializes the Math Parser, Commands, and Processes
        /// </summary>
        public XSParser()
        {
            xmParser = new XMParser();
            processes = new List<Process>();
            commands = new List<Command>();
            processes.Add(new IfProcess());
            processes.Add(new WhileProcess());
            processes.Add(new FunctionProcess());
            processes.Add(new ForProcess());
            commands.Add(new SumArray());
        }

        /// <summary>
        /// Parses an array of code lines and returns multiple arrays as result
        /// </summary>
        /// <param name="lines">Array of code lines</param>
        /// <returns>List of arrays</returns>
        public List<XArray> ParseReturn(string[] lines)
        {
            return ParseReturn(lines, 0, new List<XArray>());
        }

        /// <summary>
        /// Parses a string of code and returns multiple arrays as result
        /// </summary>
        /// <param name="code">string of code</param>
        /// <returns>List of arrays</returns>
        public List<XArray> ParseReturn(string code)
        {
            return ParseReturn(code, 0, new List<XArray>());
        }

        /// <summary>
        /// Parses an array of code lines and returns a single number as result
        /// </summary>
        /// <param name="lines">Array of code lines</param>
        /// <returns>Single number</returns>
        public double ParseReturnVariable(string[] lines)
        {
            List<XArray> arrays = ParseReturn(lines);
            if (arrays.Count == 1 && arrays[0].level == -1 && arrays[0].vars.Length == 1)
                return arrays[0].vars[0];
            throw new ParseException("Failed to return array.");
        }

        /// <summary>
        /// Parses a string of code and returns a single number as result
        /// </summary>
        /// <param name="code">String of code</param>
        /// <returns>Single number</returns>
        public double ParseReturnVariable(string code)
        {
            List<XArray> arrays = ParseReturn(code);
            if (arrays.Count == 1 && arrays[0].level == -1 && arrays[0].vars.Length == 1)
                return arrays[0].vars[0];
            throw new ParseException("Failed to return array.");
        }

        /// <summary>
        /// Parses an array of code lines and returns multiple numbers as result
        /// </summary>
        /// <param name="lines">Array of code lines</param>
        /// <returns>Array of numbers</returns>
        public double[] ParseReturnArray(string[] lines)
        {
            List<XArray> array = ParseReturn(lines);
            if (array.Count == 1 && array[0].level == -1)
                return array[0].vars;
            throw new ParseException("Failed to return array.");
        }
        
        /// <summary>
        /// Parses a string of code and returns multiple numbers as result
        /// </summary>
        /// <param name="code">String of code</param>
        /// <returns>Array of numbers</returns>
        public double[] ParseReturnArray(string code)
        {
            List<XArray> array = ParseReturn(code);
            if (array.Count == 1 && array[0].level == -1)
                return array[0].vars;
            throw new ParseException("Failed to return array.");
        }

        /// <summary>
        /// Parses a string of code with given arrays and returns multiple numbers as result
        /// </summary>
        /// <param name="code">String of code</param>
        /// <param name="arrs">Predetermined arrays</param>
        /// <returns>Array of numbers</returns>
        public double[] ParseReturnArray(string code, List<XArray> arrs)
        {
            List<XArray> array = ParseReturn(code, 0, arrs);
            if (array.Count == 1 && array[0].level == -1)
                return array[0].vars;
            throw new ParseException("Failed to return array.");
        }

        /// <summary>
        /// Parses string of code at a specific code level with arrays from an higher levels and returns multiple arrays as result
        /// </summary>
        /// <param name="code">String of code</param>
        /// <param name="codeLevel">Code level</param>
        /// <param name="mainArrays">Arrays from higher code level</param>
        /// <returns>List of arrays</returns>
        public List<XArray> ParseReturn(string code, int codeLevel, List<XArray> mainArrays)
        {
            string[] lines = code.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            return ParseReturn(lines, codeLevel, mainArrays);
        }

        /// <summary>
        /// Parses a string of code at a specific code level with arrays from an higher level and returns a single number as result
        /// </summary>
        /// <param name="code">String of code</param>
        /// <param name="codeLevel">Code level</param>
        /// <param name="mainArrays">Arrays from higher code level</param>
        /// <returns>Single number</returns>
        public double ParseReturnVariable(string code, int codeLevel, List<XArray> mainArrays)
        {
            List<XArray> arrays = ParseReturn(code, codeLevel, mainArrays);
            if (arrays.Count == 1 && arrays[0].level == -1 && arrays[0].vars.Length == 1)
                return arrays[0].vars[0];
            throw new ParseException("Failed to return variable.");
        }

        /// <summary>
        /// Parses an array of code lines at a specific code level with arrays from an higher level and returns multiple arrays as result
        /// This is the main parsing function
        /// Warning: Spaghetti code written many years ago
        /// </summary>
        /// <param name="lines">Array of code lines</param>
        /// <param name="codeLevel">Code level</param>
        /// <param name="mainArrays">Arrays from higher code level</param>
        /// <returns>List of arrays</returns>
        public List<XArray> ParseReturn(string[] lines, int codeLevel, List<XArray> mainArrays)
        {
            List<XArray> arrays = mainArrays;
            string set_arrName;
            int set_arrIndex;
            string funcName;
            string paramName;

            if (removeSpaces)
                removeSpacesFromCode(lines);

            try
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith("/*"))
                    {
                        i++;

                        for (; ; i++)
                        {
                            if (lines[i].StartsWith(XSyntax.CloseMultilineComment))
                            {
                                i++;
                                break;
                            }
                        }
                    }

                    lines[i] = Regex.Replace(lines[i], XSyntax.SinglelineComment + "(.+?)+", "");

                    if (lines[i] == "" || lines[i][0] == XSyntax.PoundSign)
                        continue;

                    set_arrName = "";
                    set_arrIndex = -1;
                    funcName = "";
                    paramName = "";
                    if (lines[i].StartsWith(XSyntax.DeclareVarWord + " "))
                    {
                        for (int j = XSyntax.DeclareVarWord.Length + 1; j < lines[i].Length; j++)
                        {
                            if (lines[i][j] == ' ')
                                continue;
                            else
                            {
                                string[] toDeclare = Helper.SplitOutOfBrackets(lines[i].Substring(j), XSyntax.Comma, XSyntax.OpenRoundBracket, XSyntax.CloseRoundBracket);
                                for (int w = 0; w < toDeclare.Length; w++)
                                {
                                    toDeclare[w] = toDeclare[w].Replace(" ", "");
                                    if (toDeclare[w].Contains(XSyntax.OpenSquareBracket.ToString()))
                                    {
                                        int u = 0;
                                        j = toDeclare[w].IndexOf(XSyntax.OpenSquareBracket);
                                        string name = toDeclare[w].Substring(0, j);
                                        j++;
                                        u = j;
                                        for (int k = 1; k > 0; j++)
                                        {
                                            if (toDeclare[w][j] == XSyntax.OpenSquareBracket)
                                                k++;
                                            else if (toDeclare[w][j] == XSyntax.CloseSquareBracket)
                                                k--;
                                        }
                                        int length = (int)ParseMathExpr(toDeclare[w].Substring(u, j - u), arrays);
                                        arrays.Add(new XArray(name, length));
                                        arrays[arrays.Count - 1].level = codeLevel;
                                    }
                                    else
                                    {
                                        arrays.Add(new XArray(toDeclare[w], 1));
                                        arrays[arrays.Count - 1].level = codeLevel;
                                    }
                                }
                                break;
                            }
                        }
                    }
                    else if (lines[i].StartsWith(XSyntax.DeleteArray + " "))
                    {
                        for (int j = XSyntax.DeleteArray.Length + 1; j < lines[i].Length; j++)
                        {
                            if (lines[i][j] == ' ')
                                continue;
                            else
                            {
                                string array = lines[i].Substring(j);
                                for (int k = 0; k < arrays.Count; k++)
                                {
                                    if (arrays[k].name == array && arrays[k].level <= codeLevel)
                                    {
                                        arrays.RemoveAt(k);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else if (lines[i].StartsWith(XSyntax.DeleteAll + " "))
                    {
                        for (int j = XSyntax.DeleteAll.Length + 1; j < lines[i].Length; j++)
                        {
                            if (lines[i][j] == ' ')
                                continue;
                            else
                            {
                                int lev = (int)ParseMathExpr(lines[i].Substring(j), arrays);
                                for (int k = arrays.Count - 1; k >= 0; k--)
                                {
                                    if (arrays[k].level <= lev)
                                        arrays.RemoveAt(k);
                                }
                                break;
                            }
                        }
                    }
                    else if (lines[i].StartsWith(XSyntax.SetVarWord + " "))
                    {
                        for (int j = XSyntax.SetVarWord.Length + 1; j < lines[i].Length; j++)
                        {
                            if (set_arrName != "")
                            {
                                if (lines[i][j] == XSyntax.PlacementEqual)
                                {
                                    string value = lines[i].Substring(j + 1);
                                    int id = -1;
                                    for (int k = 0; k < arrays.Count; k++)
                                    {
                                        if (arrays[k].name == set_arrName && arrays[k].level <= codeLevel)
                                        {
                                            id = k;
                                            break;
                                        }
                                    }
                                    arrays[id].vars[set_arrIndex] = ParseMathExpr(value, arrays);
                                    break;
                                }
                            }
                            else
                            {
                                if (lines[i][j] == ' ')
                                    continue;
                                else
                                {
                                    string all = lines[i].Substring(j, lines[i].IndexOf(XSyntax.PlacementEqual, j) - j);
                                    all = all.Replace(" ", "");
                                    if (all.Contains(XSyntax.OpenSquareBracket.ToString()))
                                    {
                                        int end = lines[i].IndexOf(XSyntax.OpenSquareBracket, j);
                                        set_arrName = lines[i].Substring(j, end - j);
                                        end++;
                                        j = end;
                                        for (int k = 1; k > 0; j++)
                                        {
                                            if (lines[i][j] == XSyntax.OpenSquareBracket)
                                                k++;
                                            else if (lines[i][j] == XSyntax.CloseSquareBracket)
                                                k--;
                                        }
                                        set_arrIndex = (int)ParseMathExpr(lines[i].Substring(end, j - end - 1), arrays);
                                    }
                                    else
                                    {
                                        set_arrName = all;
                                        set_arrIndex = 0;
                                        j = lines[i].IndexOf(XSyntax.PlacementEqual, j) - 1;
                                    }
                                    continue;
                                }
                            }
                        }
                    }
                    else if (lines[i].StartsWith(XSyntax.DeclareAndSetVarWord + " "))
                    {
                        for (int j = XSyntax.DeclareAndSetVarWord.Length + 1; j < lines[i].Length; j++)
                        {
                            if (set_arrName != "")
                            {
                                if (lines[i][j] == XSyntax.PlacementEqual)
                                {
                                    string[] values = Helper.SplitOutOfBrackets(lines[i].Substring(j + 1), XSyntax.Comma, XSyntax.OpenRoundBracket, XSyntax.CloseRoundBracket);
                                    arrays.Add(new XArray(set_arrName, values.Length));
                                    for (int k = 0; k < values.Length; k++)
                                        arrays[arrays.Count - 1].vars[k] = ParseMathExpr(values[k], arrays);
                                    break;
                                }
                            }
                            else
                            {
                                if (lines[i][j] == ' ')
                                    continue;
                                else
                                {
                                    string all = lines[i].Substring(j, lines[i].IndexOf(XSyntax.PlacementEqual, j) - j);
                                    all = all.Replace(" ", "");
                                    set_arrName = all;
                                    j = lines[i].IndexOf(XSyntax.PlacementEqual, j) - 1;
                                    continue;
                                }
                            }
                        }
                    }
                    else if (lines[i].StartsWith(XSyntax.DoWord + " "))
                    {
                        for (int j = XSyntax.DoWord.Length + 1; j < lines[i].Length; j++)
                        {
                            if (lines[i][j] == ' ')
                                continue;
                            else
                            {
                                string cmdName = lines[i].Substring(j, lines[i].IndexOf(' ', j) - j);
                                j = lines[i].IndexOf(' ', j) + 1;
                                string p = lines[i].Substring(j);
                                int k = -1;
                                for (int w = 0; w < commands.Count; w++)
                                {
                                    if (commands[w].Name == cmdName)
                                    {
                                        k = w;
                                        break;
                                    }
                                }
                                if (k == -1)
                                    throw new ParseException(Messages.CmdWasntFound);
                                arrays = commands[k].ProcessCommand(p, this, arrays);
                                break;
                            }
                        }
                    }
                    else if (lines[i].StartsWith(XSyntax.BeginWord + " ") || lines[i] == XSyntax.BeginWord)
                    {
                        for (int j = XSyntax.BeginWord.Length + 1; j < lines[i].Length; j++)
                        {
                            if (lines[i][j] == ' ')
                                continue;
                            else
                            {
                                for (int k = 0; k < processes.Count; k++)
                                {
                                    if (lines[i].Substring(j).StartsWith(processes[k].Name + " ") || lines[i].Substring(j).StartsWith(processes[k].Name + XSyntax.OpenRoundBracket))
                                    {
                                        j = lines[i].IndexOf(XSyntax.OpenRoundBracket, j) + 1;
                                        int tj = j;
                                        for (int w = 1; w > 0; j++)
                                        {
                                            if (lines[i][j] == XSyntax.OpenRoundBracket)
                                                w++;
                                            else if (lines[i][j] == XSyntax.CloseRoundBracket)
                                                w--;
                                        }
                                        j--;
                                        string con = lines[i].Substring(tj, j - tj);
                                        i++;
                                        string s = "";
                                        for (int w = 1; w > 0; i++)
                                        {
                                            if (lines[i].StartsWith(XSyntax.BeginWord + " ") || lines[i] == XSyntax.BeginWord)
                                                w++;
                                            else if (lines[i].StartsWith(XSyntax.EndWord + " ") || lines[i] == XSyntax.EndWord)
                                                w--;
                                            if (w == 0)
                                                break;
                                            s += lines[i] + Environment.NewLine;
                                        }
                                        arrays = processes[k].BeginProcess(s, arrays, this, con, codeLevel);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else if (lines[i].StartsWith(XSyntax.ReturnWord + " "))
                    {
                        for (int j = XSyntax.ReturnWord.Length + 1; j < lines[i].Length; j++)
                        {
                            if (lines[i][j] == ' ')
                                continue;
                            else
                            {
                                string var = lines[i].Substring(j);
                                string[] ar = Helper.SplitOutOfBrackets(var, XSyntax.Comma, XSyntax.OpenRoundBracket, XSyntax.CloseRoundBracket);
                                double[] param = new double[ar.Length];
                                for (int k = 0; k < ar.Length; k++)
                                    param[k] = ParseMathExpr(ar[k], arrays);
                                List<XArray> tempArr = new List<XArray>();
                                tempArr.Add(new XArray("x", param.Length));
                                tempArr[0].vars = param;
                                tempArr[0].level = -1;
                                if (codeLevel == 0)
                                    xmParser.mathFunctions.Clear();
                                return tempArr;
                            }
                        }
                    }
                }

                if (codeLevel == 0)
                    xmParser.mathFunctions.Clear();
                return arrays;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (codeLevel == 0)
                    xmParser.mathFunctions.Clear();
            }
        }

        /// <summary>
        /// Parses independant math expressions and returns the result
        /// </summary>
        /// <param name="expr">String of expression</param>
        /// <param name="arrays">List of predetermined arrays</param>
        /// <returns>Single number</returns>
        public double ParseMathExpr(string expr, List<XArray> arrays)
        {
            foreach (Match m in Regex.Matches(expr, XSyntax.OpenTriangleBracket + "(.+?)" + XSyntax.CloseTriangleBracket))
                expr = expr.Replace(XSyntax.OpenTriangleBracket + m.Groups[1].Value + XSyntax.CloseTriangleBracket, ParseMathExpr(m.Groups[1].Value, arrays).ToString());
            return xmParser.GetDouble(expr, 0, arrays);
        }

        /// <summary>
        /// Removes spaces/tabs from the beggining of each code line
        /// </summary>
        /// <param name="lines">Array of code lines</param>
        private void removeSpacesFromCode(string[] lines)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Replace(" ", "") == "")
                {
                    lines[i] = lines[i].Replace(" ", "");
                    continue;
                }
                if (lines[i].StartsWith(" "))
                {
                    for (int j = 1; j < lines[i].Length; j++)
                    {
                        if (lines[i][j] == ' ')
                            continue;
                        else
                        {
                            string s = lines[i].Substring(j);
                            lines[i] = s;
                            break;
                        }
                    }
                }
                while (lines[i].StartsWith(" ") || lines[i].StartsWith("\t"))
                {
                    string s = lines[i].Substring("\t".Length);
                    lines[i] = s;
                }
            }
        }
    }
}
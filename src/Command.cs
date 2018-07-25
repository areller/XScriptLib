using System;
using System.Collections.Generic;
using System.Text;

namespace XScriptLib
{
    public class Command
    {
        private string name;

        public string Name
        {
            get { return name; }
        }

        public Command(string Name)
        {
            name = Name;
        }

        public List<XArray> ProcessCommand(string t, XSParser parser, List<XArray> arrs)
        {
            string[] parameters = Helper.SplitOutOfBrackets(t, XSyntax.Comma, XSyntax.OpenRoundBracket, XSyntax.CloseRoundBracket);
            return OverrideCommand(parameters, parser, arrs);
        }

        protected virtual List<XArray> OverrideCommand(string[] parameters, XSParser parser, List<XArray> arrs)
        {
            return new List<XArray>();
        }
    }
}

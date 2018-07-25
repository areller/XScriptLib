using System;
using System.Collections.Generic;
using System.Text;

namespace XScriptLib
{
    class ParseException : Exception
    {
        public ParseException(string msg) : base("Parse Error(General): " + msg) { }
        public ParseException(string msg, string type) : base("Parse Error(" + type + "): " + msg) { }
    }
}

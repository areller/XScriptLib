using System;
using System.Collections.Generic;
using System.Text;

namespace XScriptLib.Exceptions
{
    class XMParseException : ParseException
    {
        public XMParseException(string msg) : base(msg, "Math") { }
    }
}

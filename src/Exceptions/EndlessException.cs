using System;
using System.Collections.Generic;
using System.Text;

namespace XScriptLib.Exceptions
{
    class EndlessException : ParseException
    {
        public EndlessException(string msg) : base(msg, "Endless") { }
    }
}

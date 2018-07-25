using System;
using System.Collections.Generic;
using System.Text;

namespace XScriptLib.Exceptions
{
    class XLogicException : ParseException
    {
        public XLogicException(string msg) : base(msg, "Logic") { }
    }
}

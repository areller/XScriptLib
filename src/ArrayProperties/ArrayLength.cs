using System;
using System.Collections.Generic;
using System.Text;

namespace XScriptLib.ArrayProperties
{
    class ArrayLength : ArrayProperty
    {
        public ArrayLength() : base(XSyntax.ArrayLength) { }

        protected override double OverrideProperty(XArray arr)
        {
            return arr.vars.Length;
        }
    }
}

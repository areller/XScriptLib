using System;
using System.Collections.Generic;
using System.Text;

namespace XScriptLib.ArrayProperties
{
    class ArrayLevel : ArrayProperty
    {
        public ArrayLevel() : base(XSyntax.ArrayLength) { }

        protected override double OverrideProperty(XArray arr)
        {
            return arr.level;
        }
    }
}

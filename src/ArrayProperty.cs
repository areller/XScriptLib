using System;
using System.Collections.Generic;

using System.Text;

namespace XScriptLib
{
    public class ArrayProperty
    {
        private string name;

        public string Name
        {
            get { return name; }
        }

        public ArrayProperty(string Name)
        {
            name = Name;
        }

        public double ProccessProperty(XArray arr, ref int index)
        {
            index += arr.name.Length + XSyntax.Arrow.Length + name.Length;
            return OverrideProperty(arr);
        }

        protected virtual double OverrideProperty(XArray arr)
        {
            return 0;
        }
    }
}

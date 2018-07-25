using System.Collections.Generic;

namespace XScriptLib
{
    /// <summary>
    /// Represents an array
    /// </summary>
    public class XArray
    {
        public string name;
        public double[] vars;
        public int level;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name">Name of the array</param>
        /// <param name="Length">Length of the array</param>
        public XArray(string Name, int Length)
        {
            name = Name;
            vars = new double[Length];
        }

        /// <summary>
        /// Connects two independant lists
        /// </summary>
        /// <param name="list1">First list</param>
        /// <param name="list2">Second list</param>
        /// <returns>The combined list</returns>
        public static List<XArray> ConnectLists(List<XArray> list1, List<XArray> list2)
        {
            List<XArray> x = new List<XArray>();
            for (int i = 0; i < list1.Count; i++)
                x.Add(list1[i]);
            for (int i = 0; i < list2.Count; i++)
                x.Add(list2[i]);
            return x;
        }
    }
}

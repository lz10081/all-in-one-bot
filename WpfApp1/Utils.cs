using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ZenAIO
{
    public class Utils
    {

        private Utils()
        {

        }

        /// <summary>
        /// Gets the file from the app's base directory.
        /// 
        /// Note: This gets called a lot, consider moving this to a static variable or seperate utility class.
        /// </summary>
        /// <param name="file"></param>
        /// <returns>string path</returns>
        public static string GetPath(string file)
        {
            return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file);
        }

        public static bool GetReplacement(ref string input, string regex, string sub, out string output)
        {
            bool result = true;
            try
            {
                output = Regex.Replace(input, regex, sub);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                output = input;
                result = false;
            }

            return result;
        }

        public class Pair<T1, T2>
        {
            public T1 item1;
            public T2 item2;

            public Pair(T1 item1 = default(T1), T2 item2 = default(T2))
            {
                this.item1 = item1;
                this.item2 = item2;
            }

            public static Pair<T1, T2> MakePair(T1 item1 = default(T1), T2 item2 = default(T2))
            {
                return new Pair<T1, T2>(item1, item2);
            }
        }

    }
}

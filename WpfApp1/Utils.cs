using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

    }
}

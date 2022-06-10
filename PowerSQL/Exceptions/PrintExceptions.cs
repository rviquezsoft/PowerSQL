using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace PowerSQL.Exceptions
{
    public class PrintExceptions : IPrintExceptions
    {
        public void printException(Exception ex)
        {
            Debug.WriteLine(
                   "method: " + MethodInfo.GetCurrentMethod().Name +
                   "\nexception: " + ex.ToString());
        }
    }
}

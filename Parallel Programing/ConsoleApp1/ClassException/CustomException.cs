using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppParallelPrograming.ClassException
{
    internal class CustomException :System.Exception
    {
        public CustomException() : base()
        {

        }
        public CustomException(string message) : base(message)
        {

        }
        public CustomException(string message, System.Exception innerException)
            : base(message, innerException)
        {

        }
    }
}

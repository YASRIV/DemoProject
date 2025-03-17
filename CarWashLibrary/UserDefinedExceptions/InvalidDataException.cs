using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Wash_Library.CustomException
{
   public  class InvalidDataInsertException : Exception
    {
        public InvalidDataInsertException(string message) : base(message) { }
    }
}

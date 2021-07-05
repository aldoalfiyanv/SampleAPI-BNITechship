using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleAPI.Helpers
{
    public class Greeter : IGreeter
    {
        public string GetMessage()
        {
            return "Greetings form ASP.NET Core Developer";
        }
    }
}

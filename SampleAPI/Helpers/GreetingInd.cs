using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleAPI.Helpers
{
    public class GreetingInd : IGreeter
    {
        public string GetMessage()
        {
            return "Salam dari ASP.NET Core Developer";
        }
    }
}

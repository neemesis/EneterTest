using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EneterTest.Common;

namespace EneterTest.Master {
    public class Talk : ITalk {
        void ITalk.Talk(string who, string sentence) => Console.WriteLine(who + ": " + sentence);
    }
}

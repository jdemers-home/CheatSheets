using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TddTests
{
    public interface IFoo
    {
        bool DoSomething(string str);
        string DoSomethingStringy(string str);
        bool TryParse(string str, out string outStr);
        bool Submit(ref Bar inst);
        int GetCount();
        int GetCountThing();
    }
}

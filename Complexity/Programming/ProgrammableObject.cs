using Complexity.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complexity.Programming {
    public interface ProgrammableObject {
        bool ContainsVariable(string name);
        bool ContainsFunction(string name);
        Variable GetVariable(string name);
        Function GetFunction(string name);
    }
}

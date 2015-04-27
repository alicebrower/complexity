using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complexity.Programming {
    public class Program {
        private List<Symbol> statements;
        private int pointer = 0;

        public Program(List<Symbol> statements) {
            this.statements = statements;
        }

        public Variable Run() {
            return Run(null);
        }

        public Variable Run(Variable[] args) {
            Stack<Symbol> tempStack = new Stack<Symbol>();
            Variable[] values;

            for(pointer = 0; pointer < statements.Count; pointer++) {
                if (!statements[pointer].isOperator) {
                    tempStack.Push(statements[pointer]);
                } else {
                    //Pop values, tempStack should contain only be 0 op SYMBOLS
                    values = new Variable[statements[pointer].argc];
                    for (int i = 0; i < values.Length; i++) {
                        values[i] = tempStack.Pop().eval(null);
                    }
                    Variable v = statements[pointer].eval(values);
                    tempStack.Push(new Symbol(false, 0, (a) => v));
                }
            }

            return tempStack.Pop().eval(null);
        }
    }
}

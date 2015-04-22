using Complexity.Managers;
using Complexity.Programming;
using Complexity.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Complexity.Math_Things {
    public class ExpressionF {
        private Program program;
        private string infix;

        public ExpressionF(string infix) {
            this.infix = infix;
            program = Compiler.Compile(infix);
        }

        public void SetInfix(string infix) {
            this.infix = infix;
            program = Compiler.Compile(infix);
        }

        /// <summary>
        /// Evaluates the expression
        /// </summary>
        /// <returns>The result of evaluation</returns>
        public float Evaluate() {
            return (float)program.Run().value;
        }

        public override string ToString() {
            return infix;
        }
    }
}

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
        private float value;
        private string infix;

        public ExpressionF(string infix) {
            this.infix = infix;
        }

        public void Set(string infix) {
            this.infix = infix;
        }

        public void Compile() {
            program = Compiler.Compile(infix);
        }

        /// <summary>
        /// Evaluates the expression
        /// </summary>
        /// <returns>The result of evaluation</returns>
        public void Evaluate() {
            value = (float)program.Run().Value();
        }

        public float Value() {
            return value;
        }

        public override string ToString() {
            return infix;
        }

        public ExpressionF Clone() {
            return (ExpressionF)MemberwiseClone();
        }
    }
}

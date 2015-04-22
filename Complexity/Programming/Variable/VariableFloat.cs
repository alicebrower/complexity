using Complexity.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complexity.Math_Things {
    public class VariableFloat {
        public readonly string name;
        public Eval eval;

        protected VariableFloat() { }

        /// <summary>
        /// This is used to create a dynamic variable who's value and/or existence 
        /// is not known until runtime
        /// </summary>
        /// <param name="name"></param>
        public VariableFloat(string name) {
            this.name = name;
        }

        /// <summary>
        /// Simple constructor for numbers only
        /// </summary>
        /// <param name="value"></param>
        public VariableFloat(float value) {
            name = "" + value;
            eval = (a) => value;
        }

        /// <summary>
        /// Convience constructor for creating variables with constant value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public VariableFloat(string name, float value) {
            this.name = name;
            eval = (a) => value;
        }

        /// <summary>
        /// General Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isOperator"></param>
        /// <param name="noOps"></param>
        /// <param name="assoc"></param>
        /// <param name="precedence"></param>
        /// <param name="eval"></param>
        public VariableFloat(string name, Eval eval) {
            this.name = name;
            this.eval = eval;
        }

        public delegate float Eval(float[] args);
    }
}
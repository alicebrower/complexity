using Complexity.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complexity.Math_Things {
    /// <summary>
    /// A vector of ExpressionDs.
    /// Maintains a VectorD of the most recently calculated values.
    /// </summary>
    public class VectorExpr : Recalculated {
        public VectorF values;
        private ExpressionF[] expressions;

        public VectorExpr(string[] exprStrings) {
            ArrayList _expressions = new ArrayList();
            foreach (string s in exprStrings) {
                _expressions.Add(new ExpressionF(s));
            }
            expressions = (ExpressionF[])_expressions.ToArray(typeof(ExpressionF));

            ExpressionManager.Add(this);
        }

        /// <summary>
        /// Recalculates the expression values and stores them in a VectorD
        /// </summary>
        public void Recalculate() {
            values = new VectorF(expressions.Length);
            for (int i = 0; i < expressions.Length; i++) {
                //values.At(i, expressions[i].Evaluate());
            }
        }

        /// <summary>
        /// Returns the value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public double ValueAt(int index) {
            return values.At(index);
        }

        /// <summary>
        /// Return all the values of this vector as a double[]
        /// </summary>
        /// <returns></returns>
        public float[] Values() {
            float[] result = new float[values.Count];
            for (int i = 0; i < values.Count; i++) {
                result[i] = values.At(i);
            }
            return result;
        }

        /// <summary>
        /// Sets the expression at the given index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="expr"></param>
        public void SetExprAt(int index, ExpressionF expr) {
            expressions[index] = expr;
        }

        public int Size() {
            return expressions.Length;
        }
    }
}

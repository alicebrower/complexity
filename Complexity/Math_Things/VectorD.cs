using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complexity.Math_Things {
    /// <summary>
    /// 
    /// </summary>
    public class VectorD : DenseVector {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        public VectorD(int size)
            : base(size) {

        }

        public VectorD(double[] data)
            : base(data) {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public new static VectorD OfArray(Double[] vec) {
            return new VectorD(vec);
        }
    }
}

using MathNet.Numerics.LinearAlgebra.Single;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complexity.Math_Things {
    /// <summary>
    /// 
    /// </summary>
    public class VectorF : DenseVector {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        public VectorF(int size)
            : base(size) {

        }

        public VectorF(float[] data)
            : base(data) {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public new static VectorF OfArray(float[] vec) {
            return new VectorF(vec);
        }
    }
}
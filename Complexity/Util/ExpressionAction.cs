using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complexity.Util {
    /// <summary>
    /// This functions like an action listener
    /// It is designed to perform a particular transformation on
    /// a Matrix/PointMatrix
    /// </summary>
    public abstract class MatrixTransformAction {
        public abstract MatrixD Transform(MatrixD matrix);
    }

    public class MatrixRotateAction : MatrixTransformAction {
        private VectorExpr vec;

        public MatrixRotateAction(VectorExpr vec) {
            this.vec = vec;
        }

        public override MatrixD Transform(MatrixD matrix) {
            vec.Recalculate();
            return matrix.Rotate(vec.Values);
        }
    }

    public class MatrixTranslateAction : MatrixTransformAction {

    }

    public class MatrixScaleAction : MatrixTransformAction {

    }
}

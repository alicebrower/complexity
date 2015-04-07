using Complexity.Math_Things;
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
        public abstract MatrixF Transform(MatrixF matrix);
    }

    public class MatrixRotateAction : MatrixTransformAction {
        private VectorExpr vec;

        public MatrixRotateAction(VectorExpr vec) {
            this.vec = vec;
        }

        public override MatrixF Transform(MatrixF matrix) {
            vec.Recalculate();
            matrix.Rotate(vec.Values());
            return matrix;
        }
    }

    public class MatrixTranslateAction : MatrixTransformAction {
        private VectorExpr vec;

        public MatrixTranslateAction(VectorExpr vec) {
            this.vec = vec;
        }

        public override MatrixF Transform(MatrixF matrix) {
            vec.Recalculate();
            matrix.Translate(vec.Values());
            return matrix;
        }
    }

    public class MatrixTranslatePoint3Action : MatrixTransformAction {
        Point3 point;

        public MatrixTranslatePoint3Action(Point3 point) {
            this.point = point;
        }

        public override MatrixF Transform(MatrixF matrix) {
            matrix.Translate(point.x, point.y, point.z);
            return matrix;
        }
    }

    public class MatrixScaleAction : MatrixTransformAction {
        private VectorExpr vec;

        public MatrixScaleAction(VectorExpr vec) {
            this.vec = vec;
        }

        public override MatrixF Transform(MatrixF matrix) {
            vec.Recalculate();
            matrix.Scale(vec.Values());
            return matrix;
        }
    }
}

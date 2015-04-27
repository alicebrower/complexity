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
        public abstract void Transform(PointMatrix matrix);
    }

    public class MatrixRotateAction : MatrixTransformAction {
        private ExpressionF x, y, z;

        public MatrixRotateAction(ExpressionF x, ExpressionF y, ExpressionF z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override void Transform(PointMatrix matrix) {
            matrix.Rotate(x.Value(), y.Value(), z.Value());
        }
    }

    public class MatrixTranslateAction : MatrixTransformAction {
        private ExpressionF x, y, z;

        public MatrixTranslateAction(ExpressionF x, ExpressionF y, ExpressionF z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override void Transform(PointMatrix matrix) {
            matrix.Translate(x.Value(), y.Value(), z.Value());
        }
    }

    public class MatrixTranslatePoint3Action : MatrixTransformAction {
        Point3 point;

        public MatrixTranslatePoint3Action(Point3 point) {
            this.point = point;
        }

        public override void Transform(PointMatrix matrix) {
            matrix.Translate(point.x, point.y, point.z);
        }
    }

    public class MatrixScaleAction : MatrixTransformAction {
        private ExpressionF x, y, z;

        public MatrixScaleAction(ExpressionF x, ExpressionF y, ExpressionF z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override void Transform(PointMatrix matrix) {
            matrix.Scale(x.Value(), y.Value(), z.Value());
        }
    }
}

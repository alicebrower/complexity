using Complexity.Math_Things;
using MathNet.Numerics.LinearAlgebra.Single;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complexity.Util {
    /// <summary>
    /// This class is a special kind of matrix. It behaves as a matrix in
    /// that you can perform all the matrix calculations on it. However,
    /// it also keeps track of other properties 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PointMatrix : DenseMatrix, IEnumerable, IEnumerator {
        public readonly Geometry originalGeo;
        private int position = -1;

        public PointMatrix(Geometry data)
            : base(data.Rows(), 3) {
            SetColumn(0, data.GetColumn(0));
            SetColumn(1, data.GetColumn(1));
            SetColumn(2, data.GetColumn(2));
            originalGeo = data;
        }

        public Point3 Get(int index) {
            return new Point3(At(index, 0), At(index, 1), At(index, 2));
        }

        public int Count() {
            return RowCount;
        }

        public void Set(int index, Point3 point) {
            SetColumn(index, point.AsArray());
        }

        /// <summary>
        /// Sets the points array
        /// </summary>
        /// <param name="points"></param>
        public void SetFromArray(Point3[] points) {
            throw new NotImplementedException();
            if (points.Length != ColumnCount) {
                throw new Exception("Invalid array length");
            }
            //this.points = new List<Point3>(points);
        }

        public void SetFromArrayList(List<Point3> points) {
            throw new NotImplementedException();
            //this.points = points;
        }

        public void Scale(float x, float y, float z) {
            if (x != 1) { SetColumn(0, Column(0).Multiply(x)); }
            if (y != 1) { SetColumn(1, Column(1).Multiply(y)); }
            if (z != 1) { SetColumn(2, Column(2).Multiply(z)); }
        }

        public void Translate(float x, float y, float z) {
            if (x != 0) { SetColumn(0, Column(0).Add(x)); }
            if (y != 0) { SetColumn(1, Column(1).Add(y)); }
            if (z != 0) { SetColumn(2, Column(2).Add(z)); }
        }

        public void Rotate(float x, float y, float z) {
            if (x != 0) { DoMultiply(MathUtil.RotationalMatrixX(x), this); }
            if (y != 0) { DoMultiply(MathUtil.RotationalMatrixY(y), this); }
            if (z != 0) { DoMultiply(MathUtil.RotationalMatrixZ(z), this); }
        }

        /// <summary>
        /// Restores the PointMatrix to it's original configuration
        /// </summary>
        public void Restore() {
            SetColumn(0, originalGeo.GetColumn(0));
            SetColumn(1, originalGeo.GetColumn(1));
            SetColumn(2, originalGeo.GetColumn(2));
        }

        #region Enumerator Stuff

        public bool MoveNext() {
            position++;
            if (position < RowCount) {
                return true;
            }
            Reset();
            return false;
        }

        public void Reset() {
            position = -1;
        }

        object IEnumerator.Current {
            get {
                return new Point3(this[position, 0], this[position, 1], this[position, 2]);
            }
        }

        public IEnumerator GetEnumerator() {
            return (IEnumerator)this;
        }

        #endregion
    }
}

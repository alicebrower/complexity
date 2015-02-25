using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Symbolics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complexity.Math_Things {
    /// <summary>
    /// This is a wrapper class
    /// </summary>
    public class MatrixD : DenseMatrix {
        #region Static

        /// <summary>
        /// Create a matrix to rotate a geometry of 3D points about the x axis
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private static Matrix<double> RotX(double x) {
            return DenseMatrix.OfArray(new Double[,] {
                {1, 0, 0},
                {0, Math.Cos(x), -Math.Sin(x)},
                {0, Math.Sin(x), Math.Cos(x)}
            });
        }

        /// <summary>
        /// Create a matrix to rotate a geometry of 3D points about the y axis
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        private static Matrix<double> RotY(double y) {
            return DenseMatrix.OfArray(new Double[,] {
                {Math.Cos(y), 0, Math.Sin(y)},
                {0, 1, 0},
                {-Math.Sin(y), 0, Math.Cos(y)}
            });
        }

        /// <summary>
        /// Create a matrix to rotate a geometry of 3D points about the z axis
        /// </summary>
        /// <param name="z"></param>
        /// <returns></returns>
        private static Matrix<double> RotZ(double z) {
            return DenseMatrix.OfArray(new Double[,] {
                {Math.Cos(z), -Math.Sin(z), 0},
                {Math.Sin(z), Math.Cos(z), 0},
                {0, 0, 1}
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public new static MatrixD OfArray(double[,] data) {
            Matrix<double> _data = DenseMatrix.OfArray(data);
            return new MatrixD(_data.RowCount, _data.ColumnCount, _data.ToColumnWiseArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="A"></param>
        /// <returns></returns>
        public static MatrixD RotateMatrix(double x, double y, double z, MatrixD A) {
            MatrixD result = ConvertMatrix((DenseMatrix)(RotX(x) * RotY(y) * RotZ(z) * A));
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="A"></param>
        /// <returns></returns>
        public static MatrixD RotateMatrix(VectorD vec, MatrixD A) {
            return RotateMatrix(vec.At(0), vec.At(1), vec.At(2), A);
        }

        /// <summary>
        /// Returns the result of multiplying matrix A by scale
        /// </summary>
        /// <param name="scale"></param>
        /// <param name="A"></param>
        /// <returns></returns>
        [Obsolete("This method applies the same scale to all dimensions.")]
        public static MatrixD ScaleMatrix(double scale, MatrixD A) {
            Matrix<double> result = DenseMatrix.OfArray(A.ToArray());
            result *= scale;
            return MatrixD.OfArray(result.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="A"></param>
        /// <returns></returns>
        public static MatrixD TranslateMatrix(double x, double y, double z, MatrixD A) {
            //Prepare translation matrix
            Matrix<double> trans = Matrix<double>.Build.Dense(A.RowCount, A.ColumnCount, 0);
            trans.SetRow(0, Vector<double>.Build.Dense(A.ColumnCount, x));
            trans.SetRow(1, Vector<double>.Build.Dense(A.ColumnCount, y));
            trans.SetRow(2, Vector<double>.Build.Dense(A.ColumnCount, z));

            return ConvertMatrix((DenseMatrix)trans + A);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="A"></param>
        /// <returns></returns>
        public static MatrixD TranslateMatrix(VectorD vec, MatrixD A) {
            Matrix<double> trans = Matrix<double>.Build.Dense(A.RowCount, A.ColumnCount, 0);
            for (int i = 0; i < vec.Count; i++) {
                trans.SetRow(i, Vector<double>.Build.Dense(A.ColumnCount, vec.At(i)));
            }
            return ConvertMatrix((DenseMatrix)trans + A);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static MatrixD Add(MatrixD A, MatrixD B) {
            return (MatrixD)(((Matrix<double>)A) + ((Matrix<double>)B));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private static MatrixD ConvertMatrix(DenseMatrix d) {
            return new MatrixD(d.RowCount, d.ColumnCount, d.ToColumnWiseArray());
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <param name="c"></param>
        public MatrixD(int rows, int columns)
            : base(rows, columns) {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        /// <param name="data"></param>
        public MatrixD(int rows, int columns, double[] data)
            : base(rows, columns, data) {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public new MatrixD Transpose() {
            Matrix<double> _this = ((Matrix<double>)this).Transpose();
            return new MatrixD(_this.RowCount, _this.ColumnCount, _this.ToColumnWiseArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public new VectorD Column(int index) {
            return new VectorD(base.Column(index).ToArray());
        }

        /// <summary>
        /// Sets the all the values in a row to a number
        /// </summary>
        /// <param name="row"></param>
        /// <param name="num"></param>
        public void SetRow(int row, double num) {
            base.SetRow(row, Vector<double>.Build.Dense(ColumnCount, num));
        }

        #region Calculations

        /// <summary>
        /// Returns the result of multiplying matrix A by scale
        /// </summary>
        /// <param name="scale"></param>
        /// <param name="A"></param>
        /// <returns></returns>
        public void Scale(double x, double y, double z) {
            SetRow(0, (Row(0) * x).ToArray());
            SetRow(1, (Row(1) * y).ToArray());
            SetRow(2, (Row(2) * z).ToArray());
        }

        public void Rotate(double x, double y, double z) {
            SetSubMatrix(0, 0, (DenseMatrix)(RotX(x) * RotY(y) * RotZ(z) * this));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="A"></param>
        /// <returns></returns>
        public void Translate(double x, double y, double z) {
            //Prepare translation matrix
            Matrix<double> trans = Matrix<double>.Build.Dense(RowCount, ColumnCount, 0);
            trans.SetRow(0, Vector<double>.Build.Dense(ColumnCount, x));
            trans.SetRow(1, Vector<double>.Build.Dense(ColumnCount, y));
            trans.SetRow(2, Vector<double>.Build.Dense(ColumnCount, z));

            SetSubMatrix(0, 0, trans + this);
        }

        public void Scale(double[] values) {
            Scale(values[0], values[1], values[2]);
        }

        public void Rotate(double[] values) {
            if (values.Length != 3) {
                throw new ArgumentException("MatrixD.Rotate : Must provide an array of length 3 for rotation.");
            }

            Rotate(values[0], values[1], values[2]);
        }

        public void Translate(double[] values) {
            if (values.Length != 3) {
                throw new ArgumentException("MatrixD.Rotate : Must provide an array of length 3 for rotation.");
            }

            Translate(values[0], values[1], values[2]);
        }

        public void Scale(VectorD scale) {
            Scale(scale.At(0), scale.At(1), scale.At(2));
        }

        public void Rotate(VectorD rot) {
            Rotate(rot.At(0), rot.At(1), rot.At(2));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="A"></param>
        /// <returns></returns>
        public void Translate(VectorD vec) {
            Matrix<double> trans = Matrix<double>.Build.Dense(RowCount, ColumnCount, 0);
            for (int i = 0; i < vec.Count; i++) {
                trans.SetRow(i, Vector<double>.Build.Dense(ColumnCount, vec.At(i)));
            }
            SetSubMatrix(0, 0, trans + this);
        }

        #endregion
    }
}

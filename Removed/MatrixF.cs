using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Single;
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
    public class MatrixF : DenseMatrix {
        #region Static

        /// <summary>
        /// Create a matrix to rotate a geometry of 3D points about the x axis
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private static Matrix<float> RotX(float x) {
            return DenseMatrix.OfArray(new float[,] {
                {1, 0, 0},
                {0, (float)Math.Cos(x), (float)-Math.Sin(x)},
                {0, (float)Math.Sin(x), (float)Math.Cos(x)}
            });
        }

        /// <summary>
        /// Create a matrix to rotate a geometry of 3D points about the y axis
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        private static Matrix<float> RotY(float y) {
            return DenseMatrix.OfArray(new float[,] {
                {(float)Math.Cos(y), 0, (float)Math.Sin(y)},
                {0, 1, 0},
                {(float)-Math.Sin(y), 0, (float)Math.Cos(y)}
            });
        }

        /// <summary>
        /// Create a matrix to rotate a geometry of 3D points about the z axis
        /// </summary>
        /// <param name="z"></param>
        /// <returns></returns>
        private static Matrix<float> RotZ(float z) {
            return DenseMatrix.OfArray(new float[,] {
                {(float)Math.Cos(z), (float)-Math.Sin(z), 0},
                {(float)Math.Sin(z), (float)Math.Cos(z), 0},
                {0, 0, 1}
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public new static MatrixF OfArray(float[,] data) {
            Matrix<float> _data = DenseMatrix.OfArray(data);
            return new MatrixF(_data.RowCount, _data.ColumnCount, _data.ToColumnWiseArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="A"></param>
        /// <returns></returns>
        public static MatrixF RotateMatrix(float x, float y, float z, MatrixF A) {
            MatrixF result = ConvertMatrix((DenseMatrix)(RotX(x) * RotY(y) * RotZ(z) * A));
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="A"></param>
        /// <returns></returns>
        public static MatrixF RotateMatrix(VectorF vec, MatrixF A) {
            return RotateMatrix(vec.At(0), vec.At(1), vec.At(2), A);
        }

        /// <summary>
        /// Returns the result of multiplying matrix A by scale
        /// </summary>
        /// <param name="scale"></param>
        /// <param name="A"></param>
        /// <returns></returns>
        public static MatrixF ScaleMatrix(float scale, MatrixF A) {
            Matrix<float> result = DenseMatrix.OfArray(A.ToArray());
            result *= scale;
            return MatrixF.OfArray(result.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="A"></param>
        /// <returns></returns>
        public static MatrixF TranslateMatrix(float x, float y, float z, MatrixF A) {
            //Prepare translation matrix
            Matrix<float> trans = Matrix<float>.Build.Dense(A.RowCount, A.ColumnCount, 0);
            trans.SetRow(0, Vector<float>.Build.Dense(A.ColumnCount, x));
            trans.SetRow(1, Vector<float>.Build.Dense(A.ColumnCount, y));
            trans.SetRow(2, Vector<float>.Build.Dense(A.ColumnCount, z));

            return ConvertMatrix((DenseMatrix)trans + A);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="A"></param>
        /// <returns></returns>
        public static MatrixF TranslateMatrix(VectorF vec, MatrixF A) {
            Matrix<float> trans = Matrix<float>.Build.Dense(A.RowCount, A.ColumnCount, 0);
            for (int i = 0; i < vec.Count; i++) {
                trans.SetRow(i, Vector<float>.Build.Dense(A.ColumnCount, vec.At(i)));
            }
            return ConvertMatrix((DenseMatrix)trans + A);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static MatrixF Add(MatrixF A, MatrixF B) {
            return (MatrixF)(((Matrix<float>)A) + ((Matrix<float>)B));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private static MatrixF ConvertMatrix(DenseMatrix d) {
            return new MatrixF(d.RowCount, d.ColumnCount, d.ToColumnWiseArray());
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <param name="c"></param>
        public MatrixF(int rows, int columns)
            : base(rows, columns) {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        /// <param name="data"></param>
        public MatrixF(int rows, int columns, float[] data)
            : base(rows, columns, data) {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public new MatrixF Transpose() {
            Matrix<float> _this = ((Matrix<float>)this).Transpose();
            return new MatrixF(_this.RowCount, _this.ColumnCount, _this.ToColumnWiseArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public new VectorF Column(int index) {
            return new VectorF(base.Column(index).ToArray());
        }

        /// <summary>
        /// Sets the all the values in a row to a number
        /// </summary>
        /// <param name="row"></param>
        /// <param name="num"></param>
        public void SetRow(int row, float num) {
            base.SetRow(row, Vector<float>.Build.Dense(ColumnCount, num));
        }

        #region Calculations

        public void Rotate(float x, float y, float z) {
            SetSubMatrix(0, 0, (DenseMatrix)(RotX(x) * RotY(y) * RotZ(z) * this));
        }

        public void Rotate(float[] values) {
            if (values.Length != 3) {
                throw new ArgumentException("MatrixF.Rotate : Must provide an array of length 3 for rotation.");
            }

            Rotate(values[0], values[1], values[2]);
        }

        public void Translate(float[] values) {
            if (values.Length != 3) {
                throw new ArgumentException("MatrixF.Rotate : Must provide an array of length 3 for rotation.");
            }

            //Translate(values[0], values[1], values[2]);
        }

        public void Rotate(VectorF rot) {
            Rotate(rot.At(0), rot.At(1), rot.At(2));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="A"></param>
        /// <returns></returns>
        public void Translate(VectorF vec) {
            Matrix<float> trans = Matrix<float>.Build.Dense(RowCount, ColumnCount, 0);
            for (int i = 0; i < vec.Count; i++) {
                trans.SetRow(i, Vector<float>.Build.Dense(ColumnCount, vec.At(i)));
            }
            SetSubMatrix(0, 0, trans + this);
        }

        #endregion
    }
}

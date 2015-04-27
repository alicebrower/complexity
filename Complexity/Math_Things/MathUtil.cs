using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Single;
using MathNet.Symbolics;
using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Complexity.Math_Things;
using Complexity.Managers;

namespace Complexity.Util {
    /// <summary>
    /// Utility math methods
    /// </summary>
    public static class MathUtil {
        /// <summary>
        /// Create a matrix to rotate a geometry of 3D points about the x axis
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Matrix<float> RotationalMatrixX(float x) {
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
        public static Matrix<float> RotationalMatrixY(float y) {
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
        public static Matrix<float> RotationalMatrixZ(float z) {
            return DenseMatrix.OfArray(new float[,] {
                {(float)Math.Cos(z), (float)-Math.Sin(z), 0},
                {(float)Math.Sin(z), (float)Math.Cos(z), 0},
                {0, 0, 1}
            });
        }

        public static double Distance3(Point3 a, Point3 b) {
            return Math.Sqrt(Math.Pow((a.x - b.x), 2)
                + Math.Pow((a.y - b.y), 2)
                + Math.Pow((a.z - b.z), 2));
        }

        public static double Distance3(double x1, double x2, double y1, double y2, double z1, double z2) {
            return Math.Sqrt(Math.Pow((x2 - x1), 2)
                + Math.Pow((y2 - y1), 2)
                + Math.Pow((z2 - z1), 2));
        }

        public static double[] ToColumnWiseArray(double[,] array) {
            double[] result = new double[array.GetLength(0) * array.GetLength(1)];
            for (int i = 0; i < array.GetLength(1); i++) {
                for (int j = 0; j < array.GetLength(0); j++) {
                    result[i * array.GetLength(0) + j] = array[j, i];
                }
            }
            return result;
        }

        public static float[] ToColumnWiseArray(float[,] array) {
            float[] result = new float[array.GetLength(0) * array.GetLength(1)];
            for (int i = 0; i < array.GetLength(1); i++) {
                for (int j = 0; j < array.GetLength(0); j++) {
                    result[i * array.GetLength(0) + j] = array[j, i];
                }
            }
            return result;
        }

        public static string[] FloatToString(float[] values) {
            string[] result = new string[values.Length];
            for (int i = 0; i < result.Length; i++) {
                result[i] = values[i].ToString();
            }

            return result;
        }

        public static float[] DoubleToFloat(double[] dubs) {
            float[] floats = new float[dubs.Length];
            for (int i = 0; i < floats.Length; i++) {
                floats[i] = (float)dubs[i];
            }

            return floats;
        }

        public static double RandomDouble(double seed) {
            string s = (seed + "").Replace(".", "");
            return (new Random(int.Parse(s))).NextDouble();
        }

        public static double RandomFloat(float seed) {
            string s = (seed + "").Replace(".", "");
            return (float)(new Random(int.Parse(s))).NextDouble();
        }
    }
}

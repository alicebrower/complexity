using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
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

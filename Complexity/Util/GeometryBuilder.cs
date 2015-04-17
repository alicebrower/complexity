using Complexity.Managers;
using Complexity.Math_Things;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complexity.Util {
    public static class GeometryBuilder {
        public static string POLAR_THETA = ResourceManager.GetRandomVarName();
        /// <summary>
        /// Creates a geometry that represents a circle. Z values are set to 0.
        /// </summary>
        /// <param name="points">The number of points to be calculated.
        /// Points are evenly spaced.</param>
        /// <returns></returns>
        public static float[,] Circle(int points) {
            float[,] result = new float[3, points];
            float t;

            for (int i = 0; i < points; i++) {
                t = ((float)i) / ((float)points);
                result[0, i] = (float)(Math.Sin(t * Math.PI * 2) / 2.0);
                result[1, i] = (float)(Math.Cos(t * Math.PI * 2) / 2.0);
                result[2, i] = 0;
            }

            return result;
        }

        public static float[,] Cube() {
            return new float[,] {
                {-0.5f, 0.5f,  0.5f, -0.5f, -0.5f,  0.5f,  0.5f, -0.5f},
                { 0.5f, 0.5f, -0.5f, -0.5f,  0.5f,  0.5f, -0.5f, -0.5f},
                { 0.5f, 0.5f,  0.5f,  0.5f, -0.5f, -0.5f, -0.5f, -0.5f}
            };
        }

        /// <summary>
        /// Returns an array of points representing a graph in polar coordinates
        /// </summary>
        /// <param name="expression">What to graph</param>
        /// <param name="start">Theta value to begin at</param>
        /// <param name="stop">Theta value to end at</param>
        /// <param name="step">Theta resolution</param>
        /// <returns></returns>
        public static float[,] GraphPolar(ExpressionF expression, double start, double stop, double step) {
            if (start >= stop || step <= 0) {
                throw new Exception("Cannot graph polar, invalid arguments");
            }

            float[,] result;
            ArrayList points = new ArrayList();

            //Perform calculations
            expression.AddSymbol(POLAR_THETA, 0);
            int index = 0;
            float theta;
            double x, y, bigX, bigY, scale;
            bigX = bigY = 1.0;
            for (double i = start; i < stop; i += step) {
                expression.SetSymbol(POLAR_THETA, (float)(i * Math.PI / 180.0));
                theta = (float)(i * Math.PI / 180.0);

                x = expression.Evaluate() * Math.Sin(theta);
                y = expression.Evaluate() * Math.Cos(theta);
                bigX = (bigX > x) ? bigX : x;
                bigY = (bigY > y) ? bigY : y;
                points.Add(new Point3(
                    x,
                    y,
                    0
                ));

                index++;
            }
            expression.RemoveSymbol(POLAR_THETA);

            //Scale
            scale = (bigX > bigY) ? 1.0 / bigX : 1.0 / bigY;
            foreach (Point3 point in points) {
                point.x *= (float)(scale);
                point.y *= (float)(scale);
            }

            //Convert
            result = new float[3, points.Count];
            int count = 0;
            foreach (Point3 p in points) {
                result[0, count] = p.x;
                result[1, count] = p.y;
                result[2, count] = p.z;
                count++;
            }

            return result;
        }

        /// <summary>
        /// Creates a grid with x rows and y columns with the origin at the center
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static float[,] Grid(int x, int y) {
            float[,] result = new float[3, x * y];
            //ArrayList points = new ArrayList();

            double xOff = (((double)x - 1) / 2.0);
            double yOff = (((double)y - 1) / 2.0);
            for (int j = 0; j < y; j++) {
                for (int i = 0; i < x; i++) {
                    result[0, i + j * x] = (float)((i - xOff) / (double)x * 2.0);
                    result[1, i + j * x] = (float)((j - yOff) / (double)y * 2.0);
                    result[2, i + j * x] = 0;
                }
            }

            return result;
        }
    }
}

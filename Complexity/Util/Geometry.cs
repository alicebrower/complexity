using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complexity.Util {
    /// <summary>
    /// A convenience class for creating geometries. Forces the user to have an array
    /// with the correct dimentions.
    /// </summary>
    public class Geometry {
        private float[,] geometry;

        public Geometry(int noPoints) {
            geometry = new float[noPoints, 3];
        }

        public Geometry(float[,] geometry) {
            if (geometry.GetLength(1) != 3) {
                throw new Exception("Invalid number of columns, must be 3");
            }

            this.geometry = geometry;
        }

        public float this[int row, int column] {
            get { return geometry[row, column]; }
            set { geometry[row, column] = value; }
        }

        public float[,] GetGeometry() {
            return geometry;
        }

        public int Rows() {
            return geometry.GetLength(0);
        }

        public float[] GetColumn(int column) {
            if (column >= 3) {
                throw new Exception("Column index must be < 3");
            }

            float[] result = new float[geometry.GetLength(0)];
            for (int i = 0; i < geometry.GetLength(0); i++) {
                result[i] = geometry[i, column];
            }

            return result;
        }

        public Geometry Clone() {
            float[,] newGeometry = new float[geometry.GetLength(0), geometry.GetLength(1)];
            for (int i = 0; i < Rows(); i++) {
                newGeometry[i, 0] = geometry[i, 0];
                newGeometry[i, 1] = geometry[i, 1];
                newGeometry[i, 2] = geometry[i, 2];
            }

            return new Geometry(newGeometry);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complexity.Util {
    /// <summary>
    /// Represents a point in 3 dimensional space.
    /// </summary>
    public class Point3 {
        public float x, y, z;

        public Point3(float x, float y, float z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Point3(double x, double y, double z) {
            this.x = (float)x;
            this.y = (float)y;
            this.z = (float)z;
        }

        public float[] AsArray() {
            return new float[] { x, y, z};
        }

        public void SetFromArray(float[] points) {
            if (points.Length != 3) {
                throw new Exception("Invalid array dimensions");
            }
            x = points[0];
            y = points[1];
            z = points[2];
        }

        public virtual Point3 Clone() {
            return new Point3(x, y, z);
        }
    }
}

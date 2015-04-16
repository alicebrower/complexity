using Complexity.Interfaces;
using Complexity.Managers;
using Complexity.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complexity.Objects.Vertex {
    public class VertexSystem : Point3, Recalculatable {
        public Object3 obj;
        public float distance;

        public VertexSystem(float x, float y, float z)
            : base(x, y, z) { }

        public void Recalculate() {
            ResourceManager.ModifyExprVal(System3.DIST, distance);
            obj.Recalculate();
        }
    }
}

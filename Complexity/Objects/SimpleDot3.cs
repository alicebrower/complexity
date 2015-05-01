using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Complexity.Util;
using System.Collections;

namespace Complexity.Objects {
    public class SimpleDot3 : Object3 {

        public SimpleDot3(int resolution)
            : base(GeometryBuilder.Circle(resolution)) {
        }

        public override void Draw() {
            GL.Begin(BeginMode.TriangleFan);

            //GL.Color4(GetColor());
            
            foreach (Point3 p in vertecies) {
                GL.Vertex3(p.x, p.y, p.z);
            }

            GL.End();
        }

        public override void Recalculate() {
            base.Recalculate();

            //GetAttribute("color").value.Recalculate();
            //color = GetAttribute("color").value.Values();
        }

        public override bool HasChildren() {
            return false;
        }

        public override List<Object3> GetChildren() {
            return null;
        }
    }
}

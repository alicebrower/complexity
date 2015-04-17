using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Complexity.Util;
using Complexity.Objects.Base;

namespace Complexity.Objects {
    /// <summary>
    /// 
    /// </summary>
    public class Cube : Object3 {
        protected byte[] triangles;

        public Cube() : base(GeometryBuilder.Cube()) {
            triangles = new byte[] {
                1, 0, 2, // front
			    3, 2, 0,
			    6, 4, 5, // back
			    4, 6, 7,
			    4, 7, 0, // left
			    7, 3, 0,
			    1, 2, 5, //right
			    2, 6, 5,
			    0, 1, 5, // top
			    0, 5, 4,
			    2, 3, 6, // bottom
			    3, 7, 6
            };
        }

        public override void Draw() {
            float[] col = GetColor();
            float[] cubeColors = {
			    col[0], col[1], col[2], col[3],
			    col[0], col[1], col[2], col[3],
			    col[0], col[1], col[2], col[3],
			    col[0], col[1], col[2], col[3],
			    col[0], col[1], col[2], col[3],
			    col[0], col[1], col[2], col[3],
			    col[0], col[1], col[2], col[3],
			    col[0], col[1], col[2], col[3]
		    };

                GL.VertexPointer(3, VertexPointerType.Float, 0, vertecies.ToColumnWiseArray());
            GL.ColorPointer(4, ColorPointerType.Float, 0, cubeColors);
            GL.DrawElements(BeginMode.Triangles, 36, DrawElementsType.UnsignedByte, triangles);
        }
    }
}

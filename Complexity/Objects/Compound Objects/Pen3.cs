using Complexity.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Complexity.Math_Things;

namespace Complexity.Objects {
    /// <summary>
    /// 
    /// </summary>
    public class Pen3 : System3 {
        /*
         * maxDist = maximum allowed distance between dots
         * speed = speed at which the pen is drawn, < 0 for instant
         */
        private float maxDist = 0.02f;
        private float speed = 1;
        private float length = 0;

        //Pen verticies have a few more properties than a regular vertex
        protected class PenVertex : SysVertex {
            public PenVertex(float x, float y, float z)
                : base(x, y, z) {
            }
            public PenVertex(SysVertex v) : base(v.x, v.y, v.z) {
            }

            public PenVertex(Point3 point) : base(point.x, point.y, point.z) {
            }
            //A unit direction vector representing the slope of the line at the point
            public float[] slope;

            public PenVertex Clone() {
                return (PenVertex)MemberwiseClone();
            }
        }

        public Pen3(double[,] geometry) : base(geometry, new SimpleDot3(30)) {
        }

        public override void SetAttributes_OLD(Dictionary<string, string> args) {
            base.SetAttributes_OLD(args);

            if (args.ContainsKey("speed")) {
                try {
                    speed = float.Parse(args["speed"]);
                } catch (Exception ex) {
                    Console.WriteLine("Pen.SetAttributes : Invalid speed");
                }
            }
        }

        protected void SetVariables(PenVertex vert) {
            ExpressionD.SetSymbolValue("dist", vert.distance);
            ExpressionD.SetSymbolValue("xslope", vert.slope[0]);
            ExpressionD.SetSymbolValue("yslope", vert.slope[1]);
            ExpressionD.SetSymbolValue("zslope", vert.slope[2]);
        }

        protected override void ReserveVariables() {
            ExpressionD.AddSymbol("dist", 0);
            ExpressionD.AddSymbol("length", length);
            ExpressionD.AddSymbol("xslope", 0);
            ExpressionD.AddSymbol("yslope", 0);
            ExpressionD.AddSymbol("zslope", 0);
        }

        protected override void ReleaseVariables() {
            ExpressionD.RemoveSymbol("dist");
            ExpressionD.RemoveSymbol("length");
            ExpressionD.RemoveSymbol("xslope");
            ExpressionD.RemoveSymbol("yslope");
            ExpressionD.RemoveSymbol("zslope");
        }

        public override void Draw() {
            foreach (PenVertex penVert in vertecies) {
                double dist = speed * ExpressionD.GetSymbolValue("time");
                double sdfs = penVert.distance;
                if (speed >= 0) {
                    if (penVert.distance < dist) {
                        penVert.obj.Draw();
                    } else {
                        vertecies.Reset();
                        break;
                    }
                } else {
                    penVert.obj.Draw();
                }
            }
        }

        protected override void SetMasterObj(Object3 obj) {
            base.SetMasterObj(new SimpleDot3(3));
            //masterObj = dot;
        }

        /// <summary>
        /// Fills in the geometry array with points to create a smooth effect.
        /// Calculates distance and slope at each point.
        /// </summary>
        /// <param name="geometry"></param>
        /// <param name="rmaxDist">The maximum distance objects are allowed to be from each other</param>
        protected override PointMatrix ConvertGeometry(double[,] g) {
            //Create the temp points array
            TypedArrayList<Point3> _points = new TypedArrayList<Point3>();
            double dist, _dist, xdist, ydist, zdist;
            float[] slope = new float[] { 0f, 0f, 0f };
            int noPoints;

            dist = 0;
            for (int i = 1; i < g.GetLength(1); i++) {
                slope = new float[] { 
                    (float)(g[0, i] / g[0, i - 1]),
                    (float)(g[1, i] / g[1, i - 1]),
                    (float)(g[2, i] / g[2, i - 1])};

                //Determine relevant values
                dist = MathUtil.Distance3(g[0, i - 1], g[0, i], g[1, i - 1], g[1, i], g[2, i - 1], g[2, i]);
                noPoints = (int)Math.Floor(dist / maxDist);

                //fill the list
                if (dist > maxDist) {
                    xdist = (g[0, i] - g[0, i - 1]) / noPoints;
                    ydist = (g[1, i] - g[1, i - 1]) / noPoints;
                    zdist = (g[2, i] - g[2, i - 1]) / noPoints;

                    //Insert intermediary points
                    for (double d = 0; d < noPoints; d += 1) {
                        _dist = (_points.Count() > 0) ? ((PenVertex)_points.Last()).distance : 0;
                        _points.Add(CreateVertex(
                            d * xdist + g[0, i - 1],
                            d * ydist + g[1, i - 1],
                            d * zdist + g[2, i - 1],
                            _dist, slope
                        ));
                    }
                }

                //calculate distance
                if (_points.Count() > 0) {
                    _dist = ((PenVertex)_points.Last()).distance + ((float)maxDist);
                } else {
                    _dist = 0;
                }
                _points.Add(CreateVertex(g[0, i], g[1, i], g[2, i], _dist, slope));
            }

            length = (_points.Count() > 0) ? ((PenVertex)_points.Last()).distance : 0;
            return new PointMatrix(_points);
        }

        protected PenVertex CreateVertex(double x, double y, double z, double dist, float[] slope) {
            PenVertex result = new PenVertex(base.CreateVertex(x, y, z, dist));
            result.slope = slope;

            Object3 obj = masterObj.Clone();
            obj.AppendTransform(new MatrixTranslatePoint3Action(result));
            result.obj = obj;

            return result;
        }
    }
}

using Complexity.Math_Things;
using Complexity.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complexity.Objects {
    /// <summary>
    /// Represents a system of objects and/or systems in 3 Dimensions
    /// </summary>
    public class System3 : ComplexObject3 {
        protected Object3 masterObj;
        protected int count;

        protected class SysVertex : Point3 {
            public SysVertex(float x, float y, float z)
                : base(x, y, z) { }
            public Object3 obj;
            public float distance;

            new public SysVertex Clone() {
                return (SysVertex)MemberwiseClone();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="geometry"></param>
        /// <param name="masterObj"></param>
        public System3(double[,] geometry, Object3 masterObj)
            : base() {
            SetMasterObj(masterObj);
            vertecies = ConvertGeometry(geometry);
            originalGeo = MatrixD.OfArray(geometry);
        }

        /// <summary>
        /// Sets up the masterObj object
        /// </summary>
        /// <param name="obj"></param>
        protected virtual void SetMasterObj(Object3 obj) {
            masterObj = obj;

            //Add transforms based on inheritence
        }

        protected override PointMatrix ConvertGeometry(double[,] _geometry) {
            TypedArrayList<Point3> _vertecies = new TypedArrayList<Point3>();
            for (int i = 0; i < _geometry.GetLength(1); i++) {
                _vertecies.Add(CreateVertex(
                    _geometry[0, i], _geometry[1, i], _geometry[2, i], i));
            }

            count = _vertecies.Count();
            return new PointMatrix(_vertecies);
        }

        protected SysVertex CreateVertex(double x, double y, double z, double index) {
            SysVertex vert = new SysVertex((float)x, (float)y, (float)z);
            vert.distance = (float)index;

            Object3 obj = masterObj.Clone();
            obj.AppendTransform(new MatrixTranslatePoint3Action(vert));
            vert.obj = obj;

            return vert;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Recalculate() {
            base.Recalculate();

            ReserveVariables();
            masterObj.Recalculate();
            foreach (SysVertex vert in vertecies) {
                SetVariables(vert);
                vert.obj.Recalculate();
            }

            ReleaseVariables();
        }

        protected void SetVariables(SysVertex vert) {
            ExpressionD.SetSymbolValue("dist", vert.distance);
        }

        protected override void ReserveVariables() {
            ExpressionD.AddSymbol("dist", 0);
            ExpressionD.AddSymbol("length", count);
        }

        protected override void ReleaseVariables() {
            ExpressionD.RemoveSymbol("dist");
            ExpressionD.RemoveSymbol("length");
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw() {
            foreach (SysVertex vert in vertecies) {
                vert.obj.Draw();
            }
        }

        /// <summary>
        /// For a system, we have to perform a recursive clone
        /// </summary>
        /// <returns></returns>
        public new System3 Clone() {
            TypedArrayList<Point3> _verts = new TypedArrayList<Point3>();
            foreach (SysVertex vert in vertecies) {
                _verts.Add(vert.Clone());
            }

            System3 result = (System3)MemberwiseClone();
            result.SetPointMatrix(new PointMatrix(_verts));
            return result;
        }

        protected void SetPointMatrix(PointMatrix vertecies) {
            this.vertecies = vertecies;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clones"></param>
        protected void SetVertecies(SysVertex[] verts) {
            vertecies.SetFromArray(verts);
        }
    }
}

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
        protected const string DIST = "dist";
        protected const string LENGTH = "length";

        static System3() {
            ExpressionF.ReserveSymbol(DIST);
            ExpressionF.ReserveSymbol(LENGTH);
        }

        protected Object3 masterObj;
        protected int count;

        protected class SysVertex : Point3 {
            public SysVertex(float x, float y, float z)
                : base(x, y, z) { }
            public Object3 obj;
            public float distance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="geometry"></param>
        /// <param name="masterObj"></param>
        public System3(float[,] geometry, Object3 masterObj)
            : base() {
            SetMasterObj(masterObj);
            vertecies = ConvertGeometry(geometry);
            originalGeo = MatrixF.OfArray(geometry);
        }

        /// <summary>
        /// For a system, we have to perform a recursive clone
        /// </summary>
        /// <returns></returns>
        public override Object3 Clone() {
            System3 result = new System3(GetVertecies().ToArray(), masterObj.Clone());

            ArrayList _transforms = new ArrayList();
            foreach (MatrixTransformAction mta in transforms) {
                _transforms.Add(mta);
            }

            Dictionary<string, ObjectAttribute> _attributes = new Dictionary<string, ObjectAttribute>();
            foreach (KeyValuePair<string, ObjectAttribute> attr in attributes) {
                _attributes.Add(attr.Key, attr.Value);
            }

            result.SetTransformArray(_transforms);
            result.SetAttributes(_attributes);

            return result;
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

        protected override PointMatrixF ConvertGeometry(float[,] _geometry) {
            TypedArrayList<Point3> _vertecies = new TypedArrayList<Point3>();
            for (int i = 0; i < _geometry.GetLength(1); i++) {
                _vertecies.Add(CreateVertex(
                    _geometry[0, i], _geometry[1, i], _geometry[2, i], i));
            }

            count = _vertecies.Count();
            return new PointMatrixF(_vertecies);
        }

        protected SysVertex CreateVertex(double x, double y, double z, double index) {
            SysVertex vert = new SysVertex((float)x, (float)y, (float)z);
            vert.distance = (float)index;

            Object3 obj = masterObj.Clone();
            obj.AppendTransform(new MatrixTranslatePoint3Action(vert));
            vert.obj = obj;

            return vert;
        }

        protected override void ReserveVariables() {
            ExpressionF.AdvanceScope();
            ExpressionF.AddScopedSymbol(DIST, 0);
            ExpressionF.AddScopedSymbol(LENGTH, count);
        }

        protected override void ReleaseVariables() {
            ExpressionF.DecreaseScope();
        }

        /// <summary>
        /// Sets up the masterObj object
        /// </summary>
        /// <param name="obj"></param>
        protected virtual void SetMasterObj(Object3 obj) {
            masterObj = obj;

            //Add transforms based on inheritence
        }

        protected void SetPointMatrix(PointMatrixF vertecies) {
            this.vertecies = vertecies;
        }

        protected void SetVariables(SysVertex vert) {
            ExpressionF.SetScopedSymbol(DIST, vert.distance);
        }

        /// <summary>
        /// Sets the vertecies
        /// </summary>
        /// <param name="clones"></param>
        protected void SetVertecies(SysVertex[] verts) {
            vertecies.SetFromArray(verts);
        }
    }
}

using Complexity.Managers;
using Complexity.Math_Things;
using Complexity.Objects.Base;
using Complexity.Objects.Vertex;
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
    public class System3 : Object3 {
        public const string DIST = "dist";
        public const string LENGTH = "length";
        protected Object3 masterObj;
        protected int count;

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
            result.SetID();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw() {
            foreach (VertexSystem vert in vertecies) {
                vert.obj.Draw();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Recalculate() {
            ResourceManager.AdvanceScope();
            ResourceManager.AddExprVal(DIST, new VariableFloat(DIST, 0));
            ResourceManager.AddExprVal(LENGTH, new VariableFloat(LENGTH, count));

            base.Recalculate();

            masterObj.Recalculate();
            foreach (VertexSystem vert in vertecies) {
                vert.Recalculate();
            }

            ResourceManager.DecreaseScope();
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

        protected VertexSystem CreateVertex(double x, double y, double z, double index) {
            VertexSystem vert = new VertexSystem((float)x, (float)y, (float)z);
            vert.distance = (float)index;

            Object3 obj = masterObj.Clone();
            obj.AppendTransform(new MatrixTranslatePoint3Action(vert));
            vert.obj = obj;

            return vert;
        }

        /// <summary>
        /// Sets up the masterObj object
        /// </summary>
        /// <param name="obj"></param>
        protected virtual void SetMasterObj(Object3 obj) {
            masterObj = obj;
        }

        protected void SetPointMatrix(PointMatrixF vertecies) {
            this.vertecies = vertecies;
        }

        protected void SetVariables(VertexSystem vert) {
            //ExpressionF.SetScopedSymbol(DIST, vert.distance);
        }

        /// <summary>
        /// Sets the vertecies
        /// </summary>
        /// <param name="clones"></param>
        protected void SetVertecies(VertexSystem[] verts) {
            vertecies.SetFromArray(verts);
        }
    }
}

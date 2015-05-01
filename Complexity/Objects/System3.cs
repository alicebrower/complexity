using Complexity.Managers;
using Complexity.Math_Things;
using Complexity.Objects.Base;
using Complexity.Programming;
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
        protected Object3 masterObj;
        protected List<Object3> vertexObjects;
        protected int count = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="geometry"></param>
        /// <param name="masterObj"></param>
        public System3(Geometry geometry, Object3 masterObj)
            : base() {
            this.masterObj = masterObj;

            vertexObjects = new List<Object3>(geometry.Rows());
            Object3 obj;
            for (int i = 0; i < geometry.Rows(); i++) {
                count = i + 1;
                obj = masterObj.Clone();
                obj.AddVaraible("dist", new Variable(Variable.FLOAT, i));
                obj.SetParent(this);
                obj.SetPosition("" + geometry[i, 0], "" + geometry[i, 1], "" + geometry[i, 2]);
                vertexObjects.Add(obj);
            }

            vertecies = ConvertGeometry(geometry);

            InitVariables();
        }

        /// <summary>
        /// For a system, we have to perform a recursive clone
        /// </summary>
        /// <returns></returns>
        public override Object3 Clone() {
            System3 result = new System3(GetVertecies().originalGeo.Clone(), masterObj.Clone());

            ArrayList _transforms = new ArrayList();
            foreach (MatrixTransformAction mta in transforms) {
                _transforms.Add(mta);
            }
            result.SetTransformArray(_transforms);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw() {
            foreach (Object3 obj in vertexObjects) {
                obj.Draw();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Recalculate() {
            base.Recalculate();
            //masterObj.Recalculate();

            ExpressionF x, y, z;
            for (int i = 0; i < vertecies.RowCount; i++) {
                x = new ExpressionF("" + vertecies[i, 0]);
                y = new ExpressionF("" + vertecies[i, 1]);
                z = new ExpressionF("" + vertecies[i, 2]);
                x.Compile();
                y.Compile();
                z.Compile();
                vertexObjects[i].SetPosition(x, y, z);
            }
        }

        protected override void InitVariables() {
            base.InitVariables();
            variables.Add("length", new Variable(Variable.FLOAT, count));
        }

        public override bool HasChildren() {
            return true;
        }

        public override List<Object3> GetChildren() {
            return vertexObjects;
        }
    }
}

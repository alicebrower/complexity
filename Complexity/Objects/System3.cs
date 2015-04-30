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
        protected Object3[] vertexObjects;
        protected int count;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="geometry"></param>
        /// <param name="masterObj"></param>
        public System3(Geometry geometry, Object3 masterObj)
            : base() {
            this.masterObj = masterObj;

            vertexObjects = new Object3[geometry.Rows()];
            for (int i = 0; i < geometry.Rows(); i++) {
                vertexObjects[i] = masterObj.Clone();
                vertexObjects[i].SetPosition("" + geometry[i, 0], "" + geometry[i, 1], "" + geometry[i, 2]);
            }

            vertecies = ConvertGeometry(geometry);
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
            ResourceManager.AdvanceScope();

            base.Recalculate();
            masterObj.Recalculate();
            for (int i = 0; i < vertecies.RowCount; i++) {
                vertexObjects[i].SetPosition("" + vertecies[i, 0], "" + vertecies[i, 1], "" + vertecies[i, 2]);
                vertexObjects[i].Recalculate();
            }

            ResourceManager.DecreaseScope();
        }

        protected override void InitVariables() {
            base.InitVariables();
            variables.Add("length", new Variable(Variable.FLOAT, 0));
        }
    }
}

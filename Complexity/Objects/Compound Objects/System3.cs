﻿using Complexity.Util;
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
        new public void Recalculate() {
            base.Recalculate();
            masterObj.Recalculate();

            foreach (SysVertex vert in vertecies) {
                vert.obj.Recalculate();

                //Set
                vert.obj.Recalculate();
                //vert.obj.SetColor(attributes["color"].value.Values());
                //vert.obj.ScaleGeo(scale.Evaluate());
                //vert.obj.TranslateGeo(vert.x, vert.y, vert.z);
            }

            UpdateClones();
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

        /// <summary>
        /// Updates all the atributes of the clones
        /// </summary>
        protected virtual void UpdateClones() {
            SetPositions();
        }

        /// <summary>
        /// Sets the positions of the clone objects.
        /// Should be used after recalculating.
        /// </summary>
        protected virtual void SetPositions() {
            foreach (SysVertex vert in vertecies) {
                //((ComplexObject3)vert.obj).SetPosition(new  double[] {
                //    vert.x, vert.y, vert.z});
            }
        }
    }
}

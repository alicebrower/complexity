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
using Complexity.Math_Things;

namespace Complexity.Objects {
    /// <summary>
    /// Represents a 3 Dimentional Object that can be rendered.
    /// SimpleObject -> Does not recalculate, must be modified externally.
    /// ComplexObject -> Has a Recalculate method and can update itself
    /// </summary>
    public abstract class Object3 : Renderable {
        private static readonly Object LockObject = new Object();

        protected const int ORIGIN_T = 0;
        protected const int SCALE_T = 1;
        protected const int ROTATE_T = 2;
        protected const int TRANSLATE_T = 3;
        protected ArrayList transforms;
        protected PointMatrix vertecies;
        protected MatrixD originalGeo;

        //Attributes
        protected class ObjectAttribute {
            public dynamic value;
            public bool inherit;
            public readonly bool removable;

            public ObjectAttribute() { }

            public ObjectAttribute(bool removable) {
                this.removable = removable;
            }

            public ObjectAttribute(dynamic value, bool inherit, bool removable) {
                this.value = value;
                this.inherit = inherit;
                this.removable = removable;
            }
        }

        protected class ObjectAttributeT<T> : ObjectAttribute {
            public T value;

            public ObjectAttributeT() { }

            public ObjectAttributeT(bool removable)
                : base(removable) {
            }

            public ObjectAttributeT(T value, bool inherit, bool removable)
                : base(removable) {
                this.value = value;
                this.inherit = inherit;
            }
        }

        protected Dictionary<string, ObjectAttribute> attributes;

        protected Object3() { }

        public Object3(double[,] geometry) {
            vertecies = ConvertGeometry(geometry);
            originalGeo = MatrixD.OfArray(geometry);

            lock (LockObject) {
                attributes = new Dictionary<string, ObjectAttribute>() {
                    {"origin", new ObjectAttributeT<MatrixTranslateAction>()},
                    {"scale", new ObjectAttributeT<MatrixScaleAction>()},
                    {"rotate", new ObjectAttributeT<MatrixRotateAction>()},
                    {"translate", new ObjectAttributeT<MatrixTranslateAction>()}
                };

                //Initialize the transform list, leave entries blank
                //they will be checked for null
                transforms = new ArrayList(4);
                transforms.Add(attributes["origin"].value);
                transforms.Add(attributes["scale"].value);
                transforms.Add(attributes["rotate"].value);
                transforms.Add(attributes["translate"].value);
            }

            Init();
        }

        public void SetInherit(string name, bool inherit) {
            attributes[name].inherit = inherit;
        }

        public void AddAttribute(string name, dynamic value, bool inherit) {
            attributes.Add(name, new ObjectAttribute(value, inherit, false));
        }

        public void SetAttribute(string name, dynamic value) {
            attributes[name].value = value;
        }

        public void RemoveAttribute(string name) {
            if (attributes[name].removable) {
                attributes.Remove(name);
            } else {
                attributes[name].value = null;
            }
        }

        /// <summary>
        /// Set object attributes from a Dictionary. 
        /// </summary>
        /// <param name="args"></param>
        [Obsolete("Object properties are being moved an attributes array and this method will no longer work.")]
        public virtual void SetAttributes(Dictionary<string, string> args) {
            if (args.ContainsKey("name")) {
                //name = args["name"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void Init() {

        }

        #region Transforms

        /// <summary>
        /// Appends a custom transform to the transforms ArrayList
        /// </summary>
        /// <param name="trans">Index of the transform</param>
        /// <returns></returns>
        public int AppendTransform(MatrixTransformAction trans) {
            return transforms.Add(trans);
        }

        public void SetTransform(int index, MatrixTransformAction trans) {
            if (index >= transforms.Count) {
                throw new IndexOutOfRangeException();
            }

            transforms[index] = trans;
        }

        public void SetOrigin(VectorExpr origin) {
            attributes["origin"].value = new MatrixTranslateAction(origin);
            transforms[ORIGIN_T] = attributes["origin"].value;
        }

        public void SetScale(VectorExpr scale) {
            attributes["scale"].value = new MatrixScaleAction(scale);
            transforms[SCALE_T] = attributes["scale"].value;
        }

        public void SetRotate(VectorExpr rotate) {
            attributes["rotate"].value = new MatrixRotateAction(rotate);
            transforms[ROTATE_T] = attributes["rotate"].value;
        }

        public void SetTranslate(VectorExpr trans) {
            attributes["translate"].value = new MatrixTranslateAction(trans);
            transforms[TRANSLATE_T] = attributes["translate"].value;
        }

        /// <summary>
        /// Removes a transform at a given index.
        /// </summary>
        /// <param name="index"></param>
        public void RemoveTransform(int index) {
            if (index < 0) {
                throw new ArgumentException("Object3.RemoveTransform : index < 0");
            }

            //If it's below 4, set it to null.
            //The point is to make the order & existence of entries 0-3 unmodifiable
            if (index < 4) {
                transforms[index] = null;
            } else {
                transforms.RemoveAt(index);
            }
        }

        #endregion

        public PointMatrix GetVertecies() {
            return vertecies;
        }

        /*
        public void ScaleGeo(double scale) {
            vertecies.Scale(scale);
        }

        public void TranslateGeo(double x, double y, double z) {
            vertecies.Translate(x, y, z);
        }
        */

        public abstract void SetColor(double[] color);

        /// <summary>
        /// 
        /// </summary>
        public void Recalculate() {
            vertecies.SetFromMatrix(originalGeo);

            foreach (MatrixTransformAction mta in transforms) {
                if (mta != null) {
                    mta.Transform(vertecies);
                }
            }
        }

        /// <summary>
        /// Need to perform recursive clone of vertecies PointMatrix
        /// </summary>
        /// <returns>a shallow copy of this object</returns>
        public virtual Object3 Clone() {
            PointMatrix newVerts = new PointMatrix(vertecies.ToArray());
            for (int i = 0; i < newVerts.ColumnCount; i++) {
                newVerts.Set(i, vertecies.Get(i).Clone());
            }

            Object3 result = (Object3)MemberwiseClone();
            result.SetVertecies(newVerts);
            return result;
        }

        public void SetVertecies(PointMatrix vertecies) {
            this.vertecies = vertecies;
        }

        /// <summary>
        /// Note, there is something called a VertexBuffer that could potentially
        /// be used to improve efficiency.
        /// </summary>
        public abstract void Draw();

        /// <summary>
        /// This method is responsible for converting a double[,] into a 
        /// populated PointMatrix representing all the object's vertecies
        /// </summary>
        /// <param name="geometry"></param>
        protected virtual PointMatrix ConvertGeometry(double[,] geometry) {
            return new PointMatrix(geometry);
        }
    }
}

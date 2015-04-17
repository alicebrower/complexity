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
using Complexity.Managers;
using Complexity.Objects.Base;
using Complexity.Interfaces;

namespace Complexity.Objects {
    //CONSTRUCTORS
    /// <summary>
    /// Represents a 3 Dimentional Object that can be rendered.
    /// SimpleObject -> Does not recalculate, must be modified externally.
    /// ComplexObject -> Has a Recalculate method and can update itself
    /// </summary>
    public abstract partial class Object3 : Renderable, Recalculatable {
        static Object3() {
            ATTRIBUTES = new Dictionary<string, ObjectAttribute>() {
                {ORIGIN, new ObjectAttributeT<MatrixTranslateAction>()},
                {SCALE, new ObjectAttributeT<MatrixScaleAction>()},
                {ROTATE, new ObjectAttributeT<MatrixRotateAction>()},
                {TRANSLATE, new ObjectAttributeT<MatrixTranslateAction>()},
                {COLOR, new ObjectAttributeT<VectorExpr>()}
            };
            ATTRIBUTES[COLOR].value = new VectorExpr(new string[]{"1", "0", "1", "1"});
        }

        protected Object3() {
            attributes = new Dictionary<string, ObjectAttribute>();

            //Initialize the transform list, leave entries blank
            //they will be checked for null
            transforms = new ArrayList(4);
            transforms.Add(GetAttribute(ORIGIN).value);
            transforms.Add(GetAttribute(SCALE).value);
            transforms.Add(GetAttribute(ROTATE).value);
            transforms.Add(GetAttribute(TRANSLATE).value);

            SetID();
        }

        public Object3(float[,] geometry)
            : this() {
            vertecies = ConvertGeometry(geometry);
            originalGeo = MatrixF.OfArray(geometry);
        }
    }

    //METHODS, NO ATTRIBUTES
    public abstract partial class Object3 {
        protected const int ORIGIN_T = 0;
        protected const int SCALE_T = 1;
        protected const int ROTATE_T = 2;
        protected const int TRANSLATE_T = 3;
        protected static ulong ID = 0;

        //Don't forget that collections need special handling in the clone method!
        protected ulong id;
        protected ArrayList transforms;
        protected PointMatrixF vertecies;
        protected MatrixF originalGeo;

        public void SetTransformArray(ArrayList trans) {
            transforms = trans;
        }

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

        public PointMatrixF GetVertecies() {
            return vertecies;
        }

        public void SetVertecies(PointMatrixF vertecies) {
            this.vertecies = vertecies;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Recalculate() {
            vertecies.SetFromMatrix(originalGeo);
            GetAttribute(COLOR).value.Recalculate();

            foreach (MatrixTransformAction mta in transforms) {
                if (mta != null) {
                    mta.Transform(vertecies);
                }
            }
        }

        public void SetID() {
            id = ID++;
        }

        /// <summary>
        /// Need to perform recursive clone of vertecies PointMatrix, transforms and attributes
        /// </summary>
        /// <returns>a shallow copy of this object</returns>
        public virtual Object3 Clone() {
            PointMatrixF _vertecies = new PointMatrixF(vertecies.ToArray());
            for (int i = 0; i < _vertecies.ColumnCount; i++) {
                _vertecies.Set(i, vertecies.Get(i).Clone());
            }

            ArrayList _transforms = new ArrayList();
            foreach (MatrixTransformAction mta in transforms) {
                _transforms.Add(mta);
            }

            Dictionary<string, ObjectAttribute> _attributes = new Dictionary<string, ObjectAttribute>();
            foreach (KeyValuePair<string, ObjectAttribute> attr in attributes) {
                _attributes.Add(attr.Key, attr.Value.Clone());
            }

            Object3 result = (Object3)MemberwiseClone();
            result.SetVertecies(_vertecies);
            result.SetTransformArray(_transforms);
            result.SetAttributes(_attributes);
            result.SetID();

            return result;
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
        protected virtual PointMatrixF ConvertGeometry(float[,] geometry) {
            return new PointMatrixF(geometry);
        }
    }

    //ATTRIBUTE METHODS
    public abstract partial class Object3 {
        protected const string ORIGIN    = "origin";
        protected const string SCALE     = "scale";
        protected const string ROTATE    = "rotate";
        protected const string TRANSLATE = "translate";
        protected const string COLOR     = "color";
        protected static Dictionary<string, ObjectAttribute> ATTRIBUTES;
        protected Dictionary<string, ObjectAttribute> attributes;

        public void SetInherit(string name, bool inherit) {
            attributes[name].inherit = inherit;
        }

        protected void SetAttributes(Dictionary<string, ObjectAttribute> attributes) {
            this.attributes = attributes;
        }

        public ObjectAttribute GetAttribute(string name) {
            if (attributes.ContainsKey(name)) {
                return attributes[name];
            } else if (ResourceManager.ContainsAttribute(name)) {
                return ResourceManager.GetAttribute(name);
            } else if (ATTRIBUTES.ContainsKey(name)) {
                return ATTRIBUTES[name];
            } else {
                throw new Exception("Attribute not defined, " + name);
            }
        }

        public void AddAttribute(string name, dynamic value, bool inherit) {
            attributes.Add(name, new ObjectAttribute(value, inherit, false));
        }

        public void SetAttribute(string name, dynamic value) {
            attributes[name].value = value;
        }

        public void SetAttribute(string attr, ObjectAttribute value) {
            if (attributes.ContainsKey(attr)) {
                attributes[attr] = value;
            } else {
                attributes.Add(attr, value);
            }
        }

        public void RemoveAttribute(string name) {
            if (attributes[name].removable) {
                attributes.Remove(name);
            } else {
                attributes[name].value = null;
            }
        }

        public void SetScale(string scale) {
            SetScale(scale, scale, scale);
        }

        public void SetScale(string x, string y, string z) {
            SetScale(new VectorExpr(new string[]{x, y, z}));
        }

        public void SetScale(VectorExpr scale) {
            if (scale.Size() != 3) {
                throw new ArgumentException("Scale vector must have three components, one for each dimension.");
            }

            ObjectAttribute objAttr = new ObjectAttribute(new MatrixScaleAction(scale), false, false);
            SetAttribute(SCALE, objAttr);
            transforms[SCALE_T] = attributes[SCALE].value;
        }

        public void SetRotate(string x, string y, string z) {
            SetRotate(new VectorExpr(new string[]{x, y, z}));
        }

        public void SetRotate(VectorExpr rotate) {
            if (rotate.Size() != 3) {
                throw new ArgumentException("Rotate vector must have three components, one for each dimension.");
            }

            ObjectAttribute objAttr = new ObjectAttribute(new MatrixRotateAction(rotate), false, false);
            SetAttribute(ROTATE, objAttr);
            transforms[ROTATE_T] = attributes[ROTATE].value;
        }

        public void SetTranslate(string x, string y, string z) {
            SetTranslate(new VectorExpr(new string[]{x, y, z}));
        }

        public void SetTranslate(VectorExpr trans) {
            if (trans.Size() != 3) {
                throw new ArgumentException("Trans vector must have three components, one for each dimension.");
            }

            ObjectAttribute objattr = new ObjectAttribute(new MatrixTranslateAction(trans), false, false);
            SetAttribute(TRANSLATE, objattr);
            transforms[TRANSLATE_T] = attributes[TRANSLATE].value;
        }

        public void SetOrigin(VectorExpr origin) {
            if (origin.Size() != 3) {
                throw new ArgumentException("Origin vector must have three cells, one for each dimension.");
            }

            ObjectAttribute objAttr = new ObjectAttribute(new MatrixTranslateAction(origin), false, false);
            SetAttribute(ORIGIN, objAttr);
            transforms[ORIGIN_T] = attributes[ORIGIN].value;
        }

        public void SetColor(string r, string g, string b, string a) {
            ObjectAttribute objAttr = new ObjectAttribute(new VectorExpr(new string[]{r, g, b, a}), false, false);
            SetAttribute(COLOR, objAttr);
        }

        public float[] GetColor() {
            return GetAttribute(COLOR).value.Values();
        }
    }
}

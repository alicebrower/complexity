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
        protected string name;
        protected const int ORIGIN_T = 0;
        protected const int SCALE_T = 1;
        protected const int ROTATE_T = 2;
        protected const int TRANSLATE_T = 3;
        protected ArrayList transforms;
        protected PointMatrix vertecies;
        protected MatrixD originalGeo;

        public Object3() { }

        public Object3(double[,] geometry) {
            vertecies = ConvertGeometry(geometry);
            originalGeo = MatrixD.OfArray(geometry);

            Init();
        }

        /// <summary>
        /// Set object attributes from a Dictionary. 
        /// </summary>
        /// <param name="args"></param>
        public virtual void SetAttributes(Dictionary<string, string> args) {
            if (args.ContainsKey("name")) {
                name = args["name"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void Init() {
            //Initialize the transform list, leave entries blank
            //they will be checked for null
            transforms = new ArrayList(4);
        }

        public void SetOrigin(VectorExpr origin) {
            transforms[ORIGIN_T] = new MatrixTranslateAction(origin);
        }

        public void SetScale(ExpressionD expr) {
            transforms[SCALE_T] = new MatrixScaleAction(expr);
        }

        public void SetRotate(VectorExpr rotate) {
            transforms[ROTATE_T] = new MatrixRotateAction(rotate);
        }

        public void SetTranslate(VectorExpr trans) {
            transforms[TRANSLATE_T] = trans;
        }

        /// <summary>
        /// Appends a custom transform to the transforms ArrayList
        /// </summary>
        /// <param name="trans">Index of the transform</param>
        /// <returns></returns>
        public int AppendTransform(MatrixTransformAction trans) {
            return transforms.Add(trans);
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

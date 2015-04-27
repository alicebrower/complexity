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
using Complexity.Programming;

namespace Complexity.Objects {
    //METHODS, NO ATTRIBUTES
    /// <summary>
    /// Represents a 3 Dimentional Object that can be rendered.
    /// SimpleObject -> Does not recalculate, must be modified externally.
    /// ComplexObject -> Has a Recalculate method and can update itself
    /// </summary>
    public abstract partial class Object3 : ProgrammableObject, Renderable, Recalculatable {
        protected static ulong ID = 0;

        //Don't forget that collections need special handling in the clone method!
        protected ulong id;
        protected ArrayList transforms;
        protected PointMatrix vertecies;

        protected Object3() {
            InitVariables();
            transforms = new ArrayList(4);
            SetID();
        }

        public Object3(Geometry geometry)
            : this() {
            vertecies = ConvertGeometry(geometry);
        }

        public void SetTransformArray(ArrayList trans) {
            transforms = trans;
        }

        /// <summary>
        /// Appends a custom transform to the transforms ArrayList
        /// </summary>
        /// <param name="trans">Index of the transform</param>
        /// <returns></returns>
        public int AddTransform(MatrixTransformAction trans) {
            return transforms.Add(trans);
        }

        public void SetTransform(int index, MatrixTransformAction trans) {
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
            transforms.RemoveAt(index);
        }

        public PointMatrix GetVertecies() {
            return vertecies;
        }

        public void SetVertecies(PointMatrix vertecies) {
            this.vertecies = vertecies;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Recalculate() {
            RecalculateVariables();
            vertecies.Restore();

            //Manipulate vertecies
            vertecies.Translate(originX.Value(), originY.Value(), originZ.Value());
            vertecies.Scale(scaleX.Value(), scaleY.Value(), scaleZ.Value());
            vertecies.Rotate(rotateX.Value(), rotateY.Value(), rotateZ.Value());
            vertecies.Translate(positionX.Value(), positionY.Value(), positionZ.Value());

            foreach (MatrixTransformAction mta in transforms) {
                mta.Transform(vertecies);
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
            PointMatrix _vertecies = new PointMatrix(vertecies.originalGeo.Clone());

            ArrayList _transforms = new ArrayList();
            foreach (MatrixTransformAction mta in transforms) {
                _transforms.Add(mta);
            }

            Object3 result = (Object3)MemberwiseClone();
            result.SetVertecies(_vertecies);
            result.SetTransformArray(_transforms);
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
        protected virtual PointMatrix ConvertGeometry(Geometry geometry) {
            return new PointMatrix(geometry);
        }
    }

    //ATTRIBUTE METHODS
    public abstract partial class Object3 {
        protected ExpressionF colorR = new ExpressionF("1");
        protected ExpressionF colorG = new ExpressionF("0");
        protected ExpressionF colorB = new ExpressionF("1");
        protected ExpressionF colorA = new ExpressionF("1");
        protected ExpressionF scaleX = new ExpressionF("1");
        protected ExpressionF scaleY = new ExpressionF("1");
        protected ExpressionF scaleZ = new ExpressionF("1");
        protected ExpressionF rotateX = new ExpressionF("0");
        protected ExpressionF rotateY = new ExpressionF("0");
        protected ExpressionF rotateZ = new ExpressionF("0");
        protected ExpressionF originX = new ExpressionF("0");
        protected ExpressionF originY = new ExpressionF("0");
        protected ExpressionF originZ = new ExpressionF("0");
        protected ExpressionF positionX = new ExpressionF("0");
        protected ExpressionF positionY = new ExpressionF("0");
        protected ExpressionF positionZ = new ExpressionF("0");
        protected ProgrammableObject parent;
        protected Dictionary<string, Variable> variables;

        protected virtual void InitVariables() {
            variables = new Dictionary<string, Variable>() {
               {"colorR", new Variable(Variable.FLOAT) {Value = () => colorR.Value()}},
               {"colorG", new Variable(Variable.FLOAT) {Value = () => colorG.Value()}},
               {"colorB", new Variable(Variable.FLOAT) {Value = () => colorB.Value()}},
               {"colorA", new Variable(Variable.FLOAT) {Value = () => colorA.Value()}},
               {"scaleX", new Variable(Variable.FLOAT) {Value = () => scaleX.Value()}},
               {"scaleY", new Variable(Variable.FLOAT) {Value = () => scaleY.Value()}},
               {"scaleZ", new Variable(Variable.FLOAT) {Value = () => scaleZ.Value()}},
               {"rotateX", new Variable(Variable.FLOAT) {Value = () => rotateX.Value()}},
               {"rotateY", new Variable(Variable.FLOAT) {Value = () => rotateY.Value()}},
               {"rotateZ", new Variable(Variable.FLOAT) {Value = () => rotateZ.Value()}},
               {"originX", new Variable(Variable.FLOAT) {Value = () => originX.Value()}},
               {"originY", new Variable(Variable.FLOAT) {Value = () => originY.Value()}},
               {"originZ", new Variable(Variable.FLOAT) {Value = () => originZ.Value()}},
               {"positionX", new Variable(Variable.FLOAT) {Value = () => positionX.Value()}},
               {"positionY", new Variable(Variable.FLOAT) {Value = () => positionY.Value()}},
               {"positionZ", new Variable(Variable.FLOAT) {Value = () => positionZ.Value()}},
               {"parent", new Variable(Variable.OBJECT) {Value = () => GetParent()}}
            };
        }

        protected void RecalculateVariables() {
            colorR.Evaluate();
            colorG.Evaluate();
            colorB.Evaluate();
            colorA.Evaluate();
            scaleX.Evaluate();
            scaleY.Evaluate();
            scaleZ.Evaluate();
            rotateX.Evaluate();
            rotateY.Evaluate();
            rotateZ.Evaluate();
            originX.Evaluate();
            originY.Evaluate();
            originZ.Evaluate();
            positionX.Evaluate();
            positionY.Evaluate();
            positionZ.Evaluate();
        }

        public bool ContainsVariable(string name) {
            return variables.ContainsKey(name);
        }

        public void AddVariable(string name, Variable value) {
            variables.Add(name, value);
        }

        public Variable GetVariable(string name) {
            return variables[name];
        }

        public bool ContainsFunction(string name) {
            throw new NotImplementedException();
        }

        public Function GetFunction(string name) {
            throw new NotImplementedException();
        }

        public ProgrammableObject GetParent() {
            return parent;
        }

        public void SetParent(ProgrammableObject parent) {
            this.parent = parent;
        }

        public void SetScale(string scale) {
            SetScale(scale, scale, scale);
        }

        public void SetScale(string x, string y, string z) {
            scaleX = new ExpressionF(x);
            scaleY = new ExpressionF(y);
            scaleZ = new ExpressionF(z);
        }

        public void SetRotate(string x, string y, string z) {
            rotateX = new ExpressionF(x);
            rotateY = new ExpressionF(y);
            rotateZ = new ExpressionF(z);
        }

        public void SetPosition(string x, string y, string z) {
            positionX = new ExpressionF(x);
            positionY = new ExpressionF(y);
            positionZ = new ExpressionF(z);
        }

        public void SetOrigin(string x, string y, string z) {
            originX = new ExpressionF(x);
            originY = new ExpressionF(y);
            originZ = new ExpressionF(z);
        }

        public void SetColor(string r, string g, string b, string a) {
            colorR = new ExpressionF(r);
            colorG = new ExpressionF(g);
            colorB = new ExpressionF(b);
            colorA = new ExpressionF(a);
        }

        public float[] GetColor() {
            return new float[] {colorR.Value(), colorG.Value(), colorB.Value(), colorA.Value()};
        }
    }
}

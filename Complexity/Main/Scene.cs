using System;


using Complexity.Objects;
using System.Collections;
using System.Threading;
using Complexity.Managers;
using Complexity.Interfaces;
using System.Collections.Generic;
using Complexity.Programming;

namespace Complexity.Main {
    /// <summary>
    /// Contains all the information that a render window needs to draw.
    /// </summary>
    public class Scene : ProgrammableObject {
        private double[] position;
        private List<Object3> objects;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public Scene() {
            Init();
        }

        /// <summary>
        /// 
        /// </summary>
        private void Init() {
            objects = new List<Object3>();

            position = new double[] { 0, 0, 0 };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void Add(Object3 obj) {
            ObjectManager.AddObject(obj);
            objects.Add(obj);
        }

        /// <summary>
        /// Removes all objects from the scene
        /// </summary>
        public void Clear() {
            objects.Clear();
        }

        public override void Compile() { }

        /// <summary>
        /// Recalculate the properties of all objects in the scene
        /// </summary>
        public override void Recalculate() {
            ResourceManager.ModifyVariable("time", Universe.GetElapsedTime());

            foreach (Object3 obj in objects) {
                obj.Recalculate();
            }
        }

        public void Draw() {
            foreach (Object3 obj in objects) {
                obj.Draw();
            }
        }

        public override bool HasChildren() {
            return true;
        }

        public override List<Object3> GetChildren() {
            return objects;
        }
    }
}

using System;


using Complexity.Objects;
using System.Collections;
using System.Threading;
using Complexity.Managers;

namespace Complexity {
    /// <summary>
    /// Contains all the information that a render window needs to draw.
    /// </summary>
    public class Scene {
        private double[] position;

        private ArrayList objects;

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
            objects = new ArrayList();
            
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

        /// <summary>
        /// Recalculate the properties of all objects in the scene
        /// </summary>
        public void Recalculate() {
            foreach (Object3 obj in objects) {
                obj.Recalculate();
            }
        }

        public void Draw() {
            foreach (Object3 obj in objects) {
                obj.Draw();
            }
        }
    }
}

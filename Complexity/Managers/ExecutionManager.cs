using Complexity.Interfaces;
using Complexity.Main;
using Complexity.Programming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complexity.Managers {
    public static class ExecutionManager {
        //These are the scenes that are being rendered and thus, need to be recalculated
        static Dictionary<int, Scene> scenes;

        static ExecutionManager() {
            scenes = new Dictionary<int, Scene>();
        }

        public static int AddScene(Scene scene) {
            int key; 
            do {
                key = Universe.RandomInt();
            } while(scenes.ContainsKey(key));

            scenes.Add(key, scene);
            return key;
        }

        public static void Remove(int key) {
            scenes.Remove(key);
        }

        public static void Recalculate() {
            foreach(KeyValuePair<int, Scene> pair in scenes) {
                RecalculateR(pair.Value);
            }
        }

        private static void RecalculateR(ProgrammableObject item) {
            ResourceManager.AdvanceScope(item.GetVariables(), item.GetFunctions());
            item.Recalculate();
            if (item.HasChildren()) {
                foreach (ProgrammableObject obj in item.GetChildren()) {
                    RecalculateR(obj);
                }
            }

            ResourceManager.DecreaseScope();
        }
    }
}

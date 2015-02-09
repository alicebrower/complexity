using Complexity.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complexity.Managers {
    public static class ObjectManager {
        private static ArrayList objects = new ArrayList();

        public static void AddObject(Object3 obj) {
            objects.Add(obj);
        }

        public static void Recalculate() {
            foreach (Object3 obj in objects) {
                obj.Recalculate();
            }
        }
    }
}

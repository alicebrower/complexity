using Complexity.Math_Things;
using Complexity.Objects.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complexity.Managers {
    /// <summary>
    /// Maintains all of the scoped managers and all other values defined during the
    /// program's execution.
    /// </summary>
    public class ResourceManager {
        private static ScopedManager<ObjectAttribute> attributes;
        private static ScopedManager<VariableFloat> exprVals;

        static ResourceManager() {
            Dictionary<string, VariableFloat> exprValues = new Dictionary<string, VariableFloat>() {
                {"time", new VariableFloat("time", (a) => (float)Global.GetElapsedTime())},
                {"pi", new VariableFloat("pi", (a) => (float)Math.PI)},
                {"e", new VariableFloat("e", (a) => (float)Math.E)}
            };

            attributes = new ScopedManager<ObjectAttribute>();
            exprVals = new ScopedManager<VariableFloat>(exprValues);
        }

        public static void AdvanceScope() {
            attributes.AdvanceScope();
            exprVals.AdvanceScope();
        }

        public static void DecreaseScope() {
            attributes.DecreaseScope();
            exprVals.DecreaseScope();
        }

        public static void AddExprVal(string name, VariableFloat val) {
            exprVals.AddAttribute(name, val);
        }
        public static void ModifyExprVal(string name, VariableFloat val) {
            exprVals.ModifyAttribute(name, val);
        }

        public static void ModifyExprVal(string name, float val) {
            exprVals.ModifyAttribute(name, new VariableFloat(val));
        }

        public static float GetExprVal(string name) {
            return exprVals.GetAttribute(name).eval(null);
        }
    }
}

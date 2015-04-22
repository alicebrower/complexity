using Complexity.Math_Things;
using Complexity.Objects.Base;
using Complexity.Programming;
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
        private static ScopedManager<Variable> variables;
        private static ScopedManager<Function> functions;
        private static Random random;

        static ResourceManager() {
            Dictionary<string, VariableFloat> exprValues = new Dictionary<string, VariableFloat>() {
                {"time", new VariableFloat("time", (a) => (float)Global.GetElapsedTime())},
                {"pi", new VariableFloat("pi", (a) => (float)Math.PI)},
                {"e", new VariableFloat("e", (a) => (float)Math.E)}
            };

            Dictionary<string, Variable> variableValues = new Dictionary<string, Variable>() {
                {"abcd", new Variable("abcd", 7.0)},
                {"parent", new Variable("parent", "object")},
                {"color", new Variable("color", "red")},
                {"time", new Variable("time", 0)},
                {"pi", new Variable("pi", (float)Math.PI)},
                {"e", new Variable("e", (float)Math.E)}
            };

            Dictionary<string, Function>functionValues = new Dictionary<string, Function>() {
                {"sin", new Function(TypeCPX.DOUBLE, "sin", 1, (a) => new Variable("", Math.Sin((double)a[0].value)))},
                {"PerformAction", new Function(TypeCPX.DOUBLE, "PerformAction", 4, (a) => new Variable("", 4.0))}
            };

            attributes = new ScopedManager<ObjectAttribute>();
            exprVals = new ScopedManager<VariableFloat>(exprValues);
            variables = new ScopedManager<Variable>(variableValues);
            functions = new ScopedManager<Function>(functionValues);

            random = new Random();
        }

        public static void AdvanceScope() {
            attributes.AdvanceScope();
            exprVals.AdvanceScope();
            variables.AdvanceScope();
        }

        public static void DecreaseScope() {
            attributes.DecreaseScope();
            exprVals.DecreaseScope();
            variables.DecreaseScope();
        }

        public static bool IsDefined(string name) {
            return (attributes.Contains(name) || exprVals.Contains(name));
        }

        public static string GetRandomVarName() {
            string result;
            string randStr;
            int num;
            //lower = 97 - 122
            //upper = 65 - 90

            do {
                result = "_R";
                randStr = "";

                for (int i = 0; i < 10; i++) {
                    num = random.Next() % 52;
                    if (num < 26) {
                        randStr += (char)(num + 65);
                    } else {
                        randStr += (char)(num - 26 + 97);
                    }
                }
                result += randStr;
            } while (IsDefined(result));

            return result;
        }

        public static bool ContainsAttribute(string name) {
            return attributes.Contains(name);
        }

        public static ObjectAttribute GetAttribute(string name) {
            return attributes.GetAttribute(name);
        }

        public static void AddExprVal(string name, VariableFloat val) {
            exprVals.AddAttribute(name, val);
        }

        public static bool ContainsExprVal(string name) {
            return exprVals.Contains(name);
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

        public static bool ContainsVariable(string name) {
            return variables.Contains(name);
        }

        public static Variable GetVariable(string name) {
            return variables.GetAttribute(name);
        }

        public static bool ContainsFunction(string name) {
            return functions.Contains(name);
        }

        public static Function GetFunction(string name) {
            return functions.GetAttribute(name);
        }
    }
}

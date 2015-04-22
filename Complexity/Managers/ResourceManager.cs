using Complexity.Math_Things;
using Complexity.Objects.Base;
using Complexity.Programming;
using Complexity.Util;
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
        private static ScopedManager<Variable> variables;
        private static ScopedManager<Function> functions;
        private static Random random;

        static ResourceManager() {
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
                {"cos", new Function(TypeCPX.DOUBLE, "cos", 1, (a) => new Variable("", Math.Cos((double)a[0].value)))},
                {"tan", new Function(TypeCPX.DOUBLE, "tan", 1, (a) => new Variable("", Math.Tan((double)a[0].value)))},
                {"asin", new Function(TypeCPX.DOUBLE, "asin", 1, (a) => new Variable("", Math.Asin((double)a[0].value)))},
                {"acos", new Function(TypeCPX.DOUBLE, "acos", 1, (a) => new Variable("", Math.Acos((double)a[0].value)))},
                {"atan", new Function(TypeCPX.DOUBLE, "atan", 1, (a) => new Variable("", Math.Atan((double)a[0].value)))},
                {"sinh", new Function(TypeCPX.DOUBLE, "sinh", 1, (a) => new Variable("", Math.Sinh((double)a[0].value)))},
                {"cosh", new Function(TypeCPX.DOUBLE, "cosh", 1, (a) => new Variable("", Math.Cosh((double)a[0].value)))},
                {"tanh", new Function(TypeCPX.DOUBLE, "tanh", 1, (a) => new Variable("", Math.Tanh((double)a[0].value)))},
                {"sqrt", new Function(TypeCPX.DOUBLE, "sqrt", 1, (a) => new Variable("", Math.Sqrt((double)a[0].value)))},
                {"log", new Function(TypeCPX.DOUBLE, "log", 1, (a) => new Variable("", Math.Log((double)a[0].value)))},
                {"rad", new Function(TypeCPX.DOUBLE, "rad", 1, (a) => new Variable("", a[0].value * Math.PI / 180.0))},
                {"deg", new Function(TypeCPX.DOUBLE, "deg", 1, (a) => new Variable("", a[0].value * 180.0 / Math.PI))},

                {"abs", new Function(TypeCPX.DOUBLE, "abs", 1, (a) => new Variable("", Math.Abs(a[0].value)))},
                {"ceil", new Function(TypeCPX.DOUBLE, "ceil", 1, (a) => new Variable("", Math.Ceiling(a[0].value)))},
                {"floor", new Function(TypeCPX.DOUBLE, "floor", 1, (a) => new Variable("", Math.Floor(a[0].value)))},
                {"round", new Function(TypeCPX.DOUBLE, "round", 1, (a) => new Variable("", Math.Round(a[0].value)))},
                {"sign", new Function(TypeCPX.DOUBLE, "sign", 1, (a) => new Variable("", Math.Sign(a[0].value)))},
                {"rand", new Function(TypeCPX.FLOAT, "rand", 1, (a) => new Variable("", MathUtil.RandomFloat(a[0].value)))},

                {"PerformAction", new Function(TypeCPX.DOUBLE, "PerformAction", 4, (a) => 
                    new Variable("", a[0].value + a[1].value + a[2].value + a[3].value))}
            };

            attributes = new ScopedManager<ObjectAttribute>();
            variables = new ScopedManager<Variable>(variableValues);
            functions = new ScopedManager<Function>(functionValues);

            random = new Random();
        }

        public static void AdvanceScope() {
            attributes.AdvanceScope();
            variables.AdvanceScope();
            functions.AdvanceScope();
        }

        public static void DecreaseScope() {
            attributes.DecreaseScope();
            variables.DecreaseScope();
            functions.DecreaseScope();
        }

        public static bool IsDefined(string name) {
            return (attributes.Contains(name)
                || variables.Contains(name)
                || functions.Contains(name));
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

        public static bool ContainsVariable(string name) {
            return variables.Contains(name);
        }

        public static void AddVariable(string name, dynamic value) {
            if (variables.Contains(name)) {
                throw new Exception("Variable " + name + " is already defined");
            }

            variables.AddAttribute(name, new Variable("", value));
        }

        public static void ModifyVariable(string name, dynamic value) {
            variables.ModifyAttribute(name, new Variable("", value));
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

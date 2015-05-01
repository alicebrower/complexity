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
        //Collections by level of scope
        private static Dictionary<string, Variable> globalVariables;
        private static Dictionary<string, Function> globalFunctions;
        private static ScopedManager<Variable> variables;
        private static ScopedManager<Function> functions;
        
        private static Random random;

        static ResourceManager() {
            globalVariables = new Dictionary<string, Variable>() {
                {"time", new Variable(Variable.DOUBLE, 0)},
                {"pi", new Variable(Variable.DOUBLE, Math.PI)},
                {"e", new Variable(Variable.DOUBLE, Math.E)}
            };

            globalFunctions = new Dictionary<string, Function>() {
                {"sin", new Function(Variable.DOUBLE, "sin", 1, (a) => new Variable(Variable.DOUBLE, Math.Sin((double)a[0].Value())))},
                {"cos", new Function(Variable.DOUBLE, "cos", 1, (a) => new Variable(Variable.DOUBLE, Math.Cos((double)a[0].Value())))},
                {"tan", new Function(Variable.DOUBLE, "tan", 1, (a) => new Variable(Variable.DOUBLE, Math.Tan((double)a[0].Value())))},
                {"asin", new Function(Variable.DOUBLE, "asin", 1, (a) => new Variable(Variable.DOUBLE, Math.Asin((double)a[0].Value())))},
                {"acos", new Function(Variable.DOUBLE, "acos", 1, (a) => new Variable(Variable.DOUBLE, Math.Acos((double)a[0].Value())))},
                {"atan", new Function(Variable.DOUBLE, "atan", 1, (a) => new Variable(Variable.DOUBLE, Math.Atan((double)a[0].Value())))},
                {"sinh", new Function(Variable.DOUBLE, "sinh", 1, (a) => new Variable(Variable.DOUBLE, Math.Sinh((double)a[0].Value())))},
                {"cosh", new Function(Variable.DOUBLE, "cosh", 1, (a) => new Variable(Variable.DOUBLE, Math.Cosh((double)a[0].Value())))},
                {"tanh", new Function(Variable.DOUBLE, "tanh", 1, (a) => new Variable(Variable.DOUBLE, Math.Tanh((double)a[0].Value())))},
                {"sqrt", new Function(Variable.DOUBLE, "sqrt", 1, (a) => new Variable(Variable.DOUBLE, Math.Sqrt((double)a[0].Value())))},
                {"log", new Function(Variable.DOUBLE, "log", 1, (a) => new Variable(Variable.DOUBLE, Math.Log((double)a[0].Value())))},
                {"rad", new Function(Variable.DOUBLE, "rad", 1, (a) => new Variable(Variable.DOUBLE, a[0].Value() * Math.PI / 180.0))},
                {"deg", new Function(Variable.DOUBLE, "deg", 1, (a) => new Variable(Variable.DOUBLE, a[0].Value() * 180.0 / Math.PI))},

                {"abs", new Function(Variable.DOUBLE, "abs", 1, (a) => new Variable(Variable.DOUBLE, Math.Abs(a[0].Value())))},
                {"ceil", new Function(Variable.DOUBLE, "ceil", 1, (a) => new Variable(Variable.DOUBLE, Math.Ceiling(a[0].Value())))},
                {"floor", new Function(Variable.DOUBLE, "floor", 1, (a) => new Variable(Variable.DOUBLE, Math.Floor(a[0].Value())))},
                {"round", new Function(Variable.DOUBLE, "round", 1, (a) => new Variable(Variable.DOUBLE, Math.Round(a[0].Value())))},
                {"sign", new Function(Variable.DOUBLE, "sign", 1, (a) => new Variable(Variable.DOUBLE, Math.Sign(a[0].Value())))},
                {"rand", new Function(Variable.FLOAT, "rand", 1, (a) => new Variable(Variable.DOUBLE, MathUtil.RandomFloat(a[0].Value())))},

                {"PerformAction", new Function(Variable.DOUBLE, "PerformAction", 4, (a) => 
                    new Variable(Variable.DOUBLE, a[0].Value() + a[1].Value() + a[2].Value() + a[3].Value()))}
            };

            variables = new ScopedManager<Variable>();
            functions = new ScopedManager<Function>();

            random = new Random();
        }

        public static void AdvanceScope() {
            variables.AdvanceScope();
            functions.AdvanceScope();
        }

        public static void AdvanceScope(Dictionary<string, Variable> vars, Dictionary<string, Function> funcs) {
            variables.AdvanceScope(vars);
            functions.AdvanceScope(funcs);
        }

        public static void DecreaseScope() {
            variables.DecreaseScope();
            functions.DecreaseScope();
        }

        public static bool IsDefined(string name) {
            return (
                globalVariables.ContainsKey(name) || globalFunctions.ContainsKey(name)
                || variables.Contains(name) || functions.Contains(name));
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

        public static bool ContainsVariable(string name) {
            return globalVariables.ContainsKey(name) || variables.Contains(name);
        }

        public static void AddVariable(string name, Variable variable) {
            if (ContainsVariable(name)) {
                throw new Exception("Variable " + name + " is already defined");
            }

            variables.AddAttribute(name, variable);
        }

        public static void ModifyVariable(string name, dynamic value) {
            GetVariable(name).SetValue(value);
        }

        public static Variable GetVariable(string name) {
            if (globalVariables.ContainsKey(name)) {
                return globalVariables[name];
            } else {
               return variables.GetAttribute(name);
            }
        }

        public static Variable LookupVariable(Variable a, Variable b) {
            if (a.type != Variable.OBJECT) {
                throw new Exception("Cannot lookup property on a non-object type");
            }
            
            if(b.type != Variable.STRING) {
                throw new ArgumentException();
            }

            if (((ProgrammableObject)a.Value()).ContainsVariable((string)b.Value())) {
                return ((ProgrammableObject)a.Value()).GetVariable((string)b.Value());
            } else {
                throw new Exception("Property does not exist");
            }
        }

        public static bool ContainsFunction(string name) {
            return globalFunctions.ContainsKey(name) || functions.Contains(name);
        }

        public static Function GetFunction(string name) {
            if (globalFunctions.ContainsKey(name)) {
                return globalFunctions[name];
            } else {
                return functions.GetAttribute(name);
            }
        }
    }
}

using Complexity.Interfaces;
using Complexity.Managers;
using Complexity.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complexity.Programming {
    public abstract class ProgrammableObject : Recalculatable {
        protected Dictionary<string, Variable> variables;
        protected Dictionary<string, Function> functions;

        public ProgrammableObject() {
            variables = new Dictionary<string, Variable>();
            functions = new Dictionary<string, Function>();
        }

        public Dictionary<string, Variable> GetVariables() {
            return variables;
        }

        public Dictionary<string, Function> GetFunctions() {
            return functions;
        }

        public bool ContainsVariable(string name) {
            return variables.ContainsKey(name);
        }

        public bool ContainsFunction(string name) {
            return functions.ContainsKey(name);
        }

        public Variable GetVariable(string name) {
            return variables[name];
        }

        public Function GetFunction(string name) {
            return functions[name];
        }

        public void AddVaraible(string name, Variable variable) {
            variables.Add(name, variable);
        }

        public void AddFunction(string name, Function function) {
            functions.Add(name, function);
        }

        public abstract void Compile();
        public abstract void Recalculate();
        public abstract bool HasChildren();
        public abstract List<Object3> GetChildren();
    }
}

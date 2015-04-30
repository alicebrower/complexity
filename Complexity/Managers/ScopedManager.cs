using Complexity.Objects.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complexity.Managers {
    /// <summary>
    /// This class manages scoped values
    /// </summary>
    public class ScopedManager<T> {
        Stack<Dictionary<string, T>> scope;

        public ScopedManager() {
            scope = new Stack<Dictionary<string, T>>();
        }

        public ScopedManager(Dictionary<string, T> initialItems) : this() {
            scope.Push(initialItems);
        }

        public void AdvanceScope() {
            scope.Push(new Dictionary<string, T>());
        }

        public void AdvanceScope(Dictionary<string, T> attributes) {
            scope.Push(attributes);
        }

        public void DecreaseScope() {
            scope.Pop();
        }

        public void AddAttribute(string name, T attr) {
            if (scope.Count > 0) {
                if(scope.Peek().ContainsKey(name)) {
                    throw new Exception("Value already exists");
                }
                scope.Peek().Add(name, attr);
            } else {
                throw new Exception("Must advance scope at least once before adding values!");
            }
        }

        public void ModifyAttribute(string name, T attr) {
            if (scope.Peek().ContainsKey(name)) {
                scope.Peek()[name] = attr;
            } else {
                throw new Exception("Cannot modify nonexistant attribute, " + name);
            }
        }

        public T GetAttribute(string name) {
            return scope.Peek()[name];
        }

        public bool Contains(string name) {
            if (scope.Count < 1) {
                return false;
            } else {
                return scope.Peek().ContainsKey(name);
            }
        }
    }
}

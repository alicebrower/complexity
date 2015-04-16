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
        //A stack of arraylists of strings which reference a stack
        //It might be better, memory wise, to use a linked list
        Stack<ArrayList> popStacks;
        //The actual lookup dictionary
        Dictionary<string, Stack<T>> values;

        public ScopedManager() {
            popStacks = new Stack<ArrayList>();
            values = new Dictionary<string, Stack<T>>();
        }

        public ScopedManager(Dictionary<string, T> initialItems) : this() {
            foreach (KeyValuePair<string, T> kvp in initialItems) {
                if (values.ContainsKey(kvp.Key)) {
                    values[kvp.Key].Push(kvp.Value);
                } else {
                    values[kvp.Key] = new Stack<T>();
                    values[kvp.Key].Push(kvp.Value);
                }
            }
        }

        public void AdvanceScope() {
            popStacks.Push(new ArrayList(0));
        }

        public void DecreaseScope() {
            //popStacks maintains references to stacks which contain variables that need
            //to be removed upon decreasing scope. Each item in the stack represents a level
            //of scope. 
            ArrayList pops = popStacks.Pop();
            foreach (string stack in pops) {
                if (values.ContainsKey(stack)) {
                    values[stack].Pop();
                    if (values[stack].Count < 1) {
                        values.Remove(stack);
                    }
                }
            }
        }

        public void AddAttribute(string name, T attr) {
            if (popStacks.Count > 0) {
                popStacks.Peek().Add(name);

                if (values.ContainsKey(name)) {
                    values[name].Push(attr);
                } else {
                    Stack<T> attrStack = new Stack<T>();
                    attrStack.Push(attr);
                    values.Add(name, attrStack);
                }
            } else {
                throw new Exception("Must advance scope at least once before adding values");
            }
        }

        public void ModifyAttribute(string name, T attr) {
            if (values.ContainsKey(name)) {
                if (values[name].Count > 0) {
                    values[name].Pop();
                    values[name].Push(attr);
                } else {
                    AddAttribute(name, attr);
                }
            } else {
                throw new Exception("Cannot modify nonexistant attribute, " + name);
            }
        }

        public T GetAttribute(string name) {
            return values[name].Peek();
        }

        public bool Contains(string name) {
            return values.ContainsKey(name);
        }
    }
}

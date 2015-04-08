using Complexity.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Complexity.Math_Things {
    public class ExpressionF {
        #region Static Expression MGMT

        private static readonly int LEFT_ASSOC = 0;
        private static readonly int RIGHT_ASSOC = 1;

        private static readonly string OPS_REGEX;
        private static readonly string VAR_REGEX = "[a-zA-Z_]+[0-9]*[a-zA-Z_]*";
        private static readonly string DEL_REGEX = "[\\(\\)]";
        private static Stack<Dictionary<string, SymbolF>> scope;
        private static HashSet<string> RESERVED;
        private static Dictionary<string, SymbolF> SymbolFS, FUNCTIONS, OPERATORS;

        static ExpressionF() {
            scope = new Stack<Dictionary<string, SymbolF>>(); 

            SymbolFS = new Dictionary<string, SymbolF>() {
                {"time", new SymbolF("time", false, 0, LEFT_ASSOC, 0, (a) => (float)Global.GetElapsedTime())},
                {"pi", new SymbolF("pi", false, 0, LEFT_ASSOC, 0, (a) => (float)Math.PI)},
                {"e", new SymbolF("e", false, 0, LEFT_ASSOC, 0, (a) => (float)Math.E)}
            };

            RESERVED = new HashSet<string>();

            FUNCTIONS = new Dictionary<string, SymbolF>() {
                {"sin", new SymbolF("sin", true, 1, LEFT_ASSOC, 10, (a) => (float)Math.Sin(a[0]))},
                {"cos", new SymbolF("cos", true, 1, LEFT_ASSOC, 10, (a) => (float)Math.Cos(a[0]))},
                {"tan", new SymbolF("tan", true, 1, LEFT_ASSOC, 10, (a) => (float)Math.Tan(a[0]))},
                {"asin", new SymbolF("asin", true, 1, LEFT_ASSOC, 10, (a) => (float)Math.Asin(a[0]))},
                {"acos", new SymbolF("acos", true, 1, LEFT_ASSOC, 10, (a) => (float)Math.Acos(a[0]))},
                {"atan", new SymbolF("atan", true, 1, LEFT_ASSOC, 10, (a) => (float)Math.Atan(a[0]))},
                {"sinh", new SymbolF("sinh", true, 1, LEFT_ASSOC, 10, (a) => (float)Math.Sinh(a[0]))},
                {"cosh", new SymbolF("cosh", true, 1, LEFT_ASSOC, 10, (a) => (float)Math.Cosh(a[0]))},
                {"tanh", new SymbolF("tanh", true, 1, LEFT_ASSOC, 10, (a) => (float)Math.Tanh(a[0]))},
                {"sqrt", new SymbolF("sqrt", true, 1, LEFT_ASSOC, 10, (a) => (float)Math.Sqrt(a[0]))},
                {"log", new SymbolF("log", true, 1, LEFT_ASSOC, 10, (a) => (float)Math.Log(a[0]))},

                {"rad", new SymbolF("rad", true, 1, LEFT_ASSOC, 10, (a) => (float)(a[0] * Math.PI / 180.0))},
                {"deg", new SymbolF("deg", true, 1, LEFT_ASSOC, 10, (a) => (float)(a[0] * 180.0 / Math.PI))},

                {"abs", new SymbolF("abs", true, 1, LEFT_ASSOC, 10, (a) => Math.Abs(a[0]))},
                {"ceil", new SymbolF("ceil", true, 1, LEFT_ASSOC, 10, (a) => (float)Math.Ceiling(a[0]))},
                {"floor", new SymbolF("floor", true, 1, LEFT_ASSOC, 10, (a) => (float)Math.Floor(a[0]))},
                {"round", new SymbolF("round", true, 1, LEFT_ASSOC, 10, (a) => (float)Math.Round(a[0]))},
                {"sign", new SymbolF("sign", true, 1, LEFT_ASSOC, 10, (a) => Math.Sign(a[0]))},
                {"rand", new SymbolF("rand", true, 1, LEFT_ASSOC, 10, (a) => (float)MathUtil.RandomFloat(a[0]))}
            };

            OPERATORS = new Dictionary<string, SymbolF>() {
                {"+", new SymbolF("+", true, 2, LEFT_ASSOC, 1, (a) => a[1] + a[0])},
                {"-", new SymbolF("-", true, 2, LEFT_ASSOC, 1, (a) => a[1] - a[0])},
                {"*", new SymbolF("*", true, 2, LEFT_ASSOC, 5, (a) => a[1] * a[0])},
                {"/", new SymbolF("/", true, 2, LEFT_ASSOC, 5, (a) => a[1] / a[0])},
                {"%", new SymbolF("%", true, 2, LEFT_ASSOC, 5, (a) => a[1] % a[0])},
                {"^", new SymbolF("^", true, 2, RIGHT_ASSOC, 9, (a) => (float)Math.Pow((double)a[1],(double)a[0]))}
            };

            //Assemble operator regex
            OPS_REGEX = "";
            foreach (KeyValuePair<string, SymbolF> entry in OPERATORS) {
                OPS_REGEX += Regex.Escape(entry.Key) + "|";
            }
            OPS_REGEX = OPS_REGEX.Substring(0, OPS_REGEX.Length - 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void AddSymbol(string name, float value) {
            if (RESERVED.Contains(name)) {
                throw new Exception("Can not add Symbol " + name + ", it is reserved.");
            }
            SymbolFS.Add(name, new SymbolF(name, value));
        }

        public static void SetSymbolValue(string name, float value) {
            SymbolFS[name] = new SymbolF(name, value);
        }

        public static float GetSymbolValue(string name) {
            if (RESERVED.Contains(name)) {
                return GetScopedSymbol(name);
            } else {
                return SymbolFS[name].eval(null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public static void RemoveSymbol(string name) {
            SymbolFS.Remove(name);
        }

        public static void ReserveSymbol(string name) {
            RESERVED.Add(name);
        }

        public static void AdvanceScope() {
            scope.Push(new Dictionary<string, SymbolF>());
        }

        public static void DecreaseScope() {
            scope.Pop();
        }

        public static void AddScopedSymbol(string name, float value) {
            scope.Peek().Add(name, new SymbolF(value));
        }

        public static void SetScopedSymbol(string name, float value) {
            bool found = false;
            Stack<Dictionary<string, SymbolF>> _scope = new Stack<Dictionary<string, SymbolF>>();

            while (scope.Peek() != null && !found) {
                if (scope.Peek().ContainsKey(name)) {
                    scope.Peek()[name] = new SymbolF(name, value);
                    found = true;
                } else {
                    _scope.Push(scope.Pop());
                }
            }

            //put all the dictionaries back on
            while (_scope.Count > 0) {
                scope.Push(_scope.Pop());
            }

            if (!found) {
                throw new Exception("SymbolF " + name + " cannot be found");
            }
        }

        public static float GetScopedSymbol(string name) {
            bool found = false;
            float value = 0;
            Stack<Dictionary<string, SymbolF>> _scope = new Stack<Dictionary<string, SymbolF>>();

            while (scope.Peek() != null && !found) {
                if (scope.Peek().ContainsKey(name)) {
                    value = scope.Peek()[name].eval(null);
                    found = true;
                } else {
                    _scope.Push(scope.Pop());
                }
            }

            //put all the dictionaries back on
            while (_scope.Count > 0) {
                scope.Push(_scope.Pop());
            }

            if (!found) {
                throw new Exception("SymbolF " + name + " cannot be found");
            }

            return value;
        }

        #endregion

        private CloneableStack<SymbolF> expression;
        private string infix;

        public ExpressionF(string infix) {
            this.infix = infix;
            expression = InfixToRPN(Parse(infix));
        }

        /// <summary>
        /// Test associativity
        /// </summary>
        /// <param name="token">Token name</param>
        /// <param name="type">Associativity to test</param>
        /// <returns></returns>
        private bool IsAssociative(string token, int type) {
            if (!IsOperator(token)) {
                throw new Exception("Invalid token: " + token);
            }

            if (ToSymbolF(token).assoc == type) {
                return true;
            }

            return false;
        }

        //Compare precedence of two operators
        private int CmpPrecedence(string token1, string token2) {
            if (!IsOperator(token1) || !IsOperator(token2)) {
                throw new Exception("Invalid tokens: " + token1 + " " + token2);
            }
            return ToSymbolF(token1).precedence - ToSymbolF(token2).precedence;
        }

        /// <summary>
        /// Converts an infix expression represented an array of strings to a
        /// corresponding RPN expression as a stack.
        /// </summary>
        /// <param name="inputTokens"></param>
        /// <returns></returns>
        private CloneableStack<SymbolF> InfixToRPN(string[] inputTokens) {
            //ArrayList outList = new ArrayList();
            CloneableStack<SymbolF> result = new CloneableStack<SymbolF>(0);
            Stack<string> stack = new Stack<string>();

            //for all the input tokens read the next token
            foreach (string token in inputTokens) {
                if (IsOperator(token)) {
                    //If token is an operator
                    while (stack.Count != 0 && IsOperator(stack.Peek())) {
                        if ((IsAssociative(token, LEFT_ASSOC) && CmpPrecedence(token, stack.Peek()) <= 0)
                            || (IsAssociative(token, RIGHT_ASSOC) && CmpPrecedence(token, stack.Peek()) < 0)) {
                            result.Push(ToSymbolF(stack.Pop()));
                            continue;
                        }
                        break;
                    }
                    //Push the new operator on the stack
                    stack.Push(token);
                } else if (token.Equals("(")) {
                    stack.Push(token);
                } else if (token.Equals(")")) {
                    while (stack.Count != 0 && !stack.Peek().Equals("(")) {
                        result.Push(ToSymbolF(stack.Pop()));
                    }
                    stack.Pop();
                } else {
                    result.Push(ToSymbolF(token));
                }
            }

            while (stack.Count != 0) {
                result.Push(ToSymbolF(stack.Pop()));
            }

            CloneableStack<SymbolF> actualResult = new CloneableStack<SymbolF>(result.Count());
            while (result.Count() > 0) {
                actualResult.Push(result.Pop());
            }
            return actualResult;
        }

        public void SetInfix(string infix) {
            this.infix = infix;
            expression = InfixToRPN(Parse(infix));
        }

        public string[] Parse(string input) {
            //Trim whitepace, there has to be a better way
            input = input.Replace(" ", "");
            input = input.Replace("\t", "");
            input = input.Replace("\n", "");
            string[] tokens = Regex.Split(input, @"(" + VAR_REGEX + "|" + DEL_REGEX + "|" + OPS_REGEX + ")");

            //Remove blanks
            ArrayList _tokens = new ArrayList();
            foreach (string token in tokens) {
                if (token.CompareTo("") != 0) {
                    _tokens.Add(token);
                }
            }

            tokens = (string[])(_tokens.ToArray(typeof(string)));
            _tokens = new ArrayList();
            int parens = 0;

            //Syntax & Symantics
            for(int i = 0; i < tokens.Length; i++) {
                if(OPERATORS.ContainsKey(tokens[i])) {
                    if (i == tokens.Length - 1) {
                        throw new Exception("Invalid expression format");
                    }

                    if (tokens[i].CompareTo("-") == 0) {
                        if(i == 0) {
                            _tokens.Add("-1");
                            _tokens.Add("*");
                        } else if (tokens[i + 1].CompareTo("-") == 0) {
                            _tokens.Add("+");
                            i++;
                        } else if (IsNumeric(tokens[i + 1])) {
                            _tokens.Add("+");
                            _tokens.Add("-" + tokens[i + 1]);
                            i++;
                        } else if (IsVariable(tokens[i + 1]) || IsDelimiter(tokens[i + 1]) || IsFunction(tokens[i + 1])) {
                            _tokens.Add("+");
                            _tokens.Add("-1");
                            _tokens.Add("*");
                        } else {
                            _tokens.Add("-");
                        }
                    } else if (tokens[i].CompareTo("+") == 0) {
                        if (i == 0 || tokens[i + 1].CompareTo("-") == 0) {
                            continue;
                        }  else {
                            _tokens.Add("+");
                        }
                    } else {
                        _tokens.Add(tokens[i]);
                    }
                } else if (tokens[i].CompareTo("(") == 0) {
                    _tokens.Add(tokens[i]);
                    parens++;
                } else if (tokens[i].CompareTo(")") == 0) {
                    if (i < tokens.Length - 1
                        && (tokens[i + 1].CompareTo("(") == 0 
                            || FUNCTIONS.ContainsKey(tokens[i + 1])
                            || SymbolFS.ContainsKey(tokens[i + 1]))) {

                        _tokens.Add(tokens[i]);
                        _tokens.Add("*");
                    } else {
                        _tokens.Add(tokens[i]);
                    }
                    parens--;
                } else if (IsNumeric(tokens[i]) || IsVariable(tokens[i])) {
                    if (i < tokens.Length - 1 && !OPERATORS.ContainsKey(tokens[i + 1]) && tokens[i + 1].CompareTo(")") != 0) {
                        _tokens.Add(tokens[i]);
                        _tokens.Add("*");
                    } else {
                        _tokens.Add(tokens[i]);
                    }
                } else if (IsFunction(tokens[i])) {
                    _tokens.Add(tokens[i]);
                } else if (tokens[i].CompareTo("") == 0) {
                } else {
                    throw new Exception("Undefined SymbolF " + tokens[i]);
                }
            }

            if (parens != 0) {
                throw new Exception("Cannot compile expression, incorrect parethesis format");
            }

            return (string[])_tokens.ToArray(typeof(string));
        }

        /// <summary>
        /// Attempts to either look up the SymbolF or convert it to one
        /// </summary>
        /// <param name="SymbolF"></param>
        /// <returns></returns>
        private SymbolF ToSymbolF(string SymbolF) {
            if (SymbolFS.ContainsKey(SymbolF)) {
                return SymbolFS[SymbolF];
            } else if (OPERATORS.ContainsKey(SymbolF)) {
                return OPERATORS[SymbolF];
            } else if (FUNCTIONS.ContainsKey(SymbolF)) {
                return FUNCTIONS[SymbolF];
            } else if (IsNumeric(SymbolF)) {
                return new SymbolF(float.Parse(SymbolF));
            } else if (IsVariable(SymbolF)) {
                return new SymbolF(SymbolF);
            }

            throw new Exception("ToSymbolF : " + SymbolF + " cannot be converted to a SymbolF");
        }

        /// <summary>
        /// Test if is an operator
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private bool IsOperator(string token) {
            return (OPERATORS.ContainsKey(token) || FUNCTIONS.ContainsKey(token));
        }

        private bool IsDelimiter(string token) {
            if (token.CompareTo("") == 0) {
                return false;
            }
            return (Regex.Match(token, "^" + DEL_REGEX + "$") != null); 
        }

        /// <summary>
        /// Tests if the given input can be parsed to a float
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool IsNumeric(string s) {
            try {
                float d = float.Parse(s);
                return true;
            } catch (Exception ex) {
                return false;
            }
        }

        /// <summary>
        /// Check if the symbol is a user defined variable
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool IsVariable(string s) {
            if (s.CompareTo("") == 0) {
                return false;
            }
            return ( (Regex.Match(s, "^" + VAR_REGEX + "$") != null) && !FUNCTIONS.ContainsKey(s));
        }

        private bool IsFunction(string token) {
            return FUNCTIONS.ContainsKey(token);
        }

        /// <summary>
        /// Tests if a given input is contained in any of the predefined SymbolF Dictionaries
        /// </summary>
        /// <param name="SymbolF"></param>
        /// <returns></returns>
        private bool IsDefined(string SymbolF) {
            return (SymbolFS.ContainsKey(SymbolF)
                || OPERATORS.ContainsKey(SymbolF)
                || FUNCTIONS.ContainsKey(SymbolF));
        }

        /// <summary>
        /// Evaluates the expression
        /// </summary>
        /// <returns>The result of evaluation</returns>
        public float Evaluate() {
            //Create a clone of expression because we don't want to mess with it
            CloneableStack<SymbolF> expr = expression.Clone();
            Stack<SymbolF> tempStack = new Stack<SymbolF>();
            float[] values;

            while (expr.Count() > 0) {
                if (!expr.Peek().isOperator) {
                    tempStack.Push(expr.Pop());
                } else {
                    //Pop values, tempStack should only be 0 op SymbolFs
                    values = new float[expr.Peek().nOps];
                    for (int i = 0; i < values.Length; i++) {
                        values[i] = tempStack.Pop().eval(null);
                    }
                    tempStack.Push(new SymbolF(expr.Pop().eval(values)));
                }
            }

            return tempStack.Pop().eval(null);
        }

        public string ToString() {
            return infix;
        }
    }

    public class SymbolF {
        public readonly string name;
        public readonly bool isOperator;
        public readonly int nOps, assoc, precedence;
        public readonly Eval eval;

        /// <summary>
        /// This is used to create a dynamic variable who's value and/or existence 
        /// is not known until runtime
        /// </summary>
        /// <param name="name"></param>
        public SymbolF(string name) {
            this.name = name;
            isOperator = false;
            nOps = 0;
            assoc = 0;
            precedence = 0;
            eval = (a) => ExpressionF.GetSymbolValue(name);
        }

        /// <summary>
        /// Simple constructor for numbers only
        /// </summary>
        /// <param name="value"></param>
        public SymbolF(float value) {
            name = "" + value;
            isOperator = false;
            nOps = 0;
            assoc = 0;
            precedence = 0;
            eval = (a) => value;
        }

        /// <summary>
        /// Convience constructor for creating variables with constant value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public SymbolF(string name, float value) {
            this.name = name;
            isOperator = false;
            nOps = 0;
            assoc = 0;
            precedence = 0;
            eval = (a) => value;
        }

        /// <summary>
        /// General Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isOperator"></param>
        /// <param name="noOps"></param>
        /// <param name="assoc"></param>
        /// <param name="precedence"></param>
        /// <param name="eval"></param>
        public SymbolF(string name, bool isOperator, int noOps, int assoc, int precedence, Eval eval) {
            this.name = name;
            this.isOperator = isOperator;
            this.nOps = noOps;
            this.assoc = assoc;
            this.precedence = precedence;
            this.eval = eval;
        }

        public delegate float Eval(float[] args);
    }
}

using Complexity.Managers;
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
        private static readonly string VAR_REGEX = "[a-zA-Z_]+([0-9]|[a-zA-Z_])*";
        private static readonly string DEL_REGEX = "[\\(\\)]";
        private static Dictionary<string, Symbol> FUNCTIONS, OPERATORS;

        static ExpressionF() {
            FUNCTIONS = new Dictionary<string, Symbol>() {
                {"sin", new Symbol("sin", true, 1, LEFT_ASSOC, 10, (a) => (float)Math.Sin(a[0]))},
                {"cos", new Symbol("cos", true, 1, LEFT_ASSOC, 10, (a) => (float)Math.Cos(a[0]))},
                {"tan", new Symbol("tan", true, 1, LEFT_ASSOC, 10, (a) => (float)Math.Tan(a[0]))},
                {"asin", new Symbol("asin", true, 1, LEFT_ASSOC, 10, (a) => (float)Math.Asin(a[0]))},
                {"acos", new Symbol("acos", true, 1, LEFT_ASSOC, 10, (a) => (float)Math.Acos(a[0]))},
                {"atan", new Symbol("atan", true, 1, LEFT_ASSOC, 10, (a) => (float)Math.Atan(a[0]))},
                {"sinh", new Symbol("sinh", true, 1, LEFT_ASSOC, 10, (a) => (float)Math.Sinh(a[0]))},
                {"cosh", new Symbol("cosh", true, 1, LEFT_ASSOC, 10, (a) => (float)Math.Cosh(a[0]))},
                {"tanh", new Symbol("tanh", true, 1, LEFT_ASSOC, 10, (a) => (float)Math.Tanh(a[0]))},
                {"sqrt", new Symbol("sqrt", true, 1, LEFT_ASSOC, 10, (a) => (float)Math.Sqrt(a[0]))},
                {"log", new Symbol("log", true, 1, LEFT_ASSOC, 10, (a) => (float)Math.Log(a[0]))},

                {"rad", new Symbol("rad", true, 1, LEFT_ASSOC, 10, (a) => (float)(a[0] * Math.PI / 180.0))},
                {"deg", new Symbol("deg", true, 1, LEFT_ASSOC, 10, (a) => (float)(a[0] * 180.0 / Math.PI))},

                {"abs", new Symbol("abs", true, 1, LEFT_ASSOC, 10, (a) => Math.Abs(a[0]))},
                {"ceil", new Symbol("ceil", true, 1, LEFT_ASSOC, 10, (a) => (float)Math.Ceiling(a[0]))},
                {"floor", new Symbol("floor", true, 1, LEFT_ASSOC, 10, (a) => (float)Math.Floor(a[0]))},
                {"round", new Symbol("round", true, 1, LEFT_ASSOC, 10, (a) => (float)Math.Round(a[0]))},
                {"sign", new Symbol("sign", true, 1, LEFT_ASSOC, 10, (a) => Math.Sign(a[0]))},
                {"rand", new Symbol("rand", true, 1, LEFT_ASSOC, 10, (a) => (float)MathUtil.RandomFloat(a[0]))}
            };

            OPERATORS = new Dictionary<string, Symbol>() {
                {"+", new Symbol("+", true, 2, LEFT_ASSOC, 1, (a) => a[1] + a[0])},
                {"-", new Symbol("-", true, 2, LEFT_ASSOC, 1, (a) => a[1] - a[0])},
                {"*", new Symbol("*", true, 2, LEFT_ASSOC, 5, (a) => a[1] * a[0])},
                {"/", new Symbol("/", true, 2, LEFT_ASSOC, 5, (a) => a[1] / a[0])},
                {"%", new Symbol("%", true, 2, LEFT_ASSOC, 5, (a) => a[1] % a[0])},
                {"^", new Symbol("^", true, 2, RIGHT_ASSOC, 9, (a) => (float)Math.Pow((double)a[1],(double)a[0]))}
            };

            //Assemble operator regex
            OPS_REGEX = "";
            foreach (KeyValuePair<string, Symbol> entry in OPERATORS) {
                OPS_REGEX += Regex.Escape(entry.Key) + "|";
            }
            OPS_REGEX = OPS_REGEX.Substring(0, OPS_REGEX.Length - 1);
        }

        #endregion

        private CloneableStack<Symbol> expression;
        private Dictionary<string, Symbol> symbols;
        private string infix;

        public ExpressionF(string infix) {
            this.infix = infix;
            symbols = new Dictionary<string, Symbol>();
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

            if (ToSymbol(token).assoc == type) {
                return true;
            }

            return false;
        }

        //Compare precedence of two operators
        private int CmpPrecedence(string token1, string token2) {
            if (!IsOperator(token1) || !IsOperator(token2)) {
                throw new Exception("Invalid tokens: " + token1 + " " + token2);
            }
            return ToSymbol(token1).precedence - ToSymbol(token2).precedence;
        }

        /// <summary>
        /// Converts an infix expression represented an array of strings to a
        /// corresponding RPN expression as a stack.
        /// </summary>
        /// <param name="inputTokens"></param>
        /// <returns></returns>
        private CloneableStack<Symbol> InfixToRPN(string[] inputTokens) {
            //ArrayList outList = new ArrayList();
            CloneableStack<Symbol> result = new CloneableStack<Symbol>(0);
            Stack<string> stack = new Stack<string>();

            //for all the input tokens read the next token
            foreach (string token in inputTokens) {
                if (IsOperator(token)) {
                    //If token is an operator
                    while (stack.Count != 0 && IsOperator(stack.Peek())) {
                        if ((IsAssociative(token, LEFT_ASSOC) && CmpPrecedence(token, stack.Peek()) <= 0)
                            || (IsAssociative(token, RIGHT_ASSOC) && CmpPrecedence(token, stack.Peek()) < 0)) {
                            result.Push(ToSymbol(stack.Pop()));
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
                        result.Push(ToSymbol(stack.Pop()));
                    }
                    stack.Pop();
                } else {
                    result.Push(ToSymbol(token));
                }
            }

            while (stack.Count != 0) {
                result.Push(ToSymbol(stack.Pop()));
            }

            CloneableStack<Symbol> actualResult = new CloneableStack<Symbol>(result.Count());
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
                    if (tokens[i + 1].CompareTo("-") == 0) {
                        _tokens.Add(tokens[i]);
                        _tokens.Add("0");
                    } else {
                        _tokens.Add(tokens[i]);
                    }
                    parens++;
                } else if (tokens[i].CompareTo(")") == 0) {
                    if (i < tokens.Length - 1
                        && (tokens[i + 1].CompareTo("(") == 0 
                            || FUNCTIONS.ContainsKey(tokens[i + 1])
                            || symbols.ContainsKey(tokens[i + 1]))) {

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
                    throw new Exception("Undefined Symbol " + tokens[i]);
                }
            }

            if (parens != 0) {
                throw new Exception("Cannot compile expression, incorrect parethesis format");
            }

            return (string[])_tokens.ToArray(typeof(string));
        }

        public void AddSymbol(string symbol) {
            symbols.Add(symbol, new Symbol(symbol));
        }

        public void AddSymbol(string symbol, float value) {
            symbols.Add(symbol, new Symbol(symbol, value));
        }

        public void SetSymbol(string symbol, float value) {
            symbols[symbol] = new Symbol(symbol, value);
        }

        public void RemoveSymbol(string symbol) {
            symbols.Remove(symbol);
        }

        /*
        private void AddSymbol(string symbol, float value) {
            symbols.Add(symbol, new Symbol(symbol, value));
        }

        private void SetSymbolValue(string symbol, float value) {
            symbols[symbol] = new Symbol(symbol, value);
        }

        private void RemoveSymbol(string symbol) {
            symbols.Remove(symbol);
        }
        */

        /// <summary>
        /// Attempts to either look up the Symbol or convert it to one
        /// </summary>
        /// <param name="Symbol"></param>
        /// <returns></returns>
        private Symbol ToSymbol(string symbol) {
            if (symbols.ContainsKey(symbol)) {
                return symbols[symbol];
            } else if (OPERATORS.ContainsKey(symbol)) {
                return OPERATORS[symbol];
            } else if (FUNCTIONS.ContainsKey(symbol)) {
                return FUNCTIONS[symbol];
            } else if (IsNumeric(symbol)) {
                return new Symbol(float.Parse(symbol));
            } else if (IsVariable(symbol)) {
                return new Symbol(symbol);
            }

            throw new Exception("ToSymbol : " + symbol + " cannot be converted to a Symbol");
        }

        private Symbol GetSymbol(string symbol) {
            if (symbols.ContainsKey(symbol)) {
                return symbols[symbol];
            } else if (symbols.ContainsKey(symbol)) {
                return symbols[symbol];
            } else {
                throw new Exception("Undefined symbol, " + symbol);
            }
        }

        private float GetSymbolValue(Symbol symbol) {
            if (symbol.isNumeric) {
                return symbol.eval(null);
            } else if (symbols.ContainsKey(symbol.name)) {
                return symbols[symbol.name].eval(null);
            } else {
                return ResourceManager.GetExprVal(symbol.name);
            }
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

            return ( Regex.IsMatch(s, "^" + VAR_REGEX + "$") && !FUNCTIONS.ContainsKey(s));
        }

        private bool IsFunction(string token) {
            return FUNCTIONS.ContainsKey(token);
        }

        /// <summary>
        /// Tests if a given input is contained in any of the predefined Symbol Dictionaries
        /// </summary>
        /// <param name="Symbol"></param>
        /// <returns></returns>
        private bool IsDefined(string Symbol) {
            return (symbols.ContainsKey(Symbol)
                || OPERATORS.ContainsKey(Symbol)
                || FUNCTIONS.ContainsKey(Symbol));
        }

        /// <summary>
        /// Evaluates the expression
        /// </summary>
        /// <returns>The result of evaluation</returns>
        public float Evaluate() {
            //Create a clone of expression because we don't want to mess with it
            CloneableStack<Symbol> expr = expression.Clone();
            Stack<Symbol> tempStack = new Stack<Symbol>();
            float[] values;

            while (expr.Count() > 0) {
                if (!expr.Peek().isOperator) {
                    tempStack.Push(expr.Pop());
                } else {
                    //Pop values, tempStack should only be 0 op SYMBOLS
                    values = new float[expr.Peek().nOps];
                    for (int i = 0; i < values.Length; i++) {
                        values[i] = GetSymbolValue(tempStack.Pop());
                    }
                    tempStack.Push(new Symbol(expr.Pop().eval(values)));
                }
            }

            return GetSymbolValue(tempStack.Pop());
        }

        public string ToString() {
            return infix;
        }

        protected class Symbol : VariableFloat {
            public readonly bool isOperator;
            public bool isNumeric = false;
            public readonly int nOps, assoc, precedence;

            public Symbol(string name)
                : base(name) {
                isOperator = false;
                nOps = 0;
                assoc = 0;
                precedence = 0;
            }

            /// <summary>
            /// Simple constructor for numbers only
            /// </summary>
            /// <param name="value"></param>
            public Symbol(float value)
                : base(value) {
                isOperator = false;
                nOps = 0;
                assoc = 0;
                precedence = 0;
                isNumeric = true;
            }

            /// <summary>
            /// Convience constructor for creating variables with constant value
            /// </summary>
            /// <param name="name"></param>
            /// <param name="value"></param>
            public Symbol(string name, float value)
                : base(name, value) {
                isOperator = false;
                isNumeric = true;
                nOps = 0;
                assoc = 0;
                precedence = 0;
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
            public Symbol(string name, bool isOperator, int noOps, int assoc, int precedence, Eval eval)
                : base(name, eval) {
                this.isOperator = isOperator;
                this.nOps = noOps;
                this.assoc = assoc;
                this.precedence = precedence;
            }
        }
    }
}

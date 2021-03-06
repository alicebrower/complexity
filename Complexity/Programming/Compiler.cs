﻿using Complexity.Managers;
using Complexity.Math_Things;
using Complexity.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Complexity.Programming {
    public class Compiler {
        public const int UNDEFINED = 0;
        public const int OPERATOR = 1;
        public const int DELIMITER = 2;
        public const int NUMBER = 3;
        public const int VARIABLE = 4;
        public const int FUNCTION = 5;
        public const int PROPERTY = 6;

        private static readonly int LEFT_ASSOC = 0;
        private static readonly int RIGHT_ASSOC = 1;

        private static readonly string OPS_REGEX;
        private static readonly string VAR_REGEX = "[a-zA-Z_]+([0-9]|[a-zA-Z_])*";
        private static readonly string DEL_REGEX = "[\\(\\)\\,]";
        private static Dictionary<string, ExpressionOperator> OPERATORS;
        private static Dictionary<string, ExpressionDelimeter> DELIMITERS;

        static Compiler() {
            OPERATORS = new Dictionary<string, ExpressionOperator>() {
                {"+", new ExpressionOperator("+", 2, LEFT_ASSOC, 1, (a) => new Variable(Variable.DOUBLE, a[1].Value() + a[0].Value()))},
                {"-", new ExpressionOperator("-", 2, LEFT_ASSOC, 1, (a) => new Variable(Variable.DOUBLE, a[1].Value() - a[0].Value()))},
                {"*", new ExpressionOperator("*", 2, LEFT_ASSOC, 5, (a) => new Variable(Variable.DOUBLE, a[1].Value() * a[0].Value()))},
                {"/", new ExpressionOperator("/", 2, LEFT_ASSOC, 5, (a) => new Variable(Variable.DOUBLE, a[1].Value() / a[0].Value()))},
                {"%", new ExpressionOperator("%", 2, LEFT_ASSOC, 5, (a) => new Variable(Variable.DOUBLE, a[1].Value() % a[0].Value()))},
                {"^", new ExpressionOperator("^", 2, RIGHT_ASSOC, 9,
                    (a) => new Variable(Variable.DOUBLE, Math.Pow(a[1].Value(), a[0].Value())))},

                {".", new ExpressionOperator(".", 2, LEFT_ASSOC, 9, (a) => ResourceManager.LookupVariable(a[1], a[0]))},
            };

            DELIMITERS = new Dictionary<string, ExpressionDelimeter>() {
                {"(", new ExpressionDelimeter("(")}, 
                {")", new ExpressionDelimeter(")")},
                {",", new ExpressionDelimeter(",")}
            };

            //Assemble operator regex
            OPS_REGEX = "";
            foreach (KeyValuePair<string, ExpressionOperator> entry in OPERATORS) {
                OPS_REGEX += Regex.Escape(entry.Key) + "|";
            }
            OPS_REGEX = OPS_REGEX.Substring(0, OPS_REGEX.Length - 1);
        }

        public static Program Compile(string input) {
            return new Program(Compile(Validate(Tokenize(input))));
        }

        private static List<ExpressionPart> Tokenize(string input) {
            //Trim whitepace, there has to be a better way
            input = input.Replace(" ", "");
            input = input.Replace("\t", "");
            input = input.Replace("\n", "");
            string[] _tokens = Regex.Split(input, @"(" + VAR_REGEX + "|" + DEL_REGEX + "|" + OPS_REGEX + ")");

            List<ExpressionPart> tokens = new List<ExpressionPart>();
            for (int i = 0; i < _tokens.Length; i++) {
                while (i < _tokens.Length && _tokens[i].Equals("")) { i++; }
                string token;
                if (i < _tokens.Length) {
                    token = _tokens[i];
                } else {
                    break;
                }

                //This is wrong, need to advance without modifying i
                //set lookaheads
                int j = i + 1;
                string lookAhead1;
                while (j < _tokens.Length && _tokens[j].Equals("")) { j++; }
                lookAhead1 = (j < _tokens.Length) ? _tokens[j] : lookAhead1 = "UNDEFINED";

                int k = j + 1;
                string lookAhead2;
                while (k < _tokens.Length && _tokens[k].Equals("")) { k++; }
                lookAhead2 = (k < _tokens.Length) ? _tokens[k] : lookAhead2 = "UNDEFINED";

                if (OPERATORS.ContainsKey(token)) {
                    if (token.Equals(".")) {
                        if (i == 0) {
                            if (Regex.IsMatch(lookAhead1, "^[0-9]*$")) {
                                tokens.Add(new ExpressionNumber(float.Parse("0" + token + lookAhead1)));
                                i = j;
                            } else {
                                throw new Exception("Invalid expression format");
                            }
                        } else if (Regex.IsMatch(lookAhead1, "^[0-9]*$")) {
                            tokens.Add(new ExpressionNumber(float.Parse("0" + token + lookAhead1)));
                            i = j;
                        } else if (Regex.IsMatch(lookAhead1, "^" + VAR_REGEX + "$")) {
                            tokens.Add(OPERATORS[token]);
                            tokens.Add(new ExpressionProperty(lookAhead1));
                            i = j;
                        }
                    } else {
                        tokens.Add(OPERATORS[token]);
                    }
                } else if (DELIMITERS.ContainsKey(token)) {
                    tokens.Add(DELIMITERS[token]);
                } else if (Regex.IsMatch(token, "^[0-9]*$")) {
                    if (lookAhead1.CompareTo(".") == 0) {
                        if (lookAhead2.Equals("UNDEFINED")) {
                            throw new Exception("Malformatted expression");
                        }

                        tokens.Add(new ExpressionNumber(
                            float.Parse(token + lookAhead1 + lookAhead2)));

                        i = k;
                    } else {
                        tokens.Add(new ExpressionNumber(float.Parse(token)));
                    }
                } else if (token.Equals("E")) {
                    tokens.Add(OPERATORS["*"]);
                    tokens.Add(new ExpressionNumber(10));
                    tokens.Add(OPERATORS["^"]);
                } else if (ResourceManager.ContainsVariable(token)) {
                    tokens.Add(new ExpressionVariable(token));
                } else if (ResourceManager.ContainsFunction(token)) {
                    tokens.Add(new ExpressionFunction(token, 0, null));
                } else {
                    throw new Exception("Unknown symbol, " + token);
                }
            }

            return tokens;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input">ArrayList of ExpressionParts</param>
        /// <returns></returns>
        private static List<ExpressionPart> Validate(List<ExpressionPart> input) {
            List<ExpressionPart> tokens = new List<ExpressionPart>();
            ExpressionPart token, lookAhead;
            int parens = 0;

            for (int i = 0; i < input.Count; i++) {
                token = (ExpressionPart)input[i];
                if (i < input.Count - 1) {
                    lookAhead = (ExpressionPart)input[i + 1];
                } else {
                    lookAhead = new ExpressionUndefined();
                }

                if (token.Type() == OPERATOR) {
                    if (i == input.Count - 1) {
                        throw new Exception("Invalid expression format");
                    }

                    if (token.name.CompareTo("-") == 0) {
                        if (i == 0) {
                            tokens.Add(new ExpressionNumber(-1));
                            tokens.Add(OPERATORS["*"]);
                        } else if (lookAhead.name.CompareTo("-") == 0) {
                            tokens.Add(token);
                            i++;
                        } else if (lookAhead.Type() == NUMBER) {
                            tokens.Add(new ExpressionNumber(float.Parse("-" + input[i + 1])));
                            i++;
                        } else if (lookAhead.Type() == VARIABLE || lookAhead.Type() == DELIMITER || lookAhead.Type() == FUNCTION) {
                            tokens.Add(OPERATORS["+"]);
                            tokens.Add(new ExpressionNumber(-1));
                            tokens.Add(OPERATORS["*"]);
                        } else {
                            tokens.Add(OPERATORS["-"]);
                        }
                    } else if (token.name.CompareTo("+") == 0) {
                        if (i == 0 || lookAhead.name.CompareTo("-") == 0) {
                            continue;
                        } else {
                            tokens.Add(OPERATORS["+"]);
                        }
                    } else {
                        tokens.Add(token);
                    }
                } else if (token.Type() == DELIMITER) {
                    if (token.name.CompareTo("(") == 0) {
                        if (lookAhead.name.CompareTo("-") == 0) {
                            tokens.Add(token);
                            tokens.Add(new ExpressionNumber(0));
                        } else {
                            tokens.Add(token);
                        }
                        parens++;
                    } else if (token.name.CompareTo(")") == 0) {
                        if (i < input.Count - 1
                            && (lookAhead.name.CompareTo("(") == 0
                                || lookAhead.Type() == FUNCTION
                                || lookAhead.Type() == VARIABLE)) {

                            tokens.Add(token);
                            tokens.Add(OPERATORS["*"]);
                        } else {
                            tokens.Add(token);
                        }
                        parens--;
                    } else {
                        tokens.Add(token);
                    }
                } else if (token.Type() == NUMBER || token.Type() == VARIABLE) {
                    if (i < input.Count - 1 && !lookAhead.Is(OPERATOR) && !lookAhead.Is(")") && !lookAhead.Is(",")) {
                        tokens.Add(token);
                        tokens.Add(OPERATORS["*"]);
                    } else if (lookAhead.name.Equals("-")) {
                        tokens.Add(OPERATORS["+"]);
                        tokens.Add(token);
                    } else {
                        tokens.Add(token);
                    }
                } else if (token.Type() == FUNCTION) {
                    tokens.Add(token);
                } else if (token.Is(PROPERTY)) {
                    tokens.Add(token);
                } else {
                    throw new Exception("Undefined Symbol " + token);
                }
            }

            if (parens != 0) {
                throw new Exception("Cannot compile expression, incorrect parethesis format");
            }

            return tokens;
        }

        /// <summary>
        /// Converts an infix expression represented an array of strings to a
        /// corresponding RPN expression as a stack.
        /// </summary>
        /// <param name="inputTokens"></param>
        /// <returns></returns>
        private static List<Symbol> Compile(List<ExpressionPart> input) {
            List<Symbol> result = new List<Symbol>(0);
            Stack<ExpressionPart> stack = new Stack<ExpressionPart>();

            foreach (ExpressionPart token in input) {
                if (token.Is(OPERATOR) || token.Is(FUNCTION)) {
                    while (stack.Count != 0 && (stack.Peek().Is(OPERATOR) || stack.Peek().Is(FUNCTION))) {
                        if ((IsAssoc(token, LEFT_ASSOC) && CmpPrec(token, stack.Peek()) <= 0)
                            || (IsAssoc(token, RIGHT_ASSOC) && CmpPrec(token, stack.Peek()) < 0)) {
                            result.Add(Link(stack.Pop()));
                            continue;
                        }
                        break;
                    }

                    stack.Push(token);
                } else if (token.name.Equals("(")) {
                    stack.Push(token);
                } else if (token.name.Equals(")")) {
                    while (stack.Count != 0 && !stack.Peek().name.Equals("(")) {
                        result.Add(Link(stack.Pop()));
                    }
                    stack.Pop();
                } else if (token.name.Equals(",")) {
                    continue;
                } else {
                    result.Add(Link(token));
                }
            }

            while (stack.Count != 0) {
                result.Add(Link(stack.Pop()));
            }

            return result;
        }

        private static Symbol Link(ExpressionPart token) {
            string exception = "Exception while linking symbols: ";
            switch (token.Type()) {
                case NUMBER:
                    return new Symbol(false, 0, (a) => new Variable(Variable.FLOAT, float.Parse(token.name)));
                case VARIABLE:
                    return new Symbol(false, 0, (a) => ResourceManager.GetVariable(token.name));
                case PROPERTY:
                    return new Symbol(false, 0, (a) => new Variable(Variable.STRING, token.name));
                case OPERATOR:
                    return new Symbol(true, OPERATORS[token.name].nOps, OPERATORS[token.name].eval);
                case FUNCTION:
                    return new Symbol(true, ResourceManager.GetFunction(token.name).argc, ResourceManager.GetFunction(token.name).Evaluate);
                case DELIMITER:
                    throw new Exception(exception + "delimiters should not be present");
                case UNDEFINED:
                    throw new Exception(exception + "undefined symbol type");
            }

            throw new Exception("Unable to link token, " + token.name);
        }

        /// <summary>
        /// Test associativity
        /// </summary>
        /// <param name="token">Token name</param>
        /// <param name="type">Associativity to test</param>
        /// <returns></returns>
        private static bool IsAssoc(ExpressionPart token, int type) {
            if (!(token.Is(OPERATOR) || token.Is(FUNCTION))) {
                throw new Exception("Invalid token: " + token);
            }

            if (((ExpressionOperator)token).assoc == type) {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Compare precedence of two operators
        /// </summary>
        /// <param name="token1"></param>
        /// <param name="token2"></param>
        /// <returns></returns>
        private static int CmpPrec(ExpressionPart token1, ExpressionPart token2) {
            if (!(token1.Is(OPERATOR) || token1.Is(FUNCTION))
                || !(token2.Is(OPERATOR) || token2.Is(FUNCTION))) {
                throw new Exception("Invalid tokens: " + token1 + ", " + token2);
            }
            return ((ExpressionOperator)token1).precedence - ((ExpressionOperator)token2).precedence;
        }

        protected class ExpressionPart {
            public readonly string name;
            protected int type = UNDEFINED;

            public ExpressionPart(string name) {
                this.name = name;
            }

            public int Type() {
                return type;
            }

            public override string ToString() {
                return name;
            }

            public bool Is(string s) {
                return name.CompareTo(s) == 0;
            }

            public bool Is(int type) {
                return this.type == type;
            }
        }

        protected class ExpressionUndefined : ExpressionPart {
            public ExpressionUndefined() : base("UNDEFINED") { }
        }

        protected class ExpressionProperty : ExpressionPart {
            public ExpressionProperty(string name)
                : base(name) {
                this.type = PROPERTY;
            }
        }

        protected class ExpressionOperator : ExpressionPart {
            public readonly int nOps, assoc, precedence;
            public Symbol.Eval eval;

            public ExpressionOperator(string name, int nOps, int assoc, int precedence, Symbol.Eval eval)
                : base(name) {
                this.nOps = nOps;
                this.assoc = assoc;
                this.precedence = precedence;
                this.eval = eval;
                type = OPERATOR;
            }
        }

        protected class ExpressionDelimeter : ExpressionPart {
            public ExpressionDelimeter(string name)
                : base(name) {
                type = DELIMITER;
            }
        }

        protected class ExpressionNumber : ExpressionPart {
            public float value;

            public ExpressionNumber(float value)
                : base("" + value) {
                this.value = value;
                type = NUMBER;
            }
        }

        protected class ExpressionVariable : ExpressionPart {
            public ExpressionVariable(string name)
                : base(name) {
                type = VARIABLE;
            }
        }

        protected class ExpressionFunction : ExpressionOperator {
            public ExpressionFunction(string name, int nOps, Symbol.Eval eval)
                : base(name, nOps, RIGHT_ASSOC, 10, eval) {
                type = FUNCTION;
            }
        }
    }
}

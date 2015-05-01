using Complexity.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complexity.Programming {
    public class Variable {
        public const ushort BOOL = 1;
        public const ushort BYTE = 2;
        public const ushort SHORT = 3;
        public const ushort INT = 4;
        public const ushort UINT = 5;
        public const ushort FLOAT = 6;
        public const ushort DOUBLE = 7;
        public const ushort LONG = 8;
        public const ushort ULONG = 9;
        public const ushort CHAR = 10;
        public const ushort STRING = 11;
        public const ushort OBJECT = 12;
        public const ushort METHOD = 13;
        public const ushort CLASS = 14;

        public readonly ushort type;
        private dynamic value;
        public ValueDelegate Value;

        public Variable(ushort type) {
            Value = ValueDefault;
            this.type = type;
        }

        public Variable(ushort type, dynamic value) {
            Value = ValueDefault;
            this.type = type;
            SetValue(value);
        }

        public void SetValue(dynamic value) {
            //Check type
            switch (type) {
                case BOOL:
                    this.value = (bool)value;
                    break;
                case BYTE:
                    this.value = (byte)value;
                    break;
                case SHORT:
                    this.value = (short)value;
                    break;
                case INT:
                    this.value = (int)value;
                    break;
                case UINT:
                    this.value = (uint)value;
                    break;
                case FLOAT:
                    this.value = (float)value;
                    break;
                case DOUBLE:
                    this.value = (double)value;
                    break;
                case LONG:
                    this.value = (long)value;
                    break;
                case ULONG:
                    this.value = (ulong)value;
                    break;
                case CHAR:
                    this.value = (char)value;
                    break;
                case STRING:
                    this.value = (string)value;
                    break;
                case OBJECT:
                    this.value = (object)value;
                    break;
                case METHOD:
                    this.value = (Function)value;
                    break;
                case CLASS:
                    throw new NotImplementedException("Class type not supported at this time");
                default:
                    throw new Exception("Variable has unknown type");
            }
        }

        public delegate dynamic ValueDelegate();

        public dynamic ValueDefault() {
            return value;
        }
    }
}

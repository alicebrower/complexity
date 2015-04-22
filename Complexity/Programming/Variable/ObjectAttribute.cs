using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complexity.Objects.Base {
    public class ObjectAttribute {
        public dynamic value;
        public bool inherit;
        public readonly bool removable;

        public ObjectAttribute() { }

        public ObjectAttribute(bool removable) {
            this.removable = removable;
        }

        public ObjectAttribute(dynamic value, bool inherit, bool removable) {
            this.value = value;
            this.inherit = inherit;
            this.removable = removable;
        }

        public ObjectAttribute Clone() {
            return (ObjectAttribute)MemberwiseClone();
        }
    }

    public class ObjectAttributeT<T> : ObjectAttribute {
        new public T value;

        public ObjectAttributeT() { }

        public ObjectAttributeT(bool removable)
            : base(removable) {
        }

        public ObjectAttributeT(T value, bool inherit, bool removable)
            : base(removable) {
            this.value = value;
            this.inherit = inherit;
        }

        public ObjectAttributeT<T> Clone() {
            return (ObjectAttributeT<T>)MemberwiseClone();
        }
    }
}

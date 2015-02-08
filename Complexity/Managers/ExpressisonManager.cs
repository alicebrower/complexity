using Complexity.Math_Things;
using Complexity.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complexity.Managers {
    public static class ExpressionManager {
        private static ArrayList expressions = new ArrayList();

        public static void Add(Recalculated expr) {
            expressions.Add(expr);
        }

        public static void Recalculate() {
            foreach (Recalculated expr in expressions) {
                expr.Recalculate();
            }
        }
    }
}

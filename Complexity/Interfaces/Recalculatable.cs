using Complexity.Programming;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complexity.Interfaces {
    public interface Recalculatable {
        /// <summary>
        /// Recalculates the object and returns an arraylist of any subObjs
        /// that need to be calculated as well.
        /// </summary>
        void Recalculate();
    }
}

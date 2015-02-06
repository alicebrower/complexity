using Complexity.Util;
using OpenTK.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complexity.Objects {
    /// <summary>
    /// A basic object with simple controls.
    /// </summary>
    public abstract class SimpleObject3 : Object3 {
        protected double[] color;

        public SimpleObject3(double[,] geometry) : base(geometry) {
        }

        public override void SetAttributes(Dictionary<string, string> args) {
            base.SetAttributes(args);
        }

        public override void SetColor(double[] color) {
            this.color = color;
        }

        protected override void Init() {
            color = new double[] { 255, 0, 255, 1 };
        }
    }
}

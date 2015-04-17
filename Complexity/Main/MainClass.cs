using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;
using Complexity.Objects;
using Complexity.Main;
using Complexity.Util;
using Complexity.Math_Things;
using Complexity.Managers;

namespace Complexity {
    /// <summary>
    /// For testing. When this is compiled as a library, this will be removed.
    /// </summary>
    public class MainClass {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static int Main(string[] args) {
            #region Objects
            //Create all the things
            Console.Write("Creating Objects... ");

            SimpleDot3 centerDot = new SimpleDot3(4);
            centerDot.SetScale(".05");
            centerDot.SetRotate("0", "0", "time");

            string t = GeometryBuilder.POLAR_THETA;
            System3 sys = new System3(GeometryBuilder.GraphPolar(
                new ExpressionF("sin(" + t + "*2)"), 0, 360, 1), centerDot);

            Scene scene = new Scene();
            //scene.Add(centerDot);
            scene.Add(sys);

            Console.WriteLine("Done.");
            #endregion

            //Create game universe
            Console.Write("Creating Universe... ");
            Universe u = new Universe();
            u.AddScene(scene);
            u.SetActiveScene(0);
            u.Begin();
            Console.WriteLine("Done.");

            while (true) {
                Console.ReadLine();
            }
            //return 0;
        }
    }
}

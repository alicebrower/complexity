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

namespace Complexity {
    /// <summary>
    /// For testing. When this is compiled as a library, this will be removed.
    /// </summary>
    class MainClass {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static int Main(string[] args) {
            #region Objects
            //Create all the things
            Console.Write("Creating Objects... ");

            SimpleDot3 centerDot = new SimpleDot3(30);
            centerDot.SetScale(".02");
            centerDot.SetColor(new string[] { "1", "1", "1", "1" });

            SimpleDot3 dot = new SimpleDot3(8);
            dot.SetScale(".02");
            dot.SetColor(new string[] { "sin(time+dist/length)^2", "0", "0" });

            System3 sys1 = new System3(GeometryBuilder.Circle(30), dot);
            sys1.SetScale(".2");

            System3 sys2 = new System3(GeometryBuilder.Grid(4, 4), sys1);
            sys2.SetScale("2");
            sys2.SetRotate(new string[] { "0", "0", "time"});

            Scene scene = new Scene();
            //scene.Add(dot);
            //scene.Add(pen);
            //scene.Add(cube);
            //scene.Add(sys);

            scene.Add(sys2);
            scene.Add(centerDot);

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

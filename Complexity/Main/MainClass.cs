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
            centerDot.SetScale("(-0.5 * time + 1) % 2");
            centerDot.SetColor(new string[] { "1", "1", "1", "1" });

            /*
            SimpleDot3 sysDot = new SimpleDot3(20);
            sysDot.SetColor(new string[] {"1", "0", "rand( dist * floor(time) )", "1"});
            sysDot.SetScale(".05");
            sysDot.AppendTransform(new MatrixTranslateAction(new VectorExpr(
                new string[] { "0", "0", "sin(time)*rand(dist)" })));

            System3 sys = new System3(GeometryBuilder.Grid(10, 10), sysDot);
            sys.SetScale(new string[] { "1", "2", "1" });
            sys.SetRotate(new string[] { "rad(70)", "0", "rad(30)" });

            System3 sys2 = new System3(GeometryBuilder.Grid(1, 4), sys);
            */

            Scene scene = new Scene();
            scene.Add(centerDot);
            //scene.Add(centerDot);
            //scene.Add(sys2);

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

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
using Complexity.Programming;

namespace Complexity {
    /// <summary>
    /// For testing
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

            Cube cube = new Cube();
            cube.SetColor(".75", "0", "(dist/parent.length+time)%1", "1");
            cube.SetRotate("0", "0", "rad(360*dist/parent.length) + parent.rotateZ");
            cube.SetScale(".2", ".5", ".2");

            System3 sys = new System3(GeometryBuilder.Circle(20), cube);
            sys.SetRotate("0", "0", "time/5");
            sys.SetScale("2");

            Scene scene = new Scene();
            scene.Add(sys);

            Console.WriteLine("Done.");
            #endregion

            //Create game universe
            Console.Write("Creating Universe... ");
            Universe.AddScene(scene);
            Universe.SetActiveScene(0);
            Universe.Begin();
            Console.WriteLine("Done.");

            while (true) {
                Console.ReadLine();
            }
            //return 0;
        }
    }
}

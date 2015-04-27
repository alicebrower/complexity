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
    /// For testing. When this is compiled as a library, this will be removed.
    /// </summary>
    public class MainClass {
        static int number = 0;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static int Main(string[] args) {
            Program p = Compiler.Compile(".5");
            Variable v = p.Run();

            #region Objects
            //Create all the things
            Console.Write("Creating Objects... ");

            Cube cube = new Cube();
            cube.SetColor("sin(time)^2", "0", "sin(time+pi/2)^2", "1");
            cube.SetRotate("rad(20)", "rad(45)+time", "0");
            cube.SetScale(".5", ".5", ".5");

            //System3 sys = new System3(new float[,]{{0, 0, 0}}, cube);

            Scene scene = new Scene();
            scene.Add(cube);

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

        public static int GetNumber() {
            return number++;
        }
    }
}

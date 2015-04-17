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
            ExpressionF expr = new ExpressionF("parent.color");

            #region Objects
            //Create all the things
            Console.Write("Creating Objects... ");

            Cube cube = new Cube();
            //cube.SetColor("sin(time)^2", "0", "sin(time+pi/2)^2", "1");
            cube.SetRotate("rad(-20)", "rad(45)+time", "0");
            //cube.SetScale("sin(time)+1", "1", "1");

            System3 system = new System3(new float[,] { {0}, {0}, {0} }, cube);

            Scene scene = new Scene();
            scene.Add(system);

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

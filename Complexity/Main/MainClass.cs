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

            //ComplexCube cube = new ComplexCube();
            //cube.SetScale("1/10");

            SimpleDot3 dot = new SimpleDot3(8);
            //string trans1 = "sin((time/5 + dist%20/20)*5) / 12";
            //dot.AppendTransform(new MatrixTranslateAction(
            //    new VectorExpr(new string[] {"0", trans1, "0"})));
            string trans2 = "( sin( (time/5 + floor(dist/40)/20) * 5 ) ) / 4";
            dot.AppendTransform(new MatrixTranslateAction(
                new VectorExpr(new string[] { "0", trans2, "0" })));

            string color0 = "0";
            string color1 = "sin(2 * time * rand(dist/length)         )^2";
            string color2 = "sin(2 * time * rand(dist/length) + 2/3*pi)^2";
            string color3 = "sin(2 * time * rand(dist/length) + 4/3*pi)^2";
            string color4 = "sin(time + dist/length)^2";
            dot.SetColor(new string[] { "1", color0, "1", "1" });

            string scale = ".01 + .01 * sin(time + dist/2%40)^2";
            dot.SetScale(new string[] { scale, scale, "1" });
            dot.SetScale(new string[] { ".01", ".01", "1" });
            

            //System testing
            System3 sys = new System3(GeometryBuilder.Grid(40, 40), dot);

            sys.SetScale(new string[] { "4", "4", "1"});
            sys.SetRotate(new string[] { "rad(70)", "0", "rad(45)"});
            
            sys.SetColor(new double[] { 0, 1, 1, 1});
            
            Pen3 pen = new Pen3(GeometryBuilder.Circle(60));

            /*
            Pen3 pen = new Pen3(GeometryBuilder.GraphPolar(new ExpressionD("sin(2*i)*2"), 0, 360, 2));
            pen.SetAttributes(new Dictionary<string, string> {
                {"speed", "2"}
            });
            //sin(time+dist/3)/2
            pen.SetAttributes(new Dictionary<string, string> {
                {"scale", ".05"},
                {"rcolor", "sin(time + dist/length)"},
                {"bcolor", "sin(time + dist/length + 2/3*pi)"},
                {"gcolor", "sin(time + dist/length + 4/3*pi)"}
            });
             * */
            Scene scene = new Scene();
            //scene.Add(dot);
            //scene.Add(pen);
            //scene.Add(cube);
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

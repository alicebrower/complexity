using Complexity.Managers;
using Complexity.Programming;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Complexity.Main {
    /// <summary>
    /// This class is responsible for maintaining an entire game state.
    /// </summary>
    public static class Universe {
        private static Scene activeScene;
        private static ArrayList scenes;
        private static RenderWindow renderWin;
        private static Stopwatch time = new Stopwatch();
        private static Random random = new Random();

        /// <summary>
        /// 
        /// </summary>
        [STAThread]
        private static void RunRenderWindow() {
            renderWin = new RenderWindow(activeScene);
            renderWin.Run(60.0);
        }

        static Universe() {
            scenes = new ArrayList();
        }

        /// <summary>
        /// Let there be light
        /// </summary>
        public static void Begin() {
            //Compile everything
            Compile(activeScene);

            //Render Thread, this is just preliminary stuff.
            //Hard coded for only one scene and one game window.
            //Logic to handle this can be added later
            if (scenes.Count > 0) {
                ExecutionManager.AddScene(activeScene);
                Thread thread = new Thread(new ThreadStart(RunRenderWindow));
                thread.Start();
            } else {
                Console.WriteLine("Universe: No scenes to render!");
            }

            time.Start();
        }

        private static void Compile(ProgrammableObject obj) {
            ResourceManager.AdvanceScope(obj.GetVariables(), obj.GetFunctions());

            obj.Compile();
            if (obj.HasChildren()) {
                foreach (ProgrammableObject pObj in obj.GetChildren()) {
                    Compile(pObj);
                }
            }

            ResourceManager.DecreaseScope();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int AddScene(Scene s) {
            return scenes.Add(s);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public static void SetActiveScene(int index) {
            activeScene = (Scene) scenes[index];
        }

        /// <summary>
        /// Returns time elapsed in seconds
        /// </summary>
        /// <returns></returns>
        public static double GetElapsedTime() {
            return ((double)time.ElapsedMilliseconds) / 1000.0;
        }

        public static int RandomInt() {
            return random.Next();
        }

        public static double RandomDouble() {
            return random.NextDouble();
        }
    }
}

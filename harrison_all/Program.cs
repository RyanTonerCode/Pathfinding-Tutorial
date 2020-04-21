using System;
using System.Diagnostics;

namespace PathfindingTutorial
{
    static partial class Program
    {

        private enum SUBPROGRAM { STACK_VS_QUEUE, MAKE_GRAPH, MAKE_WEIGHTED_GRAPH, MAKE_MAZE, SOLVE_SINGLE_PUZZLE, SOLVE_MANY_PUZZLES};

        static void Main(string[] args)
        {
            SUBPROGRAM sp = SUBPROGRAM.MAKE_MAZE;

            switch (sp)
            {
                case SUBPROGRAM.STACK_VS_QUEUE:
                    StackVsQueue();
                    break;
                case SUBPROGRAM.MAKE_GRAPH:
                    MakeGraph();
                    break;
                case SUBPROGRAM.MAKE_WEIGHTED_GRAPH:
                    MakeWeightedGraph();
                    break;
                case SUBPROGRAM.MAKE_MAZE:
                    MakeMaze();
                    break;
                case SUBPROGRAM.SOLVE_SINGLE_PUZZLE:
                    SolvePuzzle(false, true);
                    break;
                case SUBPROGRAM.SOLVE_MANY_PUZZLES:
                    Stopwatch sw = new Stopwatch();

                    sw.Start();

                    int numTrials = 1000;
                    for (int i = 0; i < numTrials; i++)
                        SolvePuzzle(false, numTrials == 1);

                    sw.Stop();

                    Console.WriteLine(sw.ElapsedMilliseconds);
                    break;
            }

            Console.ReadLine();
        }
    }
}

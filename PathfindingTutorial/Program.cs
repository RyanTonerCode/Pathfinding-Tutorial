using PathfindingTutorial.Puzzle;
using System;
using System.Diagnostics;
using PathfindingTutorial.Data_Structures;
using System.Collections.Generic;

namespace PathfindingTutorial
{
    static partial class Program
    {
        public const int MaxStackSize = 10000000;
        public const int MaxQueueSize = 10000000;
        public const int MaxHeapSize = 10000000;


        private enum SUBPROGRAM { STACK_VS_QUEUE, MAKE_GRAPH, MAKE_WEIGHTED_GRAPH, MAKE_MAZE, SOLVE_SINGLE_PUZZLE, SOLVE_MANY_PUZZLES, MAKE_FSM, MIN_SPANNING_TREE };


        static void Main(string[] args)
        {

            //Toggle this value to select which subprogram you want to run.
            SUBPROGRAM sp = SUBPROGRAM.MIN_SPANNING_TREE;

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
                case SUBPROGRAM.MAKE_FSM:
                    MakeFSM();
                    break;
                case SUBPROGRAM.MIN_SPANNING_TREE:
                    MakePrims();
                    break;
            }

            Console.ReadLine();
        }
    }
}

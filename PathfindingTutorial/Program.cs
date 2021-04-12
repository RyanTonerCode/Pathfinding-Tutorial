using System;
using System.Diagnostics;

namespace PathfindingTutorial
{
    static partial class Program
    {
        public const int MaxStackSize = 10000000;
        public const int MaxQueueSize = 10000000;
        public const int MaxHeapSize = 10000000;

        private enum SUBPROGRAM { 
            STACK_VS_QUEUE, 
            MAKE_GRAPH, MAKE_WEIGHTED_GRAPH, 
            MAKE_MAZE, 
            SOLVE_SINGLE_PUZZLE, 
            SOLVE_MANY_PUZZLES,
            MAKE_FSM, 
            MIN_SPANNING_TREE_PRIM, 
            MIN_SPANNING_TREE_KRUSKAL,
            FIND_COMPONENTS,
            PRUFER_TO_TREE,
            DEGREE_SEQUENCE_CONSTRUCTOR,
            IS_VALID_MINOR,
            ISOMORPHISM
        };

        static void Main(string[] args)
        {
            //Toggle this value to select which subprogram you want to run.
            var sp = SUBPROGRAM.IS_VALID_MINOR;

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
                    var sw = new Stopwatch();

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
                case SUBPROGRAM.MIN_SPANNING_TREE_PRIM:
                    MakePrims();
                    break;
                case SUBPROGRAM.MIN_SPANNING_TREE_KRUSKAL:
                    MakeKruskals();
                    break;
                case SUBPROGRAM.FIND_COMPONENTS:
                    MakeComponents();
                    break;
                case SUBPROGRAM.PRUFER_TO_TREE:
                    PruferToTree();
                    break;
                case SUBPROGRAM.DEGREE_SEQUENCE_CONSTRUCTOR:
                    MakeDegreeSequence();
                    break;
                case SUBPROGRAM.IS_VALID_MINOR:
                    IsValidMinor();
                    break;

                case SUBPROGRAM.ISOMORPHISM:
                    IsIsomorphic();
                    break;
            }

            Console.ReadLine();
        }
    }
}

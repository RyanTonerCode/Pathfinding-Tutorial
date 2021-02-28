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


        private enum SUBPROGRAM { TEST, STACK_VS_QUEUE, MAKE_GRAPH, MAKE_WEIGHTED_GRAPH, MAKE_MAZE, SOLVE_SINGLE_PUZZLE, SOLVE_MANY_PUZZLES, MAKE_FSM, MIN_SPANNING_TREE };


        static void Main(string[] args)
        {

            //Toggle this value to select which subprogram you want to run.
            SUBPROGRAM sp = SUBPROGRAM.TEST;

            switch (sp)
            {
                case SUBPROGRAM.TEST:
                    var A = new WeightedGraphNode<char>('A');
                    var B = new WeightedGraphNode<char>('B');
                    var C = new WeightedGraphNode<char>('C');
                    var D = new WeightedGraphNode<char>('D');
                    var E = new WeightedGraphNode<char>('E');
                    var F = new WeightedGraphNode<char>('F');

                    A.AddMutualNeighbor(B, 1);
                    A.AddMutualNeighbor(D, 2);
                    B.AddMutualNeighbor(D, 4);
                    B.AddMutualNeighbor(C, 2);
                    C.AddMutualNeighbor(D, 3);
                    C.AddMutualNeighbor(E, 4);
                    C.AddMutualNeighbor(F, 5);
                    E.AddMutualNeighbor(F, 1);

                    var G = new Graph<char>(A, B, C, D, E, F);

                    var g = G.KruskalsAlgorithmForMinimumSpanningForest();

                    var edges = g.SortedEdgeList();

                    foreach(var edge in edges)
                    {
                        Console.WriteLine(edge.ToString());
;                   }

                    break;
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

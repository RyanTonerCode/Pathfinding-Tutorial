using PathfindingTutorial.Data_Structures;
using System;
using System.Collections.Generic;

namespace PathfindingTutorial
{
    public partial class Program
    {
        static void PrintDegreeSequence(int[] prufer)
        {
            for (int i = 0; i < prufer.Length; i++)
            {
                Console.Write(prufer[i]);
                if (i < prufer.Length - 1)
                    Console.Write(",");
            }

            Console.WriteLine();
        }

        private static void MakeDegreeSequence()
        {
            int[] seq1 = { 7, 6, 6, 5, 4, 3, 2, 1 };
            int[] seq2 = { 6, 6, 6, 6, 6, 6, 6, 6, 6 };

            var degreeSequences = new List<int[]>()
            {
                seq1,
                seq2
            };

            foreach (var seq in degreeSequences)
            {
                PrintDegreeSequence(seq);
                var graph = Graph<int>.GenerateGraphForDegreeSequence(seq);
                if(graph == null)
                {
                    Console.WriteLine("Unable to make degree sequence");
                }
                else
                {
                    graph.PrintAdjacencyMatrix();
                }
            }

        }
    }
}

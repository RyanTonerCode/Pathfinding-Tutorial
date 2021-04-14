using PathfindingTutorial.Data_Structures;
using System;
using System.Collections.Generic;

namespace PathfindingTutorial
{
    public partial class Program
    {
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
                PrintArray(seq);
                var graph = Graph<int>.GenerateGraphForDegreeSequence(seq);
                if(graph == null)
                {
                    Console.WriteLine(":( Unable to construct a graph with this degree sequence");
                }
                else
                {
                    Console.WriteLine();
                    graph.PrintAdjacencyMatrix();
                    Console.WriteLine();
                    var checkSeq = graph.GetDegreeSequence(true, true);
                    PrintArray(checkSeq.ToArray());
                }
                Console.WriteLine();
            }
        }
    }
}

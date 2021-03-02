using PathfindingTutorial.Data_Structures;
using System;
using System.Collections.Generic;

namespace PathfindingTutorial
{
    public partial class Program
    {
        private static void PruferToTree()
        {
            var prufer1 = new int[] { 5, 2, 1, 6, 4, 7 };
            var prufer2 = new int[] { 5, 2, 4, 1, 2, 5 };

            List<int[]> pruferCodes = new List<int[]>()
            {
                prufer1,
                prufer2
            };

            void PrintPruferSequence(int[] prufer)
            {
                for (int i = 0; i < prufer.Length; i++)
                {
                    Console.Write(prufer[i]);
                    if (i < prufer.Length - 1)
                        Console.Write(",");
                }

                Console.WriteLine();
            }

            foreach(var prufer in pruferCodes)
            {
                PrintPruferSequence(prufer);

                var tree = Graph<int>.PruferEncodingToTree(prufer);

                Console.WriteLine("TREE WITH PRUFER CODE");
                tree.PrintEdgesUndirected();

                var reversePrufer = Graph<int>.GeneratePruferEncodingForTree(tree);

                int tally = 0;

                Console.WriteLine("PRUFER CODE OF TREE");
                PrintPruferSequence(reversePrufer.ToArray());

                Console.WriteLine("COMPARING PRUFER SEQUENCES");

                for (int i = 0; i < reversePrufer.Count; i++)
                {
                    if (prufer[i] == reversePrufer[i])
                        tally++;
                    Console.WriteLine(prufer[i] + " : " + reversePrufer[i]);
                }

                if (tally == prufer.Length && prufer.Length == reversePrufer.Count)
                    Console.WriteLine("CORRECT\n");
                else
                    Console.WriteLine("INCORRECT\n");

            }

        }
    }
}

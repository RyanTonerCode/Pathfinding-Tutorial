using PathfindingTutorial.Data_Structures;
using System;
using System.Collections.Generic;

namespace PathfindingTutorial
{
    public partial class Program
    {
        static void PrintArray(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write(array[i]);
                if (i < array.Length - 1)
                    Console.Write(",");
            }

            Console.WriteLine();
        }

        private static void PruferToTree()
        {
            var prufer1 = new int[] { 5, 2, 1, 6, 4, 7 };
            var prufer2 = new int[] { 5, 2, 4, 1, 2, 5 };

            var pruferCodes = new List<int[]>()
            {
                prufer1,
                prufer2
            };

            foreach(var prufer in pruferCodes)
            {
                PrintArray(prufer);

                var tree = Tree<int>.PruferEncodingToTree(prufer);

                Console.WriteLine("TREE WITH PRUFER CODE");
                tree.PrintEdges();

                var reversePrufer = Tree<int>.GeneratePruferEncodingForTree(tree);

                int tally = 0;

                Console.WriteLine("PRUFER CODE OF TREE");
                PrintArray(reversePrufer.ToArray());

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

using PathfindingTutorial.Data_Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PathfindingTutorial
{
    public partial class Program
    {

        private static void MakeGraphsOrderN(int n)
        {
            var graphsUpTo6 = Graph<int>.GenerateNonIsomorphicGraphsOfOrder(n);

            var sb = new StringBuilder(); 

            foreach(var g in graphsUpTo6)
                sb.AppendLine(g.GetGraphStoreFormat());

            File.WriteAllText("processing_set.g", sb.ToString());

            Console.WriteLine(graphsUpTo6.Count);
        }

        private static List<Graph<int>> LoadGraphs()
        {
            var graphs = new List<Graph<int>>();

            var file_str = File.ReadAllText("processing_set.g");

            var graph_lines = file_str.Split('\n');

            foreach (var line in graph_lines)
            {
                var sep = line.Split(":");
                if (sep[0].StartsWith("ug"))
                {
                    int order = Convert.ToInt16(sep[0].Split("ug")[1]);

                    var new_graph = Graph<int>.EmptyGraph(order);

                    var adj_str = sep[1];

                    int index = 0;
                    for (int i = 0; i < order; i++)
                    {
                        for (int j = 0; j < order; j++)
                        {
                            if (i < j && adj_str[index] == '1')
                                new_graph.AddEdge(i, j);
                            index++;
                        }
                    }

                    graphs.Add(new_graph);
                }
            }

            return graphs;
        }
    }
}

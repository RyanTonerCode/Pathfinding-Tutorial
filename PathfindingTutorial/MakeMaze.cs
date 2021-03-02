using PathfindingTutorial.Data_Structures;
using System;
using System.Text;

namespace PathfindingTutorial
{
    public partial class Program
    {
        static void MakeMaze()
        {
            WeightedCoordinateGraphNode<string>[,] grid = new WeightedCoordinateGraphNode<string>[21, 46];

            //load the maze from the resources, since it's too much of a hastle to format in-line.
            string maze = Properties.Resources.Maze;

            int iter = 0;
            for (int i = 0; i < grid.GetLength(0); i++)
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    while (maze[iter] == '\r' || maze[iter] == '\n')
                        iter++;
                    grid[i, j] = new WeightedCoordinateGraphNode<string>(maze[iter].ToString(), i, j);
                    iter++;
                }


            for (int i = 0; i < grid.GetLength(0); i++)
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (i + 1 < grid.GetLength(0) && grid[i, j].GetValue().Equals(" ") && grid[i + 1, j].GetValue().Equals(" "))
                        grid[i, j].AddMutualNeighbor(grid[i + 1, j], 1); //make the cost of going down high
                    if (j + 1 < grid.GetLength(1) && grid[i, j].GetValue().Equals(" ") && grid[i, j + 1].GetValue().Equals(" "))
                        grid[i, j].AddMutualNeighbor(grid[i, j + 1], 1);
                }

            var graph = new Graph<string>(grid);

            void ShowMazeSolution(NodePath<string> final)
            {
                Console.WriteLine("Solved the maze using path length of {0} in a search space of {1} nodes", final.PathLength, graph.LastSearchSpace);

                var backtracking = new Stack<NodePath<string>>();

                while (final != null)
                {
                    backtracking.Push(final);
                    final = final.Parent;
                }

                while (!backtracking.IsEmpty())
                    backtracking.Pop().Node.SetValue("x");

                PrintGrid(grid);

                foreach (var t in grid)
                {
                    if (t.GetValue().Equals("x"))
                        t.SetValue(" ");
                }
            }

            var startingNode = grid[1, 0];
            var endingNode = grid[19, 45];

            //try to find a solution quickly with dfs
            var finalDFS = graph.RunDFS(startingNode, endingNode);

            ShowMazeSolution(finalDFS);

            //try to find optimal solution with bfs
            var finalBFS = graph.RunBFS(startingNode, endingNode);

            ShowMazeSolution(finalBFS);

            //Use dijkstra to also find shortest path
            var finalDijkstra = graph.RunDijkstra(startingNode, endingNode);

            ShowMazeSolution(finalDijkstra);

            static int heuristic(WeightedGraphNode<string> node)
            {
                var wcgn = (WeightedCoordinateGraphNode<string>)node;
                return Math.Abs(wcgn.X - 19) + Math.Abs(wcgn.Y - 45);
            }

            //Use a* instead to try to get the best path faster
            var finalA_Star = graph.RunA_Star(startingNode, endingNode, heuristic);

            ShowMazeSolution(finalA_Star);
        }

        static void PrintGrid(WeightedGraphNode<string>[,] grid)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                sb.AppendLine();
                for (int j = 0; j < grid.GetLength(1); j++)
                    sb.Append(grid[i, j].GetValue());
            }
            Console.WriteLine(sb.AppendLine());
        }
    }
}

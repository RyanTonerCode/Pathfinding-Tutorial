using System;
using pathfinding_demo.Data_Structures;

namespace pathfinding_demo
{
    class Program
    {
        static GraphNode<string>[][] makeGrid10x10()
        {
            //Make a 10x10 grid where even columns have a barrier vertically centered height 8
            //And connect the whitespace
            GraphNode<string>[][] grid = new GraphNode<string>[10][];
            for (int i = 0; i < 10; i++)
            {
                grid[i] = new GraphNode<string>[10]; 
                for (int j = 0; j < 10; j++)
                {
                    grid[i][j] = new GraphNode<string>(i+","+j);
                    if (j > 0 && i % 2 != 0) //if we are on an odd column and there is a row above us, connect the node above us
                    {
                        GraphNode<string>.AddMutualNeighbor(grid[i][j], grid[i][j-1]);
                    }
                    if (i > 0 && (j == 1 || j == 8)) //if we are on the top or bottom row and there is a column before us, connect the node before us
                    {
                        GraphNode<string>.AddMutualNeighbor(grid[i][j], grid[i-1][j]);
                    }
                }
            }
            return grid;
        }

        static (GraphNode<string>[][], char[][]) makeGrid30x30()
        {
            //Make a 100x100 grid where even columns have two barriers of equal height 35 with the space between them vertically centered
            //And connect the whitespace
            GraphNode<string>[][] grid = new GraphNode<string>[30][];
            char[][] gridPath = new char[grid.Length][];
            for (int i = 0; i < grid.Length; i++)
            {
                grid[i] = new GraphNode<string>[30]; 
                gridPath[i] = new char[grid[i].Length];
                for (int j = 0; j < grid[i].Length; j++)
                {
                    grid[i][j] = new GraphNode<string>(i+","+j);
                    if (i % 2 != 0) //if we are on an odd column and there is a row above us, connect the node above us
                    {
                        if (j > 0 && j < grid[i].Length - 1 && i < grid.Length - 2)
                        {
                            GraphNode<string>.AddMutualNeighbor(grid[i][j], grid[i][j-1]);
                            gridPath[i][j] = 'Y';
                        }
                    }
                    if (j == 1 || j == 15 || j == 28) //if we are on the top or bottom row and there is a column before us, connect the node before us
                    {
                        if (i > 0 && i < grid.Length - 2)
                        {
                            GraphNode<string>.AddMutualNeighbor(grid[i][j], grid[i-1][j]);
                            gridPath[i][j] = 'Y';
                        }
                    }
                }
            }
            return (grid, gridPath);
        }

        static void anotherGraph()
        {
            
            //GraphNode<string>[][] grid = makeGrid10x10();
            var gridData = makeGrid30x30();
            GraphNode<string>[][] grid = gridData.Item1;
            char[][] gridPath = gridData.Item2;

            GraphNode<string>[] nodes = new GraphNode<string>[grid.Length * grid[0].Length]; //convert the grid to an array for pathfinding
            for (int i = 0; i < grid.Length; i++)
            {
                for (int j = 0; j < grid[i].Length; j++)
                {
                    nodes[(i*grid.Length)+j] = grid[i][j];
                }
            }
            
            Graph<string> a_graph = new Graph<string>(nodes); //convert array to graph object
            var path = a_graph.RunBFS(grid[1][5], grid[25][20]);
            if (path == null)
                Console.WriteLine("Nope :( \n");
            else
            {
                Console.WriteLine("Success! :) \n");
                printGrid(grid, gridPath, path);
            }
        }
        static void printGrid(GraphNode<string>[][] grid, char[][] gridPath, NodePath<string> path)
        {
            string[] route = new string[100000];
            for (int a = 0; path != null; a++)
            {
                route[a] = (path.Node.GetValue());
                path = path.Parent;
            }

            Console.Write("    ");
            for (int i = 0; i < grid[0].Length; i++)
            {
                Console.Write(" " + i + (i < 10 ? " " : ""));
            }
            Console.WriteLine("");
            
            for (int j = 0; j < grid.Length; j++)
            {
                Console.Write(" " + j + (j < 10 ? "  " : " "));
                for (int i = 0; i < grid[j].Length; i++)
                {
                    string cordos = i + "," + j;
                    if (Array.Exists(route, element => element == cordos))
                    {
                        Console.Write(" ¤ ");
                    }
                    else
                    {
                        Console.Write(gridPath[i][j] == 'Y' ? "   " : "███");
                    }
                }
                Console.WriteLine();
            }        
        }

        static void Main(string[] args)
        {
            //StackVsQueue();
            //MakeGraph();
            anotherGraph();

            Console.ReadLine();
        }
    }
}

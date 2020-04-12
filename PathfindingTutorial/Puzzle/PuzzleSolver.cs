using PathfindingTutorial.Data_Structures;
using System;
using System.Collections.Generic;

namespace PathfindingTutorial.Puzzle
{
    public static class PuzzleSolver
    {
        /// <summary>
        /// Runs an A* search to solve the board using manhattan distance as the heuristic function.
        /// </summary>
        /// <param name="initial_state"></param>
        /// <returns></returns>
        public static NodePath<GameBoard> A_Star_Search(GameBoard initial_state)
        {
            IPriorityQueue<WeightedNodePath<GameBoard>> priQueue = new Heap<WeightedNodePath<GameBoard>>(2000);

            WeightedGraphNode<GameBoard> starting_path = new WeightedGraphNode<GameBoard>(initial_state);

            var foundBoards = new HashSet<ulong>();

            var Start = new WeightedNodePath<GameBoard>(starting_path, null, 0);
            priQueue.Enqueue(Start);

            int totalProcessed = 0;
            while (!priQueue.IsEmpty())
            {
                WeightedNodePath<GameBoard> cur = priQueue.Dequeue();

                GameBoard top_gb = cur.Node.GetValue();
                totalProcessed++;

                if (top_gb.IsSolved) {
                    Console.WriteLine("Solved with Path Length of {0} with {1} boards revealed", cur.PathLength, totalProcessed);
                    return cur;
                }

                //add to marked
                foundBoards.Add(top_gb.HashValue());

                int nextPathLength = cur.PathLength + 1;

                //add all new nodes to the stack
                foreach (var neighbor in top_gb.GetAllNeighborBoards())
                {
                    if (foundBoards.Contains(neighbor.HashValue()))
                        continue;

                    //weight = h(x) + g(x)
                    double new_weight = neighbor.GetSigmaManhattanDistance() + nextPathLength;

                    var n_wgn = new WeightedGraphNode<GameBoard>(neighbor);

                    priQueue.Enqueue(new WeightedNodePath<GameBoard>(n_wgn, cur, new_weight, nextPathLength));
                }

                if (priQueue.IsEmpty())
                    Console.WriteLine("foobar!");
            }

            Console.WriteLine("foobar!");

            return null;
        }
    }
}

using PathfindingTutorial.Data_Structures;
using PathfindingTutorial.Puzzle;
using System;
using System.Text;

namespace PathfindingTutorial
{
    public partial class Program
    {
        static void SolvePuzzle(bool IsGreedy, bool print = false)
        {
             //int[] board = { 5, 0, 2, 6, 3, 4, 8, 1, 7 };
;           var gb = new GameBoard(3, 3);

            var solution = PuzzleSolver.A_Star_Search(gb, IsGreedy);

            if (solution == null && print)
            {
                Console.WriteLine("Could not find a solution!");
                return;
            }

            Stack<NodePath<GameBoard>> stk = new Stack<NodePath<GameBoard>>();

            while (solution != null)
            {
                stk.Add(solution);
                solution = solution.Parent;
            }

            if (!print)
                return;

            var sb = new StringBuilder();

            while (!stk.IsEmpty())
            {
                var top = stk.Pop().Node;
                if (print)
                {
                    sb.Append(top.GetValue().MoveInformation).AppendLine();
                    sb.Append(top.GetValue()).AppendLine();

                }
            }

            Console.WriteLine(sb);

        }
    }
}

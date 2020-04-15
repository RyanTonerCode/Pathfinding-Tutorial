﻿using PathfindingTutorial.Data_Structures;
using PathfindingTutorial.Puzzle;
using System;
using System.Text;

namespace PathfindingTutorial
{
    public partial class Program
    {
        static void SolvePuzzle(bool IsGreedy, bool print = false)
        {
            GameBoard gb = new GameBoard(3, 3);

            NodePath<GameBoard> solution = PuzzleSolver.A_Star_Search(gb, IsGreedy);
            //NodePath<GameBoard> solution2 = PuzzleSolver.A_Star_Search(gb, !IsGreedy);

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

            StringBuilder sb = new StringBuilder();

            while (!stk.IsEmpty())
            {
                var top = stk.Pop().Node;
                if (print)
                    sb.Append(top.GetValue()).AppendLine();
            }

            if (print)
                Console.WriteLine(sb);

        }
    }
}

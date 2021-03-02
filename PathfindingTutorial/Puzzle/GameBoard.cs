using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace PathfindingTutorial.Puzzle 
{
    /// <summary>
    /// Board for n^2 -1 Puzzle
    /// </summary>
    public class GameBoard
    {
        private static readonly Random Randy = new Random();

        public int Width { get; private set; }  //number of columns
        public int Height { get; private set; } //number of rows

        /// <summary>
        /// How this board was generated
        /// </summary>
        public string MoveInformation { get; private set; }

        /// <summary>
        /// Tiles stored in order from left to right for each row
        /// </summary>
        private readonly Tile[] Tiles;

        private int blankLocation; //where the blank tile is in the array

        public Tile GetTile(int X, int Y) => Tiles[GetLocNum(X,Y)];

        public int GetLocNum(int X, int Y) => X + Width * Y;

        public (int X, int Y) GetGridVal(int LocNum)
        {
            int col = LocNum % Width;
            int row = (LocNum - col) / Height;
            return (col, row);
        }

        public GameBoard(int Width, int Height)
        {
            this.Width = Width;
            this.Height = Height;
            Tiles = new Tile[Height * Width];
            GenerateBoard(true);
            MoveInformation = "Board Generated";
        }


        public GameBoard(int Width, int Height, params int[] tileStates)
        {
            this.Width = Width;
            this.Height = Height;
            Tiles = new Tile[Height * Width];
            int t = 0;
            foreach(int i in tileStates)
            {
                if (i == 0)
                    blankLocation = t;
                var (X, Y) = GetGridVal(t);
                Tiles[t++] = new Tile(i, X, Y);
            }
            Console.WriteLine(GetSigmaManhattanDistance());
            MoveInformation = "Board Generated";
        }

        public GameBoard(int Width, int Height, Tile[] oldBoard)
        {
            this.Width = Width;
            this.Height = Height;
            Tiles = new Tile[Height * Width];
            //this will create references (aliases) from the new board back to the old.
            Array.Copy(oldBoard, Tiles, oldBoard.Length);
        }

        
        private int? computedManhattanDistance = null;
        public int GetSigmaManhattanDistance() { 
            if (computedManhattanDistance == null)
                computedManhattanDistance = getSigmaManhattanDistance();
            return computedManhattanDistance.Value; 
        }  
        

        /// <summary>
        /// Returns the sum of the manhattan distance for all non-blank tiles on the board
        /// </summary>
        /// <returns></returns>
        private int getSigmaManhattanDistance()
        {
            int sum = 0;
            foreach(Tile t in Tiles) {

                if (t.IsBlank) //do not count distance for the blank tile
                    continue;

                int tileExpectedLocNum = t.Value - 1;

                //coordinate of where this value should be
                (int X, int Y) = GetGridVal(tileExpectedLocNum);


                //sum MD for all non-blank tiles
                sum += Math.Abs(t.Location.X - X) + Math.Abs(t.Location.Y - Y);
            }
            computedManhattanDistance = sum;
            return sum;
        }

        public bool IsSolved => GetSigmaManhattanDistance() == 0;

        public List<GameBoard> GetAllNeighborBoards()
        {
            List<GameBoard> neighbors = new List<GameBoard>(4);
            for(int i = 0; i <= 3; i++)
            {
                GameBoard gb = Move(i); //try moving in all 4 orientations
                //legal move found
                if (gb != null)
                    neighbors.Add(gb);
            }
            return neighbors;
        }

        public GameBoard Move(int Direction)
        {
            Tile blank = Tiles[blankLocation];
            int blankX = blank.Location.X;
            int blankY = blank.Location.Y;
            (int X, int Y) = Neighbors[Direction];
            X += blankX;
            Y += blankY;
            if (X >= 0 && X < Width && Y >= 0 && Y < Height)
            {
                //generate new board by cloning this one
                GameBoard gb = new GameBoard(Width, Height, Tiles);

                int tileLocNum = GetLocNum(X, Y);

                //make sure to clone the values that need updating, since they need new aliases
                gb.Tiles[blankLocation] = gb.Tiles[blankLocation].Clone();
                gb.Tiles[tileLocNum] = gb.Tiles[tileLocNum].Clone();

                gb.Tiles[blankLocation].SetValue(gb.Tiles[tileLocNum].Value);
                gb.Tiles[tileLocNum].SetValue(0);

                //set new blank location
                gb.blankLocation = tileLocNum;

                gb.MoveInformation = string.Format("Moved {0}", gb.Tiles[blankLocation].Value);

                //Debug.WriteLine(gb.ToString());

                return gb;
            }
            return null;
        }
        
        //List of all possible directions of neighbors (right, left, up, down) relative to the blank tile.
        private static readonly List<(int X, int Y)> Neighbors = new List<(int, int)> { (1, 0), (-1, 0), (0, 1), (0, -1) };


        /// <summary>
        /// Generate a valid board configuration.
        /// </summary>
        /// <param name="Random"></param>
        /// <param name="debug_print"></param>
        public void GenerateBoard(bool Random = false, bool debug_print = false)
        {

            if (!Random)
            #region Predefined Board
            {
                int[] list = { 0 };

                int count = 0;
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        Tiles[count] = new Tile(list[i], j, i);
                        count++;
                    }
                }
                return;

                #endregion
            }

            List<int> generations = new List<int>(Height * Width);

            for (int i = 0; i < Width * Height; i++)
                generations.Add(i);

            bool blankevenfrombottom = false;

            int t = 0;
            while (generations.Count > 0)
            {
                //pick a value for the first tile
                var r = generations[Randy.Next(generations.Count)];
                generations.Remove(r);
                (var x, var y) = GetGridVal(t);

                bool isBlank = r == 0;

                if (isBlank)
                {
                    blankevenfrombottom = (Height - y) % 2 == 1;
                    blankLocation = t;
                }
                Tiles[t++] = new Tile(r, x, y);
            }

            #region unit test for inversions
            //credit to https://www.cs.bham.ac.uk/~mdr/teaching/modules04/java2/TilesSolvability.html
            /* for this state, the inversion count should = 49
            tiles[0].Value = 12;
            tiles[1].Value = 1;
            tiles[2].Value = 10;
            tiles[3].Value = 2;

            tiles[4].Value = 7;
            tiles[5].Value = 11;
            tiles[6].Value = 4;
            tiles[7].Value = 14;

            tiles[8].Value = 5;
            tiles[9].Value = 16;
            tiles[9].IsBlank = true;
            tiles[10].Value = 9;
            tiles[11].Value = 15;

            tiles[12].Value = 8;
            tiles[13].Value = 13;
            tiles[14].Value = 6;
            tiles[15].Value = 3;
            */
            #endregion

            //For solvability:
            //width % 2 == 1 => inversion % 2 ==0
            //width % 2 == 0 => (blank on even row from bottom => inversion % 2 == 1)
            //width % 2 == 0 => (blank on odd row from bottom => inversion % 2 == 0)

            if (debug_print) {
                Debug.WriteLine("Current Board State:");
                Debug.WriteLine(this);
            }

            if (!validBoard(blankevenfrombottom))
            { //false (impossible to solve)
                if(debug_print)
                    Debug.WriteLine("Generated a bad state. Swapping now.");
                //greedily swap something
                for (int i = 0; i < Width * Height - 1; i++)
                {
                    Tile t1 = Tiles[i];
                    Tile t2 = Tiles[i + 1];

                    if (!t1.IsBlank && !t2.IsBlank)
                    {
                        if (t1.Value > t2.Value)
                        {
                            int val1 = Tiles[i + 1].Value;

                            Tiles[i + 1].SetValue(Tiles[i].Value);
                            Tiles[i].SetValue(val1);
                            if (debug_print)
                            {
                                //swap them
                                Debug.WriteLine("Swapped");
                                print = null;
                                Debug.WriteLine(this);
                            }

                            break;
                        }
                    }
                }

                bool valid = validBoard(blankevenfrombottom);
                if (!valid)
                {
                    Debug.WriteLine("Failed to resolve the board. Regenerating.");
                    GenerateBoard(true);
                }

            }
        }

        string print;

        /// <summary>
        /// Pretty-prints the board
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (print != null)
                return print;

            StringBuilder sb = new StringBuilder();

            int i = 0;
            foreach(Tile t in Tiles)
            {
                var (X, _) = GetGridVal(i);
                if(X == 0)
                    sb.Append('[');

                if (!t.IsBlank)
                    sb.Append(t.Value);
                else
                    sb.Append('X');

                if (X < Width - 1)
                    sb.Append(' ');
                else
                    sb.Append(']').AppendLine();
                i++;
            }
            print = sb.ToString();
            return print;
        }


        private uint hashValue = 0;

        /// <summary>
        /// An optimized hash of this board that encodes the location of each tile
        /// </summary>
        public uint GetHashValue()
        {
            if (hashValue != 0)
                return hashValue;

            /* Tile values are from 0-8 for 3x3 board, so need
             * 000
             * 001
             * 010
             * 011
             * 100
             * 101
             * 110
             * 111
             * So, need 3 bits per tile. 3 * 9 = 27.
             * This will fit inside of unit = 32 bits.
             */

            foreach(Tile t in Tiles)
                hashValue = (hashValue << 3) + (uint)t.Value;
            return hashValue;
        }

        /// <summary>
        /// Returns whether or not this is a valid board configuration.
        /// </summary>
        /// <param name="blankEvenRowFromBottom"></param>
        /// <returns></returns>
        private bool validBoard(bool blankEvenRowFromBottom) {
            int inversionCount = 0;

            for (int check = 0; check < Width * Height - 1; check++) //no need to consider last tile.
                for (int checkinversions = check + 1; checkinversions < Width * Height; checkinversions++) //iterate through all subsequent tiles
                    if (!(Tiles[check].IsBlank || Tiles[checkinversions].IsBlank) && //ignore blank tile if found
                        Tiles[check].Value > Tiles[checkinversions].Value)
                        inversionCount++;

            return (
                Width % 2 == 1 && inversionCount % 2 == 0 ||
                Width % 2 == 0 && !blankEvenRowFromBottom && inversionCount % 2 == 0 ||
                Width % 2 == 0 && blankEvenRowFromBottom && inversionCount % 2 == 1);
        }


    }
}

namespace PathfindingTutorial.Puzzle
{
    /// <summary>
    /// Tiles of n^2 -1 puzzle
    /// </summary>
    public class Tile
    {
        /// <summary>
        /// The value of this tile piece
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// The (X,Y) location of this tile
        /// X is the column number
        /// Y is the row number
        /// </summary>
        public (int X, int Y) Location { get; private set; }

        /// <summary>
        /// The blank value is always zero.
        /// </summary>
        public bool IsBlank => Value == 0;

        public Tile(int Value, int X, int Y)
        {
            this.Value = Value;
            Location = (X, Y);
        }

        public void SetLocation(int X, int Y)
        {
            Location = (X, Y);
        }

        public void SetValue(int val)
        {
            Value = val;
        }

        /// <summary>
        /// Create a clone tile with the same information
        /// </summary>
        /// <returns></returns>
        public Tile Clone()
        {
            return new Tile(Value, Location.X, Location.Y);
        }

    }
}

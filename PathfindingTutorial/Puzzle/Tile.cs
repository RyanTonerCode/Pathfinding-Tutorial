namespace PathfindingTutorial.Puzzle
{
    public class Tile
    {
        public int Value { get; set; }
        public (int X, int Y) Location { get; private set; }

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

        //returns a new tile instance with the same information
        public Tile Clone()
        {
            return new Tile(Value, Location.X, Location.Y);
        }

    }
}

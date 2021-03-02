using PathfindingTutorial.Data_Structures;
using System;
using System.Text;

namespace PathfindingTutorial.Data_Structures
{
    /// <summary>
    /// An edge defines a directed connection from Node1 to Node2
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Edge<T> : IComparable
    {
        public IGraphNode<T> Node1 { get; private set; }
        public IGraphNode<T> Node2 { get; private set; }
        public double Weight { get; private set; }
        public bool Directed { get; private set; }

        public Edge(IGraphNode<T> Node1, IGraphNode<T> Node2, double Weight = 1, bool Directed = true)
        {
            this.Node1 = Node1;
            this.Node2 = Node2;
            this.Weight = Weight;
            this.Directed = Directed;
        }

        public Edge<T> FlipDirection()
        {
            return new Edge<T>(Node2, Node1, Weight, Directed);
        }

        public int CompareTo(object obj)
        {
            if (obj is Edge<T> cast) //in this order, lower values will return positive
                return cast.Weight.CompareTo(Weight);
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(Node1.GetValue());
            if (Directed)
                sb.Append(" --> ");
            else
                sb.Append(" <--> ");

            sb.Append(Node2.GetValue());
            if (Node1 is WeightedGraphNode<T> || Node2 is WeightedGraphNode<T>)
                sb.Append(" (").Append(Weight).Append(')');

            return sb.ToString();
        }

    }
}

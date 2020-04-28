using PathfindingTutorial.Data_Structures;
using System;

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

        public Edge(IGraphNode<T> Node1, IGraphNode<T> Node2, double Weight = 1)
        {
            this.Node1 = Node1;
            this.Node2 = Node2;
            this.Weight = Weight;
        }

        public Edge<T> FlipDirection()
        {
            return new Edge<T>(Node2, Node1, Weight);
        }

        public int CompareTo(object obj)
        {
            if (obj is Edge<T> cast) //in this order, lower values will return positive
                return cast.Weight.CompareTo(Weight);
            throw new NotImplementedException();
        }

    }
}

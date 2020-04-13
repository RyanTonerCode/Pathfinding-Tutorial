using System;
using System.Collections.Generic;

namespace PathfindingTutorial.Data_Structures
{
    public class Graph<T> : IGraph<T>
    {

        private readonly List<IGraphNode<T>> graphStructure = new List<IGraphNode<T>>();

        /// <summary>
        /// The number of nodes processed by the last search performed on this graph.
        /// </summary>
        public int LastSearchSpace { get; private set; }

        public Graph(IGraphNode<T>[] nodes)
        {
            graphStructure = new List<IGraphNode<T>>(nodes);
        }

        public Graph(IGraphNode<T>[,] nodes)
        {
            graphStructure = new List<IGraphNode<T>>(nodes.Length);
            foreach (var n in nodes)
                graphStructure.Add(n);
            
        }

        public void AddNode(IGraphNode<T> Node)
        {
            graphStructure.Add(Node);
        }

        public void RemoveNode(IGraphNode<T> Node)
        {
            graphStructure.Remove(Node);
        }

        public NodePath<T> RunDFS(IGraphNode<T> Start, IGraphNode<T> End)
        {
            LastSearchSpace = 0;

            var stk = new Stack<NodePath<T>>();

            var begin = new NodePath<T>(Start, null);

            stk.Push(begin);

            HashSet<IGraphNode<T>> marked = new HashSet<IGraphNode<T>>();

            while (stk.Count > 0)
            {
                LastSearchSpace++;
                NodePath<T> cur = stk.Pop();

                if (cur.Node == End)
                    return cur;

                marked.Add(cur.Node);

                //add all new nodes to the stack
                foreach (var neighbor in cur.Node.GetNeighbors())
                    if (!marked.Contains(neighbor))
                        stk.Push(new NodePath<T>(neighbor, cur, cur.PathLength + 1));
            }

            return null;
        }

        public NodePath<T> RunBFS(IGraphNode<T> Start, IGraphNode<T> End)
        {
            LastSearchSpace = 0;

            var stk = new Queue<NodePath<T>>();

            var begin = new NodePath<T>(Start, null);

            stk.Enqueue(begin);

            //this is our "marked" set
            HashSet<IGraphNode<T>> marked = new HashSet<IGraphNode<T>>();

            while (stk.Count > 0)
            {
                NodePath<T> cur = stk.Dequeue();

                LastSearchSpace++;
                if (cur.Node == End)
                    return cur;

                marked.Add(cur.Node);

                //add all new nodes to the stack
                foreach (var neighbor in cur.Node.GetNeighbors())
                    if (!marked.Contains(neighbor)) //unmarked neighbor
                        stk.Enqueue(new NodePath<T>(neighbor, cur, cur.PathLength + 1));
            }

            return null;
        }

        public NodePath<T> RunSearch(IGraphSearcher<NodePath<T>> dataStructure, IGraphNode<T> Start, IGraphNode<T> End)
        {
            LastSearchSpace = 0;

            var begin = new NodePath<T>(Start, null);

            dataStructure.Add(begin);

            //this is our "marked" set
            HashSet<IGraphNode<T>> marked = new HashSet<IGraphNode<T>>();

            while (!dataStructure.IsEmpty())
            {
                NodePath<T> cur = dataStructure.Remove();

                LastSearchSpace++;
                if (cur.Node == End)
                    return cur;

                marked.Add(cur.Node);

                //add all new nodes to the stack
                foreach (var neighbor in cur.Node.GetNeighbors())
                    if (!marked.Contains(neighbor)) //unmarked neighbor
                        dataStructure.Add(new NodePath<T>(neighbor, cur, cur.PathLength + 1));
            }

            return null;
        }

        public WeightedNodePath<T> RunDijkstra(WeightedGraphNode<T> Start, WeightedGraphNode<T> End)
        {
            LastSearchSpace = 0;

            IPriorityQueue<WeightedNodePath<T>> priQueue = new Heap<WeightedNodePath<T>>(64);

            var begin = new WeightedNodePath<T>(Start,null,0);
            priQueue.Enqueue(begin);

            //this is our "marked" set
            HashSet<WeightedGraphNode<T>> marked = new HashSet<WeightedGraphNode<T>>();

            while (!priQueue.IsEmpty())
            {
                WeightedNodePath<T> cur = priQueue.Dequeue();

                LastSearchSpace++;
                if (cur.Node == End)
                    return cur;

                var wgn = (WeightedGraphNode<T>)cur.Node;

                if (marked.Contains(wgn))
                    continue;

                marked.Add(wgn);

                //add all new nodes to the stack
                foreach (var neighbor in cur.Node.GetNeighbors())
                    if (!marked.Contains((WeightedGraphNode<T>)neighbor))
                    { //unmarked neighbor

                        double edge_weight = ((WeightedGraphNode<T>)cur.Node).EdgeWeights[neighbor];

                        double new_weight = cur.PathWeightToHere + edge_weight;

                        priQueue.Enqueue(new WeightedNodePath<T>(neighbor, cur, new_weight, cur.PathLength + 1));
                    }
            }

            return null;
        }

        public WeightedNodePath<T> RunA_Star(WeightedCoordinateGraphNode<T> Start, WeightedCoordinateGraphNode<T> End, Func<WeightedCoordinateGraphNode<T>, int> heuristic)
        {
            LastSearchSpace = 0;

            IPriorityQueue<WeightedNodePath<T>> priQueue = new Heap<WeightedNodePath<T>>(64);

            var begin = new WeightedNodePath<T>(Start, null, 0);
            priQueue.Enqueue(begin);

            //this is our "marked" set
            HashSet<WeightedGraphNode<T>> marked = new HashSet<WeightedGraphNode<T>>();

            while (!priQueue.IsEmpty())
            {
                WeightedNodePath<T> cur = priQueue.Dequeue();

                LastSearchSpace++;
                if (cur.Node == End)
                    return cur;

                var wgn = (WeightedGraphNode<T>)cur.Node;

                if (marked.Contains(wgn))
                    continue;

                marked.Add(wgn);

                var newPathLength = cur.PathLength + 1;

                //add all new nodes to the stack
                foreach (var neighbor in cur.Node.GetNeighbors())
                {

                    var w_neighbor = (WeightedCoordinateGraphNode<T>)neighbor;

                    if (!marked.Contains(w_neighbor))
                    { //unmarked neighbor

                        double edge_weight = ((WeightedGraphNode<T>)cur.Node).EdgeWeights[neighbor];

                        //this is a somewhat complex weight function
                        //it adds the path length g(x)
                        //with the edge weight to get to the node e(x)
                        //with the heuristic of how close this node is to the solution h(x)
                        //so the formula is g(x) + e(x) + h(x)
                        double new_weight = newPathLength + edge_weight + heuristic(w_neighbor);

                        priQueue.Enqueue(new WeightedNodePath<T>(neighbor, cur, new_weight, newPathLength));
                    }
                }
            }

            return null;
        }

    }
}

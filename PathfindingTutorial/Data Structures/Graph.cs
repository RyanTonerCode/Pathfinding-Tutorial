using System.Collections.Generic;

namespace PathfindingTutorial.Data_Structures
{
    public class Graph<T> : IGraph<T>
    {

        private readonly List<IGraphNode<T>> graphStructure = new List<IGraphNode<T>>();


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
            var stk = new Stack<NodePath<T>>();

            var begin = new NodePath<T>(Start, null);

            stk.Push(begin);

            HashSet<IGraphNode<T>> marked = new HashSet<IGraphNode<T>>();

            while (stk.Count > 0)
            {
                NodePath<T> cur = stk.Pop();

                if (cur.Node == End)
                    return cur;

                marked.Add(cur.Node);

                //add all new nodes to the stack
                foreach (var neighbor in cur.Node.GetNeighbors())
                    if (!marked.Contains(neighbor))
                        stk.Push(new NodePath<T>(neighbor, cur));
            }

            return null;
        }

        public NodePath<T> RunBFS(IGraphNode<T> Start, IGraphNode<T> End)
        {
            var stk = new Queue<NodePath<T>>();

            var begin = new NodePath<T>(Start, null);

            stk.Enqueue(begin);

            //this is our "marked" set
            HashSet<IGraphNode<T>> marked = new HashSet<IGraphNode<T>>();

            while (stk.Count > 0)
            {
                NodePath<T> cur = stk.Dequeue();

                if (cur.Node == End)
                    return cur;

                marked.Add(cur.Node);

                //add all new nodes to the stack
                foreach (var neighbor in cur.Node.GetNeighbors())
                    if (!marked.Contains(neighbor)) //unmarked neighbor
                        stk.Enqueue(new NodePath<T>(neighbor, cur));
            }

            return null;
        }

        public NodePath<T> RunSearch(IGraphSearcher<NodePath<T>> dataStructure, IGraphNode<T> Start, IGraphNode<T> End)
        {

            var begin = new NodePath<T>(Start, null);

            dataStructure.Add(begin);

            //this is our "marked" set
            HashSet<IGraphNode<T>> marked = new HashSet<IGraphNode<T>>();

            while (!dataStructure.IsEmpty())
            {
                NodePath<T> cur = dataStructure.Remove();

                if (cur.Node == End)
                    return cur;

                marked.Add(cur.Node);

                //add all new nodes to the stack
                foreach (var neighbor in cur.Node.GetNeighbors())
                    if (!marked.Contains(neighbor)) //unmarked neighbor
                        dataStructure.Add(new NodePath<T>(neighbor, cur));
            }

            return null;
        }

        public WeightedNodePath<T> RunDijkstra(WeightedGraphNode<T> Start, WeightedGraphNode<T> End)
        {
            IPriorityQueue<WeightedNodePath<T>> priQueue = new Heap<WeightedNodePath<T>>(64);

            var begin = new WeightedNodePath<T>(Start,null,0);
            priQueue.Enqueue(begin);

            //this is our "marked" set
            HashSet<WeightedGraphNode<T>> marked = new HashSet<WeightedGraphNode<T>>();

            while (!priQueue.IsEmpty())
            {
                WeightedNodePath<T> cur = priQueue.Dequeue();

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

                        priQueue.Enqueue(new WeightedNodePath<T>(neighbor, cur, new_weight));
                    }
            }

            return null;
        }

    }
}

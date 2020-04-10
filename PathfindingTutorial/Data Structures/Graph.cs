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

            HashSet<IGraphNode<T>> found = new HashSet<IGraphNode<T>>();

            while (stk.Count > 0)
            {
                NodePath<T> cur = stk.Pop();

                if (cur.Node == End)
                    return cur;

                found.Add(cur.Node);

                //add all new nodes to the stack
                foreach (var neighbor in cur.Node.GetNeighbors())
                    if (!found.Contains(neighbor))
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
            HashSet<IGraphNode<T>> found = new HashSet<IGraphNode<T>>();

            while (stk.Count > 0)
            {
                NodePath<T> cur = stk.Dequeue();

                if (cur.Node == End)
                    return cur;

                found.Add(cur.Node);

                //add all new nodes to the stack
                foreach (var neighbor in cur.Node.GetNeighbors())
                    if (!found.Contains(neighbor)) //unmarked neighbor
                        stk.Enqueue(new NodePath<T>(neighbor, cur));
            }

            return null;
        }

        public NodePath<T> RunSearch(IGraphSearcher<NodePath<T>> dataStructure, IGraphNode<T> Start, IGraphNode<T> End)
        {

            var begin = new NodePath<T>(Start, null);

            dataStructure.Add(begin);

            //this is our "marked" set
            HashSet<IGraphNode<T>> found = new HashSet<IGraphNode<T>>();

            while (!dataStructure.IsEmpty())
            {
                NodePath<T> cur = dataStructure.Remove();

                if (cur.Node == End)
                    return cur;

                found.Add(cur.Node);

                //add all new nodes to the stack
                foreach (var neighbor in cur.Node.GetNeighbors())
                    if (!found.Contains(neighbor)) //unmarked neighbor
                        dataStructure.Add(new NodePath<T>(neighbor, cur));
            }

            return null;
        }

        public NodePath<T> RunDjikstra(WeightedGraphNode<T> Start, WeightedGraphNode<T> End)
        {
            var stk = new PriorityQueue<WeightedNodePath<T>>();

            var begin = new WeightedNodePath<T>(Start, null);
            stk.Enqueue(begin, 0);

            //this is our "marked" set
            HashSet<WeightedGraphNode<T>> found = new HashSet<WeightedGraphNode<T>>();

            while (stk.Count > 0)
            {
                WeightedNodePath<T> cur = stk.Dequeue();

                if (cur.Node == End)
                    return cur;

                found.Add((WeightedGraphNode<T>)cur.Node);

                //add all new nodes to the stack
                foreach (var neighbor in cur.Node.GetNeighbors())
                    if (!found.Contains((WeightedGraphNode<T>)neighbor))
                    { //unmarked neighbor

                        double next_weight = ((WeightedGraphNode<T>)cur.Node).EdgeWeights[neighbor];

                        double new_weight = cur.Weight + next_weight;

                        stk.Enqueue(new WeightedNodePath<T>(neighbor, cur, new_weight), new_weight);
                    }
            }

            return null;
        }

    }
}

using System;
using System.Collections.Generic;

namespace PathfindingTutorial.Data_Structures
{
    public class Graph<T> : IGraph<T>
    {

        private readonly List<IGraphNode<T>> graphStructure = new List<IGraphNode<T>>();

        public IGraphNode<T> FirstNode => graphStructure[0];
        public IGraphNode<T> LastNode => graphStructure[graphStructure.Count - 1];

        /// <summary>
        /// The number of nodes processed by the last search performed on this graph.
        /// </summary>
        public int LastSearchSpace { get; private set; }

        public Graph(params IGraphNode<T>[] nodes)
        {
            graphStructure = new List<IGraphNode<T>>(nodes);
        }

        public Graph(IGraphNode<T>[,] nodes)
        {
            graphStructure = new List<IGraphNode<T>>(nodes.Length);
            foreach (var n in nodes)
                graphStructure.Add(n);
            
        }

        public Graph(List<IGraphNode<T>> graph)
        {
            graphStructure = graph;
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

            List<IGraphNode<T>> marked = new List<IGraphNode<T>>();

            while (stk.Count > 0)
            {
                LastSearchSpace++;
                NodePath<T> cur = stk.Pop();

                if (cur.Node == End)
                    return cur;

                if (marked.Contains(cur.Node))
                    continue;
                marked.Add(cur.Node);

                //add all new nodes to the stack
                foreach (IGraphNode<T> neighbor in cur.Node.GetNeighbors())
                    if (!marked.Contains(neighbor))
                        stk.Push(new NodePath<T>(neighbor, cur, cur.PathLength + 1));
            }

            return null;
        }

        public NodePath<T> RunBFS(IGraphNode<T> Start, IGraphNode<T> End)
        {
            LastSearchSpace = 0;

            var queue = new Queue<NodePath<T>>();

            var begin = new NodePath<T>(Start, null);

            queue.Enqueue(begin);

            //this is our "marked" set
            List<IGraphNode<T>> marked = new List<IGraphNode<T>>();

            while (queue.Count > 0)
            {
                NodePath<T> cur = queue.Dequeue();

                LastSearchSpace++;
                if (cur.Node == End)
                    return cur;

                if (marked.Contains(cur.Node))
                    continue;

                try
                {
                    marked.Add((WeightedCoordinateGraphNode<T>)cur.Node);
                }
                catch(InvalidCastException ex)
                {
                    marked.Add((WeightedGraphNode<T>)cur.Node);
                }


                //add all new nodes to the stack
                foreach (IGraphNode<T> neighbor in cur.Node.GetNeighbors())
                    if (!marked.Contains(neighbor)) //unmarked neighbor
                        queue.Enqueue(new NodePath<T>(neighbor, cur, cur.PathLength + 1));
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

                if (marked.Contains(cur.Node))
                    continue;
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

        public WeightedNodePath<T> RunA_Star(WeightedGraphNode<T> Start, WeightedGraphNode<T> End, Func<WeightedGraphNode<T>, int> heuristic)
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

                    var w_neighbor = (WeightedGraphNode<T>)neighbor;

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


        public Graph<T> PrimsAlgorithmForMST()
        {
            if (graphStructure.Count == 0)
                throw new Exception("Empty graph");

            //nodes in graph G (this)
            List<WeightedGraphNode<T>> RemainingNodesInOriginal = new List<WeightedGraphNode<T>>(graphStructure.Count);
            foreach (var n in graphStructure)
                RemainingNodesInOriginal.Add((WeightedGraphNode<T>)n);

            //nodes in the MST with references in G
            List<WeightedGraphNode<T>> VT_G = new List<WeightedGraphNode<T>>(graphStructure.Count);

            //the clones
            List<WeightedGraphNode<T>> VT_Clones = new List<WeightedGraphNode<T>>(graphStructure.Count);

            //pick an arbitrary node
            WeightedGraphNode<T> arbitrary = (WeightedGraphNode<T>)graphStructure[0];

            //place on the MST nodes
            VT_G.Add(arbitrary);
            VT_Clones.Add(arbitrary.Clone());

            //remove it from the original set
            RemainingNodesInOriginal.Remove(arbitrary);

            while(RemainingNodesInOriginal.Count != 0)
            {
                WeightedGraphNode<T> current_in_vt_clone = null;

                //connects to node called current_in_vt
                WeightedGraphNode<T> edgeNodeOfCurrent = null;

                //the value associated with the edge (weight)
                double min = -1;

                //go through all nodes currently in the MST
                for (int j = 0; j < VT_G.Count; j++)
                {
                    var vt_node = VT_G[j];

                    //go through all nodes in the original not in the current MST
                    for (int k = 0; k < RemainingNodesInOriginal.Count; k++)
                    {
                         var remaining_node = RemainingNodesInOriginal[k];

                         //if it has the remaining node as a neighbor
                         if (vt_node.HasNeighbor(remaining_node))
                         {
                            if(min == -1 || vt_node.EdgeWeights[remaining_node] < min)
                            {
                                //we have found a new min!

                                edgeNodeOfCurrent = remaining_node;
                                min = vt_node.EdgeWeights[remaining_node];

                                current_in_vt_clone = VT_Clones[j];
                            }
                        }
                    }               
                }

                var edgeNodeOfCurrent_clone = edgeNodeOfCurrent.Clone();
                current_in_vt_clone.AddNeighbor(edgeNodeOfCurrent_clone, min);

                VT_G.Add(edgeNodeOfCurrent);
                VT_Clones.Add(edgeNodeOfCurrent_clone);

                RemainingNodesInOriginal.Remove(edgeNodeOfCurrent);
            }

            Graph<T> MST = new Graph<T>();

            foreach(var n in VT_Clones)
                MST.AddNode(n);

            return MST;

        }

        public void PrintNodeInfo()
        {
            foreach(var n in graphStructure)
                foreach(var neighbor in n.GetNeighbors())
                {
                    Console.Write("({0},{1})", n.GetValue(), neighbor.GetValue());

                    if (n is WeightedGraphNode<T> wgn)
                    {
                        Console.Write(" : {0}", wgn.EdgeWeights[neighbor]);
                    }
                    Console.WriteLine();
                }
        }

    }
}

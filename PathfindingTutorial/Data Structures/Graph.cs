﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace PathfindingTutorial.Data_Structures
{
    public class Graph<T> : IGraph<T>
    {
        private readonly List<IGraphNode<T>> graphStructure = new List<IGraphNode<T>>();

        /// <summary>
        /// Returns a list of all edges sorted in order by weight
        /// </summary>
        /// <returns></returns>
        public List<Edge<T>> SortedEdgeList()
        {
            List<Edge<T>> edges = EdgeListUndirected();

            edges.Sort((x, y) => x.Weight.CompareTo(y.Weight));
            return edges;
        }

        public List<Edge<T>> EdgeList()
        {
            var edges = new List<Edge<T>>();

            foreach (var node in graphStructure)
                foreach (var neighbor in node.GetNeighbors())
                {
                    if (node is WeightedGraphNode<T> wgn)
                        edges.Add(new Edge<T>(node, neighbor, wgn.EdgeWeights[neighbor]));
                    else
                        edges.Add(new Edge<T>(node, neighbor));
                }

            return edges;
        }

        public List<Edge<T>> EdgeListUndirected()
        {
            var edgeList = EdgeList();

            for(int i = edgeList.Count - 1; i >= 0 ; i--)
            {
                var edge = edgeList[i];
                int duplicateEdge = edgeList.FindIndex(x => x.Node1 == edge.Node2 && x.Node2 == edge.Node1);
                if (duplicateEdge > -1)
                {
                    edgeList.RemoveAt(duplicateEdge);
                }
            }

            return edgeList;
        }

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

                marked.Add((WeightedCoordinateGraphNode<T>)cur.Node);


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

        public bool CheckForUndirectedCycleUsingDjikstra(WeightedGraphNode<T> endpoint)
        {
            LastSearchSpace = 0;

            IPriorityQueue<WeightedNodePath<T>> priQueue = new Heap<WeightedNodePath<T>>(64);

            var begin = new WeightedNodePath<T>(endpoint, null, 0);
            priQueue.Enqueue(begin);

            /**
             * Assume the graph is a rooted tree with the endpoint as the root
             * This means we have a unique trace or path to reach each vertex from the endpoint
             * So, if we reach a vertex that already has a path assignment, we have found a cycle
             */
            var visitedByMap = new Dictionary<IGraphNode<T>, IGraphNode<T>>();

            while (!priQueue.IsEmpty())
            {
                WeightedNodePath<T> cur = priQueue.Dequeue();

                LastSearchSpace++;

                //add all new nodes to the stack
                foreach (var neighbor in cur.Node.GetNeighbors())
                {
                    var neighborVisited = visitedByMap.ContainsKey(neighbor);

                    if (!neighborVisited)
                    {   //process the unvisited neighbor

                        double edge_weight = ((WeightedGraphNode<T>)cur.Node).EdgeWeights[neighbor];

                        double new_weight = cur.PathWeightToHere + edge_weight;

                        priQueue.Enqueue(new WeightedNodePath<T>(neighbor, cur, new_weight, cur.PathLength + 1));

                        //the neighbor is visited, or found, by the current node.
                        visitedByMap.Add(neighbor, cur.Node);
                    }
                    //if the neighbor is visited, but was not visited by the current node, it means we have found a cycle i.e. another path to this node
                    else if (visitedByMap[neighbor] != cur.Node)
                        return true;
                }
            }

            return false;
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

            //nodes in this graph (G)
            var RemainingNodesInOriginal = new HashSet<IGraphNode<T>>(graphStructure.Count);
            foreach (var node in graphStructure)
                RemainingNodesInOriginal.Add(node);

            //Dictionary associates nodes in G as keys with nodes in MST(G) as values
            var VT = new Dictionary<IGraphNode<T>, WeightedGraphNode<T>>(graphStructure.Count);

            //pick an arbitrary node
            var arb_node = (WeightedGraphNode<T>)graphStructure[0];

            //remove it from the original set
            RemainingNodesInOriginal.Remove(arb_node);

            //place on the MST nodes
            VT.Add(arb_node, arb_node.Clone());

            //obtain sorted edge list for G
            var sortedEdgeList = SortedEdgeList();

            //edge list for the MST
            var ET = new List<Edge<T>>();

            while(RemainingNodesInOriginal.Count != 0)
            {
                //find the minimum edge, such that the first node is in the MST's nodes, and the second is in the remaining of G's nodes
                for (int i = 0; i < sortedEdgeList.Count; i++)
                {
                    //the min edge
                    Edge<T> e = sortedEdgeList[i];

                    WeightedGraphNode<T> newNode = null;

                    if (VT.ContainsKey((WeightedGraphNode<T>)e.Node1) && RemainingNodesInOriginal.Contains(e.Node2))
                    {
                        //a new node to add to the MST
                        newNode = (WeightedGraphNode<T>)e.Node2;
                        ET.Add(e);
                    }

                    if (VT.ContainsKey((WeightedGraphNode<T>)e.Node2) && RemainingNodesInOriginal.Contains(e.Node1))
                    {
                        //a new node to add to the MST
                        newNode = (WeightedGraphNode<T>)e.Node1;
                        ET.Add(e.FlipDirection());
                    }

                    if (newNode != null)
                    {
                        VT.Add(newNode, newNode.Clone());
                        RemainingNodesInOriginal.Remove(newNode);

                        sortedEdgeList.RemoveAt(i);
                        break;
                    }
                }

            }

            Graph<T> MST = new Graph<T>();

            foreach (var edge in ET)
            {
                //link up the clones
                VT[(WeightedGraphNode<T>)edge.Node1].AddNeighbor(VT[(WeightedGraphNode<T>)edge.Node2], edge.Weight);
            }

            foreach (var node in VT)
                MST.AddNode(node.Value);

            return MST;
        }

        public Graph<T> KruskalsAlgorithmForMinimumSpanningForest()
        {
            if (graphStructure.Count == 0)
                throw new Exception("Empty graph");

            //Dictionary associates nodes in G as keys with nodes in MST(G) as values
            var VT = new Dictionary<IGraphNode<T>, WeightedGraphNode<T>>(graphStructure.Count);

            //obtain sorted edge list for G
            var sortedEdgeList = EdgeListUndirected();

            Graph<T> MSF = new Graph<T>();

            foreach (var edge in sortedEdgeList)
            {
                if (VT.ContainsKey(edge.Node1)) {

                    //Since this edge contains both endpoints already in the MSF, check to see if a cycle is created.
                    if (VT.ContainsKey(edge.Node2))
                    {
                        //Some cycle exists!
                        if (MSF.CheckForUndirectedCycleUsingDjikstra(VT[edge.Node1]) || MSF.CheckForUndirectedCycleUsingDjikstra(VT[edge.Node2]) )
                            continue;
                    }
                    else
                    {
                        //add the second node to the list
                        var node = (WeightedGraphNode<T>)edge.Node2;
                        VT.Add(node, node.Clone());
                        MSF.AddNode(VT[node]);
                    }
                }
                else if (VT.ContainsKey(edge.Node2))
                {
                    //add the first node to the list
                    var node = (WeightedGraphNode<T>)edge.Node1;
                    VT.Add(node, node.Clone());
                    MSF.AddNode(VT[node]);
                }
                else
                {
                    //add both nodes to the list
                    var node1 = (WeightedGraphNode<T>)edge.Node1;
                    var node2 = (WeightedGraphNode<T>)edge.Node2;
                    VT.Add(node1, node1.Clone());
                    VT.Add(node2, node2.Clone());
                    MSF.AddNode(VT[node1]);
                    MSF.AddNode(VT[node2]);
                }
                VT[edge.Node1].AddMutualNeighbor(VT[edge.Node2], edge.Weight);
            }

            return MSF;
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

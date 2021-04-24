using System;
using System.Collections.Generic;
using System.Text;

namespace PathfindingTutorial.Data_Structures
{
    public partial class Graph<T> : IGraph<T>
    {
        protected readonly List<IGraphNode<T>> graphStructure = new();

        public int[,] AdjacencyMatrix { get; private set; }

        public List<int> DegreeSequence { get; private set; }

        public int TotalVertices { get; private set; }
        public int TotalEdges { get; private set; }

        public Graph<T> Clone()
        {
            var H = new Graph<T>();

            var clones = new Dictionary<IGraphNode<T>, IGraphNode<T>>(graphStructure.Count);

            foreach (var node in graphStructure)
            {
                var clone = node.Clone();
                clones.Add(node, clone);
                H.AddNode(clone);
            }
            foreach (var node in graphStructure)
                foreach (var neighbor in node.GetNeighbors())
                    clones[node].AddNeighbor(clones[neighbor]);

            H.TotalEdges = TotalEdges;

            return H;
        }

        #region Adjacency Matrix
        public int[,] GetAdjacencyMatrix(bool ForceRecalculate)
        {
            if (AdjacencyMatrix != null && !ForceRecalculate)
                return AdjacencyMatrix;

            //initialize the adjacency matrix
            AdjacencyMatrix = new int[graphStructure.Count, graphStructure.Count];

            //associate each node to a unique integer
            var nodeMap = new Dictionary<IGraphNode<T>, int>(graphStructure.Count);

            for (int i = 0; i < graphStructure.Count; i++)
                nodeMap.Add(graphStructure[i], i);

            for (int i = 0; i < graphStructure.Count; i++)
            {
                var node = graphStructure[i];
                var neighbors = node.GetNeighbors();
                foreach(var n in neighbors)
                    AdjacencyMatrix[i, nodeMap[n]] = 1;
            }

            return AdjacencyMatrix;
        }
        #endregion

        #region Degree Sequence
        /// <summary>
        /// Obtain a sorted degree sequence for the graph from greatest to least
        /// </summary>
        /// <returns></returns>
        public List<int> GetDegreeSequence(bool forceRecalculate, bool sort=true)
        {
            if (DegreeSequence != null && !forceRecalculate)
                return DegreeSequence;

            DegreeSequence = new List<int>(graphStructure.Count);

            foreach (var node in graphStructure)
                DegreeSequence.Add(node.GetDegree());

            if(sort)
                DegreeSequence.Sort((x, y) => y.CompareTo(x));

            return DegreeSequence;
        }

        private class DegreeSequenceData
        {
            public GraphNode<int> Vertex { get; private set; }
            public int DegreeValue { get; set; }

            public DegreeSequenceData(GraphNode<int> Vertex, int DegreeValue)
            {
                this.Vertex = Vertex;
                this.DegreeValue = DegreeValue;
            }
        }
        #endregion

        #region Edge Lists
        /// <summary>
        /// Returns a list of all edges sorted in order by weight
        /// </summary>
        /// <returns></returns>
        public List<Edge<T>> GetSortedEdgeList()
        {
            List<Edge<T>> edges = GetEdgeList();

            edges.Sort((x, y) => x.Weight.CompareTo(y.Weight));

            return edges;
        }

        public virtual List<Edge<T>> GetEdgeList()
        {
            var edges = new List<Edge<T>>();

            var adjacencyMatrix = GetAdjacencyMatrix(false);

            for (int i = 0; i < graphStructure.Count; i++)
            {
                var node1 = graphStructure[i];
                for (int j = i + 1; j < graphStructure.Count; j++)
                {
                    if(adjacencyMatrix[i,j] == 1)
                    {
                        var node2 = graphStructure[j];
                        if (node1 is WeightedGraphNode<T> wgn)
                            edges.Add(new Edge<T>(node1, node2, wgn.EdgeWeights[node2], false));
                        else
                            edges.Add(new Edge<T>(node1, node2, Directed:false));
                    }
                }
            }

            return edges;
        }

        public List<(int i, int j)> GetEdgeListIJPairs()
        {
            var edges = new List<(int i, int j)>(TotalEdges);

            var adjacencyMatrix = GetAdjacencyMatrix(true);

            for (int i = 0; i < graphStructure.Count; i++)
                for (int j = i + 1; j < graphStructure.Count; j++)
                {
                    if (adjacencyMatrix[i, j] == 1)
                        edges.Add((i, j));
                }

            return edges;
        }
        #endregion

        /// <summary>
        /// The number of nodes processed by the last search performed on this graph.
        /// </summary>
        public int LastSearchSpace { get; private set; }

        public Graph(int totalNodes)
        {
            graphStructure = new(totalNodes);
        }

        public Graph(params IGraphNode<T>[] nodes)
        {
            graphStructure = new List<IGraphNode<T>>(nodes);
            TotalVertices = nodes.Length;
            int degreeSum = 0;
            foreach (var node in nodes)
                degreeSum += node.GetDegree();
            TotalEdges = degreeSum / 2;
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
            TotalVertices++;
        }

        public void AddEdge(int i, int j)
        {
            IGraphNode<T>.AddMutualNeighbor(graphStructure[i], graphStructure[j]);
            TotalEdges++;
        }

        public void RemoveNode(IGraphNode<T> Node, bool RemoveNeighbors=false)
        {
            graphStructure.Remove(Node);
            TotalVertices--;
            if (RemoveNeighbors)
                foreach (var neighbor in Node.GetNeighbors())
                {
                    neighbor.RemoveNeighbor(Node);
                    TotalEdges--;
                }
        }

        public NodePath<T> RunDFS(IGraphNode<T> Start, IGraphNode<T> End)
        {
            LastSearchSpace = 0;

            var stk = new Stack<NodePath<T>>();

            var begin = new NodePath<T>(Start, null);

            stk.Push(begin);

            //this is our "marked" set
            var marked = new List<IGraphNode<T>>();

            while (stk.Count > 0)
            {
                LastSearchSpace++;
                var top = stk.Pop();

                if (top.Node == End)
                    return top;

                if (marked.Contains(top.Node))
                    continue;
                marked.Add(top.Node);

                //add all new nodes to the stack
                foreach (IGraphNode<T> neighbor in top.Node.GetNeighbors())
                    if (!marked.Contains(neighbor))
                        stk.Push(new NodePath<T>(neighbor, top, top.PathLength + 1));
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
            var marked = new List<IGraphNode<T>>();

            while (queue.Count > 0)
            {
                var front = queue.Dequeue();

                LastSearchSpace++;
                if (front.Node == End)
                    return front;

                if (marked.Contains(front.Node))
                    continue;
                marked.Add(front.Node);


                //add all new nodes to the stack
                foreach (IGraphNode<T> neighbor in front.Node.GetNeighbors())
                    if (!marked.Contains(neighbor)) //unmarked neighbor
                        queue.Enqueue(new NodePath<T>(neighbor, front, front.PathLength + 1));
            }

            return null;
        }

        public NodePath<T> RunSearch(IGraphSearcher<NodePath<T>> dataStructure, IGraphNode<T> Start, IGraphNode<T> End)
        {
            LastSearchSpace = 0;

            var begin = new NodePath<T>(Start, null);

            dataStructure.Add(begin);

            //this is our "marked" set
            var marked = new HashSet<IGraphNode<T>>();

            while (!dataStructure.IsEmpty())
            {
                var cur = dataStructure.Remove();

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

            var priQueue = new Heap<WeightedNodePath<T>>(64);

            var begin = new WeightedNodePath<T>(Start,null,0);
            priQueue.Enqueue(begin);

            //this is our "marked" set
            var marked = new HashSet<WeightedGraphNode<T>>();

            while (!priQueue.IsEmpty())
            {
                var front = priQueue.Dequeue();

                LastSearchSpace++;
                if (front.Node == End)
                    return front;

                var wgn = (WeightedGraphNode<T>)front.Node;

                if (marked.Contains(wgn))
                    continue;

                marked.Add(wgn);

                //add all new nodes to the stack
                foreach (var neighbor in front.Node.GetNeighbors())
                    if (!marked.Contains((WeightedGraphNode<T>)neighbor))
                    { //unmarked neighbor

                        double edge_weight = ((WeightedGraphNode<T>)front.Node).EdgeWeights[neighbor];

                        double new_weight = front.PathWeightToHere + edge_weight;

                        priQueue.Enqueue(new WeightedNodePath<T>(neighbor, front, new_weight, front.PathLength + 1));
                    }
            }

            return null;
        }

        /// <summary>
        /// Check for a cycle in a undirected graph
        /// </summary>
        /// <param name="endpoint">The vertex to start the BFS from. 
        /// This is the "root" of the tree structure created by the algorithm.
        /// </param>
        /// <returns>Bool indicating if a cycle was found</returns>
        public bool CheckForUndirectedCycleUsingBFS(WeightedGraphNode<T> endpoint)
        {
            LastSearchSpace = 0;

            var queue = new Queue<NodePath<T>>();

            var begin = new NodePath<T>(endpoint, null, 0);
            queue.Enqueue(begin);

            /**
             * Assume the graph is a rooted tree with the endpoint as the root
             * This means we have a unique trace or path to reach each vertex from the endpoint
             * So, if we reach a vertex that already has a path assignment, we have found a cycle
             */
            var visitedByMap = new Dictionary<IGraphNode<T>, IGraphNode<T>>();

            while (!queue.IsEmpty())
            {
                var front = queue.Dequeue();

                LastSearchSpace++;

                //add all new nodes to the stack
                foreach (var neighbor in front.Node.GetNeighbors())
                {
                    //do not process the endpoint more than once
                    if (neighbor == endpoint)
                        continue;

                    var neighborVisited = visitedByMap.ContainsKey(neighbor);

                    if (!neighborVisited)
                    {   //process the unvisited neighbor
                        //add the neighbor to the queue
                        queue.Enqueue(new NodePath<T>(neighbor, front, front.PathLength + 1));

                        //the neighbor is visited, or "found," by the current node.
                        visitedByMap.Add(neighbor, front.Node);
                    }
                    //if the neighbor is already visited by another node other than the current node,
                    //and if the current node is visited by another node other than the neighbor,
                    //it means we have found a cycle i.e. another path it is possible to reach the neighbor
                    else if (visitedByMap[front.Node] != neighbor && visitedByMap[neighbor] != front.Node)
                        return true;
                }
            }

            return false;
        }

        public WeightedNodePath<T> RunA_Star(WeightedGraphNode<T> Start, WeightedGraphNode<T> End, Func<WeightedGraphNode<T>, int> heuristic)
        {
            LastSearchSpace = 0;

            var priQueue = new Heap<WeightedNodePath<T>>(64);

            var begin = new WeightedNodePath<T>(Start, null, 0);
            priQueue.Enqueue(begin);

            //this is our "marked" set
            var marked = new HashSet<WeightedGraphNode<T>>();

            while (!priQueue.IsEmpty())
            {
                var front = priQueue.Dequeue();

                LastSearchSpace++;
                if (front.Node == End)
                    return front;

                var wgn = (WeightedGraphNode<T>)front.Node;

                if (marked.Contains(wgn))
                    continue;

                marked.Add(wgn);

                var newPathLength = front.PathLength + 1;

                //add all new nodes to the stack
                foreach (var neighbor in front.Node.GetNeighbors())
                {

                    var w_neighbor = (WeightedGraphNode<T>)neighbor;

                    if (!marked.Contains(w_neighbor))
                    { //unmarked neighbor

                        double edge_weight = ((WeightedGraphNode<T>)front.Node).EdgeWeights[neighbor];

                        //this is a somewhat complex weight function
                        //it adds the path length g(x)
                        //with the edge weight to get to the node e(x)
                        //with the heuristic of how close this node is to the solution h(x)
                        //so the formula is g(x) + e(x) + h(x)
                        double new_weight = newPathLength + edge_weight + heuristic(w_neighbor);

                        priQueue.Enqueue(new WeightedNodePath<T>(neighbor, front, new_weight, newPathLength));
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
            var remainingNodesInOriginal = new HashSet<IGraphNode<T>>(graphStructure.Count);
            foreach (var node in graphStructure)
                remainingNodesInOriginal.Add(node);

            //Dictionary associates nodes in V(G) as keys with nodes in V(MST_G) as values
            var vertexMap = new Dictionary<IGraphNode<T>, WeightedGraphNode<T>>(graphStructure.Count);

            //pick an arbitrary node
            var arb_node = (WeightedGraphNode<T>)graphStructure[0];

            //remove it from the original set
            remainingNodesInOriginal.Remove(arb_node);

            //place on the MST nodes
            vertexMap.Add(arb_node, arb_node.Clone());

            //obtain sorted edge list for G
            var sortedEdgeList = GetSortedEdgeList();

            //initialize the minimum spanning tree with the arbitrary node
            Graph<T> MST = new Graph<T>(vertexMap[arb_node]);

            while(remainingNodesInOriginal.Count > 0)
            {
                //find the minimum edge, such that the first node is in the MST's nodes, and the second is in the remaining of G's nodes
                for (int i = 0; i < sortedEdgeList.Count; i++)
                {
                    //the min edge
                    Edge<T> e = sortedEdgeList[i];

                    WeightedGraphNode<T> oldNode = null;
                    WeightedGraphNode<T> newNode = null;

                    if (vertexMap.ContainsKey((WeightedGraphNode<T>)e.Node1) && remainingNodesInOriginal.Contains(e.Node2))
                    {
                        oldNode = (WeightedGraphNode<T>)e.Node1;
                        //a new node to add to the MST
                        newNode = (WeightedGraphNode<T>)e.Node2;
                    }

                    if (vertexMap.ContainsKey((WeightedGraphNode<T>)e.Node2) && remainingNodesInOriginal.Contains(e.Node1))
                    {
                        oldNode = (WeightedGraphNode<T>)e.Node2;
                        //a new node to add to the MST
                        newNode = (WeightedGraphNode<T>)e.Node1;
                    }

                    if (newNode != null)
                    {
                        var newNodeClone = newNode.Clone();
                        vertexMap.Add(newNode, newNodeClone);
                        MST.AddNode(newNodeClone);
                        newNodeClone.AddMutualNeighbor(vertexMap[oldNode], e.Weight);

                        //remove the new node from processing
                        remainingNodesInOriginal.Remove(newNode);

                        //remove this edge from the sorted edge list
                        sortedEdgeList.RemoveAt(i);
                        break;
                    }
                }

            }

            return MST;
        }

        public Graph<T> KruskalsAlgorithmForMinimumSpanningForest()
        {
            if (graphStructure.Count == 0)
                throw new Exception("Empty graph");

            //Dictionary associates nodes in G as keys with nodes in MST(G) as values
            var vertexMap = new Dictionary<IGraphNode<T>, WeightedGraphNode<T>>(graphStructure.Count);

            //obtain sorted edge list for G
            var sortedEdgeList = GetEdgeList();

           var MSF = new Graph<T>();

            foreach (var edge in sortedEdgeList)
            {
                if (vertexMap.ContainsKey(edge.Node1)) {

                    if (vertexMap.ContainsKey(edge.Node2))
                    {
                        //add this test edge to the graph
                        vertexMap[edge.Node1].AddMutualNeighbor(vertexMap[edge.Node2], edge.Weight);
;
                        //Since this edge contains both endpoints already in the MSF, check to see if a cycle is created.
                        if (MSF.CheckForUndirectedCycleUsingBFS(vertexMap[edge.Node1])) {
                            //a cycle is created, so skip this edge.
                            vertexMap[edge.Node1].RemoveMutualNeighbor(vertexMap[edge.Node2]);
                            continue;
                        }
                    }
                    else
                    {
                        //add the second node to the list
                        var node = (WeightedGraphNode<T>)edge.Node2;
                        vertexMap.Add(node, node.Clone());
                        MSF.AddNode(vertexMap[node]);
                        vertexMap[edge.Node1].AddMutualNeighbor(vertexMap[edge.Node2], edge.Weight);
                    }
                }
                else if (vertexMap.ContainsKey(edge.Node2))
                {
                    //add the first node to the list
                    var node = (WeightedGraphNode<T>)edge.Node1;
                    vertexMap.Add(node, node.Clone());
                    MSF.AddNode(vertexMap[node]);
                    vertexMap[edge.Node1].AddMutualNeighbor(vertexMap[edge.Node2], edge.Weight);
                }
                else
                {
                    //add both nodes to the list
                    var node1 = (WeightedGraphNode<T>)edge.Node1;
                    var node2 = (WeightedGraphNode<T>)edge.Node2;
                    vertexMap.Add(node1, node1.Clone());
                    vertexMap.Add(node2, node2.Clone());
                    MSF.AddNode(vertexMap[node1]);
                    MSF.AddNode(vertexMap[node2]);
                    vertexMap[edge.Node1].AddMutualNeighbor(vertexMap[edge.Node2], edge.Weight);
                }
            }

            return MSF;
        }

        public List<Graph<T>> FindConnectedComponents()
        {
            if (graphStructure.Count == 0)
                throw new Exception("Empty graph");

            var components = new List<Graph<T>>();

            var queue = new Queue<NodePath<T>>();

            var remainingNodesInOriginal = new List<IGraphNode<T>>(TotalVertices);
            foreach (var node in graphStructure)
                remainingNodesInOriginal.Add(node);

            var arb_node = new NodePath<T>(remainingNodesInOriginal[0], null);

            queue.Enqueue(arb_node);

            var component = new Graph<T>();

            //this is our "marked" set
            var marked = new List<IGraphNode<T>>();

            void processMoreNodes()
            {
                if (queue.Count == 0)
                {
                    components.Add(component);
                    if (remainingNodesInOriginal.Count > 0)
                    {
                        queue.Enqueue(new NodePath<T>(remainingNodesInOriginal[0], null));
                        component = new Graph<T>();
                    }
                }
            }

            while (queue.Count > 0)
            {
                NodePath<T> front = queue.Dequeue();

                if (marked.Contains(front.Node)) {
                    processMoreNodes();
                    continue;
                }
                marked.Add(front.Node);
                component.AddNode(front.Node);
                remainingNodesInOriginal.Remove(front.Node);

                //add all new nodes to the stack
                foreach (IGraphNode<T> neighbor in front.Node.GetNeighbors())
                    if (!marked.Contains(neighbor)) //unmarked neighbor
                        queue.Enqueue(new NodePath<T>(neighbor, front, front.PathLength + 1));

                processMoreNodes();
            }

            return components;
        }

        public bool IsConnected()
        {
            if (graphStructure.Count == 0)
                throw new Exception("Empty graph");

            var queue = new Queue<IGraphNode<T>>(TotalVertices);

            var remainingNodesInOriginal = new List<IGraphNode<T>>(TotalVertices);
            foreach (var node in graphStructure)
                remainingNodesInOriginal.Add(node);

            var arb_node = remainingNodesInOriginal[0];

            queue.Enqueue(arb_node);

            //this is our "marked" set
            var marked = new List<IGraphNode<T>>(TotalVertices);

            while (queue.Count > 0)
            {
                var front = queue.Dequeue();

                marked.Add(front);
                remainingNodesInOriginal.Remove(front);

                //add all new nodes to the stack
                foreach (IGraphNode<T> neighbor in front.GetNeighbors())
                    if (!marked.Contains(neighbor)) //unmarked neighbor
                        queue.Enqueue(neighbor);

            }

            //all nodes processed
            return remainingNodesInOriginal.Count == 0;
        }

        private class MinorFindingGraphSearch : IComparable
        {
            public Graph<T> minor;
            public MinorFindingGraphSearch parent;
            public string action;
            public int generationNumber;

            public MinorFindingGraphSearch(Graph<T> minor, MinorFindingGraphSearch parent, int generation, string action = "")
            {
                this.minor = minor;
                this.parent = parent;
                this.generationNumber = generation;
                this.action = action;
            }

            public int CompareTo(object obj)
            {
                if (obj is MinorFindingGraphSearch cast)
                {
                    return (cast.minor.TotalVertices + cast.minor.TotalEdges).CompareTo(minor.TotalVertices + minor.TotalEdges);
                }
                return 0;
            }
        }

        
        public bool IsValidMinor(Graph<T> checkMinor, bool print=false)
        {
            if (checkMinor.TotalVertices > TotalVertices || checkMinor.TotalEdges > TotalEdges)
                return false;

            //clone the minor
            Graph<T> my_clone = Clone();

            //Get adjacency matrices for both graphs

            var adjMatrixMinor = checkMinor.GetAdjacencyMatrix(true);

            var degreeSeqGraph = GetDegreeSequence(false, true);

            //Get the degree sequence of the minor
            var degreeSeqCheck = checkMinor.GetDegreeSequence(false, true);

            if (print)
            {
                Console.WriteLine("***Adjacency Matrix of Original***\n");
                PrintAdjacencyMatrix(GetAdjacencyMatrix(true));
                Console.WriteLine("\nDegree Sequence: {0}", string.Join(",", degreeSeqGraph));
                Console.WriteLine("\n*******************************\n");

                Console.WriteLine("***Adjacency Matrix of Candidate Minor***\n");
                PrintAdjacencyMatrix(adjMatrixMinor);
                Console.WriteLine("\nDegree Sequence: {0}", string.Join(",", degreeSeqCheck));
                Console.WriteLine("\n*******************************\n");
            }

            //queue references adjacency matrix to an int of the number of edges removes
            var queue = new Heap<MinorFindingGraphSearch>(1000000);

            var graphs_found = new HashSet<string>(1000000);

            var start = new MinorFindingGraphSearch(my_clone, null, 0);

            static string get_gsf(Graph<T> x) => x.GetAdjCompactStr();

            graphs_found.Add(get_gsf(my_clone));

            queue.Enqueue(start);

            int searchSpace = 0;

            while (!queue.IsEmpty())
            {
                searchSpace++;

                var front = queue.Dequeue();

                var tot_minor_vertices = front.minor.TotalVertices;

                //do not spawn more minors if the vertex or edge difference is already met
                if (tot_minor_vertices < checkMinor.TotalVertices || front.minor.TotalEdges < checkMinor.TotalEdges)
                    continue;

                else if (tot_minor_vertices == checkMinor.TotalVertices && front.minor.TotalEdges == checkMinor.TotalEdges)
                {

                    //the graphs have the same size and order, so check for isomorphism.

                    if(print)
                        Console.WriteLine("Found a valid minor of same size and order...");

                    var isom = CheckGraphIsomorphism(checkMinor, front.minor, print);

                    if (!isom)
                        continue;

                    if (print)
                    {
                        Console.WriteLine("Minors are isomorphic ==> Valid Minor");

                        var stk = new Stack<MinorFindingGraphSearch>(front.generationNumber+1);

                        var backtracking = front;
                        while (backtracking != null)
                        {
                            stk.Push(backtracking);
                            backtracking = backtracking.parent;
                        }

                        while (stk.Count > 0)
                        {
                            var top = stk.Pop();
                            Console.WriteLine("-------------------------");
                            Console.WriteLine("Generation {0}: {1}", top.generationNumber, top.action);
                            top.minor.PrintAdjacencyMatrix();
                        }
                        PrintSearchStatistics(queue, searchSpace);
                    }

                    return true;
                }

                if (tot_minor_vertices > checkMinor.TotalVertices)
                {
                    //try removing a vertex...
                    for (int i = 0; i < front.minor.TotalVertices; i++)
                    {
                        int vertex_degree = graphStructure[i].GetDegree();

                        if (front.minor.TotalEdges - vertex_degree >= checkMinor.TotalEdges)
                        {

                            var new_minor = front.minor.Clone();

                            var vertex = new_minor.graphStructure[i];
                            new_minor.RemoveNode(vertex, true);

                            var gsf = get_gsf(new_minor);

                            if (graphs_found.Contains(gsf))
                                continue;

                            var vertex_removed_str = string.Format("Removed Node {0}", front.minor.graphStructure[i].GetValue());

                            queue.Enqueue(new MinorFindingGraphSearch(new_minor, front, front.generationNumber + 1, vertex_removed_str));
                            graphs_found.Add(gsf);
                        }
                    }
                }


                if (front.minor.TotalEdges > checkMinor.TotalEdges)
                {
                    var edgeList = front.minor.GetEdgeListIJPairs();

                    foreach (var (i, j) in edgeList)
                    {
                        //THIS IS A TEST
                        if (front.generationNumber >= TotalVertices - checkMinor.TotalVertices)
                        {

                            /*
                            var degseq_minor = front.minor.GetDegreeSequence(false, true);

                            bool impossible = false;

                            for (int k = 0; k < degseq_minor.Count; k++)
                                if (degseq_minor[k] < degreeSeqCheck[k])
                                {
                                    impossible = true;
                                    break;
                                }
                            if (impossible)
                                break;
                            */

                            //try removing an edge
                            var new_minor_edge_remove = front.minor.Clone();

                            new_minor_edge_remove.RemoveEdge(i, j);

                            var gsf = get_gsf(new_minor_edge_remove);

                            if (graphs_found.Contains(gsf))
                                continue;

                            var edge_removed_str = string.Format("Removed Edge {0}-{1}", front.minor.graphStructure[i].GetValue(), front.minor.graphStructure[j].GetValue());

                            queue.Enqueue(new MinorFindingGraphSearch(new_minor_edge_remove, front, front.generationNumber + 1, edge_removed_str));
                            graphs_found.Add(gsf);
                        }

                        if (tot_minor_vertices > checkMinor.TotalVertices)
                        {
                            int overlapEdgesRemoved = front.minor.TotalOverlapEdgesInContraction(i, j);
                            //skip bad contractions
                            if (front.minor.TotalEdges - 1 - overlapEdgesRemoved < checkMinor.TotalEdges)
                                continue;

                            //contract the edge
                            var new_minor_contract = front.minor.Clone();

                            new_minor_contract.ContractEdge(i, j);

                            var gsf = get_gsf(new_minor_contract);

                            if (graphs_found.Contains(gsf))
                                continue;

                            var edge_contracted_str = string.Format("Contracted Edge {0}-{1}", front.minor.graphStructure[i].GetValue(), front.minor.graphStructure[j].GetValue());

                            queue.Enqueue(new MinorFindingGraphSearch(new_minor_contract, front, front.generationNumber + 1, edge_contracted_str));
                            graphs_found.Add(gsf);
                        }
                    }
                }


            }

            if(print)
                PrintSearchStatistics(queue, searchSpace);

            //graph is not a valid minor
            return false;

            static void PrintSearchStatistics(Heap<MinorFindingGraphSearch> queue, int searchSpace) =>
                Console.WriteLine("\nSearch Space: {0} minors processed with {1} minors remaining in queue", searchSpace, queue.Count);
        }


        public void PrintAdjacencyMatrix(int[,] provided_adjMatrix = null)
        {
            var sb = new StringBuilder("   ");
            var adjacencyMatrix = provided_adjMatrix ?? GetAdjacencyMatrix(true);

            var totalVertices = adjacencyMatrix.GetLength(0);

            for (int i = 0; i < totalVertices; i++)
                sb.Append(graphStructure[i].GetValue()).Append(' ');

            sb.AppendLine();
            for (int i = 0; i < totalVertices; i++)
            {
                sb.Append(graphStructure[i].GetValue()).Append(':').Append(' ');
                for (int j = 0; j < totalVertices; j++)
                    sb.Append(adjacencyMatrix[i,j]).Append(' ');
                sb.AppendLine();
            }
            Console.Write(sb);
        }

        public string GetGraphStoreFormat()
        {
            //undirected graph
            var sb = new StringBuilder("ug");

            var adjacencyMatrix = GetAdjacencyMatrix(true);

            var totalVertices = adjacencyMatrix.GetLength(0);

            //total number of vertices
            sb.Append(totalVertices);

            sb.Append(':');

            for (int i = 0; i < totalVertices; i++)
                for (int j = 0; j < totalVertices; j++)
                    sb.Append(adjacencyMatrix[i,j]);

            return sb.ToString();
        }

        public string GetAdjCompactStr()
        {
            //undirected graph
            var sb = new StringBuilder(TotalVertices * (TotalVertices-1));

            var adjacencyMatrix = GetAdjacencyMatrix(false);

            for (int i = 0; i < TotalVertices; i++)
                for (int j = i; j < TotalVertices; j++)
                    sb.Append(adjacencyMatrix[i, j]);

            return sb.ToString();
        }

        public void PrintNodes()
        {
            foreach (var n in graphStructure)
                Console.WriteLine(n);
        }

        public void PrintEdges()
        {
            var edges = GetEdgeList();

            foreach (var edge in edges)
                Console.WriteLine(edge);
        }

        public int GetOrder() => graphStructure.Count;

        public int GetSize() => GetEdgeList().Count;

        public void RemoveEdge(int i, int j, bool directed = false)
        {
            TotalEdges--;
            IGraphNode<T>.RemoveMutualNeighbor(graphStructure[i], graphStructure[j]);
            if(AdjacencyMatrix != null)
            {
                AdjacencyMatrix[i, j] = 0;
                if(!directed)
                    AdjacencyMatrix[j, i] = 0;
            }
        }

        public int TotalOverlapEdgesInContraction(int i, int j)
        {
            var node1 = graphStructure[i];

            var existingNeighbors = node1.GetNeighbors();

            var node2 = graphStructure[j];

            int removed = 0;

            foreach (var neighbor in node2.GetNeighbors())
                if (existingNeighbors.Contains(neighbor))
                    removed++;

            return removed;
        }
        public void ContractEdge(int i, int j)
        {
            TotalEdges--;

            //keep node1
            var node1 = graphStructure[i];

            var existingNeighbors = node1.GetNeighbors();

            var node2 = graphStructure[j];

            //remove the edge between i and j
            IGraphNode<T>.RemoveMutualNeighbor(node1, node2);

            foreach (var neighbor in node2.GetNeighbors())
            {
                neighbor.RemoveNeighbor(node2);
                if (existingNeighbors.Contains(neighbor))
                {
                    TotalEdges--;
                    continue;
                }
                IGraphNode<T>.AddMutualNeighbor(node1, neighbor);
            }

            RemoveNode(node2);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace PathfindingTutorial.Data_Structures
{
    public class Graph<T> : IGraph<T>
    {
        protected readonly List<IGraphNode<T>> graphStructure = new();

        public int TotalVertices { get; private set; }
        public int TotalEdges { get; private set; }


        public Graph<T> Clone()
        {
            var H = new Graph<T>();

            var clones = new Dictionary<IGraphNode<T>, IGraphNode<T>>();

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
        public int[,] GetAdjacencyMatrix()
        {
            //initialize the adjacency matrix
            var adjacencyMatrix = new int[graphStructure.Count, graphStructure.Count];

            //associate each node to a unique integer
            var nodeMap = new Dictionary<IGraphNode<T>, int>();

            for (int i = 0; i < graphStructure.Count; i++)
                nodeMap.Add(graphStructure[i], i);

            for (int i = 0; i < graphStructure.Count; i++)
            {
                var node = graphStructure[i];
                var neighbors = node.GetNeighbors();
                foreach(var n in neighbors)
                    adjacencyMatrix[i, nodeMap[n]] = 1;
            }

            return adjacencyMatrix;
        }

        public static Graph<int> GenerateGraphForAdjacencyMatrix(int[,] adjacencyMatrix, bool undirected = false)
        {
            int length = adjacencyMatrix.GetLength(0);

            var G = new Graph<int>();

            for (int i = 0; i < length; i++)
                G.AddNode(new GraphNode<int>(i));

            for (int i = 0; i < length; i++)
                for (int j = 0; j < length; j++)
                    if (adjacencyMatrix[i, j] != 0)
                    {
                        G.TotalEdges++;
                        if (undirected)
                            IGraphNode<int>.AddMutualNeighbor(G.graphStructure[i], G.graphStructure[j]);
                        else
                            G.graphStructure[i].AddNeighbor(G.graphStructure[j]);
                    }

            return G;
        }
        #endregion

        #region Degree Sequence
        /// <summary>
        /// Obtain a sorted degree sequence for the graph from greatest to least
        /// </summary>
        /// <returns></returns>
        public List<int> GetDegreeSequence()
        {
            var degreeSequence = new List<int>(graphStructure.Count);

            foreach (var node in graphStructure)
                degreeSequence.Add(node.GetDegree());

            degreeSequence.Sort((x, y) => y.CompareTo(x));

            return degreeSequence;
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

        private List<ReferentialDegreeSequenceData> GetReferentialDegreeSequence()
        {
            var degreeSequenceDict = new Dictionary<IGraphNode<T>, ReferentialDegreeSequenceData>(graphStructure.Count);
            var degreeSequenceList = new List<ReferentialDegreeSequenceData>();

            foreach (var node in graphStructure)
            {
                var data = new ReferentialDegreeSequenceData(node, node.GetDegree());
                degreeSequenceList.Add(data);
                degreeSequenceDict.Add(node, data);
            }

            foreach (var node in graphStructure)
                foreach(var neighbor in node.GetNeighbors())
                    degreeSequenceDict[node].Connections.Add(neighbor, degreeSequenceDict[neighbor]);


            degreeSequenceList.Sort((x, y) => y.DegreeValue.CompareTo(x.DegreeValue));

            return degreeSequenceList;
        }

        private class ReferentialDegreeSequenceData
        {
            public IGraphNode<T> Vertex { get; private set; }
            public int DegreeValue { get; set; }
            public Dictionary<IGraphNode<T>, ReferentialDegreeSequenceData> Connections {get; private set;}

            public ReferentialDegreeSequenceData(IGraphNode<T> Vertex, int DegreeValue)
            {
                this.Vertex = Vertex;
                this.DegreeValue = DegreeValue;
                Connections = new Dictionary<IGraphNode<T>, ReferentialDegreeSequenceData>();
            }

            public ReferentialDegreeSequenceData RemoveEdge(ReferentialDegreeSequenceData other)
            {
                var clone = Clone();
                var other_clone = other.Clone();
                clone.DegreeValue--;
                other_clone.DegreeValue--;
                clone.Connections.Remove(other.Vertex);
                other_clone.Connections.Remove(Vertex);
                return clone;
            }

            public ReferentialDegreeSequenceData Clone()
            {
                var clone = new ReferentialDegreeSequenceData(Vertex, DegreeValue);
                foreach (var (key, value) in Connections)
                    clone.Connections.Add(key, value);
                return clone;
            }
        }

        /// <summary>
        /// Creates a complete graph
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static Graph<int> GenerateCompleteGraph(int n)
        {
            int[] degreeSequence = new int[n];
            for (int i = 0; i < n; i++)
                degreeSequence[i] = n-1;
            return GenerateGraphForDegreeSequence(degreeSequence);
        }

        /// <summary>
        /// Generate a graph that matches the given degree sequence. Returns null if unable to actualize.
        /// </summary>
        /// <param name="degreeSequence"></param>
        /// <returns></returns>
        public static Graph<int> GenerateGraphForDegreeSequence(int[] degreeSequence)
        {
            var degreeSequenceList = new List<DegreeSequenceData>(degreeSequence.Length);

            var G = new Graph<int>();
            for (int i = 0; i < degreeSequence.Length; i++)
            {
                var node = new GraphNode<int>(i);
                degreeSequenceList.Add(new DegreeSequenceData(node, degreeSequence[i]));
                G.AddNode(node);
            }

            while (true)
            {
                //sort the degree sequence from greatest to least degree value
                //if multiple have the same degree value, pick the smaller vertex
                degreeSequenceList.Sort((x, y) => x == y ? x.Vertex.GetValue().CompareTo(y.Vertex.GetValue()) : y.DegreeValue.CompareTo(x.DegreeValue));

                var maxDegreeData = degreeSequenceList[0];

                //get the maximum degree vertex
                int maxDegree = maxDegreeData.DegreeValue;

                //if the maximum degree vertex is 0, we can terminate the algorithm
                if (maxDegree == 0)
                    return G;

                //assign its degree to 0, as we will assign edges to use up all its degrees
                degreeSequenceList[0].DegreeValue = 0;

                //not enough nodes remain to add neighbors to
                if(maxDegree >= degreeSequence.Length)
                    return null;

                //assign a connected to the next maxDegree vertices, and decrement each degree value by 1
                for (int i = 1; i <= maxDegree; i++)
                {
                    degreeSequenceList[i].DegreeValue--;
                    //the neighbor node cannot be assigned, and therefore this degree sequence is not possible
                    if (degreeSequenceList[i].DegreeValue < 0)
                        return null;
                    //assign the vertex to each neighbor
                    IGraphNode<int>.AddMutualNeighbor(maxDegreeData.Vertex, degreeSequenceList[i].Vertex);
                }
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

        public List<Edge<T>> GetEdgeList()
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

        public List<Edge<T>> GetEdgeListUndirected()
        {
            var edges = new List<Edge<T>>();

            var adjacencyMatrix = GetAdjacencyMatrix();

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
        #endregion

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
            TotalVertices++;
        }

        public void RemoveNode(IGraphNode<T> Node)
        {
            graphStructure.Remove(Node);
            TotalVertices--;
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
            var sortedEdgeList = GetEdgeListUndirected();

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

            var remainingNodesInOriginal = new List<IGraphNode<T>>();
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

        public class EdgeRemovalGraphSearch
        {
            public int[,] adjacencyMatrix;
            public int[] totalNeighbors;
            public int edgesRemoved;
            public EdgeRemovalGraphSearch parent;
            public Edge<T> edgeRemoved;

            public EdgeRemovalGraphSearch(int[,] adjacencyMatrix, int[] totalNeighbors, int edgesRemoved, EdgeRemovalGraphSearch parent, Edge<T> edgeRemoved=null)
            {
                this.adjacencyMatrix = adjacencyMatrix;
                this.totalNeighbors = totalNeighbors;
                this.edgesRemoved = edgesRemoved;
                this.parent = parent;
                this.edgeRemoved = edgeRemoved;
            }
        }

        public bool IsValidMinor(Graph<int> checkMinor)
        {
            if (checkMinor.TotalVertices > TotalVertices || checkMinor.TotalEdges > TotalEdges)
                return false;

            //clone the minor
            var minorClone = checkMinor.Clone();


            //First, add as many vertices as needed...
            int vertexDifference = TotalVertices - minorClone.TotalVertices;

            for (int i = 0; i < vertexDifference; i++)
            {
                int label = minorClone.TotalVertices + i;
                Console.WriteLine("Created Vertex {0}", label);
                minorClone.AddNode(new GraphNode<int>(label));
            }

            var adjacencyMatrixOriginal = GetAdjacencyMatrix();

            var totalNeighborsOriginal = new int[TotalVertices];
            for (int i = 0; i < TotalVertices; i++)
                for (int j = 0; j < TotalVertices; j++)
                    if (adjacencyMatrixOriginal[i, j] == 1)
                        totalNeighborsOriginal[i]++;

            var degreeSeqMinor = minorClone.GetDegreeSequence();

            Console.WriteLine(string.Join(",", degreeSeqMinor));


            //var degreeSequenceOriginal = GetReferentialDegreeSequence();

            //var degreeSequenceMinor = minorClone.GetReferentialDegreeSequence();

            //figure out what edges to remove from the original to get to the minor

            //first figure out how many edges we have to remove
            int minorEdgeDifference = TotalEdges - minorClone.TotalEdges;

     

            //queue references adjacency matrix to an int of the number of edges removes
            var queue = new Queue<EdgeRemovalGraphSearch>();


            var start = new EdgeRemovalGraphSearch(adjacencyMatrixOriginal, totalNeighborsOriginal, 0, null);

            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                var front = queue.Dequeue();
                if (front.edgesRemoved > minorEdgeDifference)
                    continue;

                //check the neighbor sums to eliminate impossible assignments
                var sortedNeighbors = new List<int>(front.totalNeighbors);
                sortedNeighbors.Sort((x, y) => y.CompareTo(x));

                int equivalent = 0;
                for (int i = 0; i < TotalVertices; i++)
                    if (sortedNeighbors[i] < degreeSeqMinor[i])
                        continue;
                    else if (sortedNeighbors[i] == degreeSeqMinor[i])
                        equivalent++;

                if(equivalent == TotalVertices && front.edgesRemoved == minorEdgeDifference)
                {
                    Console.WriteLine("Graph is a valid minor");

                    var stk = new Stack<EdgeRemovalGraphSearch>(minorEdgeDifference);

                    var backtracking = front;
                    while (backtracking != null)
                    {
                        stk.Push(backtracking);
                        backtracking = backtracking.parent;
                    }

                    while(stk.Count > 0)
                    {
                        var top = stk.Pop();

                        var edgeRemoved = top.edgeRemoved;
                        if (edgeRemoved != null)
                            Console.WriteLine("Removed Edge {0}\n", edgeRemoved);

                        PrintAdjacencyMatrix(top.adjacencyMatrix);
                        Console.WriteLine();
                    }
                    return true;
                }

                //enqueue all possibilities of removing edges
                for (int i = 0; i < TotalVertices; i++)
                    for(int j = i + 1; j < TotalVertices; j++)
                        //for every edge, try removing it
                        if(front.adjacencyMatrix[i,j] == 1)
                        {
                            var newMatrix = (int[,])front.adjacencyMatrix.Clone();
                            newMatrix[i, j] = 0;
                            newMatrix[j, i] = 0;
                            var newTotalNeighbors = (int[])front.totalNeighbors.Clone();
                            newTotalNeighbors[i]--;
                            newTotalNeighbors[j]--;
                            var edgeRemoved = new Edge<T>(graphStructure[i], graphStructure[j]);
                            queue.Enqueue(new EdgeRemovalGraphSearch(newMatrix, newTotalNeighbors, front.edgesRemoved + 1, front, edgeRemoved));
                        }
                            
                    
            }

            //graph is not a valid minor
            return false;
        }

        public void PrintAdjacencyMatrix(int[,] provided_adjMatrix = null)
        {
            var sb = new StringBuilder("   ");
            var adjacencyMatrix = provided_adjMatrix == null ? GetAdjacencyMatrix() : provided_adjMatrix;

            for (int i = 0; i < graphStructure.Count; i++)
                sb.Append(graphStructure[i].GetValue() + " ");

            sb.AppendLine();
            for (int i = 0; i < graphStructure.Count; i++)
            {
                sb.Append(graphStructure[i].GetValue() + ": ");
                for (int j = 0; j < graphStructure.Count; j++)
                    sb.Append(adjacencyMatrix[i,j] + " ");
                sb.AppendLine();
            }
            Console.Write(sb);
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

        public void PrintEdgesUndirected()
        {
            var edges = GetEdgeListUndirected();

            foreach(var edge in edges)
                Console.WriteLine(edge);
        }

        public int GetOrder() => graphStructure.Count;

        public int GetSize() => GetEdgeListUndirected().Count;
    }
}

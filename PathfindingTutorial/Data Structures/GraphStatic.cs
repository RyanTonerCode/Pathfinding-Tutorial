using MatrixMath;
using System;
using System.Collections.Generic;
using System.Text;

namespace PathfindingTutorial.Data_Structures
{
    public partial class Graph<T> : IGraph<T>
    {

        #region Adjacency Matrix
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

            int sumDegreeSequence = 0;

            var G = new Graph<int>();
            for (int i = 0; i < degreeSequence.Length; i++)
            {
                var node = new GraphNode<int>(i);
                sumDegreeSequence += degreeSequence[i];
                degreeSequenceList.Add(new DegreeSequenceData(node, degreeSequence[i]));
                G.AddNode(node);
            }

            //By handshaking lemma
            G.TotalEdges = sumDegreeSequence / 2;

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

        // Generating permutation using Heap Algorithm
        private static void heapPermutations(int[] a, int size, int n, List<int[]> permutations)
        {
            var stk = new Stack<(int[] a, int size, int n)>();

            stk.Add((a, size, n));

            while (stk.Count > 0)
            {
                var (a1, size1, n1) = stk.Pop();

                if (size1 == 1)
                {
                    permutations.Add(a1);
                    continue;
                }

                for (int i = 0; i < size1; i++)
                {
                    stk.Add((a1, size1 - 1, n1));

                    a1 = (int[])a1.Clone();

                    // if size is odd, swap 0th i.e (first) and
                    // (size-1)th i.e (last) element
                    if (size1 % 2 == 1)
                    {
                        int temp = a1[0];
                        a1[0] = a1[size1 - 1];
                        a1[size1 - 1] = temp;
                    }

                    // If size is even, swap ith and
                    // (size-1)th i.e (last) element
                    else
                    {
                        int temp = a1[i];
                        a1[i] = a1[size1 - 1];
                        a1[size1 - 1] = temp;
                    }
                }

            }
        }

        public static bool CheckGraphIsomorphism(Graph<T> g1, Graph<T> g2)
        {
            Console.WriteLine("STARTING GRAPH ISOMORPHISM CHECK");

            //first look at the order and size
            if (g1.TotalVertices != g2.TotalVertices || g1.TotalEdges != g2.TotalEdges)
                return false;

            int totalVertices = g1.TotalVertices;

            var g1_DegreeSeq = g1.GetDegreeSequence(false, true);
            var g2_DegreeSeq = g2.GetDegreeSequence(false, true);

            //check the degree sequence next
            for (int i = 0; i < totalVertices; i++)
                if (g1_DegreeSeq[i] != g2_DegreeSeq[i])
                {
                    Console.WriteLine("Different degree sequence");
                    return false;
                }

            Console.WriteLine("Degree sequences are identical");

            var g1_adj = g1.GetAdjacencyMatrix(false);
            var g2_adj = g2.GetAdjacencyMatrix(false);

            //check adjacency matrix here
            int adjEqualCount = 0;
            for (int i = 0; i < totalVertices; i++)
                for (int j = 0; j < totalVertices; j++)
                    if (g1_adj[i, j] == g2_adj[i, j])
                        adjEqualCount++;

            if (adjEqualCount == totalVertices * totalVertices)
            {
                Console.WriteLine("Identical Adjacency Matrices");
                return true;
            }
            else
                Console.WriteLine("Adjacency matrices are distinct");

            //check eigenvalues here...


            //check permutations here...

            //map each possible degree in g1 to a list of vertices with that degree
            var g1_degreeMap = new Dictionary<int, List<int>>();
            var g2_degreeMap = new Dictionary<int, List<int>>();

            for (int i = 0; i < totalVertices; i++)
            {
                var degree_g1 = g1.graphStructure[i].GetDegree();
                if (g1_degreeMap.ContainsKey(degree_g1))
                    g1_degreeMap[degree_g1].Add(i);
                else
                    g1_degreeMap.Add(degree_g1, new List<int>() { i });

                var degree_g2 = g2.graphStructure[i].GetDegree();
                if (g2_degreeMap.ContainsKey(degree_g2))
                    g2_degreeMap[degree_g2].Add(i);
                else
                    g2_degreeMap.Add(degree_g2, new List<int>() { i });
            }

            //map vertices in g1 (keys) to exact matches in g2 for isomorphism (because they have a unique degree)
            var mapG1VerticesToG2Vertices = new Dictionary<int, int>();

            var permsFromG1ToG2 = new Dictionary<List<int>, List<int[]>>();

            foreach (var (degree, g1_node_list) in g1_degreeMap)
            {
                int totalVerticesOfDegree = g1_node_list.Count;
                if (totalVerticesOfDegree == 1)
                {
                    var g1_singleVertex = g1_degreeMap[degree][0];
                    var g2_singleVertex = g2_degreeMap[degree][0];

                    //map these nodes
                    mapG1VerticesToG2Vertices[g1_singleVertex] = g2_singleVertex;
                    Console.WriteLine("Mapping g1 Vertex {0} to g2 Vertex {1} due to unique degree {2}", g1_singleVertex, g2_singleVertex, degree);
                }
                else
                {
                    var perms = new List<int[]>();

                    heapPermutations(g2_degreeMap[degree].ToArray(), totalVerticesOfDegree, totalVerticesOfDegree, perms);

                    var g1_sb = new StringBuilder();
                    var g2_sb = new StringBuilder();

                    for (int i = 0; i < g1_degreeMap[degree].Count; i++)
                    {
                        g1_sb.Append(g1_degreeMap[degree][i]);
                        g2_sb.Append(g2_degreeMap[degree][i]);
                        if (i < g1_degreeMap[degree].Count - 1)
                        {
                            g1_sb.Append(',');
                            g2_sb.Append(',');
                        }
                    }

                    Console.WriteLine("G1 vertices of degree {0}: ({1})", degree, g1_sb);
                    Console.WriteLine("G2 vertices of degree {0}: ({1})", degree, g2_sb);


                    permsFromG1ToG2.Add(g1_node_list, perms);
                }

            }

            //need to pick a perm from each perm set...

            var permIndexer = new Dictionary<int, List<int>>();

            var permutationPicker = new List<int[]>();

            int perms_processed = 0;
            foreach(var key in permsFromG1ToG2.Keys)
            {
                var permList = permsFromG1ToG2[key];

                permIndexer.Add(perms_processed, key);

                //for the first perm set, simply add them all
                if (perms_processed == 0)
                {
                    //add all possible perms to the permutation picker
                    for (int select_perm = 0; select_perm < permList.Count; select_perm++)
                        permutationPicker.Add(new int[] { select_perm });
                }
                else
                {
                    var clone_old_perms = permutationPicker.ToArray();
                    //clear the old perms
                    permutationPicker.Clear();

                    for (int select_perm = 0; select_perm < permList.Count; select_perm++)
                    {
                        for(int prev_perm = 0; prev_perm < clone_old_perms.Length; prev_perm++)
                        {
                            var add_next_perm = new int[perms_processed + 1];
                            Array.Copy(clone_old_perms[prev_perm], add_next_perm, perms_processed);
                            add_next_perm[perms_processed] = select_perm;

                            permutationPicker.Add(add_next_perm);
                        }
                    }
                }
                
                perms_processed++;
            }

            //G2 = PG1P^(-1)

            var G1 = new Matrix(g1_adj);
            var G2 = new Matrix(g2_adj);

            //create a template permutation matrix
            var template_perm_matrix = new int[totalVertices, totalVertices];
            //link up the unique vertices
            foreach (var kvp in mapG1VerticesToG2Vertices)
            {
                int vertexInG1 = kvp.Key;
                int vertexInG2 = kvp.Value;
                template_perm_matrix[vertexInG2, vertexInG1] = 1;
            }

            //new Matrix(template_perm_matrix).Print();

            int totalPermutationMatricesGenerated = 0;

            foreach (int[] perm_pick in permutationPicker)
            {
                //create the permutation matrix for the vertices from G1 to vertices in G2
                var P = new Matrix(template_perm_matrix);

                totalPermutationMatricesGenerated++;

                var perm_str_g1 = new StringBuilder();
                var perm_str_g2 = new StringBuilder();

                //obtain the set value for each permutation
                for (int perm_set = 0; perm_set < perm_pick.Length; perm_set++)
                {
                    var perm_index = perm_pick[perm_set];

                    var perm_key = permIndexer[perm_set];

                    var perm_value = permsFromG1ToG2[perm_key][perm_index];

                    perm_str_g1.Append('(');
                    perm_str_g2.Append('(');

                    for (int i = 0; i < perm_key.Count; i++) {

                        int vertexInG1 = perm_key[i];
                        int vertexInG2 = perm_value[i];
                        P[vertexInG2, vertexInG1] = 1;

                        perm_str_g1.Append(vertexInG1);
                        perm_str_g2.Append(vertexInG2);

                    }
                    perm_str_g1.Append(')');
                    perm_str_g2.Append(')');

                }

                Console.WriteLine("Permutation\n{0}\n{1}", perm_str_g1, perm_str_g2);

                //G2= P * G1 * P^-1
                //G2*P = P * G1
                //var result = P * G1 * P.GetInverseMatrix();

                var G2P = G2 * P;
                var PG1 = P * G1;

                if (G2P.Equals(PG1))
                {
                    Console.WriteLine("\nIsomorphic!");

                    Console.WriteLine("G1");
                    G1.Print();
                    Console.WriteLine("G2");
                    G2.Print();
                    Console.WriteLine("PG1");
                    PG1.Print();
                    Console.WriteLine("G2P");
                    G2P.Print();

                    Console.WriteLine("P");
                    P.Print();

                    for (int i = 0; i < totalVertices; i++)
                        for (int j = 0; j < totalVertices; j++)
                            if(P[i,j] == 1)
                                Console.WriteLine("Mapped g1 Vertex {0} to g2 Vertex {1}", i, j);

                    return true;
                }

            }

            Console.WriteLine("Could not find an isomorphism, generated {0} permutation matrices", totalPermutationMatricesGenerated);

            return false;
        }

    }
}

using System;
using System.Collections.Generic;

namespace ReGenIndexedGraph
{
    abstract class Rule
    {
        protected String ruleName;
        public Rule()
        {
            this.ruleName = null;
        }
        public Rule(String ruleName)
        {
            this.ruleName = ruleName;
        }
        public void SetRuleName(String ruleName)
        {
            this.ruleName = ruleName;
        }
        public String GetRuleName()
        {
            return this.ruleName;
        }
        public abstract List<Graph> ImplementRule(Graph graph);
    }
    class LoveRule : Rule
    {
        public override List<Graph> ImplementRule(Graph graph)
        {
            List<Graph> returnGraph = new List<Graph>();
            List<Vertex> vertices = graph.GetGraphVertices();
            foreach (Vertex vertex in vertices)
            {
                if (vertex != null)
                {
                    List<Vertex> destVertices = graph.GetDistantOutGoingVerticesByRelationLabel(vertex, "loves");

                    foreach (Vertex vertex1 in destVertices)
                    {
                        if (!graph.GetDistantOutGoingVerticesByRelationLabel(vertex1, "loves").Contains(vertex))
                        {
                            if (graph.GetDistantOutGoingVerticesByRelationLabel(vertex1, "loves").Count > 0)
                            {
                                Graph tempGraph = new Graph();
                                tempGraph.AddVertex(vertex.GetLabel());
                                tempGraph.AddVertex(vertex1.GetLabel());
                                tempGraph.AddVertex(graph.GetDistantOutGoingVerticesByRelationLabel(vertex1, "loves")[0].GetLabel());
                                tempGraph.SetRelation(tempGraph.GetVertex(1).GetIndex(), tempGraph.GetVertex(2).GetIndex(), "love");
                                tempGraph.SetRelation(tempGraph.GetVertex(2).GetIndex(), tempGraph.GetVertex(3).GetIndex(), "love");
                                returnGraph.Add(tempGraph);
                            }
                        }
                    }
                }
            }
            return returnGraph;
        }
    }
    class VertexAttributes
    {
        private String name;
        private int rank;
        public VertexAttributes()
        {
            name = null;
            rank = 0;
        }
        public VertexAttributes(String name, int rank)
        {
            this.name = name;
            this.rank = rank;
        }
        public void SetName(String name)
        {
            this.name = name;
        }
        public void SetRank(int rank)
        {
            this.rank = rank;
        }
        public String GetName()
        {
            return this.name;
        }
        public int GetRank()
        {
            return this.rank;
        }
        public VertexAttributes GetAttributes()
        {
            return this;
        }
    }
    class Vertex
    {
        private String label;
        private int index;
        private Dictionary<String, Object> vertexAttributes;
        private List<String> edgeIndices;
        public Vertex()
        {
            this.label = null;
            this.edgeIndices = new List<String>();
            this.vertexAttributes = null;
            this.index = 0;
        }
        public Vertex(String label)
        {
            this.label = label;
            this.edgeIndices = new List<String>();
            this.vertexAttributes = null;
            this.index = 0;
        }
        public Vertex(String label, int index)
        {
            this.label = label;
            this.edgeIndices = new List<String>();
            this.vertexAttributes = null;
            this.index = index;
        }
        public Vertex(String label, List<String> edgeIndices, int index)
        {
            this.label = label;
            this.edgeIndices = edgeIndices;
            this.vertexAttributes = null;
            this.index = index;
        }
        public Vertex(String label, List<String> edgeIndices, Dictionary<String, Object> vertexAttributes, int index)
        {
            this.label = label;
            this.edgeIndices =edgeIndices;
            this.vertexAttributes = vertexAttributes;
            this.index = index;
        }
      
        public void AddEdgeIndex(String index)
        {
            this.edgeIndices.Add(index);
        }
        public void SetLabel(String label)
        {
            this.label = label;
        }
        public void SetIndex(int index)
        {
            this.index = index;
        }
        public void SetEdgeIndices(List<String> edgeIncides)
        {
            this.edgeIndices = edgeIncides;
        }
        public void SetAttributes(Dictionary<String, Object> attributes)
        {
            this.vertexAttributes = attributes;
        }
        public void SetAttribute(String key, Object attribute)
        {
            if (this.vertexAttributes == null) {
                this.vertexAttributes = new Dictionary<String, Object>();
            }
            this.vertexAttributes.Add(key, attribute);
        }

        public String GetLabel()
        {
            return this.label;
        }
        public int GetIndex()
        {
            return this.index;
        }
        public List<String> GetEdgeIndices()
        {
            return this.edgeIndices;
        }
        public Dictionary<String, Object> GetAttributes()
        {
            return this.vertexAttributes;
        }
        public T GetAttribute<T>(String key)
        {
            return (T)this.vertexAttributes[key];
        }
        public Object GetAttribute(String key)
        {
            return this.vertexAttributes[key];
        }

        public void SetVertex(String label, Dictionary<String, Object> attributes, List<String>edgeIndices)
        {
            this.label = label;
            this.vertexAttributes = attributes;
            this.edgeIndices = edgeIndices;
        }
        public Vertex GetVertex()
        {
            return this;
        }
        public override int GetHashCode()
        {
            return this.index;
        }
        public override bool Equals(Object obj)
        {
            return (obj is Vertex) && ((Vertex)obj).index == index;
        }
    }
    class Edge
    {
        private String relationLabel, reasonLabel;
        public Edge()
        {
            this.relationLabel = this.reasonLabel = null;
        }
        public Edge(String relationLabel, String reasonLabel)
        {

            this.relationLabel = relationLabel;
            if (reasonLabel == null)
            {
                this.reasonLabel = null;
                return;
            }
            this.reasonLabel = reasonLabel;
        }
        public void SetReasonLabel(String reasonLabel)
        {
            this.reasonLabel=reasonLabel;
        }
        public void SetRelationLabel(String relationLabel)
        {
            this.relationLabel=relationLabel;
        }

        public String GetReasonLabel()
        {
            return this.reasonLabel;
        }
        public String GetRelationLabel()
        {
            return this.relationLabel;
        }
        public Edge GetEdge()
        {
            return this;
        }
        public override int GetHashCode()
        {
            if (reasonLabel == null)
            {
                return this.relationLabel == null ? 0 : this.relationLabel.GetHashCode() ^ 0;
            }
            return this.relationLabel.GetHashCode() ^ this.reasonLabel.GetHashCode();
        }
        public override bool Equals(Object obj)
        {
            return (obj is Edge) && ((Edge)obj).relationLabel == relationLabel && ((Edge)obj).reasonLabel == reasonLabel;
        }
    }
    class Graph
    {
        private List<Vertex> vertices;
        private Dictionary<String, List<Edge>> edges;
        private static int verticesCount;
        static int binarySearchCount(List<int> list, int element)
        {
            int first = 0;
            int last = list.Count-1;
            int mid = 0 + (list.Count-1) / 2;
            int lesserCount=0;
            while (first <= last)
            {
                if (list[mid] < element)
                {
                    first = mid + 1;
                    lesserCount += (mid-first);
                }
                else if (list[mid] == element)
                {
                    return lesserCount;
                }
                else
                {
                    last = mid - 1;
                }
                mid = (first + last) / 2;

            }
            lesserCount = first;
            return lesserCount;

        }
        public Graph()
        {
            this.vertices = new List<Vertex>();
            this.edges = new Dictionary<string, List<Edge>>();
            Graph.verticesCount = 0;
        }
        public Graph(List<Vertex> vertices)
        {
            this.vertices = vertices;
            Graph.verticesCount = this.vertices.Count;
            this.edges = new Dictionary<string, List<Edge>>();
        }
        public Graph(List<Vertex> vertices, Dictionary<String, List<Edge>> edges)
        {
            this.vertices = vertices;
            Graph.verticesCount = this.vertices.Count;
            this.edges = edges;
        }
        public void AddVertex(String vertexLabel)
        {
            vertices.Add(new Vertex(vertexLabel, vertices.Count + 1));
            Graph.verticesCount++;
        }
        public void AddVertex(Vertex vertex)
        {
            if (vertex.GetIndex() <= Graph.verticesCount || vertex.GetIndex() > (Graph.verticesCount + 1))
            {
                Console.WriteLine("Vertex index must be equal to {0}", Graph.verticesCount + 1);
                return;
            }
            vertices.Add(vertex);
            Graph.verticesCount++;
        }
        public Vertex GetVertex(int index)
        {
            if (index == 0 || index < 0 || index > Graph.verticesCount) {
                Console.WriteLine("Index must be greater than zero or less than total vertices, returning empty vertex");
                return new Vertex();
            }
            return this.vertices[index-1];
        }
        public void SetRelation(int srcIndex, int destIndex, String relationLabel, String reasonLabel)
        {
            int verticesCount = Graph.verticesCount;
            if (srcIndex > verticesCount || srcIndex==0)
            {
                Console.WriteLine("src node doesn't exist in graph");
                return;
            }
            if (destIndex > verticesCount || destIndex == 0)
            {
                Console.WriteLine("dest node doesn't exist in graph");
                return;
            }
            String edgeKey = srcIndex.ToString() + "-" + destIndex.ToString();
            List<Edge> temp;
            if (this.edges.ContainsKey(edgeKey))
            {
                temp = this.edges[edgeKey];
                if (!temp.Contains(new Edge(relationLabel, reasonLabel)))
                {
                    temp.Add(new Edge(relationLabel, reasonLabel));
                    this.edges[edgeKey] = temp;
                }
                else
                {
                    Console.WriteLine("Same edge already exists");
                }
            }
            else
            {
                temp = new List<Edge>();
                temp.Add(new Edge(relationLabel, reasonLabel));
                this.edges.Add(edgeKey, temp);
                this.vertices[srcIndex-1].AddEdgeIndex(edgeKey);
                this.vertices[destIndex-1].AddEdgeIndex(edgeKey);
            }
        }
        public void SetRelation(int srcIndex, int destIndex, String relationLabel)
        {
            int verticesCount = Graph.verticesCount;
            if (srcIndex > verticesCount || srcIndex == 0)
            {
                Console.WriteLine("src node doesn't exist in graph");
                return;
            }
            if (destIndex > verticesCount || destIndex == 0)
            {
                Console.WriteLine("dest node doesn't exist in graph");
                return;
            }

            String edgeKey = srcIndex.ToString() + "-" + destIndex.ToString();
            List<Edge> temp;
            if (this.edges.ContainsKey(edgeKey))
            {
                temp = this.edges[edgeKey];
                Edge tempEdge = new Edge();
                tempEdge.SetRelationLabel(relationLabel);
              
                if (!temp.Contains(tempEdge))
                {
                    temp.Add(tempEdge);
                    this.edges[edgeKey] = temp;
                }
                else
                {
                    Console.WriteLine("Same edge already exists");
                }
            }
            else
            {
                temp = new List<Edge>();
                Edge tempEdge = new Edge();
                tempEdge.SetRelationLabel(relationLabel);
                temp.Add(tempEdge);
                this.edges.Add(edgeKey, temp);
                this.vertices[srcIndex-1].AddEdgeIndex(edgeKey);
                this.vertices[destIndex-1].AddEdgeIndex(edgeKey);
            }
        }

        public void SetRelation(Vertex src, Vertex dest, String relationLabel, String reasonLabel)
        {
            int srcIndex = src.GetIndex();
            int destIndex = dest.GetIndex();
            int verticesCount = Graph.verticesCount;
            if (srcIndex > verticesCount || srcIndex == 0)
            {
                Console.WriteLine("src node doesn't exist in graph");
                return;
            }
            if (destIndex > verticesCount || destIndex == 0)
            {
                Console.WriteLine("dest node doesn't exist in graph");
                return;
            }
            String edgeKey = srcIndex.ToString() + "-" + destIndex.ToString();
            List<Edge> temp;
            if (this.edges.ContainsKey(edgeKey))
            {
                temp = this.edges[edgeKey];
                if (!temp.Contains(new Edge(relationLabel, reasonLabel)))
                {
                    temp.Add(new Edge(relationLabel, reasonLabel));
                    this.edges[edgeKey] = temp;
                }
                else
                {
                    Console.WriteLine("Same edge already exists");
                }
            }
            else
            {
                temp = new List<Edge>();
                temp.Add(new Edge(relationLabel, reasonLabel));
                this.edges.Add(edgeKey, temp);
                this.vertices[srcIndex - 1].AddEdgeIndex(edgeKey);
                this.vertices[destIndex - 1].AddEdgeIndex(edgeKey);
            }
        }

        public void SetRelation(Vertex src, Vertex dest, String relationLabel)
        {
            int srcIndex = src.GetIndex();
            int destIndex = dest.GetIndex();
            int verticesCount = Graph.verticesCount;
            if (srcIndex > verticesCount || srcIndex == 0)
            {
                Console.WriteLine("src node doesn't exist in graph");
                return;
            }
            if (destIndex > verticesCount || destIndex == 0)
            {
                Console.WriteLine("dest node doesn't exist in graph");
                return;
            }

            String edgeKey = srcIndex.ToString() + "-" + destIndex.ToString();
            List<Edge> temp;
            if (this.edges.ContainsKey(edgeKey))
            {
                temp = this.edges[edgeKey];
                Edge tempEdge = new Edge();
                tempEdge.SetRelationLabel(relationLabel);

                if (!temp.Contains(tempEdge))
                {
                    temp.Add(tempEdge);
                    this.edges[edgeKey] = temp;
                }
                else
                {
                    Console.WriteLine("Same edge already exists");
                }
            }
            else
            {
                temp = new List<Edge>();
                Edge tempEdge = new Edge();
                tempEdge.SetRelationLabel(relationLabel);
                temp.Add(tempEdge);
                this.edges.Add(edgeKey, temp);
                this.vertices[srcIndex - 1].AddEdgeIndex(edgeKey);
                this.vertices[destIndex - 1].AddEdgeIndex(edgeKey);
            }
        }

        public void DeleteVertex(int index)
        {
            if (this.vertices[index - 1] == null)
            {
                Console.WriteLine("This node is already deleted");
                return;
            }
            List<String> edgeKeys = this.vertices[index-1].GetEdgeIndices();
            foreach (String edgeKey in edgeKeys)
            {
                int destIndex = Convert.ToInt32(edgeKey.Split("-")[0]);
                int srcIndex = Convert.ToInt32(edgeKey.Split("-")[1]);
                if (destIndex == index) {
                    this.vertices[srcIndex-1].GetEdgeIndices().Remove(edgeKey);
                }
                if (srcIndex == index)
                {
                    this.vertices[destIndex-1].GetEdgeIndices().Remove(edgeKey);
                }
                this.edges.Remove(edgeKey);
            }
            this.vertices[index-1]=null;
        }
        public void DeleteRelation(String edgeKey, Edge edge) {
            if (this.edges.ContainsKey(edgeKey))
            {
                if (this.edges[edgeKey].Count == 1)
                {
                    this.vertices[Convert.ToInt32(edgeKey.Split("-")[0])-1].GetEdgeIndices().Remove(edgeKey);
                    this.vertices[Convert.ToInt32(edgeKey.Split("-")[1])-1].GetEdgeIndices().Remove(edgeKey);
                    this.edges.Remove(edgeKey);
                }
                else
                {
                    List<Edge> temp = this.edges[edgeKey];
                    temp.Remove(edge);
                    this.edges[edgeKey] = temp;
                }
            }
            else {
                Console.WriteLine("Relation Doesnot exist");
            }
        }
        public void DeleteRelation(String edgeKey, String relationLabel, String reasonLabel)
        {
            if (this.edges.ContainsKey(edgeKey))
            {
                if (this.edges[edgeKey].Count == 1)
                {
                    this.vertices[Convert.ToInt32(edgeKey.Split("-")[0])-1].GetEdgeIndices().Remove(edgeKey);
                    this.vertices[Convert.ToInt32(edgeKey.Split("-")[1])-1].GetEdgeIndices().Remove(edgeKey);
                    this.edges.Remove(edgeKey);
                }
                else
                {
                    Edge edge = new Edge(relationLabel, reasonLabel);
                    List<Edge> temp = this.edges[edgeKey];
                    temp.Remove(edge);
                    this.edges[edgeKey] = temp;
                }
            }
        }
        public void DeleteRelations(String edgeKey)
        {
            if (this.edges.ContainsKey(edgeKey))
            {
                    this.vertices[Convert.ToInt32(edgeKey.Split("-")[0])-1].GetEdgeIndices().Remove(edgeKey);
                    this.vertices[Convert.ToInt32(edgeKey.Split("-")[1])-1].GetEdgeIndices().Remove(edgeKey);
                    this.edges.Remove(edgeKey);   
            }
        }

        public static int GetVerticesCount()
        {
            return Graph.verticesCount;
        }
        public void UpdateGraph() {
            Vertex prev=new Vertex();
            bool updated = false;
            prev.SetIndex(1);
            int currentIndex = 0;
            List<int> tempIndexes = new List<int>();
            foreach(Vertex v in this.vertices.ToArray())
            {
                if (v == null)
                {
                    PrintVertices();
                    this.vertices.RemoveAt(currentIndex);
                    tempIndexes.Add(currentIndex);
                    updated = true;
                }
                else
                    currentIndex++;
            }
            if (updated == false)
            {
                Console.WriteLine("Noting to Update");
                return;
            }

            Graph.verticesCount = this.vertices.Count;
            currentIndex = -1;
            List<String> tempIndices = new List<string>();
            foreach(Vertex v in this.vertices)
            {
                currentIndex++;
                if (v.GetIndex() != currentIndex + 1)
                {
                    int i = -1;
                    if (v.GetEdgeIndices().Count == 0)
                    {
                        v.SetIndex(currentIndex + 1);

                    }
                    foreach (String edgeKey in v.GetEdgeIndices().ToArray())
                    {
                        i++;
                        int srcIndex = Convert.ToInt32(edgeKey.Split("-")[0]);
                        int destIndex = Convert.ToInt32(edgeKey.Split("-")[1]);
                        int newSrcIndex = srcIndex - binarySearchCount(tempIndexes, srcIndex);
                        int newDestIndex = destIndex - binarySearchCount(tempIndexes, destIndex);
                        Console.WriteLine(tempIndexes[0]+", count"+ binarySearchCount(tempIndexes, 2)+" ,V: " + v.GetLabel() + ", src:" + srcIndex + " ,dest:" + destIndex+", nsI: " +newSrcIndex+", ndI:"+newDestIndex);
                        if ( srcIndex == v.GetIndex())
                        {
                            String updatedEdgeKey = (currentIndex + 1) + "-" + destIndex;
                            v.GetEdgeIndices()[i] = updatedEdgeKey;
                            this.vertices[newDestIndex-1].GetEdgeIndices().Remove(edgeKey);
                            this.vertices[newDestIndex-1].GetEdgeIndices().Add(updatedEdgeKey);
                            this.edges.Add(updatedEdgeKey, this.edges[edgeKey]);
                            this.edges.Remove(edgeKey);

                        }
                        else if (destIndex == v.GetIndex())
                        {
                                String updatedEdgeKey = srcIndex + "-" + (currentIndex + 1);
                                v.GetEdgeIndices()[i] = updatedEdgeKey;
                                this.vertices[newSrcIndex-1].GetEdgeIndices().Remove(edgeKey);
                                this.vertices[newSrcIndex-1].GetEdgeIndices().Add(updatedEdgeKey);
                                this.edges.Add(updatedEdgeKey, this.edges[edgeKey]);
                                this.edges.Remove(edgeKey);
                        }
                    }
                    v.SetIndex(currentIndex + 1);

                }
                
            }
        }
        public List<Vertex> GetGraphVertices()
        {
            return this.vertices;
        }
        public List<Vertex> GetIncomingVertices(Vertex vertex)
        {
            List<Vertex> srcVertices = new List<Vertex>();
            List<String> edgeKeys = vertex.GetEdgeIndices();
            foreach (String edgeKey in edgeKeys)
            {
                srcVertices.Add(this.vertices[Convert.ToInt32(edgeKey.Split("-")[0])-1]);
            }
            return srcVertices;
        }
        public List<Vertex> GetOutGoingVertices(Vertex vertex)
        {
            List<Vertex> destVertices = new List<Vertex>();
            List<String> edgeKeys = vertex.GetEdgeIndices();
            foreach (String edgeKey in edgeKeys)
            {
                destVertices.Add(this.vertices[Convert.ToInt32(edgeKey.Split("-")[1])-1]);
            }
            return destVertices;
        }
        public List<Vertex> GetDistantIncomingVertices(Vertex vertex)
        {
            List<Vertex> srcVertices = new List<Vertex>();
            List<String> edgeKeys = vertex.GetEdgeIndices();
            foreach (String edgeKey in edgeKeys)
            {
                if(!edgeKey.Split("-")[0].Equals(vertex.GetIndex().ToString()))
                    srcVertices.Add(this.vertices[Convert.ToInt32(edgeKey.Split("-")[0])-1]);
            }
            return srcVertices;
        }
        public List<Vertex> GetDistantOutGoingVertices(Vertex vertex)
        {
            List<Vertex> destVertices = new List<Vertex>();
            List<String> edgeKeys = vertex.GetEdgeIndices();
            foreach (String edgeKey in edgeKeys)
            {
                if (!edgeKey.Split("-")[1].Equals(vertex.GetIndex().ToString()))
                    destVertices.Add(this.vertices[Convert.ToInt32(edgeKey.Split("-")[1])-1]);
            }
            return destVertices;
        }
        public List<Vertex> GetDistantAllVertices(Vertex vertex)
        {
            List<Vertex> allVertices = new List<Vertex>();
            List<String> edgeKeys = vertex.GetEdgeIndices();
            foreach (String edgeKey in edgeKeys)
            {
                if (!edgeKey.Split("-")[0].Equals(vertex.GetIndex().ToString()))
                    allVertices.Add(this.vertices[Convert.ToInt32(edgeKey.Split("-")[0])-1]);
                else
                    allVertices.Add(this.vertices[Convert.ToInt32(edgeKey.Split("-")[1])-1]);
            }
            return allVertices;
        }
        public List<Vertex> GetDistantIncomingVerticesByRelationLabel(Vertex vertex, String relationLabel)
        {
            List<Vertex> srcVertices = new List<Vertex>();
            List<String> edgeKeys = vertex.GetEdgeIndices();
            foreach (String edgeKey in edgeKeys)
            {
                if (!edgeKey.Split("-")[0].Equals(vertex.GetIndex().ToString()))
                {
                    List<Edge> edges = this.edges[edgeKey];

                    foreach (Edge edge in edges)
                    {
                        if (edge.GetRelationLabel().Equals(relationLabel))
                        {
                            srcVertices.Add(this.vertices[Convert.ToInt32(edgeKey.Split("-")[0])-1]);
                            break;
                        }
                    }

                }

            }
            return srcVertices;
        }
        public List<Vertex> GetDistantOutGoingVerticesByRelationLabel(Vertex vertex, String relationLabel)
        {
            List<Vertex> destVertices = new List<Vertex>();
            List<String> edgeKeys = vertex.GetEdgeIndices();
            foreach (String edgeKey in edgeKeys)
            {
                if (!edgeKey.Split("-")[1].Equals(vertex.GetIndex().ToString())) {
                    List<Edge> edges = this.edges[edgeKey];
                    
                    foreach(Edge edge in edges)
                    {
                        if (edge.GetRelationLabel().Equals(relationLabel)) {
                            destVertices.Add(this.vertices[Convert.ToInt32(edgeKey.Split("-")[1])-1]);
                            break;
                        }
                    }
                    
                }
                    
            }
            return destVertices;
        }
        public List<Vertex> GetDistantAllVerticesByRelationLabel(Vertex vertex, String relationLabel)
        {
            List<Vertex> vertices = new List<Vertex>();
            List<String> edgeKeys = vertex.GetEdgeIndices();
            foreach (String edgeKey in edgeKeys)
            {
                if (!edgeKey.Split("-")[1].Equals(vertex.GetIndex().ToString()))
                {
                    List<Edge> edges = this.edges[edgeKey];

                    foreach (Edge edge in edges)
                    {
                        if (edge.GetRelationLabel().Equals(relationLabel))
                        {
                            vertices.Add(this.vertices[Convert.ToInt32(edgeKey.Split("-")[1])-1]);
                            break;
                        }
                    }

                }
                else {
                    List<Edge> edges = this.edges[edgeKey];

                    foreach (Edge edge in edges)
                    {
                        if (edge.GetRelationLabel().Equals(relationLabel))
                        {
                            vertices.Add(this.vertices[Convert.ToInt32(edgeKey.Split("-")[0])-1]);
                            break;
                        }
                    }
                }

            }
            return vertices;
        }
        public List<Vertex> GetDistantIncomingVerticesByRelationReason(Vertex vertex, String relationLabel, String reasonLabel)
        {
            List<Vertex> srcVertices = new List<Vertex>();
            List<String> edgeKeys = vertex.GetEdgeIndices();
            foreach (String edgeKey in edgeKeys)
            {
                if (!edgeKey.Split("-")[0].Equals(vertex.GetIndex().ToString()))
                {
                    List<Edge> edges = this.edges[edgeKey];

                    foreach (Edge edge in edges)
                    {
                        if (edge.GetRelationLabel().Equals(relationLabel) && edge.GetReasonLabel().Equals(reasonLabel))
                        {
                            srcVertices.Add(this.vertices[Convert.ToInt32(edgeKey.Split("-")[0])-1]);
                            break;
                        }
                    }

                }

            }
            return srcVertices;
        }
        public List<Vertex> GetDistantOutGoingVerticesByRelationReason(Vertex vertex, String relationLabel, String reasonLabel)
        {
            List<Vertex> destVertices = new List<Vertex>();
            List<String> edgeKeys = vertex.GetEdgeIndices();
            foreach (String edgeKey in edgeKeys)
            {
                if (!edgeKey.Split("-")[1].Equals(vertex.GetIndex().ToString()))
                {
                    List<Edge> edges = this.edges[edgeKey];

                    foreach (Edge edge in edges)
                    {
                        if (edge.GetRelationLabel().Equals(relationLabel) && edge.GetReasonLabel().Equals(reasonLabel))
                        {
                            destVertices.Add(this.vertices[Convert.ToInt32(edgeKey.Split("-")[1])-1]);
                            break;
                        }
                    }

                }

            }
            return destVertices;
        }
        public List<Vertex> GetDistantAllVerticesByRelationReason(Vertex vertex, String relationLabel, String reasonLabel)
        {
            List<Vertex> vertices = new List<Vertex>();
            List<String> edgeKeys = vertex.GetEdgeIndices();
            foreach (String edgeKey in edgeKeys)
            {
                if (!edgeKey.Split("-")[1].Equals(vertex.GetIndex().ToString()))
                {
                    List<Edge> edges = this.edges[edgeKey];

                    foreach (Edge edge in edges)
                    {
                        if (edge.GetRelationLabel().Equals(relationLabel) && edge.GetReasonLabel().Equals(reasonLabel))
                        {
                            vertices.Add(this.vertices[Convert.ToInt32(edgeKey.Split("-")[1])-1]);
                            break;
                        }
                    }

                }
                else
                {
                    List<Edge> edges = this.edges[edgeKey];

                    foreach (Edge edge in edges)
                    {
                        if (edge.GetRelationLabel().Equals(relationLabel) && edge.GetReasonLabel().Equals(reasonLabel))
                        {
                            vertices.Add(this.vertices[Convert.ToInt32(edgeKey.Split("-")[0])-1]);
                            break;
                        }
                    }
                }

            }
            return vertices;
        }
        public List<String> GetOutGoingEdgesIndexes(Vertex vertex)
        {
            List<String> edges = new List<String>(); 
            foreach (String edge in vertex.GetEdgeIndices()) {
                if (edge.Split("-")[0].Equals(vertex.GetIndex().ToString()))
                {
                    edges.Add(edge);
                }
            }
            return edges;
        }
        public List<String> GetIncomingEdgesIndexes(Vertex vertex)
        {
            List<String> edges = new List<String>();
            foreach (String edge in vertex.GetEdgeIndices())
            {
                if (edge.Split("-")[1].Equals(vertex.GetIndex().ToString()))
                {
                    edges.Add(edge);
                }
            }
            return edges;
        }
        public List<String> GetAllEdgesIndexes(Vertex vertex)
        {
            return vertex.GetEdgeIndices();
        }
        public List<Edge> GetOutGoingEdges(Vertex vertex)
        {
            List<Edge> edges = new List<Edge>();
            foreach (String edgeKey in vertex.GetEdgeIndices())
            {
                if (edgeKey.Split("-")[0].Equals(vertex.GetIndex().ToString()))
                {
                    edges.AddRange(this.edges[edgeKey]);
                }
            }
            return edges;
        }
        public List<Edge> GetInComingEdges(Vertex vertex)
        {
            List<Edge> edges = new List<Edge>();
            foreach (String edgeKey in vertex.GetEdgeIndices())
            {
                if (edgeKey.Split("-")[1].Equals(vertex.GetIndex().ToString()))
                {
                    edges.AddRange(this.edges[edgeKey]);
                }
            }
            return edges;
        }
        public List<Edge> GetAllEdges(Vertex vertex)
        {
            List<Edge> edges = new List<Edge>();
            foreach (String edgeKey in vertex.GetEdgeIndices())
            {
                edges.AddRange(this.edges[edgeKey]);               
            }
            return edges;
        }
        public Dictionary<String, List<Edge>> GetOutGoingEdgesWithIndexes(Vertex vertex)
        {
            Dictionary<String, List<Edge>> edges = new Dictionary<String, List<Edge>>();
            foreach (String edgeKey in vertex.GetEdgeIndices())
            {
                if (edgeKey.Split("-")[0].Equals(vertex.GetIndex().ToString()))
                {
                    edges.Add(edgeKey, this.edges[edgeKey]);
                }
            }
            return edges;
        }
        public Dictionary<String, List<Edge>> GetInComingEdgesWithIndexes(Vertex vertex)
        {

            Dictionary<String, List<Edge>> edges = new Dictionary<String, List<Edge>>();
            foreach (String edgeKey in vertex.GetEdgeIndices())
            {
                if (edgeKey.Split("-")[1].Equals(vertex.GetIndex().ToString()))
                {
                    edges.Add(edgeKey, this.edges[edgeKey]);
                }
            }
            return edges;
        }
        public Dictionary<String, List<Edge>> GetAllEdgesWithIndexes(Vertex vertex)
        {
            Dictionary<String, List<Edge>> edges = new Dictionary<String, List<Edge>>();
            foreach (String edgeKey in vertex.GetEdgeIndices())
            {
                edges.Add(edgeKey, this.edges[edgeKey]);
            }
            return edges;
        }
        public void PrintVertices()
        {
            foreach (Vertex vertex in this.vertices)
            {
                if (vertex != null) {
                    Console.Write(vertex.GetLabel() + " -> ");
                }
                
            }
            Console.WriteLine();
        }
        public void PrintVerticesAndIndices()
        {
            foreach (Vertex vertex in this.vertices)
            {
                if (vertex != null)
                {
                    Console.WriteLine("===");
                    Console.Write("v: " + vertex.GetLabel()+ ", index:"+vertex.GetIndex() + " -> ");
                    foreach(String s in vertex.GetEdgeIndices())
                    {
                        Console.Write("edges: " + s + "->");
                    }
                    Console.WriteLine("===");
                }

            }
            Console.WriteLine();
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            /*            Vertex v = new Vertex();
                        Vertex v1 = new Vertex();
                        v.SetLabel("1");
                        v.SetIndex(100);
                        v1.SetIndex(2);
                        Console.WriteLine("vertex label: {0}, HASHCODE: {1}, {2}", v.GetLabel(), v.GetHashCode(), v1.GetHashCode());
                        Edge edge1 = new Edge("ABC",null);
                        Edge edge2 = new Edge();
                        edge2.SetRelationLabel("ABC");

                        Console.WriteLine(edge1.Equals(edge2) + "-"+ edge1.GetHashCode()+"-"+edge2.GetHashCode());
                        Graph graph = new Graph();
                        graph.AddVertex("v1");
                        graph.AddVertex("v2");
                        graph.AddVertex("v3");
                        graph.SetRelation(graph.GetVertex(1).GetIndex(), graph.GetVertex(2).GetIndex(), "loves");
                        graph.SetRelation(graph.GetVertex(2).GetIndex(), graph.GetVertex(3).GetIndex(), "loves");
                        Console.WriteLine(graph.GetVertex(3).GetEdgeIndices().Count+" lets see"+ graph.GetVertex(1).GetEdgeIndices()[0]);
                        Graph graph2 = new Graph();
                        graph2.AddVertex("B1");
                        graph2.AddVertex("B2");
                        graph2.AddVertex("B3");
                        graph2.AddVertex("B4");
                        graph2.AddVertex("B5");
                        graph2.AddVertex("B6");
                        graph2.AddVertex("B7");
                        graph2.SetRelation(graph2.GetVertex(1).GetIndex(), graph2.GetVertex(2).GetIndex(), "loves", "some");
                        graph2.SetRelation(graph2.GetVertex(2).GetIndex(), graph2.GetVertex(1).GetIndex(), "loves", "some");
                        graph2.SetRelation(graph2.GetVertex(3).GetIndex(), graph2.GetVertex(1).GetIndex(), "loves", "some");
                        graph2.SetRelation(graph2.GetVertex(4).GetIndex(), graph2.GetVertex(1).GetIndex(), "loves", "some");
                        graph2.SetRelation(graph2.GetVertex(1).GetIndex(), graph2.GetVertex(4).GetIndex(), "loves", "some");
                        graph2.SetRelation(graph2.GetVertex(3).GetIndex(), graph2.GetVertex(2).GetIndex(), "loves", "some");
                        graph2.SetRelation(graph2.GetVertex(2).GetIndex(), graph2.GetVertex(3).GetIndex(), "loves", "some");
                        graph2.SetRelation(graph2.GetVertex(4).GetIndex(), graph2.GetVertex(2).GetIndex(), "loves", "some");




                        graph2.PrintVerticesAndIndices();


                        graph2.DeleteVertex(2);
                        graph2.DeleteVertex(3);

                        graph2.PrintVerticesAndIndices();
                        graph2.UpdateGraph();
                        graph2.PrintVerticesAndIndices();
                        graph2.SetRelation(graph2.GetVertex(3), graph2.GetVertex(2), "loves", "some");

                        graph2.SetRelation(graph2.GetVertex(2), graph2.GetVertex(4), "loves");
                        // graph2.SetRelation(graph2.GetVertex(4).GetIndex(), graph2.GetVertex(6).GetIndex(), "loves", "some");
                        Console.WriteLine("Main Graph: ");
                        graph2.PrintVerticesAndIndices();
                        //graph2.LoveRule();
                        //graph2.PrintLoveRule(); 
                        Rule loveRule = new LoveRule();

                        List<Graph> loveRuleGraph = loveRule.ImplementRule(graph2);
                        Console.WriteLine("Sub Graphs: ");
                        foreach (Graph graphx in loveRuleGraph)
                        {
                            graphx.PrintVertices();
                        }*/

                        Graph testGraph = new Graph();
                        for(int i=0; i < 100; i++)
                        {
                            testGraph.AddVertex("V_" + i);
                        }
                        for (int i = 0; i < 100; i++)
                        {
                            int index1 = new Random().Next(1, 99);
                            int index2 = new Random().Next(1, 99);
                            testGraph.SetRelation(index1, index2, "loves");
                        }

                        testGraph.PrintVerticesAndIndices();
            List<Edge> edges = testGraph.GetOutGoingEdges(testGraph.GetVertex(95));
            List<String> edgeKey = testGraph.GetOutGoingEdgesIndexes(testGraph.GetVertex(95));
            foreach (Edge e in edges)
            {
                Console.WriteLine("Relation: "+e.GetRelationLabel());
            }
            foreach (String e in edgeKey)
            {
                Console.WriteLine("Relation: " + e);
            }


            /* Vertex vertex = new Vertex("Hello");
             Console.WriteLine("Vertex Label: "+vertex.GetLabel());
             vertex.SetAttribute("power", 99999);
             vertex.SetAttribute("Rank", "Very High");
             *//*Generic Type return*//*
             if (vertex.GetAttribute<int>("power") == 99999)
             {
                 Console.WriteLine(99999);
                 if(vertex.GetAttribute<String>("Rank").Equals("Very High"))
                     Console.WriteLine(vertex.GetAttribute<String>("Rank"));
             }

             *//*Object Type return*//*
             if ((int)vertex.GetAttribute("power") == 99999)
             {
                 Console.WriteLine(99999);
                 if (vertex.GetAttribute("Rank").Equals("Very High"))
                     Console.WriteLine(vertex.GetAttribute("Rank"));
             }*/


        }
    }
}

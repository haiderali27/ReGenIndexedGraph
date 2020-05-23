# ReGenIndexedGraph

## Introduction

This project is the implementation of Graph Rewriting. Graph consists of vertices and edges. 
In this project vertices are stored in a List and Edges are stored in Dictionary of String as key and List of Edges as value.  
There are 4 class that are being used in this project.
- class Vertex
- class Edge 
- class Graph
- class Rule
- class LoveRule

```
Vertex, Edge and Graph classes are the representation of Graph. 
Vertex contains the attributes such as id, name, list of edges etc.
Edge contains the relation and reason of the relation.
Graph contains the list of Vertices and Dictionary of String as key and List of Edge as value for edges.
To be more clear, "private List<Vertex> vertices" is a data member we use to save vertices in graph and "private Dictionary<String, List<Edge>> edges" is a data member we use to save edges in graph. 
Edges Explaination:
Key of edge is String and it is created when a relation is formed, it takes src and dest ids and merge them as "srcid-destid" and save the list of edge to it. 
srcid and destid are both integer. 
And id is unique for each vertex so, we can save as many relations between two vertices as we want in one key which is "srcid-destid". 
I hope that makes sense. 

Rule and LoveRule are classes where Graph Rewriting is implemented. 
Rule is an abstract class where there is a method defined public abstract List<Graph> ImplementRule(Graph graph) which is supposed to return List of sub graphs from the main graph.
LoveRule inherets Rule class and implements LoveRule which is to find the nodes that follow love Rule and create a graph of those nodes.
And then Save all those new graph is list and return them. 


LoveRule:
Node A -Loves-> Node B -Loves-> Node C and Node B does not have outgoing love Relation to Node A

I hope that makes sense again :p 

There is also a class VertexAttributes but don't worry about it, it is not used anywhere. It was used in Vertex class but was later replaced by private Dictionary<String, Object> vertexAttributes data member.

```

### Repo Structure
This repo is the project folder of Visual Studio project which is "ReGenIndexedGraph".

### Requirements

- Microsoft Visual Studio 2015 or above.

### TODO
```
git clone git@github.com:haiderali27/ReGenIndexedGraph.git
Open the project folder which is  "ReGenIndexedGraph" with Visual Studio and Run it with Visual Studio Run button. 
```
### Other Graph Implementation
You can check my other Graph Implementation in the link given below.
The only difference between this project and the other is how graph stores the edges.

[ReGenGraph](https://github.com/haiderali27/ReGenGraph)

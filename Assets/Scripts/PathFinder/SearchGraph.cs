using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Graph search related logic
/// </summary>
public class SearchGraph {
	public Graph graph;
	public List<Node> reachable;
	public List<Node> explored;
	public List<Node> path;
	public Node goalNode;
	public bool finished;

	public SearchGraph (Graph _graph) {
		this.graph = _graph;
	}

	// Start search with start and end Node
	public void Start (Node start, Node goal) {
		reachable = new List<Node> ();
		reachable.Add (start);

		goalNode = goal;

		explored = new List<Node> ();
		path = new List<Node> ();

		for (int i = 0; i < graph.nodes.Length; ++i) {
			graph.nodes [i].Clear ();
		}
	}

	public void Step () {
		if (path.Count > 0)
			return;

		if (reachable.Count == 0) {
			finished = true;
			return;
		}

		Node node = ChooseNode ();
		if (node == goalNode) {
			while (node != null) {
				path.Insert (0, node);
				node = node.previous;
			}

			finished = true;
			return;
		}

		reachable.Remove (node);
		explored.Add (node);

		for (int i = 0; i < node.adjacent.Count; ++i) {
			AddAdjacent (node, node.adjacent [i]);
		}
	}

	private void AddAdjacent (Node Node, Node adjacent) {
		if (FindNode (adjacent, explored) || FindNode (adjacent, reachable))
			return;

		adjacent.previous = Node;
		reachable.Add (adjacent);
	}

	private int GetNodeIndex (Node node, List<Node> node_list) {
		for (int i = 0; i < node_list.Count; ++i) {
			if (node == node_list [i])
				return i;
		}
		return -1;
	}

	private bool FindNode (Node node, List<Node> list) {
		return GetNodeIndex (node, list) >= 0;
	}

	private Node ChooseNode () {
		return reachable [Random.Range (0, reachable.Count)];
	}
}

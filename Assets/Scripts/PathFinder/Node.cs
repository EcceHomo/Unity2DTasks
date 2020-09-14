using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Track list of nodes
/// </summary>
public class Node {
	// Adjecent Node List (up, down, left, right)
	public List<Node> adjacent = new List<Node>();
	// Cached previous node
	public Node previous = null;
	// Label used as index for GetChild
	public string label = "";

	// Clear previous node reference
	public void Clear () {
		previous = null;
	}
}

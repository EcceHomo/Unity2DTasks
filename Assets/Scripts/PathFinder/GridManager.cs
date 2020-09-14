using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

/// <summary>
/// Creating path and instantiating grid related elements
/// </summary>
public class GridManager : MonoBehaviour {
	[Tooltip("Tile element used for building grid.")]
	[SerializeField]
	private Transform gridTile;

	[Tooltip("Mouse prefab that will be moving to goal.")]
	[SerializeField]
	private Transform mousePrefab;

	[Tooltip("Cheese prefab that will be used as a goal.")]
	[SerializeField]
	private Transform cheesePrefab;

	[Tooltip("Audio source will play from start on loop.")]
	[SerializeField]
	private AudioSource themeMusicAudio;

	[Tooltip("Audio source will play once when goal reach.")]
	[SerializeField]
	private AudioSource tvStaticAudio;

	// Grid row and colum number
	private int gridRow = 100;
	private int gridColumn = 130;

	// Tile width and height
	private float tileWidth;
	private float tileHeight;

	// Instantiated mouse / cheese
	private GameObject mouseGo;
	private GameObject cheeseGo;

	// Cached calculated move positions
	private List<Vector3> movePositions = new List<Vector3>();

	void Start () {
		tileWidth = gridTile.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
		tileHeight = gridTile.GetComponent<SpriteRenderer>().sprite.bounds.size.y;

		SpawnGrid(gridRow, gridColumn, gridTile);

		int[,] map = new int [gridRow, gridColumn];

		Graph graph = new Graph(CreateObstacles(map));

		SearchGraph search  = new SearchGraph(graph);

		search.Start (graph.nodes[790], graph.nodes[11039]);

		while (!search.finished){
			search.Step();
		}

		// when search finished, print out the results
		Debug.LogFormat("Search done. Path Length:{0}", search.path.Count);
		ResetMapGroup(graph);

		foreach(Node node in search.path) {
			GameObject nodeObj = GetNodeByLabel (node.label);
			nodeObj.GetComponent<SpriteRenderer>().color = Color.red;
			movePositions.Add(nodeObj.transform.position);
		}

		mouseGo = Instantiate (mousePrefab, movePositions [0], Quaternion.identity).gameObject;
		MouseController mouseManager = mouseGo.GetComponent<MouseController> ();
		cheeseGo = Instantiate(cheesePrefab, movePositions[movePositions.Count - 1], Quaternion.identity).gameObject;
		StartCoroutine(mouseManager.MoveObject(movePositions));

		themeMusicAudio.Play ();

		// Watch for OnGoalReach event
		EventManager.StartListening("OnGoalReach", OnGoalReach);
	}

	void OnDisable() {
		EventManager.StopListening("OnGoalReach", OnGoalReach);
	}

	private void OnGoalReach(GameObject obj, string param) {
		EventManager.StopListening("OnGoalReach", OnGoalReach);
		themeMusicAudio.Stop ();
		Destroy (mouseGo);
		Destroy (cheeseGo);
		CreateStatic();
	}

	private void CreateStatic() {
		tvStaticAudio.Play ();
		for (int i = 0; i < transform.childCount; i++) {
			GameObject currentItem = transform.GetChild (i).gameObject;
			currentItem.GetComponent<SpriteRenderer> ().color = UnityEngine.Random.value < 0.5f ? Color.grey : Color.black;
		}
	}

	private void SpawnGrid (int gridRow, int gridColumn, Transform gridTile) {

		int gridPosition = -1;

		for (int row = 0; row < gridRow; row++) {
			for (int col = 0; col < gridColumn; col++) {
				float posX = col * tileWidth;
				float posY = row * + tileHeight;

				Transform gridTilePrefab = Instantiate(gridTile);
				gridTilePrefab.SetParent (transform);
				gridTilePrefab.localPosition = new Vector3(posX, posY, 0);
				gridPosition++;
				gridTilePrefab.name = "Position:" + gridPosition + " Row:" + row + " Column:" + col;
			}
		}
	}

	private int[,] CreateObstacles(int[,] map) {
		for (int x = 0; x < map.GetLength(0); x++) {
			for (int y = 0; y < map.GetLength (1); y++) {
				if (x == 30 && y > 20)
					map [x, y] = 1;
				else if (x == 50 && y < 50)
					map [x, y] = 1;
				else if (x == 70 && y > 60 && y < 120)
					map [x, y] = 1;
			}
		}

		return map;
	}

	private GameObject GetNodeByLabel(string label) {
		int id = Int32.Parse(label);
		GameObject tile = transform.GetChild(id).gameObject;
		return tile;
	}

	private void ResetMapGroup(Graph _graph) {
		foreach(Node node in _graph.nodes){
			if (node.adjacent.Count == 0)
				GetNodeByLabel (node.label).GetComponent<SpriteRenderer> ().color = Color.black;
		}
	}
}

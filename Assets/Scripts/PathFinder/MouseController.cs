using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for moving mouse across grid nodes
/// </summary>
public class MouseController : MonoBehaviour {
	[Tooltip("Mouse move speed modifier 0.5 is default.")]
	[SerializeField]
	private float moveSpeed = 0.5f;

	public IEnumerator MoveObject(List<Vector3> movePositions) {
		for (int i = 0; i < movePositions.Count; i++) {
			yield return StartCoroutine(Moving(movePositions[i]));
			if (transform.position == movePositions [movePositions.Count - 1]) {
				Debug.LogFormat ("GameObject:{0} has reached the goal. Playing TV static noise.", name);
				EventManager.TriggerEvent("OnGoalReach", null, null);
			}
		}
	}

	private IEnumerator Moving(Vector3 currentPosition) {
		while (transform.position != currentPosition) {
			transform.position = Vector3.MoveTowards(transform.position, currentPosition, moveSpeed * Time.deltaTime);
			yield return null;
		}
	}
}

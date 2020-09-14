using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Trigger box related events
/// </summary>
public class BoxManager : MonoBehaviour {
	void OnDestroy() {
		Debug.LogFormat ("Box destroyed with tag <color={0}>{1}</color>.", tag == "OrangeBox" ? "orange" : "blue", tag);
		EventManager.TriggerEvent("OnBoxDestroy", null, tag);
	}

	void OnTriggerEnter2D (Collider2D col) {
		switch (col.tag) {
			case "Character":
				EventManager.TriggerEvent("OnBoxTriggerEnter", gameObject, tag);
				break;
		}
	}
}

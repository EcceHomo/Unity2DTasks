using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// My event type.
/// </summary>
[System.Serializable]
public class MyEvent : UnityEvent<GameObject, string> {
	
}

/// <summary>
/// Message system.
/// </summary>
public class EventManager : MonoBehaviour {
	// Singleton
	public static EventManager instance;

    // Events list
	private Dictionary<string, MyEvent> eventDictionary = new Dictionary<string, MyEvent>();

	void OnDestroy() {
		instance = null;
	}

    /// Start listening specified event.
    public static void StartListening(string eventName, UnityAction<GameObject, string> listener) {
		if (instance == null) {
			instance = FindObjectOfType(typeof(EventManager)) as EventManager;
			if (instance == null) {
				Debug.Log("Have no event manager on scene.");
				return;
			}
		}
        MyEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
            thisEvent.AddListener(listener);
        }
        else {
            thisEvent = new MyEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }
		
    /// Stop listening specified event.
    public static void StopListening(string eventName, UnityAction<GameObject, string> listener) {
		if (instance == null)
			return;
        MyEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            thisEvent.RemoveListener(listener);
    }

    /// Trigger specified event.
    public static void TriggerEvent(string eventName, GameObject obj, string param) {
		if (instance == null)
			return;
        MyEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            thisEvent.Invoke(obj, param);
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Spawning box and updating UI score
/// </summary>
public class MapManager : MonoBehaviour {
	[Tooltip("Box prefabs that will be randomly instantiated.")]
	[SerializeField]
	private List<GameObject> boxPrefabs;

	[Tooltip("Score counter text for destroyed orange boxes.")]
	[SerializeField]
	private Text orangeScoreTxt;

	[Tooltip("Score counter text for destroyed blue boxes.")]
	[SerializeField]
	private Text blueScoreTxt;

	[Tooltip("Audio source for box destroyed.")]
	[SerializeField]
	private AudioSource boxDestroyedAudio;

	[Tooltip("Audio source for box picked.")]
	[SerializeField]
	private AudioSource boxPickedAudio;

	[Tooltip("Audio source for theme music.")]
	[SerializeField]
	private AudioSource themeMusicAudio;

	void Start() {
		InvokeRepeating("SpawnRandomBox", 1.0f, 4f);
		// Watch for OnBoxDestroy event
		EventManager.StartListening("OnBoxDestroy", OnBoxDestroy);
		EventManager.StartListening("OnBoxPicked", OnBoxPicked);
		themeMusicAudio.Play ();
	}

	void SpawnRandomBox() {
		Instantiate(boxPrefabs[Random.Range(0, boxPrefabs.Count)], new Vector3(Random.Range(-3.5f, 3.5f), transform.position.y, 0), Quaternion.identity);
	}

	void OnDisable() {
		EventManager.StopListening("OnBoxDestroy", OnBoxDestroy);
		EventManager.StopListening("OnBoxPicked", OnBoxPicked);
	}

	private void OnBoxDestroy(GameObject obj, string param) {
		if (!orangeScoreTxt || !blueScoreTxt || !boxDestroyedAudio)
			return;

		boxDestroyedAudio.Play ();

		if (param == "OrangeBox")
			orangeScoreTxt.text  = (int.Parse (orangeScoreTxt.text)  + 1).ToString();
		else
			blueScoreTxt.text = (int.Parse (blueScoreTxt.text) + 1).ToString();
	}

	private void OnBoxPicked(GameObject obj, string param) {
		boxPickedAudio.Play ();
	}
}

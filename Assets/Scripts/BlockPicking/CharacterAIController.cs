using System.Collections;
using UnityEngine;

/// <summary>
/// Character movement and related animation triggers 
/// </summary>
public class CharacterAIController : MonoBehaviour {
	[Tooltip("Move speed modifier, default is 3.")]
	[SerializeField]
	private float moveSpeed = 3f;

	[Tooltip("Character arm required for box transform")]
	[SerializeField]
	private Transform characterArm;

	// Is character facing right
	private bool isFacingRight;
	// Cached move speed after disable/enable
	private float cachedMoveSpeed;
	// direction x, 1 is right, -1 is left
	private float directionX;
	// Animator reference for playing animations
	private Animator anim;
	// Box that character is holding
	private GameObject currentBox;
	// Rigidbody reference used for velocity
	private Rigidbody2D rigidBody;
	// Cached local scale for rotation
	private Vector3 localScale;

	void Start () {
		cachedMoveSpeed = moveSpeed;
		directionX = 1f;
		anim = GetComponent<Animator> ();
		rigidBody = GetComponent<Rigidbody2D> ();
		localScale = transform.localScale;

		// Watch for OnBoxDestroy event
		EventManager.StartListening("OnBoxTriggerEnter", OnBoxTriggerEnter);
	}

	void FixedUpdate() {
		rigidBody.velocity = new Vector2 (directionX * moveSpeed, rigidBody.velocity.y);
		CheckToTurn();
	}

	// Wait for end of frame, get length and enable character after length
	IEnumerator TriggerAnim(string triggerAnim, string boxTag, bool inverse) {
		moveSpeed = 0f;
		anim.SetTrigger(triggerAnim);
		yield return new WaitForEndOfFrame ();
		float clipLength = GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).length;
		StartCoroutine(EnableCharacterAfter(clipLength, boxTag, inverse));
	}

	// Enable chacater movement after x sec
	IEnumerator EnableCharacterAfter(float EnableCharacterAfter, string boxTag, bool inverse) {
		yield return new WaitForSeconds (EnableCharacterAfter);
		moveSpeed = cachedMoveSpeed;
		if (boxTag == "OrangeBox")
			directionX = !inverse ? 1f : -1f;
		else if (boxTag == "BlueBox")
			directionX = !inverse ? -1f : 1f;
	}

	void OnDisable() {
		EventManager.StopListening("OnBoxTriggerEnter", OnBoxTriggerEnter);
	}

	// Box is triggered set it inside character arm
	public void OnBoxTriggerEnter(GameObject obj, string param) {
		if (currentBox)
			return;

		StartCoroutine (TriggerAnim("pickUp", param, true));

		EventManager.TriggerEvent("OnBoxPicked", null, null);

		currentBox = obj;
		currentBox.GetComponent<SpriteRenderer> ().sortingOrder = characterArm.GetComponent<SpriteRenderer> ().sortingOrder + 1;
		currentBox.transform.position = characterArm.position;
		currentBox.transform.SetParent(characterArm);
		currentBox.GetComponent<Rigidbody2D> ().simulated = false;

		Debug.LogFormat ("Box picked with tag <color={0}>{1}</color>.", param == "OrangeBox" ? "orange" : "blue", param);
	}

	void CheckToTurn() {
		isFacingRight = directionX > 0 ? true : false;

		if (((isFacingRight) && (localScale.x < 0)) || ((!isFacingRight) && (localScale.x > 0)))
			localScale.x *= -1;

		transform.localScale = localScale;
	}

	void OnTriggerEnter2D (Collider2D col) {
		switch (col.tag) {
			case "OrangeDumpster":
			case "BlueDumpster":
				DumpsterTrigger (col.tag);
				break;
		}
	}

	void DumpsterTrigger(string dumpsterTag) {
		string boxTag = dumpsterTag == "OrangeDumpster" ? "OrangeBox" : "BlueBox";

		if (currentBox && currentBox.name.StartsWith (boxTag))
			StartCoroutine (TriggerAnim ("throwUp", boxTag, false));
		else
			directionX = dumpsterTag == "OrangeDumpster" ? 1f : -1f;
		
		Destroy (currentBox);
	}
}

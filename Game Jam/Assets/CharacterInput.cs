using UnityEngine;
using System.Collections;

public class CharacterInput : MonoBehaviour {
	[SerializeField]Rigidbody2D _rb;

	[SerializeField]private float _speed;

	// Use this for initialization
	void Start () {
		StartCoroutine (PlayerInput());
	}

	private IEnumerator PlayerInput(){
		while (true) {
			if (Input.GetAxis ("Vertical") < -0.3f) {
				_rb.AddRelativeForce (Vector2.down * _speed);
			} else if (Input.GetAxis ("Vertical") > .3f) {
				_rb.AddRelativeForce (Vector2.up * _speed);

			}


			if (Input.GetAxis ("Horizontal") < -0.3f) {
				_rb.AddRelativeForce (Vector2.left * _speed);
			} else if (Input.GetAxis ("Horizontal") > .3f) {
				_rb.AddRelativeForce (Vector2.right * _speed);

			}
			yield return new WaitForEndOfFrame ();
		}
	}
}

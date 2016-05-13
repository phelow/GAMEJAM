using UnityEngine;
using System.Collections;

public class CharacterInput : MonoBehaviour {
	[SerializeField]Rigidbody2D _rb;

	[SerializeField]private float _speed;
	[SerializeField]private float _projectilePower = 10.0f;

	[SerializeField]private GameObject _projectile;

	// Use this for initialization
	void Start () {
		StartCoroutine (PlayerInput());
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			GameObject projectile = GameObject.Instantiate (_projectile);
			projectile.transform.position = transform.position;
			Vector3 sp = Camera.main.WorldToScreenPoint (transform.position);
			Vector3 dir = (Input.mousePosition - sp).normalized;
			projectile.GetComponent<Rigidbody2D>().AddForce (dir * _projectilePower);
		}
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

using UnityEngine;
using System.Collections;

public class Melee : Attack {
	[SerializeField]Rigidbody2D _rb;

	[SerializeField]private float _initialWait = 1.0f;
	[SerializeField]private float _waitToDestroy = 1.0f;

	// Use this for initialization
	void Start () {
		StartCoroutine (MeleeRoutine ());
	}

	private IEnumerator MeleeRoutine(){
		yield return new WaitForSeconds(_initialWait);
		_rb.velocity *= -1;

		yield return new WaitForSeconds(_waitToDestroy);
		Destroy (this.gameObject);
	}

	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag == "Enemy") {
			coll.gameObject.GetComponent<Enemy> ().Damage (m_damage);
		}
	}
}

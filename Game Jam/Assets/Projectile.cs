using UnityEngine;
using System.Collections;

public class Projectile : Attack {
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag == "Enemy") {
			coll.gameObject.GetComponent<Enemy> ().Damage (m_damage);
		}
		Destroy (this.gameObject);
	}
}

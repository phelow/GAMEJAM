using UnityEngine;
using System.Collections;

public class Projectile : Attack {
	private static float ms_damageMultiplier = 1.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void SetDamageMultiplier(float damMultiplier){
		ms_damageMultiplier = damMultiplier;
	}

	public static void ResetDamageMultiplier(){
		ms_damageMultiplier = 1.0f;
	}


	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag == "Enemy") {
			coll.gameObject.GetComponent<Enemy> ().Damage (m_damage * ms_damageMultiplier);
		}
		Destroy (this.gameObject);
	}
}

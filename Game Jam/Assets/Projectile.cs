using UnityEngine;
using System.Collections;

public class Projectile : Attack {
	private static float ms_damageMultiplier = 1.0f;
	[SerializeField]private ParticleSystem m_particleSystem;

	// Use this for initialization
	void Start () {
		//choose random color for the particle system
		m_particleSystem.startColor = new Color(Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f));
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
			Destroy (this.gameObject);
		}
		StartCoroutine (DestroyLater ());
	}

	private IEnumerator DestroyLater(){
		yield return new WaitForSeconds (3.0f);
		Destroy (this.gameObject);
	}
}

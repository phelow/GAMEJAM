using UnityEngine;
using System.Collections;

public class SuicideBomberEnemy : Enemy {

	[SerializeField]private float m_explosionRange = 10.0f;
	[SerializeField]private float m_explosionTrigger = 10.0f;
	[SerializeField]private float m_explosionBuildupTime = 3.0f;

	[SerializeField]private float m_minDamage = 1.0f;
	[SerializeField]private float m_maxDamage = 10.0f;



	private bool m_exploding = false;
	
	// Update is called once per frame
	void Update () {
		if (m_exploding == false) {
			MoveTowardsPlayer (Time.deltaTime);
			if (PlayerInExplodeRange ()) {
				Explode ();
			}
		}
	}


	protected bool PlayerInExplodeRange(){
		return (Vector3.Distance (s_player.gameObject.transform.position, transform.position) < m_explosionTrigger);
	}

	void Explode(){
		m_rb.velocity = Vector2.zero;
		m_exploding = true;
		StartCoroutine (ExplodeRoutine ());
	}

	private IEnumerator ExplodeRoutine(){
		yield return new WaitForSeconds (m_explosionBuildupTime);
		float dist = Vector3.Distance (s_player.gameObject.transform.position, transform.position);
		if (dist < m_explosionRange) {
			Health.TakeDamage (Mathf.Lerp (m_minDamage, m_maxDamage, ((m_explosionRange - dist) / m_explosionRange)));
		}
		Destroy (this.gameObject);
	}

}

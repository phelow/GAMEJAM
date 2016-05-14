using UnityEngine;
using System.Collections;

public class ShooterEnemy : Enemy {
	[SerializeField]private float m_startShootingRange = 5.0f; //how close we need to be to start shooting
	[SerializeField]private float m_minBetweenShotTime = 1.0f;
	[SerializeField]private float m_maxBetweenShotTime = 5.0f;

	[SerializeField]private float m_projectileSpeed = 5.0f;
	[SerializeField]private GameObject m_enemyProjectile;




	private float m_timeToNextShot = 0.0f;
	// Update is called once per frame
	void Update () {
		//while we are too far away from the player move closer to the player
		if (!PlayerInShotRange ()) {
			MoveTowardsPlayer (Time.deltaTime);
		}
		else{
		//If we get close enough start shooting.
			ShootAtPlayer();
			m_rb.velocity = Vector2.zero;
		
		}
	}

	protected bool PlayerInShotRange(){
		return (Vector3.Distance (s_player.gameObject.transform.position, transform.position) < m_startShootingRange);
	}

	protected void ShootAtPlayer(){
		m_timeToNextShot -= Time.deltaTime;
		if (m_timeToNextShot <= 0.0f) {
			Debug.Log ("Shooting at player");
			GameObject projectile = GameObject.Instantiate (m_enemyProjectile);
			projectile.transform.position = transform.position;

			projectile.GetComponent<Rigidbody2D> ().AddForce ((s_player.transform.position - transform.position) * m_projectileSpeed);
			m_timeToNextShot = Random.Range (m_minBetweenShotTime, m_maxBetweenShotTime);
		}
	}
}

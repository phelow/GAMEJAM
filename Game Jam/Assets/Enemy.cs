using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	protected static GameObject s_player;

	[SerializeField]protected Rigidbody2D m_rb;
	[SerializeField]protected float m_speed;

	[SerializeField]private float m_maxHealth = 100.0f;
	[SerializeField]private float m_curHealth;


	// Use this for initialization
	void Start () {
		s_player = GameObject.FindGameObjectWithTag ("Player");
		m_curHealth = m_maxHealth;
	}

	protected void MoveTowardsPlayer(float dt){
		m_rb.AddForce (m_speed * (s_player.transform.position - transform.position));
	}

	public void Damage(float dam){
		m_curHealth -= dam;
		if (m_curHealth < 0.0f) {
			Destroy (this.gameObject);
		}
	}

}

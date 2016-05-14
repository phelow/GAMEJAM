using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	protected static GameObject s_player;

	[SerializeField]protected Rigidbody2D m_rb;
	[SerializeField]protected float m_speed;


	// Use this for initialization
	void Start () {
		s_player = GameObject.FindGameObjectWithTag ("Player");
	}

	protected void MoveTowardsPlayer(float dt){
		m_rb.AddForce (m_speed * (s_player.transform.position - transform.position));
	}

}

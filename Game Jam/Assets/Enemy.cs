using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public enum Buff
	{
		None,
		Speed
	}

	public enum Animation
	{
		None,
		Melee1,
		Melee2
	}

	protected static GameObject s_player;

	[SerializeField]private GameObject m_deathAnnouncer;
	[SerializeField]protected Animation m_animChoice;

	[SerializeField]protected SpriteRenderer m_spriteRenderer;
	[SerializeField]protected Buff m_buff = Buff.Speed;
	[SerializeField]protected Rigidbody2D m_rb;
	[SerializeField]protected float m_speed = 1.0f;
	[SerializeField]protected float m_defaultSpeed = 1.0f;

	[SerializeField]protected float m_maxHealth = 100.0f;
	[SerializeField]protected float m_curHealth;
	[SerializeField]private float m_timeBetweenBuffBursts = .1f;
	[SerializeField]private float m_buffRange = 10.0f;
	[SerializeField]private float m_buffedSpeed = 10.0f;
	[SerializeField]protected Animator m_animator;


	// Use this for initialization
	void Start () {

		switch (m_animChoice) {
		case Animation.Melee1:
			m_animator.CrossFade ("Melee1",0.0f);
			break;
		case Animation.Melee2:
			m_animator.CrossFade ("Melee2",0.0f);
			break;
		}

		s_player = GameObject.FindGameObjectWithTag ("Player");
		m_curHealth = m_maxHealth;
		if (m_buff == Buff.Speed) {
			StartCoroutine (BuffOtherEnemies ());
		}
	}

	private IEnumerator SpeedBoost(float duration){
		try{
			m_spriteRenderer.color = Color.yellow;
		}
		catch{
			Debug.LogWarning ("Need a sprite render for Enemy");
		}
		m_speed = m_buffedSpeed;
		yield return new WaitForSeconds (duration);
		m_speed = m_defaultSpeed;try{
			m_spriteRenderer.color = Color.white;
		}
		catch{
			Debug.LogWarning ("Need a sprite render for Enemy");
		}
	}

	public void BuffEnemy(Buff buff,float duration){
		switch (buff) {
		case Buff.Speed:
			StartCoroutine (SpeedBoost (duration));
			break;
			
		}
	}

	protected IEnumerator BuffOtherEnemies(){
		while (true) {
			Collider2D[] hitColliders = Physics2D.OverlapCircleAll (transform.position, m_buffRange);
			foreach (Collider2D col in hitColliders) {
				Enemy en = col.GetComponent<Enemy> ();
				if(en){
					en.BuffEnemy (m_buff, m_timeBetweenBuffBursts);
				}

			}

			yield return new WaitForSeconds (m_timeBetweenBuffBursts);
		}
	}

	protected void MoveTowardsPlayer(float dt){
		m_rb.AddForce (m_speed * (s_player.transform.position - transform.position));
	}

	private IEnumerator DamageEffect(){
		for (int i = 0; i < 10; i++) {
			m_spriteRenderer.color = Color.red;
			yield return new WaitForEndOfFrame ();
			m_spriteRenderer.color = Color.white;
			yield return new WaitForEndOfFrame ();
		}
	}

	public void Damage(float dam){
		StartCoroutine (DamageEffect ());	
		m_curHealth -= dam;
		if (m_curHealth < 0.0f) {
			GameObject go = GameObject.Instantiate (m_deathAnnouncer);
			go.transform.position = transform.position;
			Destroy (this.gameObject);

		}
	}

}

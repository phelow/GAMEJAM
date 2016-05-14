using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public enum Buff
	{
		None,
		Speed
	}

	protected static GameObject s_player;

	[SerializeField]protected SpriteRenderer m_spriteRenderer;
	[SerializeField]protected Buff m_buff = Buff.Speed;
	[SerializeField]protected Rigidbody2D m_rb;
	[SerializeField]protected float m_speed = 1.0f;
	[SerializeField]protected float m_defaultSpeed = 1.0f;

	[SerializeField]private float m_maxHealth = 100.0f;
	[SerializeField]private float m_curHealth;
	[SerializeField]private float m_timeBetweenBuffBursts = .1f;
	[SerializeField]private float m_buffRange = 10.0f;
	[SerializeField]private float m_buffedSpeed = 10.0f;


	// Use this for initialization
	void Start () {
		s_player = GameObject.FindGameObjectWithTag ("Player");
		m_curHealth = m_maxHealth;
		StartCoroutine (BuffOtherEnemies ());
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
			Debug.Log (hitColliders.Length);
			foreach (Collider2D col in hitColliders) {
				//if the angle betweeen is less than amulet angle
				Vector3 directionToTarget = transform.position - col.transform.position;

				float angle = Vector3.Angle (transform.up * -1, directionToTarget);

				if (Mathf.Abs (angle) < 45.0f && col.tag == "Enemy") {
					col.GetComponent<Enemy> ().BuffEnemy (m_buff, m_timeBetweenBuffBursts);
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
			Destroy (this.gameObject);
		}
	}

}

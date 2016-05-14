using UnityEngine;
using System.Collections;

public class CharacterInput : MonoBehaviour {
	[SerializeField]Rigidbody2D _rb;

	private static CharacterInput s_instance;

	[SerializeField]private float _speed;
	[SerializeField]private float _projectilePower = 10.0f;
	[SerializeField]private float _meleePower = 10.0f;

	[SerializeField]private GameObject _projectile;
	[SerializeField]private GameObject _melee;

	[SerializeField]private float m_maxPower = 100.0f;
	[SerializeField]private float m_curPower;
	[SerializeField]private float m_drainRate = 10.0f;

	[SerializeField]private float m_amuletRadius = 100.0f;
	[SerializeField]private float m_amuletAngle = 90.0f;

	[SerializeField]private float m_amuletPower = 10.0f;
	[SerializeField]private float m_amuletMinStrength = 1.0f;
	[SerializeField]private float m_amuletMaxStrength = 5.0f;

	[SerializeField]private float m_timeBetweenAttacks = 1.0f;
	private float m_timeSinceLastAttack;

	private static bool m_immobilized = false;

	public static void ImmobilizeCharacter (){
		m_immobilized = true;
		Debug.Log ("Character has been immobilized");
		s_instance._rb.velocity = Vector2.zero;
	}


	public static void UnImmobilizeCharacter (){
		m_immobilized = false;
	}

	// Use this for initialization
	void Start () {
		s_instance = this;
		StartCoroutine (PlayerInput());
		StartNight ();
	}


	void StartNight(){
		m_curPower = m_maxPower;
	}

	void Update(){
		m_timeSinceLastAttack -= Time.deltaTime;
		if (m_immobilized == false && Shrine.IsDay() == false && m_timeSinceLastAttack < 0.0f) {
			if (Input.GetKeyDown (KeyCode.Mouse0)) {
				m_timeSinceLastAttack = m_timeBetweenAttacks;
				GameObject projectile = GameObject.Instantiate (_projectile);
				projectile.transform.position = transform.position;
				Vector3 sp = Camera.main.WorldToScreenPoint (transform.position);
				Vector3 dir = (Input.mousePosition - sp).normalized;
				projectile.GetComponent<Rigidbody2D> ().AddForce (dir * _projectilePower);
			}
			if (Input.GetKeyDown (KeyCode.Mouse1) && m_timeSinceLastAttack < 0.0f) {
				ImmobilizeCharacter ();
				m_timeSinceLastAttack = m_timeBetweenAttacks;
				GameObject projectile = GameObject.Instantiate (_melee);
				projectile.transform.parent = this.transform;
				projectile.transform.localPosition = Vector2.zero;
				Vector3 sp = Camera.main.WorldToScreenPoint (transform.position);
				Vector3 dir = (Input.mousePosition - sp).normalized;
				projectile.GetComponent<Rigidbody2D> ().AddForce (dir * _meleePower);
			
			}
			if (Input.GetKey (KeyCode.Space) && m_curPower > 0.0f && m_timeSinceLastAttack < 0.0f) {
				m_timeSinceLastAttack = m_timeBetweenAttacks;
				//Use the amulet
				m_curPower -= Time.deltaTime * m_drainRate;

				Collider2D[] hitColliders = Physics2D.OverlapCircleAll (transform.position, 100.0f);
				Debug.Log (hitColliders.Length);
				foreach (Collider2D col in hitColliders) {
					//if the angle betweeen is less than amulet angle
					Vector3 directionToTarget = transform.position - col.transform.position;

					float angle = Vector3.Angle (transform.up * -1, directionToTarget);

					if (Mathf.Abs (angle) < 45.0f && col.tag != "Player") {


						Debug.Log ("Object is in front of the player:" + angle);
						//then push them away from the player.'
						Rigidbody2D rb = col.GetComponent<Rigidbody2D> ();
						if (rb) {
							rb.AddForce (transform.up * m_amuletPower * Mathf.Lerp (m_amuletMinStrength, m_amuletMaxStrength, m_curPower / m_maxPower));
						}
					}

				}

			}
		}
	}

	private IEnumerator PlayerInput(){
		while (true) {
			if (m_immobilized == false) {
				if (Input.GetAxis ("Vertical") < -0.3f) {
					transform.up = Vector2.down;
					_rb.AddRelativeForce (transform.up * _speed);
				} else if (Input.GetAxis ("Vertical") > .3f) {
					transform.up = Vector2.up;
					_rb.AddRelativeForce (transform.up * _speed);

				}


				if (Input.GetAxis ("Horizontal") < -0.3f) {

					transform.up = Vector2.left;
					_rb.AddRelativeForce (transform.up * _speed);
				} else if (Input.GetAxis ("Horizontal") > .3f) {
					transform.up = Vector2.right;
					_rb.AddRelativeForce (transform.up * _speed);

				}
			}
			yield return new WaitForEndOfFrame ();
		}
	}
}

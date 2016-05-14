using UnityEngine;
using System.Collections;

public class CharacterInput : MonoBehaviour {
	[SerializeField]Rigidbody2D _rb;

	private static CharacterInput s_instance;

	[SerializeField]private float _speed;
	[SerializeField]private float _defaultSpeed;
	[SerializeField]private float m_defaultFireRate;
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
	[SerializeField]private float m_meleeRange = 1.0f;
	[SerializeField]private float m_meleeDamage = 1.0f;

	[SerializeField]private Animator m_animator;

	[SerializeField]private AudioSource m_as;
	[SerializeField]private AudioClip m_click;
	[SerializeField]private AudioClip m_dig;
	[SerializeField]private AudioClip m_magic;
	[SerializeField]private AudioClip m_melee;
	[SerializeField]private AudioClip m_unearth;


	private float m_timeSinceLastAttack;
	private GameMaster.ItemsEnabled m_itemsEnabled;

	private static bool m_immobilized = false;


	public static void PlayClickSoundEffect(){
		s_instance.m_as.PlayOneShot (s_instance.m_click);
	}

	public static void ImmobilizeCharacter (){
		s_instance.m_animator.CrossFade ("Digging", 0.0f);
		s_instance.m_as.PlayOneShot (s_instance.m_dig);

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

	public static void SetWeaponForLevel(GameMaster.ItemsEnabled master){
		Debug.Log (s_instance.m_itemsEnabled);
		s_instance.m_itemsEnabled = master;
	}

	void Update(){
		m_timeSinceLastAttack -= Time.deltaTime;
		if (m_immobilized == false && Shrine.IsDay() == false && m_timeSinceLastAttack < 0.0f) {
			if (Input.GetKey(KeyCode.Mouse0) && m_itemsEnabled == GameMaster.ItemsEnabled.MedallionAndStaff) {
				m_timeSinceLastAttack = m_timeBetweenAttacks;
				GameObject projectile = GameObject.Instantiate (_projectile);
				projectile.transform.position = transform.position;
				Vector3 sp = Camera.main.WorldToScreenPoint (transform.position);
				Vector3 dir = (Input.mousePosition - sp).normalized;
				projectile.GetComponent<Rigidbody2D> ().AddForce (dir * _projectilePower);


				m_as.PlayOneShot (m_magic);

			}
			if (Input.GetKey (KeyCode.Mouse1) && m_timeSinceLastAttack < 0.0f && m_itemsEnabled == GameMaster.ItemsEnabled.MedallionAndStaff) {
				m_timeSinceLastAttack = m_timeBetweenAttacks;
				m_timeSinceLastAttack = m_timeBetweenAttacks;
				//Use the amulet
				m_curPower -= Time.deltaTime * m_drainRate;

				m_as.PlayOneShot (m_melee);
				Collider2D[] hitColliders = Physics2D.OverlapCircleAll (transform.position, m_meleeRange);
				Debug.Log (hitColliders.Length);
				foreach (Collider2D col in hitColliders) {
					//if the angle betweeen is less than amulet angle
					Vector3 directionToTarget = transform.position - col.transform.position;

					float angle = Vector3.Angle (transform.up * -1, directionToTarget);

					if (Mathf.Abs (angle) < 45.0f && col.tag != "Player") {

						if (col.tag == "Enemy") {
							col.GetComponent<Enemy> ().Damage (m_meleeDamage);
						}

						Debug.Log ("Object is in front of the player:" + angle);
						//then push them away from the player.'
						Rigidbody2D rb = col.GetComponent<Rigidbody2D> ();
						if (rb) {
							rb.AddForce (transform.up * m_amuletPower * Mathf.Lerp (m_amuletMinStrength, m_amuletMaxStrength, m_curPower / m_maxPower));
						}
					}

				}
			
			}
			if (Input.GetKey (KeyCode.Space) && m_curPower > 0.0f && m_timeSinceLastAttack < 0.0f && (m_itemsEnabled == GameMaster.ItemsEnabled.MedallionAndStaff && m_itemsEnabled == GameMaster.ItemsEnabled.MedallionOnly)) {
				m_timeSinceLastAttack = m_timeBetweenAttacks;
				//Use the amulet
				m_curPower -= Time.deltaTime * m_drainRate;

				m_as.PlayOneShot (m_magic);
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

	public static void SetFireRate(float fireRate){
		s_instance.m_timeBetweenAttacks = fireRate;
		Debug.Log ("Setting fire rate to: " + fireRate);
	}

	public static void SetMovementSpeed(float movementSpeed){
		s_instance._speed = movementSpeed;
		Debug.Log ("Setting movement speed: " + movementSpeed);
	}

	public static void ResetFireRate(){
		s_instance.m_timeBetweenAttacks = s_instance.m_defaultFireRate;
	}

	public static void ResetSpeed(){
		s_instance._speed = s_instance._defaultSpeed;
	}
	private enum direction
	{
		up,
		down,
		left,
		right
	}
	private direction dir = direction.up;

	private IEnumerator PlayerInput(){
		while (true) {
			if (m_immobilized == false) {
				if (Input.GetAxis ("Vertical") < -0.3f) {
					if (dir != direction.up) {
						m_animator.CrossFade ("Forward", 0.0f);
						dir = direction.up;
					}
				} else if (Input.GetAxis ("Vertical") > .3f) {
					if (dir != direction.down) {
						m_animator.CrossFade ("Backward", 0.0f);
						dir = direction.down;
					}

				}else if (Input.GetAxis ("Horizontal") < -0.3f) {
					if (dir != direction.left) {
						m_animator.CrossFade ("Left", 0.0f);
						dir = direction.left;
					}
				} else if (Input.GetAxis ("Horizontal") > .3f) {
					if (dir != direction.right) {
						m_animator.CrossFade ("Right",0.0f);
						dir = direction.right;
					}

				}


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

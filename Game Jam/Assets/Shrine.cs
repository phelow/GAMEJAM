﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shrine : MonoBehaviour {
	public enum ShrineTypePositiveEffect{
		Healing,
		FireRate,
		PlayerMovementIncrease,
		ImprovePlayerDamageDealt,
		ReducePlayerDamageTaken,
		None
	}

	public enum ShrineTypeNegativeEffect{
		Hurting,
		Spawner,
		ReducePlayerDamageDealt,
		PlayerMovementDecrease,
		IncreasePlayerDamageTaken,
		None
	}

	public enum ExcavationStatus{
		Buried,
		Excavated
	}

	public enum Phase{
		Night,
		Day
	}

	private static Shrine s_instance;

	[SerializeField]private GameObject m_spawnable;

	[SerializeField]private ExcavationStatus m_excavationStatus = ExcavationStatus.Excavated;


	[SerializeField]private ShrineTypePositiveEffect m_type;
	[SerializeField]private ShrineTypeNegativeEffect m_negativeEffect;

	[SerializeField]private string m_shrineExplanation;

	[SerializeField]private List<string> m_shrinePoems;

	[SerializeField]private float m_minPlayerMoveSpeed = 1.0f;
	[SerializeField]private float m_maxPlayerMoveSpeed = 1.0f;

	[SerializeField]private float m_minPlayerMoveDebuffSpeed = 0.5f;
	[SerializeField]private float m_maxPlayerMoveDebuffSpeed = 0.1f;

	[SerializeField]private float m_excavationTime = 3.0f;

	[SerializeField]Rigidbody2D m_rb;

	[SerializeField]private float m_minSpawnTime = 1.0f;
	[SerializeField]private float m_maxSpawnTime = 5.0f;

	[SerializeField]private float m_minFireRate = 1.0f;
	[SerializeField]private float m_maxFireRate = .1f;


	[SerializeField]private float m_xSpawningOffset = 10.0f;
	[SerializeField]private float m_ySpawningOffset = 10.0f;

	[SerializeField]private float m_minPlayerDamageMult = 1.0f;
	[SerializeField]private float m_maxPlayerDamageMult = 10.0f;


	[SerializeField]private float m_minPlayerDamageDealtDebuffMult = 1.0f;
	[SerializeField]private float m_maxPlayerDamageDealtDebuffMult = 10.0f;


	[SerializeField]private float m_minPlayerDamageDebuffMult = .5f;
	[SerializeField]private float m_maxPlayerDamageDebuffMult = .1f;


	[SerializeField]private float m_minPlayerDamageTakenMult = 1.0f;
	[SerializeField]private float m_maxPlayerDamageTakenMult = .1f;


	[SerializeField]private float m_minPlayerDamageDebuffTakenMult = 2.0f;
	[SerializeField]private float m_maxPlayerDamageDebuffTakenMult = 10.0f;

	[SerializeField]private int m_spawnOnLevel = 0;
	[SerializeField]private int m_dissappearOnLevel = 5;

	[SerializeField]private bool m_activated = false;

	[SerializeField]private List<Sprite> m_bottoms;
	[SerializeField]private List<Sprite> m_bottomsBuried;
	[SerializeField]private List<Sprite> m_tops;

	[SerializeField]private SpriteRenderer m_topRenderer;
	[SerializeField]private SpriteRenderer m_bottomRenderer;

	[SerializeField]private AudioClip m_unearthSound;

	private static Phase s_phase;

	private const float c_baseReadingTime = 7.0f;

	private static bool m_examining = false;

	private static GameObject s_player;

	private static bool m_clickedToContinue = false;

	private const string s_clickToExcavate = " Click the shrine to excavate statue.";
	private const string s_clickToContinue = " Click the shrine to continue.";
	private const string m_excavatingBlurb = " Excavating...";

	private static float m_triggerDistance = 10.0f;

	private IEnumerator m_spawnReference;

	[SerializeField]private SpriteRenderer m_spriteRenderer;
	[SerializeField]private AudioSource m_as;
	[SerializeField]private AudioClip m_spawnclip;

	// Use this for initialization
	void Start () {
		m_shrineExplanation = m_shrinePoems [1];
		m_bottomRenderer.sprite = m_bottomsBuried [(int)m_type];
		m_topRenderer.sprite = m_tops [(int)m_negativeEffect];

		s_instance = this;
		try{
			s_instance.m_spriteRenderer.color = new Color(Color.black.r,Color.black.g,Color.black.b,.1f);
			Debug.LogWarning("greyed out");
		}
		catch{
			Debug.LogError ("Assign sprite renderer");
		}

		m_particleSystem.enableEmission = false;
		if (s_player == null) {
			s_player = GameObject.FindGameObjectWithTag ("Player");
		}
	}

	private IEnumerator SpawnRoutine(){
		//todo
		while (true) {
			GameObject.Instantiate (m_spawnable);
			m_as.PlayOneShot (m_spawnclip);
			m_spawnable.transform.position = new Vector3(gameObject.transform.position.x + m_xSpawningOffset,gameObject.transform.position.y + m_ySpawningOffset,gameObject.transform.position.z);
			yield return new WaitForSeconds (Random.Range (m_minSpawnTime, m_maxSpawnTime));
		}
	}

	// Update is called once per frame
	void Update () {
		if (s_instance.m_activated) {
			if (s_phase == Phase.Night) {
				if (m_spawnReference == null && m_negativeEffect == ShrineTypeNegativeEffect.Spawner && m_excavationStatus == ExcavationStatus.Excavated) {
					m_spawnReference = SpawnRoutine ();
					StartCoroutine (m_spawnReference);
				}
			} else {
				if (m_spawnReference != null && m_negativeEffect == ShrineTypeNegativeEffect.Spawner) {
					StopCoroutine (m_spawnReference);
					m_spawnReference = null;
				}
			}
		}
	}

	public static void SetNight(){ //needs to be called for every individual gameobject
		s_phase = Phase.Night;
		s_instance.m_spawnReference = null;
		/*if (s_instance.m_negativeEffect == ShrineTypeNegativeEffect.Spawner) {
			Debug.Log ("Starting spawning coroutine");
			s_instance.StartCoroutine (s_instance.m_spawnReference);


		}*/

	}


	public static void SetDay(){
		s_phase = Phase.Day;

		if (GameMaster.day == s_instance.m_spawnOnLevel) {
			//activate
			try{
				s_instance.m_spriteRenderer.color = Color.white;
			}
			catch{
				Debug.LogError ("Assign sprite renderer");
			}
			s_instance.m_activated = true;
		} else if (GameMaster.day == s_instance.m_dissappearOnLevel) {
			Destroy (s_instance.gameObject);
		}

		//s_instance.StopCoroutine (s_instance.m_spawnReference);
	}


	private IEnumerator WaitForSpace(){
		yield return new WaitForEndOfFrame ();
		m_clickedToContinue = false;
		yield return new WaitForEndOfFrame ();

	}

	private IEnumerator ExcavateShrine(){
		m_examining = true;
		//While you are within range of the gameobject
		bool finished = false;
		m_clickedToContinue = false;
		//set onscreen text to first line

		if (Vector3.Distance (s_player.transform.position, transform.position) < m_triggerDistance) {
			TextManager.SetText (m_shrinePoems [Random.Range (0, m_shrinePoems.Count)]);
		}
		m_clickedToContinue = false; 
		yield return new WaitForSeconds (.1f);
		StartCoroutine (WaitForSpace());
		yield return new WaitForSeconds (.1f);
		//Wait for player to respond
		float t = 0.0f;
		while( Vector3.Distance(s_player.transform.position, transform.position) < m_triggerDistance && t < c_baseReadingTime && m_clickedToContinue == false){
			t += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}
		if (Vector3.Distance (s_player.transform.position, transform.position) < m_triggerDistance) {
			
			TextManager.AddText (s_clickToExcavate);
		}
		//wait for player to respond
		while( Vector3.Distance(s_player.transform.position, transform.position) < m_triggerDistance && m_clickedToContinue == false ){
			yield return new WaitForEndOfFrame ();
		}
		if (Vector3.Distance (s_player.transform.position, transform.position) < m_triggerDistance) {
			Debug.Log (113);
			CharacterInput.ImmobilizeCharacter ();
			TextManager.SetText (m_excavatingBlurb);
			yield return new WaitForSeconds (m_excavationTime);
			CharacterInput.UnImmobilizeCharacter ();

			m_excavationStatus = ExcavationStatus.Excavated;
			m_as.PlayOneShot (m_unearthSound);

			m_bottomRenderer.sprite = m_bottoms [(int)m_type];
			m_rb.isKinematic = false;
			TextManager.SetText (m_shrinePoems[1]);
			m_clickedToContinue = false; 
			yield return new WaitForSeconds (.1f);
			StartCoroutine (WaitForSpace ()); 
			yield return new WaitForSeconds (.1f);
			while (Vector3.Distance (s_player.transform.position, transform.position) < m_triggerDistance && m_clickedToContinue == false) {


				yield return new WaitForEndOfFrame ();
			}
		}
		//show explanation


		//show explanation

		TextManager.SetText ("");
		m_examining = false;
	}

	private IEnumerator ExamineShrine(){
		m_examining = true;
		//While you are within range of the gameobject
		bool finished = false;
		m_clickedToContinue = false;
		//set onscreen text to first line


		TextManager.SetText(m_shrinePoems[Random.Range(0,m_shrinePoems.Count)]);
		m_clickedToContinue = false; 
		yield return new WaitForSeconds (.1f);
		StartCoroutine (WaitForSpace());
		yield return new WaitForSeconds (.1f);
		//Wait for player to respond
		float t = 0.0f;
		while( Vector3.Distance(s_player.transform.position, transform.position) < m_triggerDistance && t < c_baseReadingTime && m_clickedToContinue == false){
			t += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}
		if (Vector3.Distance (s_player.transform.position, transform.position) < m_triggerDistance) {
			TextManager.AddText (s_clickToContinue);
		}

		//wait for player to respond
		while( Vector3.Distance(s_player.transform.position, transform.position) < m_triggerDistance && m_clickedToContinue == false ){


			yield return new WaitForEndOfFrame ();
		}
		if (Vector3.Distance (s_player.transform.position, transform.position) < m_triggerDistance) {
			TextManager.SetText (m_shrineExplanation);
		}
		m_clickedToContinue = false; 
		yield return new WaitForSeconds (.1f);
		StartCoroutine (WaitForSpace()); 
		yield return new WaitForSeconds (.1f);
		while( Vector3.Distance(s_player.transform.position, transform.position) < m_triggerDistance && m_clickedToContinue == false ){


			yield return new WaitForEndOfFrame ();
		}
		//show explanation

		TextManager.SetText ("");
		m_examining = false;
	}

	[SerializeField]private float m_activeTime = 5.0f;
	[SerializeField]private ParticleSystem m_particleSystem;
	[SerializeField]private float m_effectiveness = 1.0f;

	private IEnumerator ShrineActive(){
		m_particleSystem.enableEmission = true;

		float t = 0.0f;
		while (t < m_activeTime) {
			float dt = Time.deltaTime;
			t += dt;
			DoEffect (dt);
			yield return new WaitForEndOfFrame ();
		}

		CharacterInput.LerpColor (Color.white, Color.white,1.0f);
		CharacterInput.ResetFireRate ();
		CharacterInput.ResetSpeed ();
		Projectile.ResetDamageMultiplier ();
		m_particleSystem.enableEmission = false;
		Destroy (this);

	}
	private void DoEffect(float dt){
		Debug.Log ("DO effect");
		float dist = Vector3.Distance (s_player.transform.position, transform.position);
		switch (m_type) {
		case ShrineTypePositiveEffect.Healing:
			//See if the player is close enough
			if( dist < m_triggerDistance){
				//if he is heal him as a function of time and effectiveness and distance
				Health.Heal((1 + m_triggerDistance - dist) * m_effectiveness * dt);
				CharacterInput.LerpColor (Color.white, Color.green,(1 + m_triggerDistance - dist) * m_effectiveness * dt);

			}
			break;

		case ShrineTypePositiveEffect.FireRate:
			//See if the player is close enough
			if (dist < m_triggerDistance) {
				//if he is heal him as a function of time and effectiveness and distance
				CharacterInput.SetFireRate (Mathf.Lerp (m_minFireRate, m_maxFireRate, (1 + m_triggerDistance - dist)));
				CharacterInput.LerpColor (Color.white, Color.red,(1 + m_triggerDistance - dist) * m_effectiveness * dt);
			}
			break;
		case ShrineTypePositiveEffect.PlayerMovementIncrease:
			if (dist < m_triggerDistance) {
				//if he is heal him as a function of time and effectiveness and distance
				CharacterInput.SetMovementSpeed (Mathf.Lerp (m_minPlayerMoveSpeed, m_maxPlayerMoveSpeed, (1 + m_triggerDistance - dist)));
				CharacterInput.LerpColor (Color.white, Color.yellow,(1 + m_triggerDistance - dist) * m_effectiveness * dt);
			}
			break;
		case ShrineTypePositiveEffect.ImprovePlayerDamageDealt:
			if (dist < m_triggerDistance) {
				//if he is heal him as a function of time and effectiveness and distance
				Projectile.SetDamageMultiplier (Mathf.Lerp (m_minPlayerDamageMult, m_maxPlayerDamageMult, (1 + m_triggerDistance - dist)));
				CharacterInput.LerpColor (Color.white, Color.blue,(1 + m_triggerDistance - dist) * m_effectiveness * dt);
			}
			break;
		case ShrineTypePositiveEffect.ReducePlayerDamageTaken:
			if (dist < m_triggerDistance) {
				//if he is heal him as a function of time and effectiveness and distance
				Health.SetDamageMultiplier (Mathf.Lerp (m_minPlayerDamageTakenMult, m_maxPlayerDamageTakenMult, (1 + m_triggerDistance - dist)));
				CharacterInput.LerpColor (Color.white, Color.magenta,(1 + m_triggerDistance - dist) * m_effectiveness * dt);
			}
			break;
		}


		switch (m_negativeEffect) {
		case ShrineTypeNegativeEffect.Hurting:
			//See if the player is close enough
			if (dist < m_triggerDistance) {
				//if he is heal him as a function of time and effectiveness and distance
				Health.TakeDamage ((1 + m_triggerDistance - dist) * m_effectiveness * dt);
			}
			break;
		case ShrineTypeNegativeEffect.ReducePlayerDamageDealt:
			//See if the player is close enough
			if (dist < m_triggerDistance) {
				//if he is heal him as a function of time and effectiveness and distance
				Projectile.SetDamageMultiplier (Mathf.Lerp (m_minPlayerDamageDebuffMult, m_maxPlayerDamageDebuffMult, (1 + m_triggerDistance - dist)));

			}
			break;
		case ShrineTypeNegativeEffect.PlayerMovementDecrease:
			if (dist < m_triggerDistance) {
				//if he is heal him as a function of time and effectiveness and distance
				CharacterInput.SetMovementSpeed (Mathf.Lerp (m_minPlayerMoveDebuffSpeed, m_maxPlayerMoveDebuffSpeed, (1 + m_triggerDistance - dist)));
			}
			break;
		case ShrineTypeNegativeEffect.IncreasePlayerDamageTaken:
			if (dist < m_triggerDistance) {
				//if he is heal him as a function of time and effectiveness and distance
				Health.SetDamageMultiplier (Mathf.Lerp (m_minPlayerDamageDebuffTakenMult, m_maxPlayerDamageDebuffTakenMult, (1 + m_triggerDistance - dist)));
			}
			break;
		}
	}

	public static bool IsDay(){
		return s_phase == Phase.Day;
	}
	[SerializeField] private float m_cooldownTime = 30.0f;
	bool enabled = true;
	public void TurnOn(Collider2D col){
		if(m_excavationStatus == ExcavationStatus.Excavated && enabled){
			enabled = false;
			StartCoroutine (Reenable ());
			StartCoroutine (ShrineActive ());

		}
	}

	private IEnumerator Reenable(){
		yield return new WaitForSeconds (m_cooldownTime);
		enabled = true;
	}

	void OnMouseDown(){
		if (m_activated) {
			CharacterInput.PlayClickSoundEffect ();

			if (s_phase == Phase.Day) {

				if (m_examining == false) {
					if (m_excavationStatus == ExcavationStatus.Excavated) {
						StartCoroutine (ExamineShrine ());
					} else {
						StartCoroutine (ExcavateShrine ());
					}
				} else {
					m_clickedToContinue = true;
				}
			}
		} else {
		}
	}
}

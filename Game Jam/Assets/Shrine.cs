﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shrine : MonoBehaviour {
	public enum ShrineTypePositiveEffect{
		Healing
	}

	public enum ShrineTypeNegativeEffect{
		Hurting,
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

	[SerializeField]private ExcavationStatus m_excavationStatus = ExcavationStatus.Excavated;


	[SerializeField]private ShrineTypePositiveEffect m_type;
	[SerializeField]private ShrineTypeNegativeEffect m_negativeEffect;

	[SerializeField]private string m_shrineExplanation;

	[SerializeField]private List<string> m_shrinePoems;

	[SerializeField]private float m_excavationTime = 3.0f;

	[SerializeField]Rigidbody2D m_rb;

	private static Phase s_phase;

	private const float c_baseReadingTime = 7.0f;



	private static GameObject s_player;

	private bool m_spacePressed = false;

	private const string s_spaceToExcavate = " Press f to excavate statue.";
	private const string s_spaceToContinue = " Press f to continue.";

	private static float m_triggerDistance = 3.0f;

	// Use this for initialization
	void Start () {
		m_particleSystem.enableEmission = false;
		if (s_player == null) {
			s_player = GameObject.FindGameObjectWithTag ("Player");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static void SetNight(){
		s_phase = Phase.Night;
	}


	public static void SetDay(){
		s_phase = Phase.Day;
	}

	private IEnumerator WaitForSpace(){
		yield return new WaitForEndOfFrame ();
		m_spacePressed = false;
		yield return new WaitForEndOfFrame ();
		while (Input.GetKey (KeyCode.F) == false && Input.GetKeyDown (KeyCode.F) == false) {
			Debug.Log ("Waiting for space");
			yield return new WaitForEndOfFrame ();
		}
		m_spacePressed = true;

	}

	private IEnumerator ExcavateShrine(){
		//While you are within range of the gameobject
		bool finished = false;
		m_spacePressed = false;
		//set onscreen text to first line


		TextManager.SetText(m_shrinePoems[Random.Range(0,m_shrinePoems.Count)]);
		m_spacePressed = false; 
		yield return new WaitForSeconds (.1f);
		StartCoroutine (WaitForSpace());
		yield return new WaitForSeconds (.1f);
		//Wait for player to respond
		float t = 0.0f;
		while( Vector3.Distance(s_player.transform.position, transform.position) < m_triggerDistance && t < c_baseReadingTime && m_spacePressed == false){
			t += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}

		TextManager.AddText (s_spaceToExcavate);

		//wait for player to respond
		while( Vector3.Distance(s_player.transform.position, transform.position) < m_triggerDistance && m_spacePressed == false ){


			yield return new WaitForEndOfFrame ();
		}
		if (Vector3.Distance (s_player.transform.position, transform.position) < m_triggerDistance) {
			CharacterInput.ImmobilizeCharacter ();
			yield return new WaitForSeconds (m_excavationTime);
			CharacterInput.UnImmobilizeCharacter ();

			m_excavationStatus = ExcavationStatus.Excavated;
			m_rb.isKinematic = false;
			TextManager.SetText (m_shrineExplanation);
			m_spacePressed = false; 
			yield return new WaitForSeconds (.1f);
			StartCoroutine (WaitForSpace ()); 
			yield return new WaitForSeconds (.1f);
			while (Vector3.Distance (s_player.transform.position, transform.position) < m_triggerDistance && m_spacePressed == false) {


				yield return new WaitForEndOfFrame ();
			}
		}
		//show explanation


		//show explanation

		TextManager.SetText ("");
	}

	private IEnumerator ExamineShrine(){
		//While you are within range of the gameobject
		bool finished = false;
		m_spacePressed = false;
		//set onscreen text to first line


		TextManager.SetText(m_shrinePoems[Random.Range(0,m_shrinePoems.Count)]);
		m_spacePressed = false; 
		yield return new WaitForSeconds (.1f);
		StartCoroutine (WaitForSpace());
		yield return new WaitForSeconds (.1f);
		//Wait for player to respond
		float t = 0.0f;
		while( Vector3.Distance(s_player.transform.position, transform.position) < m_triggerDistance && t < c_baseReadingTime && m_spacePressed == false){
			t += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}

		TextManager.AddText (s_spaceToContinue);

		//wait for player to respond
		while( Vector3.Distance(s_player.transform.position, transform.position) < m_triggerDistance && m_spacePressed == false ){


			yield return new WaitForEndOfFrame ();
		}

		TextManager.SetText(m_shrineExplanation);
		m_spacePressed = false; 
		yield return new WaitForSeconds (.1f);
		StartCoroutine (WaitForSpace()); 
		yield return new WaitForSeconds (.1f);
		while( Vector3.Distance(s_player.transform.position, transform.position) < m_triggerDistance && m_spacePressed == false ){


			yield return new WaitForEndOfFrame ();
		}
		//show explanation

		TextManager.SetText ("");
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
		m_particleSystem.enableEmission = false;


	}

	private void DoEffect(float dt){
		float dist = Vector3.Distance (s_player.transform.position, transform.position);
		switch (m_type) {
		case ShrineTypePositiveEffect.Healing:
			//See if the player is close enough
			if( dist < m_triggerDistance){
				//if he is heal him as a function of time and effectiveness and distance
				Health.Heal((1 + m_triggerDistance - dist) * m_effectiveness * dt);
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
		}
	}

	public static bool IsDay(){
		return s_phase == Phase.Day;
	}

	public void TurnOn(){
		Debug.Log ("127: Turn on");
		StartCoroutine (ShrineActive ());
	}

	void OnMouseDown(){
		if (s_phase == Phase.Day) {
			if (m_excavationStatus == ExcavationStatus.Excavated) {
				StartCoroutine (ExamineShrine ());
			} else {
				StartCoroutine (ExcavateShrine ());
			}
		}
	}
}

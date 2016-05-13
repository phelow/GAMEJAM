using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shrine : MonoBehaviour {
	public enum ShrineType{
		FirePower
	}

	[SerializeField]private string m_shrineExplanation;

	[SerializeField]private List<string> m_shrinePoems;

	private const float c_baseReadingTime = 7.0f;

	private static GameObject s_player;

	private bool m_spacePressed = false;

	private const string s_spaceToContinue = "Press space to continue.";

	private static float m_triggerDistance = 3.0f;

	// Use this for initialization
	void Start () {
		if (s_player == null) {
			s_player = GameObject.FindGameObjectWithTag ("Player");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private IEnumerator WaitForSpace(){
		yield return new WaitForEndOfFrame ();
		m_spacePressed = false;
		yield return new WaitForEndOfFrame ();
		while (Input.GetKey (KeyCode.Space) == false && Input.GetKeyDown (KeyCode.Space) == false) {
			Debug.Log ("Waiting for space");
			yield return new WaitForEndOfFrame ();
		}
		m_spacePressed = true;

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

	void OnMouseDown(){
		StartCoroutine (ExamineShrine ());
	}
}

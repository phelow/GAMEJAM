using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {
	[SerializeField]private float m_dayLength = 10.0f;
	[SerializeField]private float m_nightLength = 10.0f;
	// Use this for initialization
	void Start () {
		StartCoroutine (DayNightCycle ());
	}

	private IEnumerator DayNightCycle(){
		while (true) {
			//Set Daytime
			Shrine.SetDay();

			//Wait for the day to end
			yield return new WaitForSeconds(m_dayLength);


			//Set nightTime
			Shrine.SetNight();

			yield return new WaitForSeconds (m_nightLength);
		}
	}
}

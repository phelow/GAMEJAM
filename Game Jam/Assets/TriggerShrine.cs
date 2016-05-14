using UnityEngine;
using System.Collections;

public class TriggerShrine : MonoBehaviour {
	[SerializeField]Collider2D m_collider;
	[SerializeField]Shrine m_shrine;

	void OnTriggerStay2D(Collider2D other){
		Debug.Log (9);
		if (other.gameObject.tag == "Player" && Shrine.IsDay() == false) {
			m_shrine.TurnOn (m_collider);
		}
	}
}

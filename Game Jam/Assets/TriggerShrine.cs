using UnityEngine;
using System.Collections;

public class TriggerShrine : MonoBehaviour {
	[SerializeField]Collider2D m_collider;
	[SerializeField]Shrine m_shrine;

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Player" && Shrine.IsDay() == false) {
			m_shrine.TurnOn (m_collider);
		}
	}
}

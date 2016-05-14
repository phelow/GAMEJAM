using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour {
	private static Music m_instance;

	[SerializeField]private AudioSource m_as;
	[SerializeField]private AudioClip m_night;
	[SerializeField]private AudioClip m_day;


	// Use this for initialization
	void Start () {
		m_instance = this;
	}

	public static void SetDay(){
		
		m_instance.m_as.clip = m_instance.m_day;
		m_instance.m_as.Play ();
	}

	public static void SetNight(){
		m_instance.m_as.clip = m_instance.m_night;
		m_instance.m_as.Play ();

	}
}

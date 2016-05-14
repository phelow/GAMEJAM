using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour {
	private static Health s_instance;
	[SerializeField]private float m_maxHealth;
	private static float m_damageMultiplier = 1.0f;
	private float m_curHealth;
	[SerializeField]private Text m_healthText;

	// Use this for initialization
	void Start () {
		s_instance = this;
		m_curHealth = m_maxHealth;

		//TODO: remove the following
		TakeDamage(50.0f);
	}

	public static void Heal(float amt){
		s_instance.m_curHealth += amt;
		if (s_instance.m_curHealth > s_instance.m_maxHealth) {
			s_instance.m_curHealth = s_instance.m_maxHealth;
		}
		s_instance.m_healthText.text = "" + s_instance.m_curHealth;
	}

	public static void SetDamageMultiplier(float mult){
		m_damageMultiplier = mult;
	}

	public static void TakeDamage(float amt){
		s_instance.m_curHealth -= amt * m_damageMultiplier;
		if (s_instance.m_curHealth < 0) {
			Debug.Log ("You have died");
			//TODO: gameover
			SceneManager.LoadScene(2);
		}
		s_instance.m_healthText.text = "" + s_instance.m_curHealth;
	}
}

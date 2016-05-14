using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {
	public enum ItemsEnabled{
		None,
		MedallionOnly,
		MedallionAndStaff
	}
	public static int day;

	[SerializeField]private float m_dayLength = 10.0f;
	[SerializeField]private float m_nightLength = 10.0f;
	[SerializeField]private Text m_timeText;

	[SerializeField]private SpriteRenderer m_backgroundSpriteRenderer;
	[SerializeField]private Sprite m_backgroundDaySprite;
	[SerializeField]private Sprite m_backgroundNightSprite;


	[SerializeField]private SpriteRenderer m_foregroundSpriteRenderer;
	[SerializeField]private Sprite m_foregroundDaySprite;
	[SerializeField]private Sprite m_foregroundNightSprite;

	[SerializeField]private int[] m_dayLengthForLevels;
	[SerializeField]private int[] m_nightLengthForLevels;
	[SerializeField]private ItemsEnabled[] m_itemsEnabledForLevels;

	// Use this for initialization
	void Start () {
		StartCoroutine (DayNightCycle ());
	}

	private IEnumerator DayNightCycle(){
		yield return new WaitForSeconds (.5f);
		for(day = 0; day < m_dayLengthForLevels.Length; day++){
			m_dayLength = m_dayLengthForLevels [day];
			m_nightLength = m_nightLengthForLevels [day];

			CharacterInput.SetWeaponForLevel (m_itemsEnabledForLevels [day]);


			//Set Daytime
			Shrine.SetDay();
			m_backgroundSpriteRenderer.sprite = m_backgroundDaySprite;
			m_foregroundSpriteRenderer.sprite = m_foregroundDaySprite;

			//Wait for the day to end
			//yield return new WaitForSeconds(m_dayLength);
			float t = m_dayLength;
			while (t >= 0.0f) {
				t -= Time.deltaTime;
				m_timeText.text = "Day:" + t;
				yield return new WaitForEndOfFrame ();
			}


			//Set nightTime
			Shrine.SetNight();
			m_backgroundSpriteRenderer.sprite = m_backgroundNightSprite;
			m_foregroundSpriteRenderer.sprite = m_foregroundNightSprite;
			t = m_nightLength;
			while (t >= 0.0f) {
				t -= Time.deltaTime;
				m_timeText.text = "Night:" + t;
				yield return new WaitForEndOfFrame ();
			}
		}
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextManager : MonoBehaviour {
	private static TextManager s_instance;

	[SerializeField]private Text m_bottomText;
	// Use this for initialization
	void Start () {
		s_instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void SetText(string text){
		s_instance.m_bottomText.text = text;
	}

	public static void AddText(string text){
		s_instance.m_bottomText.text = s_instance.m_bottomText.text+ text;

	}

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour {
	[SerializeField]List<int> m_times;
	[SerializeField]List<string> m_lines;
	[SerializeField]Text m_text;

	// Use this for initialization
	void Start () {
		StartCoroutine (DialogForScene());
	}

	private IEnumerator DialogForScene(){
		for (int i = 0; i < m_times.Count; i++) {
			m_text.text = m_lines [i];
			yield return new WaitForSeconds (m_times [i]);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ClickToGoTo : MonoBehaviour {
	[SerializeField]private int m_targetScene = 1;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			SceneManager.LoadScene (m_targetScene);
		}
	}
}

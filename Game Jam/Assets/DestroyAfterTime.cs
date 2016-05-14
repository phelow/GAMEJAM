using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (DestroyLater ());
	}

	private IEnumerator DestroyLater(){
		yield return new WaitForSeconds(3.0f);

		Destroy(this.gameObject)
	}
}

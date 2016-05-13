using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {
	[SerializeField]GameObject _player;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (_player.transform.position.x, _player.transform.position.y, _player.transform.position.z - 10.0f);
	}
}

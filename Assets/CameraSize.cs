using UnityEngine;
using System.Collections;

public class CameraSize : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Camera.main.orthographicSize = 13 * Screen.height / 400;
	}
	
	// Update is called once per frame
	void Update () {
		Camera.main.orthographicSize = 13 * Screen.height / 400;
	}
}

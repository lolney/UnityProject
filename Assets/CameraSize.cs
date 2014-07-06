using UnityEngine;
using System.Collections;

public class CameraSize : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Camera.main.orthographicSize = Mathf.Min(13 * Screen.height / 400, 26);
	}
	
	// Update is called once per frame
	void Update () {
		Camera.main.orthographicSize = Mathf.Min(13 * Screen.height / 400, 26);
	}
}

using UnityEngine;
using System.Collections;

public class DestroyCage : MonoBehaviour {
	
	void Start() {}
	void Update() {}
	
	void OnTriggerEnter2D(Collider2D other){
			
		if(other.gameObject.name.Equals("Main Character")) {
			Debug.Log("Main Char Entered");
			GameObject scripts = GameObject.Find("Scripts");
			MazeGeneration mg = scripts.GetComponent<MazeGeneration>();
			
			GameObject WeeWee = transform.GetChild(0).gameObject;
			
			transform.DetachChildren();
			
			WeeWeeAI ai = WeeWee.GetComponent<WeeWeeAI>();
			
			Node start = PathFinding.findClosestNode(WeeWee.transform);
			Debug.Log("Postion: " + WeeWee.transform.position);
			Debug.Log("Closest Node: " + start.center);
			Node end = mg.map[mg.gridSize - 2, mg.gridSize - 2];
			ai.beginPathNav(start, end);
			
			Destroy(gameObject);
		}
	}
}

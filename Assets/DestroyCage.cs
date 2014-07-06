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
			
			if(transform.childCount != 0){
				GameObject WeeWee = transform.GetChild(transform.childCount - 1).gameObject;
				
				WeeWeeAI ai = WeeWee.GetComponent<WeeWeeAI>();
				
				Node start = PathFinding.findClosestNode(WeeWee.transform);
				Debug.Log("Postion: " + WeeWee.transform.position);
				Debug.Log("Closest Node: " + start.center);
				Node end = mg.map[mg.gridSize - 2, mg.gridSize - 2];
				ai.beginPathNav(start, end);
				
				for(int c=0; c<transform.childCount; c++) {
					Rigidbody2D b = transform.GetChild(c).GetComponent<Rigidbody2D>();
					b.WakeUp();
					b.velocity = Random.insideUnitCircle * 50;
				}
				
				transform.DetachChildren();
			}
			
			Destroy(gameObject);
		}
	}
}

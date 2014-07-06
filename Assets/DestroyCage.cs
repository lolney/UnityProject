using UnityEngine;
using System.Collections;

public class DestroyCage : MonoBehaviour {
	
	void Start() {}
	void Update() {}
	
	void OnTriggerEnter2D(Collider2D other){
	
		GameObject scripts = GameObject.Find("Scripts");
		MazeGeneration mg = scripts.GetComponent<MazeGeneration>();
		Node end;
		
		GameObject gui = GameObject.Find("GUI Text");
		UpdateText txt = gui.GetComponent<UpdateText>();
		
		if(other.gameObject.name.Equals("Main Character")) {
			Debug.Log("Main Char Entered");
			end = mg.map[MazeGeneration.gridSize - 2, MazeGeneration.gridSize - 2];
			destroy(end);
			txt.playerCages++;
			txt.cages--;
		}
		else if(other.gameObject.name.Equals("Cat")) {
			Debug.Log("Cat Entered");
			end = mg.map[0, MazeGeneration.gridSize - 2];
			destroy(end);
			txt.catCages++;
			txt.cages--;
		}
			
	}
	
	
	private void destroy(Node end) {
		
		
		if(transform.childCount != 0){
			GameObject WeeWee = transform.GetChild(transform.childCount - 1).gameObject;
			
			WeeWeeAI ai = WeeWee.GetComponent<WeeWeeAI>();
			
			Node start = PathFinding.findClosestNode(WeeWee.transform);
			start.cage = false;
			Debug.Log("Postion: " + WeeWee.transform.position);
			Debug.Log("Closest Node: " + start.center);
			
			ai.beginPathNav(start, end);
			
			for(int c=0; c<transform.childCount -1; c++) {
				Transform child = transform.GetChild(c);
				Rigidbody2D b = child.GetComponent<Rigidbody2D>();
				b.WakeUp();
				b.velocity = Random.insideUnitCircle * 50;
				b.angularVelocity = Random.Range(100, 600);
				
				child.gameObject.AddComponent<DestroyAfterSeconds>();
			}
			
			transform.DetachChildren();
			Destroy(gameObject);

		}
	}
}
	
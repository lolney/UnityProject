using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CatAI : MonoBehaviour {

	public state current;
	public bool stateOverride = true;
	private SimpleMovementController controller = null;
	
	private float Speed= 1f;	
	public float JumpPower= 30.0f;	
	
	private Node[,] map;
	private Node end;
	
	void  Start (){
				
		if(controller == null)
			controller = new SimpleMovementController(gameObject, Speed, JumpPower);
			
		current = state.idle;
		stateOverride = true;
		
		StartCoroutine(wait());
		
	}
	
	IEnumerator wait() {
		Vector2 pos = (Vector2)GameObject.Find("Main Character").transform.position;
		
		yield return new WaitForSeconds(2);
		
		GameObject scripts = GameObject.Find("Scripts");
		MazeGeneration mg = scripts.GetComponent<MazeGeneration>();
		
		map = mg.map;
		end = map[0,0];
		
		current = state.path;
		transform.position = pos + new Vector2(15f,5f);
	}
	
	IEnumerator idle() {
		
		yield return new WaitForSeconds(.5f);
		current = state.path;
	}
	
	void  OnCollisionEnter2D ( Collision2D collision  ){
		if(controller == null)
			controller = new SimpleMovementController(gameObject, Speed, JumpPower);
		controller.OnCollisionEnter2D(collision);
	}
	
	public void beginPathNav(Node start, Node end) {
		current = state.path;
		
		controller.initPath(start, end);
	}
	
	void  Update (){
				
		// Keep the wee wee upright
		transform.localRotation = Quaternion.identity; 
		
		if(!stateOverride){
			int rand = Random.Range(0, 250);
			switch(rand) {
			case 0:
				print(gameObject + " Switched state to idle");
				current = state.idle;
				break;
			case 1:
				print(gameObject + " Switched state to left");
				current = state.left;
				break;
			case 2:
				print(gameObject + " Switched state to right");
				current = state.right;
				break;
			case 3:
				print(gameObject + " Switched state to jump");
				controller.jump();
				break;
			case 4:
				print(gameObject + " Switched state to fly");
				current = state.fly;
				break;
			}
		}
		
		
		
		switch(current) {
		case state.left:
			controller.move(-1f);
			break;
		case state.right:
			controller.move(1f);
			break;
		case state.fly:
			controller.fly(.25f);
			break;
		case state.path:			
			if(!end.cage || controller.followPath()){
				Node begin = PathFinding.findClosestNode(transform);
				end = findCage(begin);
				if(end == null){
					current = state.idle;
					stateOverride = false;
					break;
				}
				Debug.Log("Found cage at: " + end.center);
				
				beginPathNav(begin, end);
				
				current = state.idle;
				StartCoroutine(idle());
				
			}
			break;
		} 
		
	}
	
	private Node findCage(Node current) {
		
		for(int i=0; i< MazeGeneration.gridSize - 1;i++)
		for(int j=0; j< MazeGeneration.gridSize - 1;j++){
			map[i,j].explored = false;
		}
		
		Queue<Node> frontier;
		
		frontier = current.findNeigbors(true);
	
		int count = 0;
		while(frontier.Count != 0){
			count++;
			current = frontier.Dequeue();
			if(current.cage) return current;
			current.findNeigbors(frontier, true);
			
		}
		
		Debug.Log("No cages found. Searched nodes: " + count);
		return null;
	}
}

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
	private Animator anim;
	
	void  Start (){
		
		anim = GetComponent<Animator>();
		
		if(controller == null)
			controller = new SimpleMovementController(gameObject, Speed, JumpPower);
			
		current = state.idle;
		stateOverride = true;
		
		StartCoroutine(wait());
		
	}
	
	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.name.StartsWith("Note")){
			
			anim.Play("CatAngel", 0);
			current = state.idle;
			
			StartCoroutine(idle(15));
			Destroy(other);
		}
	}
	
	IEnumerator wait() {
		Vector2 pos = (Vector2)GameObject.Find("Main Character").transform.position;
		
		yield return new WaitForSeconds(0f);
		
		GameObject scripts = GameObject.Find("Scripts");
		MazeGeneration mg = scripts.GetComponent<MazeGeneration>();
		
		map = mg.map;
		end = map[0,0];
		
		current = state.path;
		transform.position = pos + new Vector2(15f,5f);
	}
	
	IEnumerator idle(float s) {
		
		yield return new WaitForSeconds(s);
		current = state.path;
		
		anim.Play("Flying", 0);
	}
	
	void  OnCollisionEnter2D ( Collision2D collision  ){
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
				end = begin.findCage(map);
				if(end == null){
					current = state.idle;
					stateOverride = false;
					break;
				}
				Debug.Log("Found cage at: " + end.center);
				
				beginPathNav(begin, end);
				
				current = state.idle;
				StartCoroutine(idle(.5f));
				
			}
			break;
		} 
		
	}
	
}

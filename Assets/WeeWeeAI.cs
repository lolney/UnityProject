using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum state {idle, left, right, fly, path};

public class WeeWeeAI : MonoBehaviour {
	
	public state current;
	public bool stateOverride = false;
	private SimpleMovementController controller = null;

	public float Speed= 4.0f;	
	public float JumpPower= 30.0f;	

	void  Start (){
	
		current = state.idle;
		if(controller == null)
			controller = new SimpleMovementController(gameObject, Speed, JumpPower);
			
	}

	void  OnCollisionEnter2D ( Collision2D collision  ){
		if(controller == null)
			controller = new SimpleMovementController(gameObject, Speed, JumpPower);
		controller.OnCollisionEnter2D(collision);
	}
	
	public void beginPathNav(Node start, Node end) {
		current = state.path;
		stateOverride = true;
		
		controller.initPath(start, end);
	}

	void  Update (){
		
		// Keep the wee wee upright
		transform.localRotation = Quaternion.identity; 
	
		if(!stateOverride){
			int rand = Random.Range(0, 250);
			switch(rand) {
				case 0:
					//print(gameObject + " Switched state to idle");
					current = state.idle;
					break;
				case 1:
					//print(gameObject + " Switched state to left");
					current = state.left;
					break;
				case 2:
					//print(gameObject + " Switched state to right");
					current = state.right;
					break;
				case 3:
					//print(gameObject + " Switched state to jump");
					controller.jump();
					break;
				case 4:
					//print(gameObject + " Switched state to fly");
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
				controller.followPath();
				break;
		} 

	}
}
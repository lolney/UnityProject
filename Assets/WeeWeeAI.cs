using UnityEngine;
using System.Collections;

public class WeeWeeAI : MonoBehaviour {


	enum state {idle, left, right, fly};
	private state current;
	private SimpleMovementController controller;

	public float Speed= 4.0f;	
	public float JumpPower= 30.0f;	

	void  Start (){
		current = state.idle;
		
		controller = new SimpleMovementController(gameObject, Speed, JumpPower);

	}

	void  OnCollisionEnter2D ( Collision2D collision  ){
		controller.OnCollisionEnter2D(collision);
	}

	void  Update (){
		int rand = Random.Range(0, 250);
		
		transform.localRotation = Quaternion.identity; 

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
		
		switch(current) {
			case state.left:
				controller.move(-1);
				break;
			case state.right:
				controller.move(1);
				break;
			case state.fly:
				controller.fly(.25f);
				break;
		} 

	}
}
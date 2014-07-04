#pragma strict

import SimpleMovementController;

enum state {idle, left, right, fly};
private var current : state;
private var controller : SimpleMovementController;

public var Speed = 4.0;	
public var JumpPower = 30.0;	

function Start () {
	current = state.idle;
	
	controller = new SimpleMovementController(gameObject, Speed, JumpPower);

}

function OnCollisionEnter2D(collision : Collision2D) {
	controller.OnCollisionEnter2D(collision);
}

function Update () {
	var rand : int = Random.Range(0, 250);
	
	transform.localRotation = Quaternion.identity; 

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
	
	switch(current) {
		case state.left:
			controller.move(-1);
			break;
		case state.right:
			controller.move(1);
			break;
		case state.fly:
			controller.fly(.25);
			break;
	} 

}
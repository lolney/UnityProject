#pragma strict

import SimpleMovementController;

enum state {idle, left, right};
private var current : state;
private var controller : SimpleMovementController;

public var Speed = 4.0;	
public var JumpPower = 30.0;	

function Start () {
	current = state.idle;
	var anim = GetComponent(Animator);
	anim.enabled = false;
	
	controller = new SimpleMovementController(transform, rigidbody2D, anim, Speed, JumpPower);

}

function Update () {
	var rand : int = Random.Range(0, 250);
	 
	switch(rand) {
		case 0:
			print("Switched state to idle");
			current = state.idle;
			break;
		case 1:
			print("Switched state to left");
			current = state.left;
			break;
		case 2:
			print("Switched state to right");
			current = state.right;
			break;
		case 3:
			print("Switched state to jump");
			controller.jump();
			break;
	}
	
	switch(current) {
		case state.left:
			controller.move(-1);
			break;
		case state.right:
			controller.move(1);
			break;
	} 

}
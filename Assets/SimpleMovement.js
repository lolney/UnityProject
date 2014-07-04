#pragma strict

import SimpleMovementController;

public var character : GameObject;
private var anim : Animator;
private var controller : SimpleMovementController;

public var Speed = 4.0;	
public var JumpPower = 10.0;	

function Start () {
	character = GameObject.Find("Main Character");
	anim = character.GetComponent(Animator);
	anim.enabled = false;
	
	controller = new SimpleMovementController(transform, rigidbody2D, anim, Speed, JumpPower);

}

function OnCollisionEnter2D(collision : Collision2D) {
	controller.OnCollisionEnter2D(collision);
}

	
function Update () {

	var axis = Input.GetAxis("Horizontal");
	var axisUp = Input.GetAxis("Vertical");
	
	// If upside down, stop
/*	if(transform.eulerAngles.z > 90 && transform.eulerAngles.z < 270){
		return 0;
	} */
						
	// Move
	controller.move(axis);
	
	// Jump
	if(Input.GetKey (KeyCode.Space)) {
		controller.jump();
	}
	
	if(Input.GetAxis("Vertical") != 0) {
		controller.fly(axisUp);
	}
	
	if(Input.GetKey(KeyCode.Z)) {
		controller.rotate(Mathf.PI / 100.0);
	}
	
	if(Input.GetKey(KeyCode.X)) {
		controller.rotate(-Mathf.PI / 100.0);
	}
	
	transform.localRotation = Quaternion.identity; 
	
	return 0;
		
}


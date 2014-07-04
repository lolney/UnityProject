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
		
	if(collision.contacts.Length > 0)
	{
		var contact = collision.contacts[0];
		
		if(Vector3.Dot(contact.normal, Vector3.up) > 0.5)
		{
			controller.colBelow = true;
			anim.enabled = false;
			
		}
	}
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
	
	return 0;
		
}


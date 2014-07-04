#pragma strict

public var character : GameObject;
private var anim : Animator;

public var Speed = 4.0;	
public var JumpPower = 10.0;	

private var facingRight = true;
private var colBelow = false;

function Start () {
	character = GameObject.Find("Main Character");
	anim = character.GetComponent(Animator);
	anim.enabled = false;
	
}

function OnCollisionEnter2D(collision : Collision2D) {
		
	if(collision.contacts.Length > 0)
	{
		var contact = collision.contacts[0];
		
		if(Vector3.Dot(contact.normal, Vector3.up) > 0.5)
		{
			colBelow = true;
			anim.enabled = false;
			
		}
	}
}
	
function Update () {

	var axis = Input.GetAxis("Horizontal");
	
	// If upside down, stop
/*	if(transform.eulerAngles.z > 90 && transform.eulerAngles.z < 270){
		return 0;
	} */
						
	// Move
	move(axis);
	
	// Jump
	if(Input.GetKey (KeyCode.Space)) {
		jump();
	}
	
	if(Input.GetAxis("Vertical") > 0) {
		fly();
	}
	
	return 0;
		
}

function move(movementAxis : float) {
	if((movementAxis > 0 && facingRight) || (movementAxis < 0 && !facingRight)) 
			flip();

	rigidbody2D.velocity = Vector2(Speed * movementAxis, rigidbody2D.velocity.y);
	/*if(colBelow && move != 0) {
		colBelow = false;
		rigidbody2D.velocity = Vector2(move * Speed, JumpPower/2.0);
	}*/
}
function fly() {
	if(rigidbody2D.velocity.y < JumpPower * 2)
		 rigidbody2D.velocity += JumpPower * transform.up / 10;
	anim.Play("Flying", 0);
	colBelow = false;
	anim.enabled = true;
}

function jump() {
	if(colbelow) {
		colBelow = false;
		rigidbody2D.velocity += Vector2(0, JumpPower);
		anim.Play("Flying", 0);
		anim.enabled = true;
	}
}

function flip() {
	if(facingRight) facingRight = false;
	else facingRight = true;
	
	transform.localScale.x *= -1;
}



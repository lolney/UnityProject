#pragma strict

public var Speed = 4.0;	
public var JumpPower = 10.0;	

private var facingRight = true;

function Start () {

}

function Update () {

	var move = Input.GetAxis("Horizontal");
	
	// If upside down, stop
	if(transform.eulerAngles.z > 90 && transform.eulerAngles.z < 270){
		return 0;
	}
		
	// Flip
	if((move > 0 && facingRight) || (move < 0 && !facingRight)) 
			flip();
				
	// Move
	rigidbody2D.velocity = Vector2(Speed * move, rigidbody2D.velocity.y);
	
	// Jump
	// TODO: replace with collision below check
	if(Input.GetKey (KeyCode.Space) && rigidbody2D.velocity.y < .01 && rigidbody2D.velocity.y > -.01)
		rigidbody2D.velocity += Vector2(0, JumpPower);
		
	return 0;
		
}

function flip() {
	if(facingRight) facingRight = false;
	else facingRight = true;
	
	transform.localScale.x *= -1;
}


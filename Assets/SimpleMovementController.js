#pragma strict

public class SimpleMovementController {

	private var character : GameObject;
	private var anim : Animator; 
	private var facingRight = true;
	public var colBelow = false;
	
	private var Speed : float;	
	private var JumpPower : float;	
	private var transform : Transform;
	private var rigidbody2D : Rigidbody2D;
	
	public var OnCollisionEnter2D = function(collision : Collision2D) {
			
		if(collision.contacts.Length > 0)
		{
			var contact = collision.contacts[0];
			
			if(Vector3.Dot(contact.normal, Vector3.up) > 0.5)
			{
				colBelow = true;
				anim.enabled = false;
				
			}
		}
	};

	function SimpleMovementController(gameobject : GameObject, Speed : float, JumpPower : float) {
		this.transform = gameobject.transform;
		this.rigidbody2D = gameobject.rigidbody2D;
		
		anim = gameobject.GetComponent(Animator);
		anim.enabled = false;
		
		this.Speed = Speed;
		this.JumpPower = JumpPower;
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
	 
	function fly(movementAxis : float) {
		if(rigidbody2D.velocity.y < JumpPower * 2)
			 rigidbody2D.velocity += JumpPower * movementAxis * transform.up / 10;
		anim.Play("Flying", 0);
		colBelow = false;
		anim.enabled = true;
	}

	function jump() {
		if(colBelow) {
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
	
	function rotate(angle : float) {
		transform.RotateAroundLocal(Vector3(0,0,1), angle);
	}

}
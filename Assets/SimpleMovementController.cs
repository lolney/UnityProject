using UnityEngine;
using System.Collections;

public class SimpleMovementController {
		
		private GameObject character;
		private Animator anim; 
		private bool facingRight = true;
		public bool colBelow = false;
		
		private float Speed;	
		private float JumpPower;	
		private Transform transform;
		private Rigidbody2D rigidbody2D;
		
		public void OnCollisionEnter2D(Collision2D collision  ) {
			
			if(collision.contacts.Length > 0)
			{				
				if(Vector3.Dot(collision.contacts[0].normal, Vector3.up) > 0.5f)
				{
					colBelow = true;
					anim.enabled = false;
					
				}
			}
		}
		
		public SimpleMovementController( GameObject gameobject ,   float Speed ,   float JumpPower  ){
			this.transform = gameobject.transform;
			this.rigidbody2D = gameobject.rigidbody2D;
			
			anim = gameobject.GetComponent<Animator>();
			anim.enabled = false;
			
			this.Speed = Speed;
			this.JumpPower = JumpPower;
		}
		
		public void  move ( float movementAxis  ){
			if((movementAxis > 0 && facingRight) || (movementAxis < 0 && !facingRight)) 
				flip();
			
			rigidbody2D.velocity = new Vector2(Speed * movementAxis, rigidbody2D.velocity.y);
			/*if(colBelow && movementAxis != 0) {
				colBelow = false;
				rigidbody2D.velocity = new Vector2(movementAxis * Speed, JumpPower/2.0f);
			}*/
		}
		
		public void  fly ( float movementAxis  ){
			if(rigidbody2D.velocity.y < JumpPower * 2)
				rigidbody2D.velocity += JumpPower * movementAxis * (Vector2)transform.up / 10;
			anim.Play("Flying", 0);
			colBelow = false;
			anim.enabled = true;
		}
		
		public void  jump (){
			if(colBelow) {
				colBelow = false;
				rigidbody2D.velocity += new Vector2(0, JumpPower);
				anim.Play("Flying", 0);
				anim.enabled = true;
			}
		}
		
		public void  flip (){
			if(facingRight) facingRight = false;
			else facingRight = true;
			
			Vector3 temp = transform.localScale;
			temp.x *= -1;
			transform.localScale = temp;
		}
		
		public void  rotate ( float angle  ){
			transform.RotateAroundLocal(new Vector3(0,0,1), angle);
		}
		

}
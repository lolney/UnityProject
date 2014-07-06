using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleMovementController {
		
		private GameObject character;
		private Animator anim; 
		private bool facingRight = true;
		public bool colBelow = false;
		
		private float Speed;	
		private float JumpPower;	
		private Transform transform;
		private Rigidbody2D rigidbody2D;
		
		private List<Node> path;
		private PathFinding pathfind;
		private Vector2 destination;
		private float startTime;
		private Node end;
		
		public void OnCollisionEnter2D(Collision2D collision  ) {
			
			if(collision.contacts.Length > 0)
			{				
				if(Vector3.Dot(collision.contacts[0].normal, Vector3.up) > 0.5f)
				{
					colBelow = true;
					anim.Play("Idle", 0);
					
				}
			}
		}
		
		public SimpleMovementController( GameObject gameobject ,   float Speed ,   float JumpPower  ){
			this.character = gameobject;
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
		
		public void initPath(Node start, Node end) {	
			this.end = end;	
			pathfind = new PathFinding();
			path = pathfind.A_Star(start, end);
			rigidbody2D.velocity = Vector2.zero;
			destination = path[0].center;
			startTime = Time.time;
		}
		
		public int followPath(){
		
			Vector2 current = rigidbody2D.position;
			
			colBelow = false;
			anim.enabled = true;
			
			if(EqualsWithFudge(destination, current)) {
				if(path.Count == 0) {
					WeeWeeAI ai = character.GetComponent<WeeWeeAI>();
					ai.stateOverride = false;
					ai.current = state.idle;
					return 1;
				}
				destination = path[0].center;			
				path.RemoveAt(0);
				startTime = Time.time;				
			}
			else if(Time.time - startTime > 2f){
				startTime = Time.time;
				Node start = PathFinding.findClosestNode(transform);
				path = pathfind.A_Star(start, end);
				path.RemoveAt(0);
				
				Debug.Log("Rerouting to: " + path[0].center);
			}
			
			//Debug.Log("Destination: " + destination);
			//Debug.Log("Current: " + current);
			
			rigidbody2D.velocity = destination - current;
			rigidbody2D.velocity.Normalize();
			rigidbody2D.velocity *= Speed;
			
			float dot = Vector2.Dot(rigidbody2D.velocity, Vector2.right);
			if((facingRight &&  dot > 0) || (!facingRight && dot < 0))
				flip ();
				
			return 0;
			
		}
		
		private bool EqualsWithFudge(Vector2 a, Vector2 b) {
		
			if((a.x < (b.x + 2) && a.x > (b.x - 2)) &&
			   (a.y < (b.y + 2) && a.y > (b.y - 2)))
				return true;
			else return false;
		}
		

}
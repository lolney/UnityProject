using UnityEngine;
using System.Collections;

public class SimpleMovement : MonoBehaviour {


	private Animator anim;
	private SimpleMovementController controller;

	public float speed = 12.0f;	
	public float jumpPower= 10.0f;	

	void  Start (){
		controller = new SimpleMovementController(gameObject, speed, jumpPower);

	}

	void  OnCollisionEnter2D ( Collision2D collision  ){
		controller.OnCollisionEnter2D(collision);
	}

		
	int  Update (){

		float axis= Input.GetAxis("Horizontal");
		float axisUp= Input.GetAxis("Vertical");
		
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
			controller.rotate(Mathf.PI / 100.0f);
		}
		
		if(Input.GetKey(KeyCode.X)) {
			controller.rotate(-Mathf.PI / 100.0f);
		}
		
		transform.localRotation = Quaternion.identity; 
		
		return 0;
			
	}

}
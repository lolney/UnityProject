using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleMovement : MonoBehaviour {


	private Animator anim;
	private SimpleMovementController controller;

	public float speed = 12.0f;	
	public float jumpPower= 10.0f;
	
	private float time = 0;	
	private List<GameObject> arrows;
	
	void  Start (){
		controller = new SimpleMovementController(gameObject, speed, jumpPower);
		time = Time.time;
		arrows = new List<GameObject>();
	}

	void  OnCollisionEnter2D ( Collision2D collision  ){
		controller.OnCollisionEnter2D(collision);
	}

		
	int  Update (){
		
		if(UpdateText.cages <= 0 && transform.position.y > MazeGeneration.gridSize * 10)
			Application.LoadLevel(Application.loadedLevel + 1);
		
		float axis= Input.GetAxis("Horizontal");
		float axisUp= Input.GetAxis("Vertical");
		
		// If upside down, stop
	/*	if(transform.eulerAngles.z > 90 && transform.eulerAngles.z < 270){
			return 0;
		} */
		// Show hint
		
		if(Input.GetKeyDown(KeyCode.M))
			if(UpdateText.hints > 0) showHint();
						
		// Move
		controller.move(axis);
		
		// Jump
		if(Input.GetKey (KeyCode.Space)) {
			controller.jump();
		}
		
		if(Input.GetMouseButton(0)) {
			if(Time.time - time > .2)
				fireNote();
		}
		
		if(Input.GetAxis("Vertical") != 0) {
			controller.fly(axisUp);
		}
				
		transform.localRotation = Quaternion.identity; 
		
		return 0;
			
	}
	
	void fireNote() {
		
		string name;
		if((int)Random.Range(0,2) == 0)
			name = "Note1";
		else 
			name = "Note2";
			
		GameObject note = (GameObject)Instantiate(GameObject.Find(name),transform.position,Quaternion.identity);
		
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 direction = (Vector2)(mousePos - transform.position);
		note.rigidbody2D.velocity = direction * 2;
		
		Color color = note.renderer.material.color;
		color.r = Random.Range(.7f,1f);
		color.g = Random.Range(.7f,1f);
		color.b = Random.Range(.7f,1f);
		note.renderer.material.color = color;
	
		time = Time.time;
		DestroyAfterSeconds.Destroy(note, 1);
	}
	
	 void showHint(){
		
		UpdateText.hints--;
		foreach(GameObject obj in arrows)
			Destroy(obj);
		
		PathFinding pfinder = new PathFinding();
		GameObject scripts = GameObject.Find("Scripts");
		MazeGeneration mg = scripts.GetComponent<MazeGeneration>();
		
		Node[,] map = mg.map;
		Node start = PathFinding.findClosestNode(transform);
		Node end = start.findCage(map);
		if(end == null) 
			end = map[MazeGeneration.gridSize - 2, MazeGeneration.gridSize - 2];
		
		List<Node> path = pfinder.A_Star(start, end);
		
		Node next;
		while(path.Count != 0) {
			Node current = path[0];
			if(path.Count != 1)
				next = path[1];
			else 
				next = end;
			path.RemoveAt(0);
			
			Quaternion rotation = Quaternion.identity; 
			
			if((next.center.x - current.center.x) == 0){	// Vertical bar
				if(next.center.y > current.center.y)	// Adjacent vertex is above
					rotation = Quaternion.AngleAxis(90, new Vector3(0,0,1));
				else 
					rotation = Quaternion.AngleAxis(270, new Vector3(0,0,1));
			}
			if(next.center.y - current.center.y == 0){	// Horizontal bar
				if(next.center.x > current.center.x)	// Adjacent vertex to the right
					rotation = Quaternion.identity;
				else
					rotation = Quaternion.AngleAxis(180, new Vector3(0,0,1));
			}
			
			arrows.Add((GameObject)Instantiate(GameObject.Find("Arrow"), current.center, rotation));
		}
	}
}
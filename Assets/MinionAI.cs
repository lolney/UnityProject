using UnityEngine;
using System.Collections;

public class MinionAI : MonoBehaviour {

	private SimpleMovementController controller;
	public GameObject prefab;
	
	public float speed = 4.0f;
	public state current;
	
	// Use this for initialization
	void Start () {
		controller = new SimpleMovementController(gameObject, speed, 0);
		
		current = state.path;
				
		Node start = PathFinding.findClosestNode(transform.position);
		int x;
		if(transform.position.x > 0)
			x = -1;
		else x = 1;
		Node end = PathFinding.findClosestNode(new Vector3(140 * x,
											WallGeneration.outerOpening + 5));
													
		float t = Time.time;
		controller.initPath(start, end); 
		Debug.Log(Time.time - t);
	}
	
	// Update is called once per frame
	void Update () {
		if(controller.followPath())
			Destroy(gameObject);
	}
}

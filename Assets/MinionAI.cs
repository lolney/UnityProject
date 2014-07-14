using UnityEngine;
using System.Collections;

public enum species {WeeWee, Moose};

public class MinionAI : MonoBehaviour {

	private SimpleMovementController controller;
	public GameObject prefab;
	
	public float speed = 4.0f;
	public state current;
	public species myspecies;
	
	Node8D end;
	
	// Use this for initialization
	void Start () {
		controller = new SimpleMovementController(gameObject, speed, 0);
		
		current = state.path;
				
		Node8D start = (Node8D)PathFinding.findClosestNode(transform.position);
		int x;
		if(transform.position.x > 0){
			myspecies = species.WeeWee;
			x = -1;
		}	
		else{
			x = 1;
			myspecies = species.Moose;
		} 
		end = (Node8D)PathFinding.findClosestNode(new Vector3(140 * x,
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
	
	void reroute(species s) {
		if(myspecies == s){
			Node start = (Node8D)PathFinding.findClosestNode(transform.position);
			controller.initPath(start, end);
			Debug.Log("New end point: " + end.center);
		}
	}
}

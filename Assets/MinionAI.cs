using UnityEngine;
using System.Collections;

public class MinionAI : MonoBehaviour {

	private SimpleMovementController controller;
	
	public float speed = 12.0f;
	public state current;
	
	// Use this for initialization
	void Start () {
		controller = new SimpleMovementController(gameObject, speed, 0);
		
		current = state.path;
		
		// TODO: Generalize Pathfinding for current grid 
		/*
		Node begin = PathFinding.findClosestNode(transform.position);
		Node end = PathFinding.findClosestNode(new Vector3(WallGeneration.innerOpening - WallGeneration.gridSize,
											WallGeneration.outerOpening));
		
		controller.initPath(start, end); */
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

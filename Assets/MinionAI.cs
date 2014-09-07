using UnityEngine;
using System.Collections;

public enum species {WeeWee, Moose};

public class MinionAI : MonoBehaviour {

	private SimpleMovementController controller;
	private TFPlayer player;	// perhaps attached to minion instead
	public GameObject prefab;
	
	public float speed = 4.0f;
	public species myspecies;
	public state current;
		
	// Use this for initialization
	void Start () {
		controller = new SimpleMovementController(gameObject, speed, 0);
		player = new TFPlayer(myspecies);
		
		current = state.path;
				
		Node8D start = (Node8D)PathFinding.findClosestNode(transform.position);
													
		float t = Time.time;
		controller.initPath(start, player.destination); 
		Debug.Log(Time.time - t);
	}
	
	// Update is called once per frame
	void Update () {
		if(controller.followPath()){
			controller.destroyArrows();
			LevelProperties.minions.Remove(gameObject);
			Destroy(gameObject);
		}
	}
	
	void reroute(species s) {
		if(player.myspecies == s){
			Node start = (Node8D)PathFinding.findClosestNode(transform.position);
			controller.initPath(start, player.destination);
			Debug.Log("New end point: " + player.destination.center);
		}
	}
}

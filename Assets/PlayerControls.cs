using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerControls : MonoBehaviour {
	
	public GameObject musicBox;
	public float radius = 10;
	public float multiplier = 10;
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(1)) {
			Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Node center = PathFinding.findClosestNode(pos);
			spreadSlowFromCenter(center, radius, multiplier);
			Instantiate(musicBox, center.center, Quaternion.identity);
			
			foreach(GameObject obj in LevelProperties.minions)
				obj.SendMessage("reroute", species.Moose);
		}
	}
	
	void spreadSlowFromCenter(Node center, float radius, float multiplier){
	
		LevelProperties.map.resetExploration();
		
		Queue<Node> neighbors = center.findNeigbors(true);
		
		while(neighbors.Count != 0){
			Node current = neighbors.Dequeue();
			float distance = center.distance(current);
			if(distance < radius){
				current.slowModifier += multiplier * (radius - distance) + 1;
				current.findNeigbors(neighbors, true);
			}
		}
	}
}

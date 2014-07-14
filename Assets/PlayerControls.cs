using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerControls : MonoBehaviour {
	
	public GameObject musicBox;
	public float radius = 10;
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(1)) {
			Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Node center = PathFinding.findClosestNode(pos);
			spreadSlowFromCenter(center, radius);
			Instantiate(musicBox, center.center, Quaternion.identity);
		}
	}
	
	void spreadSlowFromCenter(Node center, float radius){
	
		LevelProperties.map.resetExploration();
		
		Queue<Node> neighbors = center.findNeigbors(true);
		
		while(neighbors.Count != 0){
			Node current = neighbors.Dequeue();
			float distance = center.distance(current);
			if(distance < radius){
				current.slowModifier += (radius - distance) + 1;
				current.findNeigbors(neighbors, true);
			}
		}
	}
}

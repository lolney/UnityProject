using UnityEngine;
using System.Collections.Generic;

public class PathFinding {
	
	private Node[,] map;
	
	void Start () {
		GameObject scripts = GameObject.Find("Scripts");
		MazeGeneration mg = scripts.GetComponent<MazeGeneration>();
		map = mg.map;
	}
	
	void A_Star() {
		
	}
	
	private float distance(Node n1, Node n2) {
		float deltaX = n1.center.x - n2.center.x;
		float deltaY = n1.center.y - n2.center.y;
		return Mathf.Sqrt((deltaY * deltaY) + (deltaX * deltaX));
	}
	
	
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinding {
	
	public Node[,] map;
	private List<Vector2>[,] maze;
	private int gridSize;
		
	public PathFinding () {
		GameObject scripts = GameObject.Find("Scripts");
		MazeGeneration mg = scripts.GetComponent<MazeGeneration>();
		
		map = mg.map;
		maze = mg.maze;
		gridSize = MazeGeneration.gridSize;
	}
		
	public List<Node> A_Star(Node start, Node end) {
		
		float btwNodes = 10.0f;
		
		Queue<Node> neighbors = new Queue<Node>(); 
		BHeap<Node> considered = new BHeap<Node>();
		Dictionary<Node, Node> navigated = new Dictionary<Node, Node>();
		
		for(int i=0; i< gridSize - 1;i++)
		for(int j=0; j< gridSize - 1;j++){
			map[i,j].accumulated = Mathf.Infinity;
			map[i,j].score = Mathf.Infinity;
		}
		
		
		start.accumulated = 0;
		start.score = start.distance(end);
		considered.insert(start);
		
		while(!considered.isEmpty()) {
			Node current = considered.remove();
			if(current.center.Equals(end.center))
				return constructPath(navigated, current);
			
			neighbors = current.findNeigbors();
			Node neighbor;
			while(neighbors.Count != 0) {
				
				neighbor = neighbors.Dequeue();
				float tentativeScore = btwNodes + current.accumulated;
				if(tentativeScore >= neighbor.accumulated)
					continue;
				
				neighbor.accumulated = tentativeScore;
				neighbor.score = neighbor.accumulated + neighbor.distance(end);
				try{
					navigated.Add(neighbor, current);
				}
				catch(System.ArgumentException) {
					navigated.Remove(neighbor);
					navigated.Add(neighbor, current);
				}				
				
				if(!considered.contains(neighbor))
					considered.insert(neighbor);
				
			}
		}
		
		return new List<Node>();
	}
	
	List<Node> constructPath(Dictionary<Node, Node> navigated, Node current) {
		Node previous;
		List<Node> path;
		
		if(navigated.TryGetValue(current, out previous)) {
			path = constructPath(navigated, previous);
			path.Insert(path.Count, current);
		}
		else {
			path = new List<Node>();
			path.Insert(path.Count, current);
		}
		
		return path;
		
		
	}
	
	public static Node findClosestNode(Transform obj) {
		
		GameObject scripts = GameObject.Find("Scripts");
		MazeGeneration mg = scripts.GetComponent<MazeGeneration>();
		Vector3 mazePos = mg.pos;
		Node[,] map = mg.map;
		
		int yPos = (int)(obj.position.y - (mazePos.y + 5));
		int xPos = (int)(obj.position.x - (mazePos.x + 5));
		
		int y = yPos % 10;
		int x = xPos % 10;
		yPos -= y;
		xPos -= x;
		 
		if(x >= 5) x = 10;
		if(x < 5) x = 0;
		if(y >= 5) y = 10;
		if(y < 5) y = 0;
		
		yPos += y;
		xPos += x;
		
		Debug.Log(xPos + " " + yPos);
		yPos /= 10;
		xPos /= 10;
		Debug.Log(xPos + " " + yPos);
		
		if(xPos < 0) xPos = 0;
		if(yPos < 0) yPos = 0;
		if(xPos > map.GetLength(0)) xPos = map.GetLength(0) - 1;
		if(yPos > map.GetLength(1)) yPos = map.GetLength(1) - 1;
		
		return map[xPos, yPos];
	}
	
	
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinding {
	
	public Node[,] map;
	private int gridSizeX;
	private int gridSizeY;
		
	public PathFinding () {		
		map = LevelProperties.map;
		gridSizeX = LevelProperties.sizeX;
		gridSizeY = LevelProperties.sizeY;
	}
		
	public List<Node> A_Star(Node start, Node end) {
		
		float btwNodes = 10.0f;
		
		Queue<Node> neighbors = new Queue<Node>(); 
		BHeap<Node> considered = new BHeap<Node>();
		Dictionary<Node, Node> navigated = new Dictionary<Node, Node>();
		
		for(int i=0; i< gridSizeX - 1;i++)
		for(int j=0; j< gridSizeY - 1;j++){
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
	
	public static Node findClosestNode(Vector3 position) {
		
		Vector3 mazePos = LevelProperties.origin;
		Node[,] map = LevelProperties.map;
		
		int s = LevelProperties.resolution;
		
		int yPos = (int)(position.y - (mazePos.y + s/2));
		int xPos = (int)(position.x - (mazePos.x + s/2));
		
		int y = yPos % s;
		int x = xPos % s;
		yPos -= y;
		xPos -= x;
		 
		if(x >= s/2) x = s;
		if(x < s/2) x = 0;
		if(y >= s/2) y = s;
		if(y < s/2) y = 0;
		
		yPos += y;
		xPos += x;
		
		Debug.Log(xPos + " " + yPos);
		yPos /= s;
		xPos /= s;
		Debug.Log(xPos + " " + yPos);
		
		if(xPos < 0) xPos = 0;
		if(yPos < 0) yPos = 0;
		if(xPos > map.GetLength(0)) xPos = map.GetLength(0) - 1;
		if(yPos > map.GetLength(1)) yPos = map.GetLength(1) - 1;
		
		return map[xPos, yPos];
	}
	
	
}

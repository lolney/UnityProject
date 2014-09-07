using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinding {
			
	public static List<Node> A_Star(Node start, Node end) {
	
		Node[,] map = LevelProperties.map;
		int gridSizeX = LevelProperties.sizeX;
		int gridSizeY = LevelProperties.sizeY;
				
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
				float tentativeScore = current.calcScoreIncrease() + current.accumulated;
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
	
	/* Need to have separate scores for white eyes, moose */
	public static void dijkstra(Node end, species myspecies){	// end is the destination node; scores radiate outward from it
		Node[,] map;
		LevelProperties.maps.TryGetValue(myspecies, out map);
		int gridSizeX = LevelProperties.sizeX;
		int gridSizeY = LevelProperties.sizeY;
		
		BHeap<Node> unexplored = new BHeap<Node>();
		
		for(int i=0; i< gridSizeX - 1;i++)
		for(int j=0; j< gridSizeY - 1;j++){
			if(!map[i,j].Equals(end)){
				map[i,j].score = Mathf.Infinity;
				map[i,j].previous = null;	// Previous node in shortest path to destination
			}
			else map[i,j].score = 0;
			unexplored.insert(map[i,j]);
		}
		
		while(!unexplored.isEmpty()){
			// Choose node in set with least distance from end
			Node current = unexplored.remove();
			// Update score of neighbors, if (score of current node) + distance(current node, neighbor) <
			// current score
			// Update to neighbor must be reflected in queue (update priority)
			// For a BHeap, the complexity of this operation is O(n + log(n)) = O(n) : search for Node, move up/down tree
			// Other priority queue implementations are more efficient (eg, Fibinocci Heap)
			Queue<Node> neighbors = current.findNeigbors();
			while(neighbors.Count != 0){
				Node neighbor = neighbors.Dequeue();
				float score = current.score + current.distance(neighbor);
				if(score < neighbor.score) {
					neighbor.score = score;
					neighbor.previous = current;
					unexplored.update(neighbor);
				}
			}
		}
		
	}
	
	static List<Node> constructPathDijkstra(Vector3 position, species myspecies){
		Node[,] map;
		LevelProperties.maps.TryGetValue(myspecies, out map);
		Node start = findClosestNode(position, map);
		
		List<Node> path = new List<Node>();
		Node current = start;
		while(current.score != 0){
			path.Insert(path.Count, current);
			current = current.previous;
		}
		path.Insert(path.Count, current);
		
		return path;
		
	}
	
	static List<Node> constructPath(Dictionary<Node, Node> navigated, Node current) {
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
	
	public static List<Node> showHint(Node start, Node end, List<GameObject> arrows){
		
		foreach(GameObject obj in arrows)
			GameObject.Destroy(obj);
		arrows.Clear();
		
		List<Node> path = PathFinding.A_Star(start, end);
		List<Node> result = new List<Node>();
		
		Node next;
		while(path.Count != 0) {
			Node current = path[0];
			if(path.Count != 1)
				next = path[1];
			else 
				next = end;
			
			result.Add(path[0]);
			path.RemoveAt(0);
			
			
			Quaternion rotation = Quaternion.identity; 
			if((next.center.x - current.center.x) != 0 && (next.center.y - current.center.y != 0)){
				if(next.center.y > current.center.y){	// Adjacent vertex is above
					if(next.center.x > current.center.x)	// Adjacent vertex to the right
						rotation = Quaternion.AngleAxis(45, new Vector3(0,0,1));
					else
						rotation = Quaternion.AngleAxis(90+45, new Vector3(0,0,1));
				}
				else {
					if(next.center.x > current.center.x)	// Adjacent vertex to the right
						rotation = Quaternion.AngleAxis(-45, new Vector3(0,0,1));
					else
						rotation = Quaternion.AngleAxis(180+45, new Vector3(0,0,1));
				}
			}
			else if((next.center.x - current.center.x) == 0){	// Vertical bar
				if(next.center.y > current.center.y)	// Adjacent vertex is above
					rotation = Quaternion.AngleAxis(90, new Vector3(0,0,1));
				else 
					rotation = Quaternion.AngleAxis(270, new Vector3(0,0,1));
			}
			else if(next.center.y - current.center.y == 0){	// Horizontal bar
				if(next.center.x > current.center.x)	// Adjacent vertex to the right
					rotation = Quaternion.identity;
				else
					rotation = Quaternion.AngleAxis(180, new Vector3(0,0,1));
			}
			
			GameObject arr = (GameObject)GameObject.Instantiate(GameObject.Find("Arrow"), current.center, rotation);
			arrows.Add(arr);
		}
		
		return result;
	}
	
	public static Node findClosestNode(Vector3 position, Node[,] map = null) {
		
		Vector3 mazePos = LevelProperties.origin;
		if(map == null)
			map = LevelProperties.map;
		
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
		
//		Debug.Log(xPos + " " + yPos);
		yPos /= s;
		xPos /= s;
//		Debug.Log(xPos + " " + yPos);
		
		if(xPos < 0) xPos = 0;
		if(yPos < 0) yPos = 0;
		if(xPos > map.GetLength(0)) xPos = map.GetLength(0) - 1;
		if(yPos > map.GetLength(1)) yPos = map.GetLength(1) - 1;
		
		return map[xPos, yPos];
	}
	
	
}

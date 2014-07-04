using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinding : MonoBehaviour {
	
	private Node[,] map;
	private List<Vector2>[,] maze;
	private int gridSize;
	
	void Start () {
		GameObject scripts = GameObject.Find("Scripts");
		MazeGeneration mg = scripts.GetComponent<MazeGeneration>();
		while(!mg.isDone) {
			StartCoroutine("Wait");
		}
		
		map = mg.map;
		maze = mg.maze;
		gridSize = mg.gridSize;
	}
	
	IEnumerator Wait()
	{
		yield return new WaitForSeconds(1);
	}
	
	void Update () {
		if(Input.GetKey(KeyCode.M)) {
		
			Debug.Log("called2");
			List<Node> path = A_Star(map[0,0], map[gridSize - 2, gridSize - 2]);
			Debug.Log(path.Count);
			
			while(path.Count != 0) {
				Node current = path[0];
				path.RemoveAt(0);
				Instantiate(GameObject.Find("TestBlock"), current.center, Quaternion.identity);
			}
		}
	}
	
	List<Node> A_Star(Node start, Node end) {
		
		float btwNodes = 10.0f;
		
		Queue<Node> neighbors = new Queue<Node>(); 
		BHeap<Node> considered = new BHeap<Node>();
		Dictionary<Node, Node> navigated = new Dictionary<Node, Node>();
		
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
			path.Insert(0, current);
		}
		else {
			path = new List<Node>();
			path.Insert(0, current);
		}
		
		return path;
		
		
	}
	
	
}

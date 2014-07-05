using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MazeGeneration : MonoBehaviour {
	

	public int gridSize = 21;
	private int blockSize = 10;
	
	public Transform Player;
	public GameObject[] blockPrefabs;
	
	public List<Vector2>[,] maze;
	public Node[,] map; 
	public bool isDone = false;

	void  Start (){
		int[,] grid= new int[gridSize,gridSize];
		map = new Node[gridSize-1,gridSize-1];
		
		for(int i=0; i<gridSize; i++)
			for(int j=0; j<gridSize; j++)
				grid[i, j] = Random.Range(0,100);
		
		maze = Prim (grid);
		// Remove upper right piece
		maze[gridSize - 2, gridSize - 1].RemoveRange(0, 1);
		
		createSprites(maze);
		createPath();
		
		isDone = true;
		
	}
	
	void Update () {
		if(Input.GetKey(KeyCode.M)) {
			
			Debug.Log("called2");
			Node end = map[gridSize - 2, gridSize - 2];
			List<Node> path = A_Star(map[0,0], end);
			
			Node next;
			while(path.Count != 0) {
				Node current = path[0];
				if(path.Count != 1)
					 next = path[1];
			 	else 
			 		next = end;
				path.RemoveAt(0);
				
				Quaternion rotation = Quaternion.identity; 
				
				if((next.center.x - current.center.x) == 0){	// Vertical bar
					if(next.center.y > current.center.y)	// Adjacent vertex is above
						rotation = Quaternion.AngleAxis(90, new Vector3(0,0,1));
					else 
						rotation = Quaternion.AngleAxis(270, new Vector3(0,0,1));
				}
				if(next.center.y - current.center.y == 0){	// Horizontal bar
					if(next.center.x > current.center.x)	// Adjacent vertex to the right
						rotation = Quaternion.identity;
					else
						rotation = Quaternion.AngleAxis(180, new Vector3(0,0,1));
				}
				
				Instantiate(GameObject.Find("Arrow"), current.center, rotation);
			}
		}
	}
	
	void createSprites(List<Vector2>[,] maze) {
		int jj = 0;
		Vector3 pos = Player.position;
		for(int a= 0; a < blockPrefabs.Length; a++) {	// Types of block prefabs
			int limit = (a + 1) * (int)(gridSize / blockPrefabs.Length);
			for (int j = jj; j < limit; j++, jj++)			// Segmented along y axis
				for (int i= 0; i < gridSize; i++)				// x axis		
				for(int k= 0; k < maze[i,j].Count; k++) {			// Edges per vertex
					Quaternion rotation = Quaternion.identity; 
					
					if((i - maze[i,j][k].x) == 0){	// Vertical bar
						if(maze[i,j][k].y > j)	// Adjacent vertex is above
							rotation = Quaternion.AngleAxis(90, new Vector3(0,0,1));
						else 
							rotation = Quaternion.AngleAxis(270, new Vector3(0,0,1));
					}
					if((j - maze[i,j][k].y) == 0){	// Horizontal bar
						if(maze[i,j][k].x > i)	// Adjacent vertex to the right
							rotation = Quaternion.identity;
						else
							rotation = Quaternion.AngleAxis(180, new Vector3(0,0,1));
					}
					
					Instantiate(blockPrefabs[a], pos + new Vector3 (i * blockSize, j * blockSize, 0), rotation);
					
					// Change colors as you go up
					/*
					SpriteRenderer sprite = block.GetComponent<SpriteRenderer>();
					sprite.material.color.r = 1 - ((j + .01) / gridSize);
					sprite.material.color.g = 1 - ((j + .01) / gridSize);*/
					
				}
		
		}
	}
	
	List<Vector2>[,] Prim (int[,] grid) {
		
		List<Vector2>[,] VNew = new List<Vector2>[gridSize,gridSize];
		for(int i=0; i<gridSize; i++)
			for(int j=0; j<gridSize; j++)
				VNew[i, j] = null;
		
		BHeap<AdjacencyObject> minHeap = new BHeap<AdjacencyObject>();
		fillInEdges(VNew, minHeap, grid);
		
		// Initialize at center
		int index = gridSize / 2;
		VNew[index, index] = new List<Vector2>();
		addAdjacents(minHeap, grid, index, index);
		
		
		AdjacencyObject edge;
		while(!minHeap.isEmpty()) { // heap is not empty)	
			// Take from heap, add to tree
			do {
				if(minHeap.isEmpty()) 
					return VNew;
				edge = minHeap.remove();
			}
			while(VNew[(int)edge.b.x, (int)edge.b.y] != null); // vertex not yet in list
			
			VNew[(int)edge.b.x, (int)edge.b.y] = new List<Vector2>();	// Put vertex b in list
			VNew[(int)edge.a.x, (int)edge.a.y].Add(edge.b);	// Add vertex b to adj list for vertex a
			
			// Add adjacents for vertex b to the heap
			addAdjacents(minHeap, grid, (int)edge.b.x, (int)edge.b.y);
			
		}
		
		return VNew;
	}
	
	void fillInEdges(List<Vector2>[,] VNew, BHeap<AdjacencyObject> minHeap, int[,] grid) {
		List<Vector2> list;
		int g = (gridSize - 1); 
		// Fill in the left and top sides
		for(int i=0; i < g; i++){
			
			list = new List<Vector2>();
			list.Add(new Vector2(0, i+1));
			VNew[0, i] = list;
			minHeap.insert(new AdjacencyObject(new Vector2(0,i), new Vector2(1,i), grid[1, i]));
			
			list = new List<Vector2>();
			list.Add(new Vector2(i+1, gridSize - 1));
			VNew[i, gridSize - 1] = list;
			minHeap.insert(new AdjacencyObject(new Vector2(i, gridSize - 1), new Vector2(i, gridSize - 2), grid[i, gridSize - 2]));	
		}
		// Fill in the right and bottom sides
		for(int i=g; i > 0; i--){
			
			list = new List<Vector2>();
			list.Add(new Vector2(gridSize - 1, i-1));
			VNew[gridSize - 1, i] = list;
			minHeap.insert(new AdjacencyObject(new Vector2(gridSize - 1, i), new Vector2(gridSize - 2, i), grid[gridSize - 2, i]));
			
			
			list = new List<Vector2>();
			if(i != 1)
				list.Add(new Vector2(i-1, 0));
			VNew[i, 0] = list;	
			minHeap.insert(new AdjacencyObject(new Vector2(i,0), new Vector2(i,1), grid[i, 1]));
			
		}
		
	}
	
	void  addAdjacents ( BHeap<AdjacencyObject> minHeap ,int[ , ] grid,  int i ,   int j  ){
		
		List<Vector2> adjs = adjacent(i, j);
		for(int ii=0; ii<adjs.Count; ii++) {
			int a = (int)adjs[ii].x;
			int b = (int)adjs[ii].y;
			minHeap.insert(new AdjacencyObject(new Vector2(i,j), adjs[ii], grid[a, b]));
		}
		
	}
	
	List<Vector2> adjacent ( int i ,   int j  )   {
		
		List<Vector2> result= new  List<Vector2>();
		
		if(i + 1 < gridSize)
			result.Add(new Vector2(i + 1, j));
		if(j + 1 < gridSize)
			result.Add(new Vector2(i, j + 1));
		if(i - 1 >= 0)
			result.Add(new Vector2(i - 1, j));
		if(j - 1 >= 0)
			result.Add(new Vector2(i, j - 1));
		
		return result;
		
	}
		
	private void createPath() {
		
	//	map = new Node[gridSize-1,gridSize-1];
		Vector2 org = (Vector2)Player.position;
		
		for(int i=0; i<gridSize-1; i++)
			for(int j=0; j<gridSize-1; j++){
				Vector2 center = org + new Vector2((float)blockSize * ((float)i + .5f), (float)blockSize * ((float)j + .5f));
				map[i,j] = new Node(center);
			}
			
		map[0,0].start = true;
		map[gridSize-2,gridSize-2].end = true;
				
		int counter = 0;
		for(int i=0; i<gridSize-1; i++)
			for(int j=0; j<gridSize-1; j++) {
				Vector2 loc = map[i,j].center;
				
				if(!Physics2D.Raycast(loc, Vector2.up, (float)blockSize * 1.3f))
					if(j != gridSize - 2)
						map[i,j].up = map[i, j+1];
						
				if(!Physics2D.Raycast(loc, -Vector2.up, (float)blockSize * 1.3f))
					if(j != 0)
						map[i,j].down = map[i, j-1];
				
				if(!Physics2D.Raycast(loc, -Vector2.right, (float)blockSize * 1.3f))
					if(i != 0)
						map[i,j].left = map[i-1, j];
							
				if(!Physics2D.Raycast(loc, Vector2.right, (float)blockSize * 1.3f))
					if(i != gridSize - 2)
						map[i,j].right = map[i+1, j];
				
				if(map[i,j].findNeigbors().Count == 1){
					if(map[i,j].down == null){
						int rand = (int)Random.Range(0,10);
						if(rand == 0){
							Instantiate(GameObject.Find("Wee Wee NPC"), loc, Quaternion.identity);
							Instantiate(GameObject.Find("BambooCage"), loc, Quaternion.identity);
						}
							
					}
				}
				
			}
	}
	
	List<Node> A_Star(Node start, Node end) {
		
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
				Debug.Log(neighbors.Count);
				
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
	
}

public class Node : System.IComparable<Node> {
	public Vector2 center;
	
	public Node up = null;
	public Node down = null;
	public Node right = null;
	public Node left = null;
	
	public bool start = false;
	public bool end = false;
	
	public float score = Mathf.Infinity;
	public float accumulated = Mathf.Infinity;
	
	public Node(Vector2 center, bool start = false, bool end = false) {
		this.center = center;
		this.start = start;
		this.end = end;
	}
	
	public Queue<Node> findNeigbors() {
		Queue<Node> result = new Queue<Node>();
		
		if(up != null)
			result.Enqueue(up);
		if(down != null)
			result.Enqueue(down);
		if(right != null)
			result.Enqueue(right);
		if(left != null)
			result.Enqueue(left);
			
		return result;
	}
	
	public float distance(Node n2) {
		float deltaX = this.center.x - n2.center.x;
		float deltaY = this.center.y - n2.center.y;
		return Mathf.Sqrt((deltaY * deltaY) + (deltaX * deltaX));
	}
	
	public int CompareTo(Node a) {
		return (int)(score - a.score);
	}
	
	public bool Equals(Node a) {
		return center.Equals(a.center);
	}
	
}

public class AdjacencyObject : System.IComparable<AdjacencyObject> {
	public Vector2 a;
	public Vector2 b;
	public int z;
	
	public AdjacencyObject ( Vector2 a ,   Vector2 b ,   int edge  ){
		this.a = a;
		this.b = b; 
		this.z = edge;
	}
	
	public int CompareTo(AdjacencyObject a) {
		return z - a.z;
	}
	
	public bool Equals(AdjacencyObject a) {
		if(CompareTo(a) == 0) return true;
		else return false;
	}
}

public class BHeap<T> where T : System.IComparable<T>{
	
	private T[] A;
	public int size;
	private int maxSize= 500;
	
	public BHeap (){
		A = new T[maxSize];
		
		for(int i=0; i<maxSize; i++)
			A[i] = default(T);
		
		size = 0;
	}
	
	public bool  isEmpty (){
		if(size <= 0) return true;
		else return false;
	}
	
	public bool  insert ( T elem  ){
		if(size == maxSize) {
			maxSize *= 2;
			T[] B = new T[maxSize];
			A.CopyTo(B, 0);
			A = B;
			
			for(int i=size; i<maxSize; i++)
				A[i] = default(T);
		}
		
		A[size] = elem;
		upHeapify(size);		
		size++;
		
		
		return true;
		
	}
	
	public T  remove (){
		if(size == 0) return default(T);
		T result = A[0];
		
		size--;
		A[0] = A[size];
		A[size] = default(T);	
		
		minHeapify(0);
		
		return result;
	}
	
	public bool contains (T goal) {
		return containsRecursive(goal, 0);
	}
	
	private bool containsRecursive(T goal, int index) {
		if(index >= size) return false;
		if(A[index].Equals(goal)) return true;
		
		if(containsRecursive(goal, (index+1) * 2 - 1)) return true;
		return containsRecursive(goal, (index+1) * 2);		
	}
	
	private int upHeapify ( int i  ){
		if(i == 0) return 0;
		
		int parent = 0;
		
		if(i % 2 == 1) parent = ((i + 1) / 2) - 1;
		else parent = (i / 2) - 1;
		
		int cmp = A[parent].CompareTo(A[i]);
		if(cmp > 0) {
			swap(i, parent);
			upHeapify(parent);
		}
		
		return 0;
	}
	
	private int minHeapify ( int i  ){
		int left= (i+1) * 2 - 1;
		int right= left + 1;
		
		if(left >= maxSize || right >= maxSize)
			return 0;
		
		int smallest= i;
		int cmp;
		
		if(left < size)
			cmp = A[left].CompareTo(A[i]);
		else cmp = 0;	
		if(cmp < 0) // left is less than parent
			smallest = left;
			
		if(right < size)
			cmp = A[right].CompareTo(A[smallest]);
		else cmp = 0;
		if(cmp < 0)
			smallest = right;
			
		if(smallest != i) {
			swap(smallest, i);
			minHeapify(smallest);
		}
		
		return 0;
	}
	
	private void  swap ( int a ,   int b  ){
		T temp = A[a];
		A[a] = A[b];
		A[b] = temp;
	}
	
}

	


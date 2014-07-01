#pragma strict

import System.Collections.Generic;

private var gridSize = 5;

public var Player : Transform;
public var blockPrefabs : GameObject[];
private var blockTypes = new Dictionary.<String, Texture2D>();

function Start () {
	var grid = new int[gridSize,gridSize];
	
	for(var i=0; i<gridSize; i++)
		for(var j=0; j<gridSize; j++)
			grid[i, j] = Random.Range(0,100);
	
	var maze = Prim (grid);
	
	createSprites(maze);
	
}

function createSprites(maze : List.<Vector2>[,]) {

	var pos = Player.position;
	for (var i = 0; i < gridSize; i++)
		for (var j = 0; j < gridSize; j++){
			print("For " + i + j);
			for(var k = 0; k < maze[i,j].Count; k++) {
	            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
	            //cube.AddComponent(Rigidbody2D);
	            if((i - maze[i,j][k].x) == 0)
	            	cube.transform.localScale = Vector3(1,4,1);
            	else
	            	cube.transform.localScale = Vector3(4,1,1);
	            
	            cube.transform.position = pos + Vector3 (maze[i,j][k].x * 5, maze[i,j][k].y * 5, 0);
	            print(maze[i,j][k].x + " " + maze[i,j][k].y);
     		}
     }
     
}

function Prim (grid : int[,]) : List.<Vector2>[,] {
	
	var VNew = new List.<Vector2>[gridSize,gridSize];
	for(var i=0; i<gridSize; i++)
		for(var j=0; j<gridSize; j++)
			VNew[i, j] = null;
			
	var minHeap = BHeap();
	
	// Initialize at 0,0
	VNew[0, 0] = new List.<Vector2>();
	addAdjacents(minHeap, grid, 0, 0);
	
	
	
	while(!minHeap.isEmpty()) { // heap is not empty)	
		// Take from heap, add to tree
		do {
			if(minHeap.isEmpty()) return VNew;
			var edge = minHeap.remove();
		}
		while(VNew[edge.b.x, edge.b.y] != null); // vertex not yet in list
		
		VNew[edge.b.x, edge.b.y] = new List.<Vector2>();	// Put vertex b in list
		VNew[edge.a.x, edge.a.y].Add(edge.b);	// Add vertex b to adj list for vertex a
		
		// Add adjacents for vertex b to the heap
		addAdjacents(minHeap, grid, edge.b.x, edge.b.y);
		
	}
	
	return VNew;
}

function addAdjacents(minHeap : BHeap, grid : int[,], i : int, j : int) {

	var adjs :  List.<Vector2> = adjacent(i, j);
	for(var ii=0; ii<adjs.Count; ii++) {
		var a : int = adjs[ii].x;
		var b : int = adjs[ii].y;
		minHeap.insert(AdjacencyObject(Vector2(i,j), adjs[ii], grid[a, b]));
	}
	
}

function adjacent (i:int, j:int) :  List.<Vector2> {
	
	var result = new  List.<Vector2>();

	if(i + 1 < gridSize)
		result.Add(Vector2(i + 1, j));
	if(j + 1 < gridSize)
		result.Add(Vector2(i, j + 1));
			
	return result;
		
}

function addAdjacent() {

}

private class BHeap {

	private var A : AdjacencyObject[];
	public var size : int;
	private var maxSize = 500;
	private var infinity = AdjacencyObject(Vector2(0,0), Vector2(0,0), 9999);
	
	function BHeap() {
		A = new AdjacencyObject[maxSize];
		
		for(var i=0; i<maxSize; i++)
			A[i] = infinity;
			
		size = 0;
	}
	
	function isEmpty() {
		if(size <= 0) return true;
		else return false;
	}
	
	function insert(elem : AdjacencyObject) {
		if(size == maxSize) {
			maxSize *= 2;
			var B = new AdjacencyObject[maxSize];
			A.CopyTo(B, 0);
			A = B;
			
			for(var i=size; i<maxSize; i++)
				A[i] = infinity;
		}
		
		A[size] = elem;
		upHeapify(size);		
		size++;
		
		
		return true;
			
	}
	
	function remove() {
		if(size == 0) return null;
		var result = A[0];
		
		size--;
		A[0] = A[size];
		A[size] = infinity;	
		
		minHeapify(0);
		
		return result;
	}
	
	private function upHeapify(i : int) : int {
		if(i == 0) return 0;
		
		var parent = 0;
		
		if(i % 2 == 1) parent = ((i + 1) / 2) - 1;
		else parent = (i / 2) - 1;
		
		if(A[parent].z > A[i].z) {
			swap(i, parent);
			upHeapify(parent);
		}
		
		return 0;
	}
	
	private function minHeapify(i : int) : int {
		var left = (i+1) * 2 - 1;
		var right = left + 1;
		
		if(left >= maxSize || right >= maxSize)
			return 0;
		
		var smallest = i;
		
		if(A[left].z < A[i].z)
			smallest = left;
		if(A[right].z < A[smallest].z)
			smallest = right;
		if(smallest != i) {
			swap(smallest, i);
			minHeapify(smallest);
		}
		
		return 0;
	}
	
	private function swap(a : int, b : int) {
		var temp : AdjacencyObject = A[a];
		A[a] = A[b];
		A[b] = temp;
	}
		
}

private class AdjacencyObject {
	public var a : Vector2;
	public var b : Vector2;
	public var z : int;
	
	function AdjacencyObject(a : Vector2, b : Vector2, edge : int) {
		this.a = a;
		this.b = b;
		this.z = edge;
	}
}
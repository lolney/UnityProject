#pragma strict

import System.Collections.Generic;

public var gridSize = 20;

public var Player : Transform;
public var blockPrefabs : GameObject[];
private var blockTypes = new Dictionary.<String, Texture2D>();

function Start () {
	var grid = new int[gridSize,gridSize];
	
	for(var i=0; i<gridSize; i++)
		for(var j=0; j<gridSize; j++)
			grid[i, j] = Random.Range(0,100);
	
	var maze = Prim (grid);
	// Remove upper right piece
	maze[gridSize - 2, gridSize - 1].RemoveRange(0, 1);
	
	createSprites(maze);
	
}

function createSprites(maze : List.<Vector2>[,]) {

	var pos = Player.position;
for (var i = 0; i < gridSize; i++)
		for (var j = 0; j < gridSize; j++){
			print("For " + i + j);
			for(var k = 0; k < maze[i,j].Count; k++) {
			
				var rotation : Quaternion;
				
				if((i - maze[i,j][k].x) == 0){	// Vertical bar
					if(maze[i,j][k].y > j)	// Adjacent vertex is above
						rotation = Quaternion.AxisAngle(Vector3(0,0,1), Mathf.PI / 2);
					else
						rotation = Quaternion.AxisAngle(Vector3(0,0,1), 3 * Mathf.PI / 2);
				}
				if((j - maze[i,j][k].y) == 0){	// Horizontal bar
					if(maze[i,j][k].x > i)	// Adjacent vertex to the right
						 rotation = Quaternion.identity;
					 else
					 	rotation = Quaternion.AxisAngle(Vector3(0,0,1), Mathf.PI);
				}
				
				var block = Instantiate(blockPrefabs[0], pos + Vector3 (i * 10, j * 10, 0), rotation);
				
				// Change colors as you go up
				var sprite : SpriteRenderer = block.GetComponent(SpriteRenderer);
				sprite.material.color.r = 1 - ((j + .01) / gridSize);
				sprite.material.color.g = 1 - ((j + .01) / gridSize);
				
     		}
     }
     
}

function Prim (grid : int[,]) : List.<Vector2>[,] {
	
	var VNew = new List.<Vector2>[gridSize,gridSize];
	for(var i=0; i<gridSize; i++)
		for(var j=0; j<gridSize; j++)
			VNew[i, j] = null;
			
	var minHeap = BHeap();
	fillInEdges(VNew, minHeap, grid);
	
	// Initialize at center
	var index : int = gridSize / 2;
	VNew[index, index] = new List.<Vector2>();
	addAdjacents(minHeap, grid, index, index);
	
	
	
	while(!minHeap.isEmpty()) { // heap is not empty)	
		// Take from heap, add to tree
		do {
			if(minHeap.isEmpty()) 
				return VNew;
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

function fillInEdges(VNew : List.<Vector2>[,], minHeap : BHeap, grid : int[,]) {
	// Fill in the left and top sides
	for(var i=0; i<gridSize - 1; i++){
	
		var list = new List.<Vector2>();
		list.Add(Vector2(0, i+1));
		VNew[0, i] = list;
		minHeap.insert(AdjacencyObject(Vector2(0,i), Vector2(1,i), grid[1, i]));
		
		list = new List.<Vector2>();
		list.Add(Vector2(i+1, gridSize - 1));
		VNew[i, gridSize - 1] = list;
		minHeap.insert(AdjacencyObject(Vector2(i, gridSize - 1), Vector2(i, gridSize - 2), grid[i, gridSize - 2]));	
	}
	// Fill in the right and bottom sides
	for(i=gridSize-1; i > 0; i--){
	
		list = new List.<Vector2>();
		list.Add(Vector2(gridSize - 1, i-1));
		VNew[gridSize - 1, i] = list;
		minHeap.insert(AdjacencyObject(Vector2(gridSize - 1, i), Vector2(gridSize - 2, i), grid[gridSize - 2, i]));
		
		
		list = new List.<Vector2>();
		if(i != 1)
			list.Add(Vector2(i-1, 0));
		VNew[i, 0] = list;	
		minHeap.insert(AdjacencyObject(Vector2(i,0), Vector2(i,1), grid[i, 1]));
			
	}
		
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
	if(i - 1 >= 0)
		result.Add(Vector2(i - 1, j));
	if(j - 1 >= 0)
		result.Add(Vector2(i, j - 1));
			
	return result;
		
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
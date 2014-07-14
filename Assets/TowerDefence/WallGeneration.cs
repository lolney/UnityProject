using UnityEngine;
using System.Collections;

public class WallGeneration : MonoBehaviour {

	public GameObject[] blockPrefabs;
	public static int gridSize = 100;
	public static int inner = -40;
	
	public static int outerOpening;
	public static int innerOpening;
	
	// Use this for initialization
	void Start () {
		
		GenerateWalls();
		
		GenerateMap();
		
	}
	
	void GenerateWalls(){
		int top = gridSize/2;
		int bottom = -gridSize/2;
		
		int doorSize = 10;
		int outer = inner - gridSize;
		
		LevelProperties.resolution = 2;
		LevelProperties.origin = new Vector3(outer - 20, bottom - 20);
		
		// Lower end of door:
		outerOpening = (int)Random.Range(bottom, top - doorSize);
		innerOpening = (int)Random.Range(bottom, top - doorSize);
		
		if(blockPrefabs.Length > 2)
			throw new UnityException("Can add no more than 2 types of blocks");
		
		for(int i=0; i<blockPrefabs.Length; i++){
			
			GameObject obj = blockPrefabs[i];
			
			inner *= (int)Mathf.Pow(-1, i);
			outer *= (int)Mathf.Pow(-1, i);
			
			// Outer wall:
			verticalWall(top, outerOpening + doorSize, outer, obj);
			verticalWall(bottom, outerOpening, outer, obj);
			
			// Inner:
			verticalWall(top, innerOpening + doorSize, inner, obj);
			verticalWall(bottom, innerOpening, inner, obj);
			
			// Upper:
			horizontalWall(outer, inner, top, obj);
			
			// Lower:
			horizontalWall(inner, outer, bottom, obj);
		}
	}
	
	void verticalWall(int top, int bottom, int x, GameObject obj){
	
		Vector3 origin = new Vector3(x, top);
		GameObject wall = (GameObject)Instantiate(obj, origin, Quaternion.identity);
		
		Vector3 temp = wall.transform.localScale;
		temp.y *= top - bottom;
		wall.transform.localScale = temp;
		
	}
	
	void horizontalWall(int left, int right, int y, GameObject obj){
		
		Vector3 origin = new Vector3(left, y);
		Quaternion rotate = Quaternion.AngleAxis(-90, new Vector3(0,0,1));
		
		GameObject wall = (GameObject)Instantiate(obj, origin, rotate);
		
		Vector3 temp = wall.transform.localScale;
		temp.y *= left - right;
		wall.transform.localScale = temp;
	}
	
	void GenerateMap(){
		
		int sizeX = (int)Mathf.Ceil(Mathf.Abs(LevelProperties.resolution * LevelProperties.origin.x * 2));
		int sizeY = (int)Mathf.Ceil(Mathf.Abs(LevelProperties.resolution * LevelProperties.origin.y * 2));
		
		LevelProperties.sizeX = sizeX;
		LevelProperties.sizeY = sizeY;
		
		Node8D[,] map = new Node8D[sizeX,sizeY];
		
		float blockSize = (float)LevelProperties.resolution;
		Vector2 org = (Vector2)LevelProperties.origin;
		
		for(int i=0; i<sizeX; i++)
		for(int j=0; j<sizeY; j++){
			Vector2 center = org + new Vector2(blockSize * ((float)i + .5f), blockSize * ((float)j + .5f));
			map[i,j] = new Node8D(center);
		}
		
		for(int i=0; i<gridSize-1; i++)
		for(int j=0; j<gridSize-1; j++) {
			Vector2 loc = map[i,j].center;
			
			if(!Physics2D.Raycast(loc, Vector2.up, (blockSize * 1.1f)))
				if(j != sizeY - 1)
					map[i,j].up = map[i, j+1];
			
			if(!Physics2D.Raycast(loc, -Vector2.up, (blockSize * 1.1f)))
				if(j != 0)
					map[i,j].down = map[i, j-1];
			
			if(!Physics2D.Raycast(loc, -Vector2.right, (blockSize * 1.1f)))
				if(i != 0)
					map[i,j].left = map[i-1, j];
			
			if(!Physics2D.Raycast(loc, Vector2.right, (blockSize * 1.1f)))
				if(i != sizeX - 1)
					map[i,j].right = map[i+1, j];
			
		   if(map[i,j].right != null && map[i,j].up != null)
		   		map[i,j].up_right = map[i+1, j+1];
	   		
		   if(map[i,j].left != null && map[i,j].up != null)
			   map[i,j].up_left = map[i-1, j+1];
			   
			if(map[i,j].right != null && map[i,j].down != null)
			   map[i,j].down_right = map[i+1, j-1];
			   
			if(map[i,j].left != null && map[i,j].down != null)
			   map[i,j].down_left = map[i-1, j-1];
		   	
		}
		
		LevelProperties.map = map;
	}
}

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
		
		Node[,] map = new Node[sizeX,sizeY];
		
		for(int i=0; i<sizeX; i++)
			for(int j=0; j<sizeY; j++){
			
			}
	}
}

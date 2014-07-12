using UnityEngine;
using System.Collections;

public class WallGeneration : MonoBehaviour {

	public GameObject[] blockPrefabs;
	
	// Use this for initialization
	void Start () {
		
		int gridSize = 100;
		
		int top = gridSize/2;
		int bottom = -gridSize/2;
		
		int doorSize = 10;
		int inner = -40;
		int outer = inner - gridSize;
		
		// Lower end of door:
		int outerOpening = (int)Random.Range(bottom, top - doorSize);
		int innerOpening = (int)Random.Range(bottom, top - doorSize);
		
		
		for(int i=0; i<blockPrefabs.Length; i++){
		
			GameObject obj = blockPrefabs[i];
			
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
	
	// Update is called once per frame
	void Update () {
	
	}
}

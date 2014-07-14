using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterGeneration : MonoBehaviour {
	
	public GameObject[] prefabs;
	private float lastTime;
	
	// Use this for initialization
	void Start () {
		lastTime = Time.time;
		LevelProperties.minions = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		// Generate a character every 5 seconds
		if(Time.time - lastTime > 5){

			for(int i=0; i<prefabs.Length; i++){
			
				int inner = WallGeneration.inner;
				inner *= (int)Mathf.Pow(-1, i);
				Vector3 position = new Vector3(inner, WallGeneration.innerOpening + 4);
				
				LevelProperties.minions.Add((GameObject)Instantiate(prefabs[i], position, Quaternion.identity));
			}
			
			lastTime = Time.time;
			
		}
	}
}

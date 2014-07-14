using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class PlayerControls : MonoBehaviour {
	
	public GameObject musicBox;
	public float radius = 10;
	public float multiplier = 10;
		
	// Update is called once per frame
	void Update () {
		//GameObject.Find("Quad").transform.localScale = new Vector3(2*LevelProperties.sizeX, 2*LevelProperties.sizeY);
		if(Input.GetMouseButtonDown(1)) {
			Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Node center = PathFinding.findClosestNode(pos);
			spreadSlowFromCenter(center, radius, multiplier);
			Instantiate(musicBox, center.center, Quaternion.identity);
			
			generateTexture();
			foreach(GameObject obj in LevelProperties.minions)
				obj.SendMessage("reroute", species.Moose);
		}
	}
	
	void spreadSlowFromCenter(Node center, float radius, float multiplier){
	
		LevelProperties.map.resetExploration();
		
		Queue<Node> neighbors = center.findNeigbors(true);
		
		while(neighbors.Count != 0){
			Node current = neighbors.Dequeue();
			float distance = center.distance(current);
			if(distance < radius){
				current.slowModifier += multiplier * (radius - distance) + 1;
				current.findNeigbors(neighbors, true);
			}
		}
	}
	
	void generateTexture(){
		Texture2D tex = new Texture2D(2*LevelProperties.sizeX, 2*LevelProperties.sizeY);
		for(int i=0; i<LevelProperties.sizeX*2; i++)
		for(int j=0; j<LevelProperties.sizeY*2; j++){
			float slow = LevelProperties.map[i/LevelProperties.resolution, j/LevelProperties.resolution].slowModifier;
			Color c = new Color(0, (slow - 1)/20, 0);
			if(c.a > 0) c.a = 1;
			else c.a = 0;
			tex.SetPixel(i,j,c);
		}
		tex.Apply();
		/*
		SpriteRenderer shader = GameObject.Find("Background").GetComponent<SpriteRenderer>();
		Sprite sprite = Sprite.Create(tex, new Rect(-LevelProperties.sizeX, LevelProperties.sizeY,
		                                            2*LevelProperties.sizeX, 2*LevelProperties.sizeY),
		                                   new Vector2(-LevelProperties.sizeX, LevelProperties.sizeY));
		shader.sprite = sprite;*/
		MeshRenderer shader = GameObject.Find("Quad").GetComponent<MeshRenderer>();
		shader.materials[0].mainTexture = tex;//SetTexture("_MainTex", tex);
		byte[] data = tex.EncodeToPNG();
		File.WriteAllBytes(Application.dataPath + "/text.png", data);
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpdateText : MonoBehaviour {

	static public int cages = 0;
	static public int catCages = 0;
	static public int playerCages = 0;
	static public int hints = 3;
	
	private TextMesh textMesh;
	private float start;
	private float start2 = 0;
	
	void Start(){
		
		textMesh = GetComponent<TextMesh>();
		textMesh.text = "";
		start = Time.time;
		
		var pos = transform.position;
		pos = Camera.main.transform.position;
		pos.y += 3 * Camera.main.orthographicSize / 4;
		transform.position = pos;
		
		renderer.sortingOrder = 110;
		
		
	}
	
	void Update(){
			
		float dif = Time.time - start;
		
		if(cages == 0){
			if(start2 == 0)	start2 = Time.time;
			dif = Time.time - start2;
			showGameOver(dif);
			enable();
		}
		else if(dif < 8 * 2){
			animateStart(dif);
			enable();
		}
		else{
			textMesh.text = "";
			renderer.enabled = false;
			transform.GetChild(0).gameObject.renderer.enabled = false;
		}
			
	}
	
	void enable(){
	
		renderer.enabled = true;
		transform.GetChild(0).gameObject.renderer.enabled = true;
		
		var pos = transform.position;
		pos = Camera.main.transform.position;
		pos.y += 3 * Camera.main.orthographicSize / 4;
		transform.position = pos;	
		
		pos.z = 0;
		transform.GetChild(0).position = pos;
		
	}
	
	void animateStart(float dif){
	
		textMesh.transform.localScale = Vector3.one;
		
		if(dif < 8){
			textMesh.text = "Find the wees wees\n Before the cat does";
		}
		else if(dif < 16) {
			dif -= 8;
			textMesh.text = "Use the mouse\n To fire notes \n At the enemy";
		}
		
		textMesh.transform.localScale *= Mathf.Min(Mathf.Log(dif)/Mathf.Log(10), .15f);
	}
	
	void showGameOver(float dif){
		textMesh.transform.localScale = Vector3.one;
		textMesh.transform.localScale *= Mathf.Min(Mathf.Log(dif)/Mathf.Log(10), .18f);
		if(catCages > playerCages)
			textMesh.text = "You have lost!";
		else if(playerCages > catCages)
			textMesh.text = "You have won!\nFind the exit.";
		else
			textMesh.text = "You tied the cat!\nFind the exit \nto decide the winner.";
	}
	
	void OnGUI () {
		int x = 10;
		int y = 10;
		
		int textHeight = 24;
		string[] fields = {	"Trapped: " + cages,
							"Freed by you: " + playerCages,
							 "Freed by cat: " + catCages
							 };
		string[] fieldsRight = {"Hints remaining: " + hints
		};
		
		int numFields = fields.Length;
		
		int width = 140;
		int spacing = (int)((float)textHeight * 1.5);
		int height = spacing * (numFields + 1);
		int heightLeft = spacing * (fieldsRight.Length);
		
		GUI.Box(new Rect(x,y,width,height), "Wee Wees:");
		
		for(int i=0; i<numFields; i++)
			GUI.Box(new Rect(2*x,y+(i+1)*spacing,width-(2*x),textHeight), fields[i]);
		
		
		GUI.Box(new Rect(Screen.width - width - x,y,width,heightLeft), "");
		
		for(int i=0; i<fieldsRight.Length; i++)
			GUI.Box(new Rect(Screen.width - width,y+i*spacing,width-(2*x),textHeight), fieldsRight[i]);
			
	}
}
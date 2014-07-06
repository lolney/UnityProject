using UnityEngine;
using System.Collections;

public class UpdateText : MonoBehaviour {

	public int cages = 0;
	public int catCages = 0;
	public int playerCages = 0;
	
	public Transform Player;
	
	void OnGUI () {
		int x = 10;
		int y = 10;
		
		int textHeight = 24;
		int numFields = 3;
		
		int width = 130;
		int spacing = (int)((float)textHeight * 1.5);
		int height = spacing * (numFields + 1);
		
		GUI.Box(new Rect(x,y,width,height), "Wee Wees:");
		GUI.Box(new Rect(2*x,y+spacing,width-(2*x),textHeight), "Trapped: " + cages);
		GUI.Box(new Rect(2*x,y+2*spacing,width-(2*x),textHeight), "Freed by you: " + playerCages);
		GUI.Box(new Rect(2*x,y+3*spacing,width-(2*x),textHeight), "Freed by cat: " + catCages);
	}
}
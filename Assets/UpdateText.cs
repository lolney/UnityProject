using UnityEngine;
using System.Collections;

public class UpdateText : MonoBehaviour {


	public Transform Player;

	void  Update (){
		if(Player.eulerAngles.z > 90 && Player.eulerAngles.z < 270)
			guiText.text = "Use Z and X to rotate";
		else guiText.text = "";
	}
}
#pragma strict

public var Player : Transform;

function Update () {
	if(!(Player.eulerAngles.z < 90 || Player.eulerAngles.z > 270))
		guiText.text = "Game Over!!";
}
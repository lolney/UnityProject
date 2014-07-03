#pragma strict

public var Player : Transform;

function Update () {
	
	transform.position.x = Player.position.x;
	transform.position.y = Player.position.y + 2 * Camera.main.orthographicSize / 3;
	
}
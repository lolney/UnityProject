#pragma strict

public var Player : Transform;

function Update () {
	
	var offset = ( 2 * ( Camera.main.orthographicSize / 3) - Player.position.y);
	if(offset < 0) offset = 0;
		
	transform.position.x = Player.position.x;
	transform.position.y = Player.position.y + offset;
	
}
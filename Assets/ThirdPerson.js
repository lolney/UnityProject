#pragma strict

public var Player : Transform;

function Update () {
	
	var offset = (4 * ( Camera.main.orthographicSize / 5) - Player.position.y);
	if(offset < 0) offset = 0;
		
	transform.position.x = Player.position.x;
	transform.position.y = Player.position.y + offset;
	
}
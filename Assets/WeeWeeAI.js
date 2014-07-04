#pragma strict

enum state {idle, left, right};
private var current : state;

function Start () {
	//current = idle;
}

function Update () {
	var rand : int = Random.Range(0, 2000);
	 /*
	switch(rand) {
		case 0:
			current = idle;
			break;
		case 1:
			current = left;
			break;
		case 2:
			current = right;
			break;
		case 3:
			break;
	}
	
	switch(current) {
		case left:
			break;
		case right:
			break;
	} */

}
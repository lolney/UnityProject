#pragma strict

enum state {idle, left, right};
private var current : state;

function Start () {
	current = state.idle;
}

function Update () {
	var rand : int = Random.Range(0, 2000);
	 
	switch(rand) {
		case 0:
			current = state.idle;
			break;
		case 1:
			current = state.left;
			break;
		case 2:
			current = state.right;
			break;
		case 3:
			break;
	}
	
	switch(current) {
		case state.left:
			break;
		case state.right:
			break;
	} 

}
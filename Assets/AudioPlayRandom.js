#pragma strict

public var src : AudioSource[] = new AudioSource[10];
public var frequency : int = 1000;

function Update () {
	var rand : int = Random.Range(0, frequency);
	for(var i=0; i < src.Length; i++)
		if(rand == i) src[i].Play();
}
#pragma strict

public var src : AudioSource[];
public var period : int = 1000;

function Update () {
	var rand : int = Random.Range(0, period);
	for(var i=0; i < src.Length; i++)
		if(rand == i) src[i].Play();
}
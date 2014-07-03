#pragma strict

public var textures : Texture2D[];

public var character : Transform;
public var script : MazeGeneration;
public var transition = 20;

private var shader : Skybox;
private var maxHeight : int;
private var lastSegment = 0;

function Start () {
	shader = GetComponent(Skybox);
	maxHeight = script.gridSize * 10;
	setTextures(0);
	
}

function Update () {

	var seg = ( (character.position.y - (transition / 2.0) ) * textures.Length) / (maxHeight + .00001);
	var segment = Mathf.Floor(seg);
	if(segment < 0) segment = 0;
	if(segment > textures.Length - 1) segment = textures.Length - 1;
	
	var low = ((segment+1) * maxHeight / textures.Length) - 10;
	
	var blend = (character.position.y - low) / 20.0;
	
	shader.material.SetFloat( "_Blend", Mathf.Clamp01(blend));
	
	if(lastSegment != segment)
		setTextures(segment);
	lastSegment = segment;
	
}

function setTextures(segment : int) {
	shader.material.SetTexture( "_FrontTex", textures[segment]);
	
	if(segment != textures.Length - 1)
		shader.material.SetTexture( "_FrontTex2", textures[segment + 1]);
	else
		shader.material.SetTexture( "_FrontTex2", textures[segment]);
}
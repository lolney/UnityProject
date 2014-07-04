using UnityEngine;
using System.Collections;

public class BlendMaterials : MonoBehaviour {


	public Texture2D[] textures;
	
	public Transform character;
	public MazeGeneration script;
	public int transition = 20;
	
	private Skybox shader;
	private int maxHeight;
	private int lastSegment = 0;
	
	void  Start () {
		shader = GetComponent<Skybox>();
		maxHeight = script.gridSize * 10;
		setTextures(0);
		
	}
	
	void  Update (){
	
		float seg = ( ((float)character.position.y - ((float)transition / 2.0f) ) * (float)textures.Length) / ((float)maxHeight + .00001f);
		int segment = Mathf.FloorToInt(seg);
		if(segment < 0) segment = 0;
		if(segment > textures.Length - 1) segment = textures.Length - 1;
		
		int low= ((segment+1) * maxHeight / textures.Length) - 10;
		
		float blend= (character.position.y - low) / 20.0f;
		
		shader.material.SetFloat( "_Blend", Mathf.Clamp01(blend));
		
		if(lastSegment != segment)
			setTextures(segment);
		lastSegment = segment;
		
	}
	
	void  setTextures ( int segment  ){
		shader.material.SetTexture( "_FrontTex", textures[segment]);
		
		if(segment != textures.Length - 1)
			shader.material.SetTexture( "_FrontTex2", textures[segment + 1]);
		else
			shader.material.SetTexture( "_FrontTex2", textures[segment]);
	}
}
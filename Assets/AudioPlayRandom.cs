using UnityEngine;
using System.Collections;

public class AudioPlayRandom : MonoBehaviour {


	public AudioSource[] src;
	public int period = 1000;

	void  Update (){
		int rand = Random.Range(0, period);
		for(int i=0; i < src.Length; i++)
			if(rand == i) src[i].Play();
	}
}
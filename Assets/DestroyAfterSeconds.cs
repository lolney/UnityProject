using UnityEngine;
using System.Collections;

public class DestroyAfterSeconds : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine(destroy());
	}
	
	IEnumerator destroy() {
		yield return new WaitForSeconds(2);
		Debug.Log("Object " + gameObject + " destroyed");
		Destroy (gameObject);
	}
	
}

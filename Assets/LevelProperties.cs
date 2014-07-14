using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelProperties : MonoBehaviour {

	public static Node[,] map;
	public static Vector3 origin;
	public static int resolution;
	public static int sizeX;
	public static int sizeY;
	
	// List of all characters in scene
	public static List<GameObject> minions;
}

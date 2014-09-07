using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelProperties : MonoBehaviour {

	public static Node[,] map;
	public static Vector3 origin;
	public static int resolution;
	public static int sizeX;
	public static int sizeY;
	
	public static List<Player> players;
		
	// List of all characters in scene
	public static List<GameObject> minions;
	
	public static Dictionary<species, Node[,]> maps;
}

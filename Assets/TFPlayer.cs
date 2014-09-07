using UnityEngine;
using System.Collections;

public class TFPlayer
{

	public Node[,] map;
	public species myspecies;
	public int score = 0;
	public Node8D destination;
	public TFPlayer opponent;
	
	public TFPlayer(species myspecies){
		this.myspecies = myspecies;
		
		int x = 0;
		if(myspecies == species.WeeWee)
			x = -1;
		else if(myspecies == species.Moose)
			x = 1;
		destination = (Node8D)PathFinding.findClosestNode(new Vector3(140 * x,
											WallGeneration.outerOpening + 5));
	}
	
}


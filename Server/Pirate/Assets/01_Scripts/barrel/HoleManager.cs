using DummyClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleManager 
{
	Player myPlayer;

	int holeIndex;

	public static HoleManager Instance { get;} = new HoleManager();

	public void BroadCastHole(S_BroadCastHole packet)
	{
		holeIndex = packet.holeNumber;
		Debug.Log($"Hole Num : {holeIndex}");
	}

	public int ReturnIndex()
	{ return holeIndex; }

}

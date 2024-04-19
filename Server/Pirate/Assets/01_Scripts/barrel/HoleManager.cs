using DummyClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleManager 
{
	Player myPlayer;

	int holeIndex;

	public static HoleManager Instance { get;} = new HoleManager();

	public void BroadCastStone(S_BroadCastStone packet)
	{
		holeIndex = packet.StonePosition;
		Debug.Log($"Hole Num : {holeIndex}");
	}

	public int ReturnIndex()
	{ return holeIndex; }

}

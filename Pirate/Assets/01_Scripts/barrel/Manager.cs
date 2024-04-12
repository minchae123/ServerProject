using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] private Transform holeParent;
    [SerializeField] private Hole[] holes;

    private void Start()
    {
        ResetGame();
    }

    public void ResetGame()
    {
        holes = holeParent.GetComponentsInChildren<Hole>();
        for(int i = 0; i < holes.Length; i++)
        {
            holes[i].ResetGame();
        }

        int random = Random.Range(0, holes.Length);
        holes[random].SetBoom();
    }
}

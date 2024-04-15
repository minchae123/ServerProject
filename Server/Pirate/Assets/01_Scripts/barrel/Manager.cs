using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager Instance;

    [SerializeField] private Transform holeParent;
    [SerializeField] private Hole[] holes;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ResetGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetGame();
        }
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

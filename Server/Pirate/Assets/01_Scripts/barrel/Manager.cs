using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Manager : MonoBehaviour
{
    public static Manager Instance;

    [SerializeField] private Transform holeParent;
    [SerializeField] private Hole[] holes;

    [SerializeField] private GameObject pirate;

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

    public void SetPirateBoom()
    {
        pirate.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 10f, 0), ForceMode.VelocityChange);
    }
}

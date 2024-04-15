using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    public bool IsBoom = false;

    public void SetBoom()
    {
        IsBoom = true;
    }

    public void ResetGame()
    {
        IsBoom = false;
    }
}

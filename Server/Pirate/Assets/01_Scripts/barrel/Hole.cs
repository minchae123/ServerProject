using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Hole : MonoBehaviour
{
    [SerializeField] private GameObject knifePref;
    private Vector3 knifeOriginalTransform;

    public bool IsSelected = false;
    public bool IsBoom = false;

    private void Start()
    {
        knifeOriginalTransform = knifePref.transform.position;
    }

    public void SetBoom()
    {
        IsBoom = true;
    }

    public void ResetGame()
    {
        IsBoom = false;
        IsSelected = false;
        knifePref.SetActive(false);

        knifePref.GetComponent<MeshRenderer>().material.color = Random.ColorHSV();
        knifePref.transform.position = knifeOriginalTransform;
    }

    public void SetSelected()
    {
        IsSelected = true;
        knifePref.SetActive(true);
        knifePref.transform.DOLocalMoveX(-0.85f, 0.3f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DummyClient;
using System;
using Random = UnityEngine.Random;
public class Hole : MonoBehaviour
{
    [SerializeField] private GameObject knifePref;
    private Vector3 originVec = new Vector3(-2f, 0, 0);

    public bool IsSelected = false;
    public bool IsBoom = false;

    public int numOfList;

    public void SetBoom()
    {
        IsBoom = true;
    }

    public void ResetGame(int i)
    {
        numOfList = i;

        IsBoom = false;
        IsSelected = false;
        knifePref.SetActive(false);

        knifePref.GetComponent<MeshRenderer>().material.color = Random.ColorHSV();
        knifePref.transform.localPosition = originVec;
    }

    public void SetSelected()
    {
        if(IsSelected == false)
        {
            IsSelected = true;
            knifePref.SetActive(true);
            knifePref.transform.DOLocalMoveX(-0.85f, 0.3f).OnComplete(() => CheckIsBoom());

            Manager.Instance.SetIndex(numOfList);
            Manager.Instance.HoleSelect(numOfList);
		    print(numOfList);
        }
    }

	public void CheckSelected()
    {
        knifePref.SetActive(true);
        knifePref.transform.localPosition = new Vector3(-0.85f, 0, 0);
    }

    private void CheckIsBoom()
    {
        if (IsBoom) // 터지는 거면
        {
            print("팡");
            Manager.Instance.SetPirateBoom();
        }
        /*else
        {
            print("다음사람");
        }*/
    }
}

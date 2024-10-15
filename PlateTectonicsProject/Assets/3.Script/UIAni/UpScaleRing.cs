using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UpScaleRing : MonoBehaviour
{
    private void Start()
    {
        transform.DOScale(1, 3).OnComplete(() =>  Destroy(gameObject));
    }

}

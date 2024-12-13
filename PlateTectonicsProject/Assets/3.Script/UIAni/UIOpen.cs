using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIOpen : MonoBehaviour
{
    [SerializeField]
    private Transform tartgetTransform;

    private Vector3 originScale;
    private Vector3 origintrasform;

    private void Awake()
    {
       //originScale = transform.localScale;
        origintrasform = transform.localPosition;
        transform.localScale = Vector3.zero;
    }
    private void OnEnable()
    {
        transform.DOScale(1.15f, 0.3f).SetEase(Ease.OutCirc).OnComplete(() => { AABBCollisionResolve.Instance.CheckOverLab(); });
        //transform.DOMove(tartgetTransform.position, 0.3f).SetEase(Ease.OutCirc).OnComplete(() =>  AABBCollisionResolve.Instance.CheckOverLab());
    }
    private void OnDisable()
    {
        transform.localPosition = origintrasform;
        transform.localScale = Vector3.zero;
    }

}

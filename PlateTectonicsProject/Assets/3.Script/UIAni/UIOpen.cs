using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIOpen : MonoBehaviour
{
    [SerializeField]
    private Transform tartgetTransform;

    private Vector3 originScale;

    private void Start()
    {
       originScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }
    private void OnEnable()
    {
        transform.DOScale(0.9f, 0.3f).SetEase(Ease.OutCirc).OnComplete(() => { AABBCollisionResolve.Instance.CheckOverLab(); });
        //transform.DOMove(tartgetTransform.position, 0.3f).SetEase(Ease.OutCirc).OnComplete(() =>  AABBCollisionResolve.Instance.CheckOverLab());
    }
    private void OnDisable()
    {
        transform.localScale = Vector3.zero;
        //transform.localPosition = Vector3.zero;
    }

}

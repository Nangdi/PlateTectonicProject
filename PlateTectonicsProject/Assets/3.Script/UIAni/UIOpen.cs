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
    }
    private void OnEnable()
    {
        transform.DOScale(0.9f, 0.3f).SetEase(Ease.OutCirc);
        transform.DOMove(tartgetTransform.position, 0.3f).SetEase(Ease.OutCirc);
    }
    private void OnDisable()
    {
        transform.localScale = Vector3.zero;
        transform.localPosition = Vector3.zero;
    }

}

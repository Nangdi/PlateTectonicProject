using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIScalePulse : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Image Image;
    public float fadeValue;

    private Vector3 pulseScale = Vector3.one;
    void Start()
    {
        Image.DOFade(0, 0);
        transform.DOScale(pulseScale, 2).SetLoops(-1, LoopType.Yoyo);
        Image.DOFade(1, 1.5f).SetLoops(-1, LoopType.Yoyo);
    }

}

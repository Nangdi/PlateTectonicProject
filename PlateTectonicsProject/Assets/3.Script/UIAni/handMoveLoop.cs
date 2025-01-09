using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class handMoveLoop : MonoBehaviour
{
    // Start is called before the first frame update
    public Tween moveTween;
    public float moveDistance;
    public float moveCycle;
    public Vector3 dir;
    void Start()
    {
        SetDirMoveArrow(dir);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetDirMoveArrow(Vector3 dir)
    {
        moveTween = transform.DOLocalMove(transform.localPosition + dir * moveDistance, moveCycle).SetLoops(-1, LoopType.Restart);
    }
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccentArrowMove : ArrowMove
{
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        if(moveTween != null)
        {
            moveTween.Kill();
        }
    }
    
}

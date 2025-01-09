using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

public class ArrowMove : MonoBehaviour
{

    private RectTransform rectTransform;
    public Tween moveTween;
    public float moveDistance = 15f;
    public float moveCycle ;
    public bool isAutoDir = true;
    public Vector3 dir = Vector2.right;
    private Vector3 originPos;
    protected virtual void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originPos = transform.localPosition;
        //if(moveTween != null)
        //{
        //    DOTween.Kill(moveTween);
        //}
        if (isAutoDir)
        {
            AutoMoveArrow();
        }
        else
        {
            SetDirMoveArrow(dir);
        }


    }
    private void OnEnable()
    {

        //if (moveTween != null)
        //{

        //    moveTween.Play();

        //}
    }
  
    public void AutoMoveArrow()
    {
        // Z 축 회전 값 가져오기 (EulerAngles로 접근)
        float zRotation = transform.eulerAngles.z;

        // Z 회전 값을 라디안으로 변환 (Trig 함수는 라디안을 사용)
        float radians = zRotation * Mathf.Deg2Rad;

        // 화살표가 가리키는 방향 계산 (단위 벡터)
        Vector3 direction = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0);
        transform.localPosition = originPos;

        // DoLocalMove를 통해 해당 방향으로 이동
        moveTween = transform.DOLocalMove(transform.localPosition + direction * moveDistance, moveCycle).SetLoops(-1, LoopType.Restart);

    }
    public void SetDirMoveArrow(Vector3 dir)
    {
        moveTween = transform.DOLocalMove(transform.localPosition + dir * moveDistance, moveCycle).SetLoops(-1, LoopType.Restart);
    }
    //private void OnDisable()
    //{
    //    moveTween.Pause();
    //}
}

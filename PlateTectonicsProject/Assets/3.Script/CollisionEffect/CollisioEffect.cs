using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CollisioEffect : MonoBehaviour
{
    public enum Direction
    {
        Left =-1,
        Right = 1
    }
    public Direction dir;
    [SerializeField]
    private Transform tartgetPos;
    private Sequence sequence;
    private Tween collision;
    private Tween upDownMove;
    private bool Istrigger = false;
    private Vector3 originPos;
    private void OnEnable()
    {
        sequence = DOTween.Sequence();
        sequence.Pause();
        //sequence.Append(collision);
        if (originPos != Vector3.zero)
        {
            transform.localPosition = originPos;
        }
        GetComponent<Image>().DOFade(1, 0);
        ;
        sequence
            .Append(collision)
            .AppendInterval(2f)
            .AppendCallback(() => EffectPlay())
            .Append(upDownMove)
            .AppendCallback(() => { GetComponent<Image>().DOFade(0, 3); }) // 첫 번째 객체 페이드 아웃
            ;
        //애니메이션 진행 타이밍이 원하는대로 진행이안됌
        //disEable연속으로하면 망가짐
    }
    private void Start()
    {
        //sequence.Append(objectA.DOMoveX(objectA.position.x + moveDistance, moveDuration).SetEase(Ease.Linear))
        //       .Join(objectB.DOMoveX(objectB.position.x - moveDistance, moveDuration).SetEase(Ease.Linear))
        //       .AppendInterval(effectDelay) // 0.5초의 딜레이
        //       .AppendCallback(() => EffectPlay()) // 이펙트 호출
        //       .Append(objectA.DOMoveY(objectA.position.y + 30, moveDuration).SetEase(Ease.OutCirc)) // 첫 번째 객체 위로 이동
        //       .Join(objectB.DOMoveY(objectB.position.y - 30, moveDuration).SetEase(Ease.OutCirc)) // 두 번째 객체 아래로 이동
        //       .AppendInterval(moveDuration) // 모든 객체가 움직이는 동안 대기
        //       .Append(objectA.GetComponent<CanvasGroup>().DOFade(0, fadeDuration)) // 첫 번째 객체 페이드 아웃
        //       .Join(objectB.GetComponent<CanvasGroup>().DOFade(0, fadeDuration)) // 두 번째 객체 페이드 아웃
        //       .AppendCallback(() =>
        //       {
        //           // 페이드 아웃이 끝난 후 새로운 객체를 페이드 인
        //           newObjectCanvas.alpha = 0; // 초기 투명도 설정
        //           newObjectCanvas.DOFade(1, fadeDuration); // 새로운 객체 페이드 인  
        //       });

        // sequence에 추가
       
        //초기화할것
        //1. 지각판위치
        //2. 알파값

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //transform.DOMove(tartgetPos.position, 3f).SetLoops(5, LoopType.Restart);                  
            if (originPos == Vector3.zero/* && !sequence.IsPlaying()*/)
            {
                originPos = transform.localPosition;
            }
            collision = transform.DOMove(tartgetPos.position, 3f);
            float targetY = transform.position.y + ((int)dir * 30);
            upDownMove = transform.DOMoveY(targetY, 3).SetEase(Ease.InExpo);
            sequence.Play();
        }
    }
    public void EffectPlay()
    {
     
        Debug.Log("이펙트");
     
    }
    private void OnDisable()
    {

        if (sequence.IsPlaying())
        {
            sequence.Kill();
        }
    }
}

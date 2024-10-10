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
    [SerializeField]
    private PlusButton plusButton;
    public Direction dir;
    [SerializeField]
    private Transform tartgetPos;
    [SerializeField]
    private Image resultPlate;
    [SerializeField]
    private Image arrowImage;
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
        resultPlate.DOFade(0,0);
        arrowImage.DOFade(1, 0);

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
            PlaySimulator();
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
    private void InitSequence(PlusButton.HandAction handAction)
    {
        switch (handAction)
        {
            case PlusButton.HandAction.ZoomOut:

                //collision = transform.DOMove(tartgetPos.position, 3f);
                float targetY = transform.position.y + ((int)dir * 30);
                //upDownMove = transform.DOMoveY(targetY, 1f).SetEase(Ease.InExpo);

                sequence
               .Append(transform.DOMove(tartgetPos.position, 3f))
               //.AppendInterval(1f)
               .AppendCallback(() => EffectPlay())
               //.AppendInterval(1f)
               //.Append(upDownMove)
               .AppendCallback(() => { GetComponent<Image>().DOFade(0, 3); }) // 첫 번째 객체 페이드 아웃
               .AppendCallback(() => resultPlate.DOFade(1, 3))
               .AppendCallback(() => arrowImage.DOFade(0,3))
               .AppendInterval(3f)
               .AppendCallback(() => plusButton.PlayVideo());
                
                break;
            case PlusButton.HandAction.ZoomIn:
                sequence
                .Append(transform.DOMoveX(transform.position.x + ((int)dir * 30), 3))
                .AppendCallback(() => { GetComponent<Image>().DOFade(0, 3); }) // 첫 번째 객체 페이드 아웃
                .AppendCallback(() => resultPlate.DOFade(1, 3))
                .AppendCallback(() => arrowImage.DOFade(0, 3))
                .AppendInterval(3f)
                .AppendCallback(() => plusButton.PlayVideo());
                break;
            case PlusButton.HandAction.HandsMoveUpDown:
                Debug.Log("시뮬온");
                sequence
                .Append(transform.DOMoveX(transform.position.x + ((int)dir *17), 3))
                .Join(transform.DOMoveY(transform.position.y - ((int)dir * 7), 3))
                .AppendCallback(() => { GetComponent<Image>().DOFade(0, 3); }) // 첫 번째 객체 페이드 아웃
                .AppendCallback(() => resultPlate.DOFade(1, 3))
                .AppendCallback(() => arrowImage.DOFade(0, 3))
                .AppendInterval(3f)
                .AppendCallback(() => plusButton.PlayVideo());
                break;
        }
    }
    public void PlaySimulator()
    {
        if (originPos == Vector3.zero/* && !sequence.IsPlaying()*/)
        {
            originPos = transform.localPosition;
        }

        InitSequence(plusButton.handAction);

        sequence.Play();
    }
}
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccentControll : MonoBehaviour
{
    [SerializeField]
    private PlusButton plusBtn;

    [SerializeField]
    private Image accentPanel;
    [SerializeField]
    public GameObject cir;
    [SerializeField]
    private AccentArrowMove arrow1;
    [SerializeField]
    private AccentArrowMove arrow2;
    //[SerializeField]
    //private GameObject arrow2;
    private Vector3 originScale;
    private Vector3 targetScale;
    public Sequence accentSequence; // Sequence를 저장할 변수

    private void Awake()
    {
        //arrow 기본크기 저장
        originScale = cir.transform.localScale;
        targetScale = originScale * 2f;
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        //accentPanel 투명 0 초기화
        // arrow 원래크기로 초기화
        accentPanel.DOFade(0f, 0f);
        //cir.transform.localScale = cir.transform.localScale = originScale;
        //PlayAccent();
    }
    //판넬 페이드인 아웃
    //화살표 크기키우기
    public void PlayAccent()
    {
        gameObject.SetActive(true);
        accentSequence = DOTween.Sequence();
        accentSequence/*.Append(cir.transform.DOScale(targetScale, 1))*/
            .Join(DOVirtual.DelayedCall(0, () => arrow1.AutoMoveArrow()))
            .Join(DOVirtual.DelayedCall(0, () => arrow2.AutoMoveArrow()))
            .Join(accentPanel.DOFade(1f, 1f).SetLoops(300, LoopType.Yoyo))
            .Insert(3f, DOVirtual.DelayedCall(0, () => CompleteAni()));//
    }
    public void StopAccent()
    {
        if (arrow1.moveTween != null && arrow2.moveTween != null)
        {
            arrow1.moveTween.Kill();
            arrow2.moveTween.Kill();
        }
        if (accentSequence != null)
        {
            accentSequence.Kill(false);
            gameObject.SetActive(false); // 원하는 동작 수행
        }
    }
    private void CompleteAni()
    {
        //if (arrow1.moveTween != null && arrow2.moveTween != null)
        //{
        //    arrow1.moveTween.Kill();
        //    arrow2.moveTween.Kill();
        //}
        //Debug.Log("온컴플리트");
        //gameObject.SetActive(false);
        //todo 화살표 Acrive 켜주기
      
        plusBtn.SetExplanationUI(true);
        //plusBtn.currentCursor.cursorImage.DOFade(0, 0);
    }
    private void OnDisable()
    {
        //cir.transform.DOScale(1, 1);
    }


}

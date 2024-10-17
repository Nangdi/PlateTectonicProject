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
    private GameObject arrow1;
    //[SerializeField]
    //private GameObject arrow2;
    private Vector3 originScale;
    private Vector3 targetScale;
    private Sequence accentSequence; // Sequence를 저장할 변수

    private void Awake()
    {
        //arrow 기본크기 저장
        originScale = arrow1.transform.localScale;
        targetScale = originScale * 2f;
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        //accentPanel 투명 0 초기화
        // arrow 원래크기로 초기화
        accentPanel.DOFade(0f, 0f);
        arrow1.transform.localScale = arrow1.transform.localScale = originScale;
        //PlayAccent();
    }
    //판넬 페이드인 아웃
    //화살표 크기키우기
    public void PlayAccent()
    {
        gameObject.SetActive(true);
        accentSequence = DOTween.Sequence();
        accentSequence.Append(arrow1.transform.DOScale(targetScale, 1))
            //.Join(arrow2.transform.DOScale(targetScale, 1))
            .Join(accentPanel.DOFade(1f, 1f).SetLoops(6, LoopType.Yoyo))
            .OnComplete(() => CompleteAni());
    }
    public void StopAccent()
    {
        if (accentSequence != null)
        {
            accentSequence.Kill(false);
            gameObject.SetActive(false); // 원하는 동작 수행
        }
    }
    private void CompleteAni()
    {
        Debug.Log("온컴플리트");
        gameObject.SetActive(false);
        //todo 화살표 Acrive 켜주기
      
        plusBtn.SetExplanationUI(true);
    }
    private void OnDisable()
    {
        arrow1.transform.DOScale(1, 1);
    }


}

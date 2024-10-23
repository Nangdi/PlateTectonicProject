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
    public Sequence accentSequence; // Sequence�� ������ ����

    private void Awake()
    {
        //arrow �⺻ũ�� ����
        originScale = cir.transform.localScale;
        targetScale = originScale * 2f;
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        //accentPanel ���� 0 �ʱ�ȭ
        // arrow ����ũ��� �ʱ�ȭ
        accentPanel.DOFade(0f, 0f);
        //cir.transform.localScale = cir.transform.localScale = originScale;
        //PlayAccent();
    }
    //�ǳ� ���̵��� �ƿ�
    //ȭ��ǥ ũ��Ű���
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
            gameObject.SetActive(false); // ���ϴ� ���� ����
        }
    }
    private void CompleteAni()
    {
        //if (arrow1.moveTween != null && arrow2.moveTween != null)
        //{
        //    arrow1.moveTween.Kill();
        //    arrow2.moveTween.Kill();
        //}
        //Debug.Log("�����ø�Ʈ");
        //gameObject.SetActive(false);
        //todo ȭ��ǥ Acrive ���ֱ�
      
        plusBtn.SetExplanationUI(true);
        //plusBtn.currentCursor.cursorImage.DOFade(0, 0);
    }
    private void OnDisable()
    {
        //cir.transform.DOScale(1, 1);
    }


}

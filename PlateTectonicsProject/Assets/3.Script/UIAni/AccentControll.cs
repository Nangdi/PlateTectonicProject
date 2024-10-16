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
    [SerializeField]
    private GameObject arrow2;
    private Vector3 originScale;
    private Vector3 targetScale;
    private Sequence accentSequence; // Sequence�� ������ ����

    private void Awake()
    {
        //arrow �⺻ũ�� ����
        originScale = arrow1.transform.localScale;
        targetScale = originScale * 1.5f;
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        //accentPanel ���� 0 �ʱ�ȭ
        // arrow ����ũ��� �ʱ�ȭ
        accentPanel.DOFade(0f, 0f);
        arrow1.transform.localScale = arrow2.transform.localScale = originScale;
        //PlayAccent();
    }
    //�ǳ� ���̵��� �ƿ�
    //ȭ��ǥ ũ��Ű���
    public void PlayAccent()
    {
        gameObject.SetActive(true);
        accentSequence = DOTween.Sequence();
        accentSequence.Append(arrow1.transform.DOScale(targetScale, 1))
            .Join(arrow2.transform.DOScale(targetScale, 1))
            .Join(accentPanel.DOFade(0.45f, 1f).SetLoops(6, LoopType.Yoyo))
            .OnComplete(() => CompleteAni());
    }
    public void StopAccent()
    {
        if (accentSequence != null)
        {
            accentSequence.Kill(false);
            gameObject.SetActive(false); // ���ϴ� ���� ����
        }
    }
    private void CompleteAni()
    {
        Debug.Log("�����ø�Ʈ");
        gameObject.SetActive(false);
        //todo ȭ��ǥ Acrive ���ֱ�

        plusBtn.SetExplanationUI(true);
    }


}

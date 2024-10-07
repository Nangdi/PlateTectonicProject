using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuidePanel : MonoBehaviour
{
    [SerializeField]
    private LeapMouseCursor playerCursor;
    //0 : ��������ó�� ���� �־� ü���غ�����
    //1 : ���� ������ ��ư�� �����غ�����
    //2 : �ùķ��̼��� �����غ�����
    //3 : ���� ���������� ������ ������ ������ �˾ƺ�����
    //0 1 2 3 = 1P / 4 5 6 7 = 2P
    [SerializeField]
    private GameObject[] UISequence;
    [SerializeField]
    //private void Start()
    //{
    //    for (int i = 0; i < transform.childCount; i++)
    //    {
    //        UISequence[i] = transform.GetChild(i).gameObject;
    //    }
    //}
    public void UpdateGuideText()
    {
        switch (playerCursor.actionState)
        {
            case ActionState.Idle:
                SetGuideUI(0);
                break;
            case ActionState.playback:
                SetGuideUI(1);
                break;
            case ActionState.Select:
                SetGuideUI(2);
                break;
            case ActionState.Off:
                SetGuideUI(0);
                break;
        }
    }
    public void SetGuideUI(int index)
    {
        if (UISequence[index].activeSelf) return;

        for (int i = 0; i < UISequence.Length; i++)
        {
            UISequence[i].SetActive(false);
        }
            UISequence[index].SetActive(true);

    }
}
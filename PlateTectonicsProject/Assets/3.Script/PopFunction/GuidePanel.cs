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
    //2 : ���� ���������� ������ ������ ������ �˾ƺ�����
    //3 : �ùķ��̼��� �����غ����� > <
    //4 : �ùķ��̼��� �����غ����� < >
    //5 : �ùķ��̼��� �����غ����� ^ v
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
                SetGuideUI(1);
                break;
            case ActionState.playback:
                SetGuideUI(2);
                break;
            case ActionState.Select:

                SetGuideUI(GetHandType());
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
    private int GetHandType()
    {
        switch (playerCursor.lastbtn.handAction)
        {
            case PlusButton.HandAction.ZoomOut:
                return 3;
            case PlusButton.HandAction.ZoomIn:
                return 4;
            case PlusButton.HandAction.HandsMoveUpDown:
                return 5;
        }
        return 0;
    }
}
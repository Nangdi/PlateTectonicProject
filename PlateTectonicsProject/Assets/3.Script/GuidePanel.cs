using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuidePanel : MonoBehaviour
{
    [SerializeField]
    private LeapMouseCursor playerCursor;
    [SerializeField]
    private TextMeshProUGUI text;

    public void UpdateGuideText()
    {
        switch (playerCursor.actionState)
        {
            case ActionState.Idle:
                text.text = "���� ������ ��ư�� �����غ�����";

                break;
            case ActionState.playback:
                text.text = "���� ���������� ������ ������ ������ �˾ƺ�����";
                break;
            case ActionState.Select:
                text.text = "�ùķ��̼��� �����غ�����";
                break;
            case ActionState.Off:
                text.text = "��������ó�� ���� �־� ü���غ�����";
                break;
        }
    }
}
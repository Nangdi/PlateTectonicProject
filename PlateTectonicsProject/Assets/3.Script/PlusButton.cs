using Leap;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlusButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public enum ButtonState
    {
        Idle =0,
        OntheMouse,
        ClickButton

    }
    //������ ���� ��ȣ�ۿ� ��� ���� = closer �߻� = Apart
    public enum HandAction
    {
        ZoomOut,     // �հ� ���� ������� ��
        ZoomIn,      // �հ� ���� �־��� ��
        HandsMoveUpDown,    // ���� ���Ʒ��� �־�����

    }
    public HandAction handAction;
    public ButtonState state;
    public GameObject nameUI;
    public GameObject ExplanationUI;
    public List<LeapMouseCursor> players = new List<LeapMouseCursor>();
    public LeapMouseCursor currentCursor;
    public RawImage video;
    private void OnEnable()
    {
        //state = ButtonState.Idle;
    }
    private void Update()
    {
        switch (state)
        {
            case ButtonState.Idle:
                if (nameUI.activeSelf)
                {
                    SetNameUI(false);
                }
                break;
            case ButtonState.OntheMouse:
                if (!nameUI.activeSelf)
                {
                    SetNameUI(true);
                }
                break;
            case ButtonState.ClickButton:
                break;
        }
    }

   //��ưŬ�������� ClickButton���� ����
   //Exit�Ҷ� state �� ClickButton�����̸� Idle�� �������� �ʴ´�.
   //ClickButton ������ ��ư�� �ùķ��̼��� �����ų� ������ ������ �˾Ƽ� Idle�� �ٲ��ش�
   //�����̳� �ùķ��̼��� ������� �ʾ�����? .. ����

    
    public void SetNameUI(bool active)
    {
        nameUI.SetActive(active);
    }
    public void SetExplanationUI(bool active)
    {
        ExplanationUI.SetActive(active);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        state = ButtonState.OntheMouse;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        state = ButtonState.Idle;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (ExplanationUI.activeSelf)
        {
            Debug.Log("�̹�Ȱ��ȭ�� ��ư�Դϴ�.");
            return;
        }
        Debug.Log("�ùķ��̼� ���� ��� :  " + handAction);
        currentCursor = eventData.pointerClick.GetComponent<LeapMouseCursor>();
        if (currentCursor.lastbtn != this)
        {
            //������ ���� �г��� �ִٸ�
            if (currentCursor.lastbtn != null )
            {
                //���������г� �ݱ�
                currentCursor.lastbtn.SetExplanationUI(false);
            }
            //��ư��ũ��Ʈ�� �����г� Active Ű�� �޼ҵ� ������ �߰�

           
        }
        currentCursor.lastbtn = this;

        SetExplanationUI(true); // �����г�Ű�� �޼ҵ�
                             //������ư�� players���� ���� ���������
        currentCursor.UpdateCursorState(ActionState.Select);

    }
    public void PlaySimulator()
    {
        ExplanationUI.GetComponent<ExplanationPanel>().Play();
    }
    private void OnDisable()
    {
        state = ButtonState.Idle;
    }
}
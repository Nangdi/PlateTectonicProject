using Leap;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
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
    public enum PlateNum
    {
        EastAfricanRiftValley,  //  0 ��������ī �����
        HimalayaMountains,      //  1 ������� ���
        MarianaTrench,          //  2 �����Ƴ� �ر�
        JapanTrench,            //  3 �Ϻ��ر�
        SanAndreasFault,        //  4 �� �ȵ巹�ƽ� ����
        MidAtlanticRidge,       //  5 �뼭�� �߾�
        AndesMountains,         //  6 �ᵥ�� ���
    }
    public HandAction handAction;
    public ButtonState state;
    public PlateNum plateNum;
    public GameObject nameUI;
    public GameObject ExplanationUI;
    public GameObject videoUI;
    public GameObject interaction;
    public List<LeapMouseCursor> players = new List<LeapMouseCursor>();
    public LeapMouseCursor currentCursor;
    public LeapMouseCursor mouseCursor;
    public RawImage video;
    public List<CollisioEffect> effects = new List<CollisioEffect>();
    public AccentControll accentControl;
    public VideoController videoController;
    public RectTransform particlePos;

    public GameObject unReadyImage;
    public GameObject ReadyImage;
    private void Start()
    {
        for (int i = 0; i < interaction.transform.childCount; i++) {

            if (!interaction.transform.GetChild(i).gameObject.activeSelf)
            {
                continue;
            }
            for (int j = 0; j < interaction.transform.GetChild(i).childCount; j++)
            {
               
                if (interaction.transform.GetChild(i).GetChild(j).TryGetComponent<CollisioEffect>(out CollisioEffect effect))
                {
                    effects.Add(effect);
                }
               
            }
        }
    }

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
   //�����̳� �ùķ��̼��� ������� �ʾ�����? .. ���

    
    public void SetNameUI(bool active)
    {
        nameUI.SetActive(active);
    }
    public void SetExplanationUI(bool active)
    {
        ExplanationUI.SetActive(active);
        if (active)
        {
            currentCursor.UpdateCursorState(ActionState.Select);
        }
      
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
            if (currentCursor.lastbtn != null)
            {
                //���������г� �ݱ�
                currentCursor.lastbtn.SetExplanationUI(false);
                currentCursor.lastbtn.accentControl.StopAccent();
            }
            //��ư��ũ��Ʈ�� �����г� Active Ű�� �޼ҵ� ������ �߰�

           
        }
        
        currentCursor.lastbtn = this;
        //accentControl.gameObject.SetActive(true);
        //���⼭ ȭ��ǥ ���ֱ�? or �����ǳ� ������ ���ֱ� or �μ��� ��� �νĵ����� ȭ��ǥ ���ֱ� 

        accentControl.PlayAccent();
        //SetExplanationUI(true);
        // �����г�Ű�� �޼ҵ� => AccentControll���� ani������ ����
        //������ư�� players���� ���� ���������


    }
    public void PlayVideo()
    {
        ExplanationUI.GetComponent<ExplanationPanel>().Play();
    }
    public void PlaySimulrator()
    {
        videoController.PlayVideo();

        ReadyImage.SetActive(false);
        ////todo �ùķ��̼� ����� ȭ��ǥ���ֱ�
        //for (int i = 0; i < effects.Count; i++)
        //{
        //    effects[i].PlaySimulator();
        //}
    }
    public void ReadySimulrator(bool _isReady)
    {
        //if (arrowHand.activeSelf == _isReady) return;
        unReadyImage.gameObject.SetActive(!_isReady); //�غ�Ǹ鲨�ֱ� �ȵǸ� Ű��
        ReadyImage.SetActive(_isReady); //�غ�Ǹ����ֱ� �ȵǸ� ����
    }
    private void OnDisable()
    {
        state = ButtonState.Idle;
    }

}

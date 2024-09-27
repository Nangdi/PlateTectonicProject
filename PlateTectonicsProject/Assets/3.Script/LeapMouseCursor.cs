using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using Leap;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using LeapInternal;
using UnityEngine.InputSystem.Controls;

public enum ActionState {
    Idle = 0,
    Select,
    playback,
    Off
}


public class LeapMouseCursor : MonoBehaviour
{

    //// Windows API�� SetCursorPos �Լ� ����
    //[DllImport("user32.dll")]
    //static extern bool SetCursorPos(int x, int y);

    //[DllImport("user32.dll")]
    //static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);

    //const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
    //const uint MOUSEEVENTF_LEFTUP = 0x0004;

    


    public LeapServiceProvider leapServiceProvider;
    public int mouseSensitivity= 3000;
    public float lerpSpeed = 10f;
    public float OffTime =10f;
    public float timer;
    public ActionState actionState;
    public GameObject cursorObject;
    private RectTransform cursorRect;
    public GraphicRaycaster raycaster;
    private GameObject lastObject;
    public PlusButton lastbtn;
    public GuidePanel gudie;
    private float previousDistance;
    private float previousYDistance;
    private float waitTime = 1.0f;
    private bool IsHandsInitialized;
    public float distanceThreshold = 0.003f;
    private void OnEnable()
    {
        timer = 0f;
    }
    void Start()
    {

        cursorRect = GetComponent<RectTransform>();
        UpdateCursorState(ActionState.Idle);
        //raycaster = GetComponent<GraphicRaycaster>(); // ���� ���� ������Ʈ���� ��������

        // Leap Motion ��Ʈ�ѷ� �ʱ�ȭ


    }

    void Update()
    {
        // Leap Motion���� ������ �����͸� ������
        Frame frame = leapServiceProvider.CurrentFrame;
        // ���� �����Ǿ����� Ȯ��
        if (frame.Hands.Count > 0)
        {
            //������ �켱����
            Hand hand = frame.Hands.Count > 1 ? frame.Hands[1] : frame.Hands[0];
            //����ġ�� ���Ӿ��� mouseObject ����
            MapHandToCursor(hand);
            RaycastUI(hand);
            if (actionState == ActionState.Select && frame.Hands.Count == 2 && !IsHandsInitialized)
            {
                // ���� ó�� �νĵǸ� ���� �ð��� ����
                StartCoroutine(InitializeHands(frame.Hands[0], frame.Hands[1]));
            }
            //Btn Ŭ���� ���� �ΰ��ν� ������
            if (actionState == ActionState.Select && frame.Hands.Count== 2 && IsHandsInitialized)
            {
                Hand leftHand = frame.Hands[0];
                Hand rightHand = hand;

                //�μչٴ��� ��ġ
                Vector3 leftHandPosition = leftHand.PalmPosition;
                Vector3 rightHandPosition = rightHand.PalmPosition;
                // �� �� ������ �Ÿ� ���
                float currentDistance = Vector3.Distance(leftHandPosition, rightHandPosition);
                float currentYDistance = Mathf.Abs(leftHandPosition.z - rightHandPosition.z);
                // ���� �Ÿ��� ���� �Ÿ��� ���Ͽ� ���� �־������� �Ǵ�

                switch (lastbtn.handAction)
                {
                    case PlusButton.HandAction.ZoomOut: //���� ��������� : ����
                        if (currentDistance - previousDistance < -distanceThreshold)
                        {
                            //�μ��� ��������� ����Ǵ� ��
                            Debug.Log("������ ��� : ���� �������");
                            SimulationPlay();
                        }
                        break;
                    case PlusButton.HandAction.ZoomIn: //���̸־����� : �߻�
                        if (currentDistance - previousDistance > distanceThreshold)
                        {
                            Debug.Log("�߻��� ��� : ���� �־���");
                            SimulationPlay();
                            // ���⿡ �� ���� �־����� �� ������ ���� �߰� 
                        }
                        break;
                    case PlusButton.HandAction.HandsMoveUpDown:  //�������Ʒ��θ־����� : ����
                        if (currentYDistance - previousYDistance > distanceThreshold)
                        {
                            //����� ���Ʒ��� �Ÿ��� ��������
                            Debug.Log("������ ��� : ���� ���Ʒ��� �־��� : " + (currentYDistance - previousYDistance));
                            SimulationPlay();
                        }
                        break;
                }
                
                
            

                // ���� �Ÿ��� ���� �Ÿ��� ������Ʈ
                previousDistance = currentDistance;

            }
            else
            {
                IsHandsInitialized = false;
            }




        }
        else
        {
            //���̰������� ������ �����ð��� �帥�� state�� Off�� �ٲ㼭 ��� ��Ҹ� �ʱ�ȭ�Ѵ�.
            //�ѳ��� �г� , �÷��̾� ���̵��Ʈ , 
            timer += Time.deltaTime;
            if (timer > OffTime)
            {
                UpdateCursorState(ActionState.Off);
                gameObject.SetActive(false);
            }

        }

    }
    private Vector3 UpdateCursorPosition(float Xpos , float Ypos) 
    {
        Vector3 tempVector3 = new Vector3(Xpos, Ypos , 0);
        return tempVector3;

    }
    private void MapHandToCursor(Hand hand)
    {
        Vector3 handPosition = hand.PalmPosition;
        int mouseX = (int)Mathf.Clamp(handPosition.x * mouseSensitivity, -Screen.width / 2, Screen.width / 2);
        int mouseY = (int)Mathf.Clamp((handPosition.y - 0.2f) * mouseSensitivity, -Screen.height / 2, Screen.height / 2);
        //Debug.Log("mouse" + mouseX + ", " + mouseY);
        //Debug.Log("handPositon : " + handPosition);

        //���콺 �ն��������� Lerp ���
        cursorRect.anchoredPosition = Vector2.Lerp(
         cursorRect.anchoredPosition, // ���� ��ġ
         UpdateCursorPosition(mouseX, mouseY), // ��ǥ ��ġ
         Time.deltaTime * 2   // ���� �ӵ�
         );
    }

    private void RaycastUI(Hand hand)
    {

        // Raycast�� ���� ������ �غ�
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = cursorRect.position
        };

        // ����� ������ ����Ʈ ����
        List<RaycastResult> results = new List<RaycastResult>();

        // Raycast ����
        raycaster.Raycast(pointerEventData, results );

        if (results.Count > 0)
        {
            GameObject targetObject = results[0].gameObject;

            // PointerEnter �̺�Ʈ �߻�
            if (lastObject != targetObject)
            {
                // ������ ȣ���� ������Ʈ�� ���� ��� PointerExit �̺�Ʈ �߻�
                if (lastObject != null)
                {
                    ExecuteEvents.Execute(lastObject, pointerEventData, ExecuteEvents.pointerExitHandler);
                }

                // ���ο� ������Ʈ�� PointerEnter �̺�Ʈ �߻�
                ExecuteEvents.Execute(targetObject, pointerEventData, ExecuteEvents.pointerEnterHandler);
                lastObject = targetObject; // ���� ȣ���� ������Ʈ ������Ʈ
            }
            
        }
        else
        {
            // PointerExit �̺�Ʈ �߻�
            if (lastObject != null)
            {
                ExecuteEvents.Execute(lastObject, pointerEventData, ExecuteEvents.pointerExitHandler);
                lastObject = null; // ȣ���� ������Ʈ �ʱ�ȭ
            }
        }


  
    }
    public void SimulateMouseClick()
    {
        //SetCursorPos((int)cursorRect.anchoredPosition.x, (int)cursorRect.anchoredPosition.y);
        //mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
        //mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);

        // ���콺 Ŭ���� �ùķ��̼��ϱ� ���� Ŭ�� �̺�Ʈ ����
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = cursorRect.position;
        pointerEventData.pointerClick = gameObject;
        //pointerEventData.button = PointerEventData.InputButton.Left; // Ŭ���� ���콺 ��ư ����

        // Ŭ���� ��� ������Ʈ ����
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        raycaster.Raycast(pointerEventData, raycastResults);
        //EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        // + ��ư�� ����������. �ùķ��̼� ��ư������ ���� ��������
        if (raycastResults.Count > 0)
        {
            // ������ ��ư ��������
            GameObject clickedObject = raycastResults[0].gameObject;
            //Ŭ���̺�Ʈ �߻�
            ExecuteEvents.Execute(clickedObject, pointerEventData, ExecuteEvents.pointerClickHandler);
            
            //+��ư�� ���������� �ùķ��̼� ��ư�� ���������� ����� �������ҰŰ���
          
           
           
        }
    }
    public void UpdateCursorState(ActionState state)
    {
        actionState = state;
        gudie.UpdateGuideText();
        Debug.Log("�ؽ�Ʈ������Ʈ");
    }

    IEnumerator InitializeHands(Hand leftHand, Hand rightHand)
    {
        yield return new WaitForSeconds(waitTime);  // 1�� ���

        // �� ���� ù ��° �Ÿ� ��� (���� �ð� ��)
        Vector3 leftHandPosition = leftHand.PalmPosition;
        Vector3 rightHandPosition = rightHand.PalmPosition;
        previousDistance = Vector3.Distance(leftHandPosition, rightHandPosition);
        previousYDistance = Mathf.Abs(leftHandPosition.z - rightHandPosition.z);
        IsHandsInitialized = true;
    }
    public void SimulationPlay()
    {
        UpdateCursorState(ActionState.playback);
        IsHandsInitialized = false;
        lastbtn.PlaySimulator();
    }
}
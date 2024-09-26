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

public enum ActionType {
    Idle = 0,
    move ,
    Select
}


public class LeapMouseCursor : MonoBehaviour
{

    // Windows API�� SetCursorPos �Լ� ����
    [DllImport("user32.dll")]
    static extern bool SetCursorPos(int x, int y);

    [DllImport("user32.dll")]
    static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);

    const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
    const uint MOUSEEVENTF_LEFTUP = 0x0004;

    

    Controller leapController;
    public LeapServiceProvider leapServiceProvider;
    public HandPoseDetector handPoseDetector;
    public HandPoseScriptableObject ClickPose;
    public HandPoseScriptableObject[] MoivingPose;
    private ActionType actionType;
    public int mouseSensitivity= 3000;
    public float lerpSpeed = 10f;
    public GameObject cursorObject;
    private RectTransform cursorRect;

    void Start()
    {
        
        cursorRect = GetComponent<RectTransform>();
        // Leap Motion ��Ʈ�ѷ� �ʱ�ȭ
        leapController = new Controller();
        if (handPoseDetector != null)
        {
            // ��� ������ ������ ȣ��� �ݹ��� ���
            //handPoseDetector.OnPoseDetected.AddListener(HandlePoseDetected);
        }
    }

    void Update()
    {
        // Leap Motion���� ������ �����͸� ������
        //Frame frame = leapController.Frame();
        Frame frame = leapServiceProvider.CurrentFrame;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            SimulateMouseClick();
        }
        // ���� �����Ǿ����� Ȯ��
        if (frame.Hands.Count > 0)
        {
            //������ �켱����
            Hand hand = frame.Hands.Count > 1 ? frame.Hands[1] : frame.Hands[0];
            // ù ��° ���� ��ġ ��������
            if (frame.Hands.Count > 1)
            {
            }
            else
            {

            }

            Vector3 handPosition = hand.PalmPosition;
            //handPosition.y = -handPosition.y;
            //Debug.Log("position" + handPosition);
            // Leap Motion ��ǥ�� ȭ�� �ػ󵵿� �°� ����
            //int mouseX = (int)Mathf.Clamp(Screen.width / 2 + handPosition.x * test, 0, Screen.width);
            //int mouseY = (int)Mathf.Clamp(Screen.height / 2 + (handPosition.y + 0.2f) * test, 0, Screen.height);
            int mouseX = (int)Mathf.Clamp(handPosition.x * mouseSensitivity, -Screen.width / 2, Screen.width / 2);
            int mouseY = (int)Mathf.Clamp((handPosition.y - 0.2f) * mouseSensitivity, -Screen.height / 2, Screen.height / 2);
            //Debug.Log("mouse" + mouseX + ", " + mouseY);

            cursorRect.anchoredPosition = Vector2.Lerp(
             cursorRect.anchoredPosition, // ���� ��ġ
             UpdateCursorPosition(mouseX, mouseY), // ��ǥ ��ġ
             Time.deltaTime * 2   // ���� �ӵ�
             );

            //transform.position = UpdateCursorPosition(mouseX, mouseY);


            //SetCursorPos(mouseX, mouseY);
            //transform.position = Input.mousePosition;

            // ���콺 Ŀ�� ��ġ ����



        }

    }
    public void SimulateMouseClick()
    {
        //SetCursorPos((int)cursorRect.anchoredPosition.x, (int)cursorRect.anchoredPosition.y);
        //mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
        //mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        Debug.Log(gameObject.name);

        // ���콺 Ŭ���� �ùķ��̼��ϱ� ���� Ŭ�� �̺�Ʈ ����
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = cursorRect.position;
        //pointerEventData.button = PointerEventData.InputButton.Left; // Ŭ���� ���콺 ��ư ����

        // Ŭ���� ��� ������Ʈ ����
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

      
        if (raycastResults.Count > 0)
        {
            foreach (var item in raycastResults)
            {
                Debug.Log(item.gameObject.name);
            }
            // ���� ���� ������ UI ��ҿ� ���� Ŭ�� �̺�Ʈ ó��
            GameObject clickedObject = raycastResults[0].gameObject;
            ExecuteEvents.Execute(clickedObject, pointerEventData, ExecuteEvents.pointerClickHandler);
        }
    }
    private void HandlePoseDetected(Hand hand)
    {
        // ���� ������ ����� ��ȭ�� ��� ��ġ�ϴ��� Ȯ��
        Debug.Log(hand.GetPalmPose());
        HandPoseScriptableObject detectedPose = handPoseDetector.GetCurrentlyDetectedPose();
        if (detectedPose != null && detectedPose == ClickPose)
        {
            Debug.Log(detectedPose.name);
            // ��� ��ġ�ϸ� ���� Ŭ�� �̺�Ʈ ȣ��

        }
    }
 
    public void MovingUnDetected()
    {
        actionType = ActionType.Idle;
    }
    public void SelectDetected()
    {
        actionType = ActionType.Idle;
    }
    public void SelectUnDetected()
    {
        actionType = ActionType.Idle;
    }
    public void testbutton()
    {
        Debug.Log("��ư����");
    }
    private Vector3 UpdateCursorPosition(float Xpos , float Ypos) 
    {
        Vector3 tempVector3 = new Vector3(Xpos, Ypos , 0);
        return tempVector3;

    }


}
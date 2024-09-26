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

    // Windows API의 SetCursorPos 함수 선언
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
        // Leap Motion 컨트롤러 초기화
        leapController = new Controller();
        if (handPoseDetector != null)
        {
            // 포즈가 감지될 때마다 호출될 콜백을 등록
            //handPoseDetector.OnPoseDetected.AddListener(HandlePoseDetected);
        }
    }

    void Update()
    {
        // Leap Motion에서 프레임 데이터를 가져옴
        //Frame frame = leapController.Frame();
        Frame frame = leapServiceProvider.CurrentFrame;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            SimulateMouseClick();
        }
        // 손이 감지되었는지 확인
        if (frame.Hands.Count > 0)
        {
            //오른손 우선추적
            Hand hand = frame.Hands.Count > 1 ? frame.Hands[1] : frame.Hands[0];
            // 첫 번째 손의 위치 가져오기
            if (frame.Hands.Count > 1)
            {
            }
            else
            {

            }

            Vector3 handPosition = hand.PalmPosition;
            //handPosition.y = -handPosition.y;
            //Debug.Log("position" + handPosition);
            // Leap Motion 좌표를 화면 해상도에 맞게 조정
            //int mouseX = (int)Mathf.Clamp(Screen.width / 2 + handPosition.x * test, 0, Screen.width);
            //int mouseY = (int)Mathf.Clamp(Screen.height / 2 + (handPosition.y + 0.2f) * test, 0, Screen.height);
            int mouseX = (int)Mathf.Clamp(handPosition.x * mouseSensitivity, -Screen.width / 2, Screen.width / 2);
            int mouseY = (int)Mathf.Clamp((handPosition.y - 0.2f) * mouseSensitivity, -Screen.height / 2, Screen.height / 2);
            //Debug.Log("mouse" + mouseX + ", " + mouseY);

            cursorRect.anchoredPosition = Vector2.Lerp(
             cursorRect.anchoredPosition, // 현재 위치
             UpdateCursorPosition(mouseX, mouseY), // 목표 위치
             Time.deltaTime * 2   // 보간 속도
             );

            //transform.position = UpdateCursorPosition(mouseX, mouseY);


            //SetCursorPos(mouseX, mouseY);
            //transform.position = Input.mousePosition;

            // 마우스 커서 위치 설정



        }

    }
    public void SimulateMouseClick()
    {
        //SetCursorPos((int)cursorRect.anchoredPosition.x, (int)cursorRect.anchoredPosition.y);
        //mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
        //mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        Debug.Log(gameObject.name);

        // 마우스 클릭을 시뮬레이션하기 위해 클릭 이벤트 생성
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = cursorRect.position;
        //pointerEventData.button = PointerEventData.InputButton.Left; // 클릭할 마우스 버튼 설정

        // 클릭할 대상 오브젝트 감지
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

      
        if (raycastResults.Count > 0)
        {
            foreach (var item in raycastResults)
            {
                Debug.Log(item.gameObject.name);
            }
            // 가장 먼저 감지된 UI 요소에 대해 클릭 이벤트 처리
            GameObject clickedObject = raycastResults[0].gameObject;
            ExecuteEvents.Execute(clickedObject, pointerEventData, ExecuteEvents.pointerClickHandler);
        }
    }
    private void HandlePoseDetected(Hand hand)
    {
        // 현재 감지된 포즈와 녹화된 포즈가 일치하는지 확인
        Debug.Log(hand.GetPalmPose());
        HandPoseScriptableObject detectedPose = handPoseDetector.GetCurrentlyDetectedPose();
        if (detectedPose != null && detectedPose == ClickPose)
        {
            Debug.Log(detectedPose.name);
            // 포즈가 일치하면 더블 클릭 이벤트 호출

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
        Debug.Log("버튼눌림");
    }
    private Vector3 UpdateCursorPosition(float Xpos , float Ypos) 
    {
        Vector3 tempVector3 = new Vector3(Xpos, Ypos , 0);
        return tempVector3;

    }


}
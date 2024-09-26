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

public enum ActionState {
    Idle = 0,
    Select,
    playback,
    Off
}


public class LeapMouseCursor : MonoBehaviour
{

    //// Windows API의 SetCursorPos 함수 선언
    //[DllImport("user32.dll")]
    //static extern bool SetCursorPos(int x, int y);

    //[DllImport("user32.dll")]
    //static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);

    //const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
    //const uint MOUSEEVENTF_LEFTUP = 0x0004;

    


    public LeapServiceProvider leapServiceProvider;
    public int mouseSensitivity= 3000;
    public float lerpSpeed = 10f;

    public ActionState actionState;
    public GameObject cursorObject;
    private RectTransform cursorRect;
    public GraphicRaycaster raycaster;
    private GameObject lastObject;
    public PlusButton lastbtn;
    public GuidePanel gudie;

    void Start()
    {

        cursorRect = GetComponent<RectTransform>();
        UpdateCursorState(ActionState.Idle);
        //raycaster = GetComponent<GraphicRaycaster>(); // 같은 게임 오브젝트에서 가져오기

        // Leap Motion 컨트롤러 초기화


    }

    void Update()
    {
        // Leap Motion에서 프레임 데이터를 가져옴
        Frame frame = leapServiceProvider.CurrentFrame;
        // 손이 감지되었는지 확인
        if (frame.Hands.Count > 0)
        {
            //오른손 우선추적
            Hand hand = frame.Hands.Count > 1 ? frame.Hands[1] : frame.Hands[0];
            //손위치와 게임씬의 mouseObject 매핑
            MapHandToCursor(hand);
            RaycastUI(hand);







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

        //마우스 손떨림때문에 Lerp 사용
        cursorRect.anchoredPosition = Vector2.Lerp(
         cursorRect.anchoredPosition, // 현재 위치
         UpdateCursorPosition(mouseX, mouseY), // 목표 위치
         Time.deltaTime * 2   // 보간 속도
         );
    }

    private void RaycastUI(Hand hand)
    {

        // Raycast를 위한 데이터 준비
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = cursorRect.position
        };

        // 결과를 저장할 리스트 생성
        List<RaycastResult> results = new List<RaycastResult>();

        // Raycast 수행
        raycaster.Raycast(pointerEventData, results );

        if (results.Count > 0)
        {
            GameObject targetObject = results[0].gameObject;

            // PointerEnter 이벤트 발생
            if (lastObject != targetObject)
            {
                // 이전에 호버된 오브젝트가 있을 경우 PointerExit 이벤트 발생
                if (lastObject != null)
                {
                    ExecuteEvents.Execute(lastObject, pointerEventData, ExecuteEvents.pointerExitHandler);
                }

                // 새로운 오브젝트에 PointerEnter 이벤트 발생
                ExecuteEvents.Execute(targetObject, pointerEventData, ExecuteEvents.pointerEnterHandler);
                lastObject = targetObject; // 현재 호버된 오브젝트 업데이트
            }
            
        }
        else
        {
            // PointerExit 이벤트 발생
            if (lastObject != null)
            {
                ExecuteEvents.Execute(lastObject, pointerEventData, ExecuteEvents.pointerExitHandler);
                lastObject = null; // 호버된 오브젝트 초기화
            }
        }


  
    }
    public void SimulateMouseClick()
    {
        //SetCursorPos((int)cursorRect.anchoredPosition.x, (int)cursorRect.anchoredPosition.y);
        //mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
        //mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);

        // 마우스 클릭을 시뮬레이션하기 위해 클릭 이벤트 생성
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = cursorRect.position;
        pointerEventData.pointerClick = gameObject;
        //pointerEventData.button = PointerEventData.InputButton.Left; // 클릭할 마우스 버튼 설정

        // 클릭할 대상 오브젝트 감지
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        raycaster.Raycast(pointerEventData, raycastResults);
        //EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        // + 버튼을 눌렀을때만. 시뮬레이션 버튼누를땐 따로 만들어야함
        if (raycastResults.Count > 0)
        {
            // 감지된 버튼 가져오기
            GameObject clickedObject = raycastResults[0].gameObject;
            //클릭이벤트 발생
            ExecuteEvents.Execute(clickedObject, pointerEventData, ExecuteEvents.pointerClickHandler);
            
            //+버튼을 눌럿을때와 시뮬레이션 버튼을 눌렀을때의 기능을 나눠야할거같음
          
           
           
        }
    }
    public void UpdateCursorState(ActionState state)
    {
        actionState = state;
        gudie.UpdateGuideText();
        Debug.Log("텍스트업데이트");
    }
}
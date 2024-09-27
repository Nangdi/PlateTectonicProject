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
            if (actionState == ActionState.Select && frame.Hands.Count == 2 && !IsHandsInitialized)
            {
                // 손이 처음 인식되면 유예 시간을 시작
                StartCoroutine(InitializeHands(frame.Hands[0], frame.Hands[1]));
            }
            //Btn 클릭후 손이 두개인식 됐을때
            if (actionState == ActionState.Select && frame.Hands.Count== 2 && IsHandsInitialized)
            {
                Hand leftHand = frame.Hands[0];
                Hand rightHand = hand;

                //두손바닥의 위치
                Vector3 leftHandPosition = leftHand.PalmPosition;
                Vector3 rightHandPosition = rightHand.PalmPosition;
                // 두 손 사이의 거리 계산
                float currentDistance = Vector3.Distance(leftHandPosition, rightHandPosition);
                float currentYDistance = Mathf.Abs(leftHandPosition.z - rightHandPosition.z);
                // 이전 거리와 현재 거리를 비교하여 손이 멀어졌는지 판단

                switch (lastbtn.handAction)
                {
                    case PlusButton.HandAction.ZoomOut: //손이 가까워질때 : 수렴
                        if (currentDistance - previousDistance < -distanceThreshold)
                        {
                            //두손이 가까워질때 실행되는 곳
                            Debug.Log("수렴형 경계 : 손이 가까워짐");
                            SimulationPlay();
                        }
                        break;
                    case PlusButton.HandAction.ZoomIn: //손이멀어질때 : 발산
                        if (currentDistance - previousDistance > distanceThreshold)
                        {
                            Debug.Log("발산형 경계 : 손이 멀어짐");
                            SimulationPlay();
                            // 여기에 두 손이 멀어졌을 때 실행할 동작 추가 
                        }
                        break;
                    case PlusButton.HandAction.HandsMoveUpDown:  //손이위아래로멀어질때 : 보존
                        if (currentYDistance - previousYDistance > distanceThreshold)
                        {
                            //양손이 위아래로 거리가 벌려질때
                            Debug.Log("보존형 경계 : 손이 위아래로 멀어짐 : " + (currentYDistance - previousYDistance));
                            SimulationPlay();
                        }
                        break;
                }
                
                
            

                // 현재 거리를 이전 거리로 업데이트
                previousDistance = currentDistance;

            }
            else
            {
                IsHandsInitialized = false;
            }




        }
        else
        {
            //손이감지되지 않을시 일정시간이 흐른후 state를 Off로 바꿔서 모든 요소를 초기화한다.
            //켜놓은 패널 , 플레이어 가이드멘트 , 
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

    IEnumerator InitializeHands(Hand leftHand, Hand rightHand)
    {
        yield return new WaitForSeconds(waitTime);  // 1초 대기

        // 두 손의 첫 번째 거리 계산 (유예 시간 후)
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
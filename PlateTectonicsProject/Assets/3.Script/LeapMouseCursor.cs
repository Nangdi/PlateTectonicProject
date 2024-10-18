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
using DG.Tweening;
using System.Runtime.CompilerServices; //import


public enum ActionState {
    Idle = 0,
    Select,
    playback,
    Off
}
public enum PlayerNum
{
    Player1,
    Player2
}


public class LeapMouseCursor : MonoBehaviour
{


    public LeapServiceProvider leapServiceProvider;
    public float OffTime =30f; //손이 인식되지않을때 OFF되기까지의 시간
    public float timer; //손이 인식되지 않을때의 타이머
    public ActionState actionState; //현재 플레이어의 상태
    public PlayerNum playerNum; //플레이어 번호
    public GameObject cursorObject; //모션에 따라 움직이는 플레이어의 커서
    private RectTransform cursorRect; //커서의 Rect
    public GraphicRaycaster raycaster; //커서가 UI를 감지하기 위한 ray
    private GameObject lastObject; //커서가 btn을 벗어났는지 알기위해 담아두는 마지막에 감지한 오브젝트
    public PlusButton lastbtn; //커서가 마지막으로 클릭한 버튼
    public GuidePanel gudie; //플레이어의 상태에 따라 안내문구가 달라지는 가이드판넬
    private float previousDistance; //움직이기전 두손의 거리
    private float previousYDistance; // 움직이기전 두손의 y좌표의 거리
    private float waitTime = 1.0f; //한손만 인식된 상태에서 두손이 인식될때 손이 중앙으로 갈 시간을 주는 유예시간
    private bool IsHandsInitialized; //유예시간이 지난후 바뀌는 bool값
    private Vector3 clickSCale = new Vector3(0.5f,0.5f,0.5f);
    //옵션창에서 커스텀 설정할 수 있게 해야하는 것들
    public float mouseSpeed= 0.5f;
    public float motionSensitivity = 0.025f;
    public float handHeight= 0.3f;
    public float handStability = 0.5f;
    Sequence clickSequence;
    public GameObject prefab;
    public RectTransform spawnPos;
    public Transform particlePool;
    private bool isTrigger;

    //손바닥쥐는 애니메이션 변수들
    public UnityEngine.UI.Image cursorImage;
    public Sprite palmSprite; // 손바닥 스프라이트
    public Sprite fistSprite; // 주먹 스프라이트
    private Tween currentTween; // 현재 진행 중인 트윈

    private float cumulativeDistance = 0f;   // 누적된 거리
    private float cumulativeYDistance = 0f;  // 누적된 Y축 거리
    private float checkInterval = 0.1f;      // 매 0.1초마다 동작을 체크
    private float lastCheckTime = 0f;        // 마지막으로 체크한 시간
    private float motiondistance;
    void Start()
    {

        cursorRect = GetComponent<RectTransform>();
        cursorImage = GetComponent<UnityEngine.UI.Image>();
        // playecursor 상태 초기화 : off상태시작
        UpdateCursorState(ActionState.Off);
        gameObject.SetActive(false);
        //clickSequence = DOTween.Sequence();

        //// Append: 순차적으로 실행 (첫 번째 애니메이션이 끝나고 두 번째가 실행됨)
        //clickSequence.Append(cursorObject.transform.DOScale(0.5f, 0.2f));  // 그 후에 1초 동안 스케일을 0.5로 줄임
        //clickSequence.Append(cursorObject.transform.DOScale(1, 0.2f));  // 그 후에 1초 동안 스케일을 0.5로 줄임


    }
    private void OnEnable()
    {
        timer = 0f;
    }

    void Update()
    {
        // Leap Motion에서 프레임 데이터를 가져옴
        Frame frame = leapServiceProvider.CurrentFrame;
        // 손이 감지되었는지 확인
        if (frame.Hands.Count > 0)
        {
            //손이감지됐을때 타이머 초기화
            timer = 0f;
            //오른손 우선추적
            Hand hand = frame.Hands.Count > 1 ? frame.Hands[1] : frame.Hands[0];
            //손위치와 게임씬의 mouseObject 매핑
            MapHandToCursor(hand);
            RaycastUI(hand);
           
            //버튼을 클릭하고 손두개가 인식되어 있을때
            if(actionState == ActionState.Select && frame.Hands.Count == 2)
            {

                if (IsHandsInitialized)
                {
                    StandbySimulation(hand, frame);
                }
                else
                {
                    if (!isTrigger)
                    {
                        isTrigger = true;
                        StartCoroutine(InitializeHands(frame.Hands[0], frame.Hands[1]));
                    }
                }
            }
            else
            {
                IsHandsInitialized = false;
                if (lastbtn != null && actionState == ActionState.Select)
                {
                    lastbtn.ReadySimulrator(false);
                }
            }

        }
        else
        {
            //손이감지되지 않을시 일정시간이 흐른후 state를 Off로 바꿔서 모든 요소를 초기화한다.
            //켜놓은 패널 , 플레이어 가이드멘트 , 
            //인식됐을때 timer 초기화와 Active가 꺼졌을때 켜줄 Manager를 새로 만들어야 할거같음
            IsHandsInitialized = false;
            if (lastbtn != null && actionState == ActionState.Select)
            {
                lastbtn.ReadySimulrator(false);
               
            }
            else
            {
              
              
            }
            timer += Time.deltaTime;
            if (timer > OffTime)
            {
                if(actionState == ActionState.Select || actionState == ActionState.playback)
                {
                    lastbtn.SetExplanationUI(false);
                    lastbtn.videoUI.SetActive(false);
                }

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
        float motionSpeed = (10000 + (20000 * mouseSpeed));
        int mouseX = (int)Mathf.Clamp(handPosition.x * motionSpeed, -Screen.width / 2, Screen.width / 2);
        int mouseY = (int)Mathf.Clamp((handPosition.y - (0.5f - handHeight)) * motionSpeed, -Screen.height / 2, Screen.height / 2);
        //Debug.Log("mouse" + mouseX + ", " + mouseY);
        //Debug.Log("handPositon : " + handPosition);

        //마우스 손떨림때문에 Lerp 사용
        cursorRect.anchoredPosition = Vector2.Lerp(
         cursorRect.anchoredPosition, // 현재 위치
         UpdateCursorPosition(mouseX, mouseY), // 목표 위치
         Time.deltaTime * (3 - (handStability * 2))   // 보간 속도
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
            if(targetObject.name == "subBtn")
            {
                targetObject = targetObject.transform.parent.gameObject;
            }
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

        // 마우스 클릭을 시뮬레이션하기 위해 클릭 이벤트 생성
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = cursorRect.position;
        pointerEventData.pointerClick = gameObject;


        // 클릭할 대상 오브젝트 감지
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        raycaster.Raycast(pointerEventData, raycastResults);

        // + 버튼을 눌렀을때만. 시뮬레이션 버튼누를땐 따로 만들어야함
        GameObject clickedObject;
        if (raycastResults.Count > 0)
        {
          
            IsHandsInitialized = false;
            if (raycastResults[0].gameObject.name == "subBtn")
            {
                clickedObject = raycastResults[0].gameObject.transform.parent.gameObject;
            }
            else
            {
            clickedObject = raycastResults[0].gameObject;

            }
            // 감지된 버튼 가져오기
            //클릭이벤트 발생

            //+버튼을 눌럿을때와 시뮬레이션 버튼을 눌렀을때의 기능을 나눠야할거같음

            //누른 판넬 Rect AABB에 전달하기 , 판넬의 우선순위 전달하기
            ExecuteEvents.Execute(clickedObject, pointerEventData, ExecuteEvents.pointerClickHandler);
            if (lastbtn == null) return;
            if (playerNum == 0)
            {
                AABBCollisionResolve.Instance.rectTransform1 = lastbtn.ExplanationUI.GetComponent<RectTransform>();
            }
            else
            {
                AABBCollisionResolve.Instance.rectTransform2 = lastbtn.ExplanationUI.GetComponent<RectTransform>();
            }
           


        }
    }
    public void StandbySimulation(Hand hand, Frame frame)
    {
        // 두 손을 가져옴
        Hand leftHand = frame.Hands[0];
        Hand rightHand = hand;

        // 두 손바닥의 위치
        Vector3 leftHandPosition = leftHand.PalmPosition;
        Vector3 rightHandPosition = rightHand.PalmPosition;

        // 두 손 사이의 거리 계산
        float currentDistance = Vector3.Distance(leftHandPosition, rightHandPosition);  // ZoomIn/ZoomOut
        float currentYDistance = Mathf.Abs(leftHandPosition.z - rightHandPosition.z);    // HandsMoveUpDown

        // motionSensitivity에 따른 거리 민감도 조정
        motiondistance = 0.01f + (0.03f * motionSensitivity);

        // 두 손의 거리 차이 계산 (현재 거리와 이전 거리의 차이)
        float distanceDelta = currentDistance - previousDistance;
        float yDistanceDelta = currentYDistance - previousYDistance;

        // 거리 변화량을 누적
        //cumulativeDistance += Mathf.Abs(distanceDelta);
        //cumulativeYDistance += Mathf.Abs(yDistanceDelta);
        // 현재 시간을 기준으로 일정 시간 경과 시 동작을 체크
        if (Time.time - lastCheckTime >= checkInterval)
        {
            lastCheckTime = Time.time;

            switch (lastbtn.handAction)
            {
                case PlusButton.HandAction.ZoomOut: // 손이 가까워질 때 : 수렴
                    if (currentDistance < 0.3f * motionSensitivity)
                    {
                        Debug.Log("수렴형 경계 : 손이 가까워짐");
                        SimulationPlay();
                        cumulativeDistance = 0f;  // 누적 거리 초기화
                    }
                    break;

                case PlusButton.HandAction.ZoomIn: // 손이 멀어질 때 : 발산
                    if (currentDistance > 0.9f *motionSensitivity )
                    {
                        Debug.Log("발산형 경계 : 손이 멀어짐");
                        SimulationPlay();
                        cumulativeDistance = 0f;  // 누적 거리 초기화
                    }
                    break;

                case PlusButton.HandAction.HandsMoveUpDown:  // 손이 위아래로 멀어질 때 : 보존
                    if (currentYDistance > 0.4f*motionSensitivity )
                    {
                        Debug.Log("보존형 경계 : 손이 위아래로 멀어짐 : " + (currentYDistance - previousYDistance));
                        SimulationPlay();
                        cumulativeYDistance = 0f;  // 누적 Y축 거리 초기화
                    }
                    break;
            }
        }

        // 현재 거리를 이전 거리로 업데이트
        previousDistance = currentDistance;
        previousYDistance = currentYDistance;
    }

    public void UpdateCursorState(ActionState state)
    {
        actionState = state;
        gudie.UpdateGuideText();
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
        AudioManager.instance.Play("enterHand");
        lastbtn.ReadySimulrator(true);
        isTrigger = false;
    }
    //시뮬레이션 종료후 부르는 비디오판넬 
    public void SimulationPlay()
    {
        UpdateCursorState(ActionState.playback);
        IsHandsInitialized = false;
        lastbtn.PlaySimulrator();
    }
    public void ChangeToFist()
    {
        // 현재 트윈이 실행 중이면 취소
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill(); // 진행 중인 트윈 취소
        }

        // 주먹으로 스프라이트 변경
        cursorImage.sprite = fistSprite;

        // 0.3초 후 손바닥으로 다시 변경하는 트윈 실행
        currentTween = DOVirtual.DelayedCall(0.3f, () =>
        {
            cursorImage.sprite = palmSprite;
        });
    }
    public void ParticleRing()
    {

    }
    IEnumerator SpawnParticle()
    {
        UpdateCursorState(ActionState.playback);
        IsHandsInitialized = false;
        lastbtn.unReadyImage.gameObject.SetActive(false); //준비되면꺼주기 안되면 키기
        Debug.Log(lastbtn.ReadyImage.activeSelf);
        lastbtn. ReadyImage.SetActive(false); //준비되면켜주기 안되면 끄기
        Debug.Log(lastbtn.ReadyImage.activeSelf);
        spawnPos = lastbtn.particlePos;
        for (int i = 0; i < 6; i++)
        {
            if (lastbtn.ReadyImage.activeSelf)
            {
                lastbtn.ReadyImage.SetActive(false); //준비되면켜주기 안되면 끄기
            }
            Debug.Log("파티클생성");
            // 프리팹을 지정된 위치에 생성
           GameObject Ob = Instantiate(prefab, spawnPos.position, Quaternion.identity);
            Ob.transform.SetParent(particlePool);

            // 0.5초 대기
            yield return new WaitForSeconds(0.2f);
        }
            yield return new WaitForSeconds(3f);
        SimulationPlay();
    }
}
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
    public float OffTime =30f; //���� �νĵ��������� OFF�Ǳ������ �ð�
    public float timer; //���� �νĵ��� �������� Ÿ�̸�
    public ActionState actionState; //���� �÷��̾��� ����
    public PlayerNum playerNum; //�÷��̾� ��ȣ
    public GameObject cursorObject; //��ǿ� ���� �����̴� �÷��̾��� Ŀ��
    private RectTransform cursorRect; //Ŀ���� Rect
    public GraphicRaycaster raycaster; //Ŀ���� UI�� �����ϱ� ���� ray
    private GameObject lastObject; //Ŀ���� btn�� ������� �˱����� ��Ƶδ� �������� ������ ������Ʈ
    public PlusButton lastbtn; //Ŀ���� ���������� Ŭ���� ��ư
    public GuidePanel gudie; //�÷��̾��� ���¿� ���� �ȳ������� �޶����� ���̵��ǳ�
    private float previousDistance; //�����̱��� �μ��� �Ÿ�
    private float previousYDistance; // �����̱��� �μ��� y��ǥ�� �Ÿ�
    private float waitTime = 1.0f; //�Ѽո� �νĵ� ���¿��� �μ��� �νĵɶ� ���� �߾����� �� �ð��� �ִ� �����ð�
    private bool IsHandsInitialized; //�����ð��� ������ �ٲ�� bool��
    private Vector3 clickSCale = new Vector3(0.5f,0.5f,0.5f);
    //�ɼ�â���� Ŀ���� ������ �� �ְ� �ؾ��ϴ� �͵�
    public float motionSensitivity= 0.5f;
    public float distanceThreshold = 0.025f;
    public float handHeight= 0.3f;
    public float mouseSpeed = 0.5f;
    Sequence clickSequence;
    public GameObject prefab;
    public RectTransform spawnPos;
    public Transform particlePool;
    void Start()
    {

        cursorRect = GetComponent<RectTransform>();

        // playecursor ���� �ʱ�ȭ : off���½���
        UpdateCursorState(ActionState.Off);
        gameObject.SetActive(false);
        clickSequence = DOTween.Sequence();

        // Append: ���������� ���� (ù ��° �ִϸ��̼��� ������ �� ��°�� �����)
        clickSequence.Append(cursorObject.transform.DOScale(0.5f, 0.2f));  // �� �Ŀ� 1�� ���� �������� 0.5�� ����
        clickSequence.Append(cursorObject.transform.DOScale(1, 0.2f));  // �� �Ŀ� 1�� ���� �������� 0.5�� ����


    }
    private void OnEnable()
    {
        timer = 0f;
    }

    void Update()
    {
        // Leap Motion���� ������ �����͸� ������
        Frame frame = leapServiceProvider.CurrentFrame;
        // ���� �����Ǿ����� Ȯ��
        if (frame.Hands.Count > 0)
        {
            //���̰��������� Ÿ�̸� �ʱ�ȭ
            timer = 0f;
            //������ �켱����
            Hand hand = frame.Hands.Count > 1 ? frame.Hands[1] : frame.Hands[0];
            //����ġ�� ���Ӿ��� mouseObject ����
            MapHandToCursor(hand);
            RaycastUI(hand);
           
            //��ư�� Ŭ���ϰ� �յΰ��� �νĵǾ� ������
            if(actionState == ActionState.Select && frame.Hands.Count == 2)
            {
                if (IsHandsInitialized)
                {
                    
                    StandbySimulation(hand, frame);
                }
                else
                {
                    StartCoroutine(InitializeHands(frame.Hands[0], frame.Hands[1]));
                }
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
            //�νĵ����� timer �ʱ�ȭ�� Active�� �������� ���� Manager�� ���� ������ �ҰŰ���
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
        float motionSpeed = (3000 + (4000 * motionSensitivity));
        int mouseX = (int)Mathf.Clamp(handPosition.x * motionSpeed, -Screen.width / 2, Screen.width / 2);
        int mouseY = (int)Mathf.Clamp((handPosition.y - (0.5f - handHeight)) * motionSpeed, -Screen.height / 2, Screen.height / 2);
        //Debug.Log("mouse" + mouseX + ", " + mouseY);
        //Debug.Log("handPositon : " + handPosition);

        //���콺 �ն��������� Lerp ���
        cursorRect.anchoredPosition = Vector2.Lerp(
         cursorRect.anchoredPosition, // ���� ��ġ
         UpdateCursorPosition(mouseX, mouseY), // ��ǥ ��ġ
         Time.deltaTime * (3 - (mouseSpeed * 2))   // ���� �ӵ�
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

        // ���콺 Ŭ���� �ùķ��̼��ϱ� ���� Ŭ�� �̺�Ʈ ����
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = cursorRect.position;
        pointerEventData.pointerClick = gameObject;


        // Ŭ���� ��� ������Ʈ ����
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        raycaster.Raycast(pointerEventData, raycastResults);


        // + ��ư�� ����������. �ùķ��̼� ��ư������ ���� ��������
        if (raycastResults.Count > 0)
        {
            IsHandsInitialized = false;
            // ������ ��ư ��������
            GameObject clickedObject = raycastResults[0].gameObject;
            //Ŭ���̺�Ʈ �߻�

            //+��ư�� ���������� �ùķ��̼� ��ư�� ���������� ����� �������ҰŰ���

            //���� �ǳ� Rect AABB�� �����ϱ� , �ǳ��� �켱���� �����ϱ�
            ExecuteEvents.Execute(clickedObject, pointerEventData, ExecuteEvents.pointerClickHandler);
            if(playerNum == 0)
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
        Hand leftHand = frame.Hands[0];
        Hand rightHand = hand;

        //�μչٴ��� ��ġ
        Vector3 leftHandPosition = leftHand.PalmPosition;
        Vector3 rightHandPosition = rightHand.PalmPosition;
        // �� �� ������ �Ÿ� ���
        float currentDistance = Vector3.Distance(leftHandPosition, rightHandPosition); //ZoomIn ZoomOut �Ǻ��� ���� ���� �μջ����� �Ÿ�
        float currentYDistance = Mathf.Abs(leftHandPosition.z - rightHandPosition.z); // HandsUpDown�� �Ǻ��ϱ� ���� ���� y�Ÿ�
                                                                                      // ���� �Ÿ��� ���� �Ÿ��� ���Ͽ� ���� �־������� �Ǵ�

        switch (lastbtn.handAction)
        {
            case PlusButton.HandAction.ZoomOut: //���� ��������� : ����
                if (currentDistance - previousDistance < -distanceThreshold)
                {
                    //�μ��� ��������� ����Ǵ� ��
                    Debug.Log("������ ��� : ���� �������");

                    StartCoroutine(SpawnParticle());
                }
                break;
            case PlusButton.HandAction.ZoomIn: //���̸־����� : �߻�
                if (currentDistance - previousDistance > distanceThreshold)
                {
                    Debug.Log("�߻��� ��� : ���� �־���");
                    StartCoroutine(SpawnParticle());
                    // ���⿡ �� ���� �־����� �� ������ ���� �߰� 
                }
                break;
            case PlusButton.HandAction.HandsMoveUpDown:  //�������Ʒ��θ־����� : ����
                if (currentYDistance - previousYDistance > distanceThreshold)
                {
                    //����� ���Ʒ��� �Ÿ��� ��������
                    Debug.Log("������ ��� : ���� ���Ʒ��� �־��� : " + (currentYDistance - previousYDistance));
                    StartCoroutine(SpawnParticle());
                }
                break;
        }




        // ���� �Ÿ��� ���� �Ÿ��� ������Ʈ
        previousDistance = currentDistance;
    }
    public void UpdateCursorState(ActionState state)
    {
        actionState = state;
        gudie.UpdateGuideText();
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
    //�ùķ��̼� ������ �θ��� �����ǳ� 
    public void SimulationPlay()
    {
        UpdateCursorState(ActionState.playback);
        IsHandsInitialized = false;
        lastbtn.PlaySimulrator();
    }
    //public void ClickAni()
    //{
    //    // ���� �ִϸ��̼��� ������ ����
    //    cursorObject.transform.DOKill();

    //    // �������� 1�� ��� �ʱ�ȭ
    //    cursorObject.transform.localScale = Vector3.one;
    //    cursorObject.transform.DOScale(0.5f, 0.1f).SetLoops(2, LoopType.Yoyo);
    //}
    public void ParticleRing()
    {

    }
    IEnumerator SpawnParticle()
    {
        IsHandsInitialized = false;
        spawnPos = lastbtn.particlePos;
        for (int i = 0; i < 6; i++)
        {
            Debug.Log("��ƼŬ����");
            // �������� ������ ��ġ�� ����
           GameObject Ob = Instantiate(prefab, spawnPos.position, Quaternion.identity);
            Ob.transform.SetParent(particlePool);

            // 0.5�� ���
            yield return new WaitForSeconds(0.2f);
        }
            yield return new WaitForSeconds(2.8f);
        SimulationPlay();
    }
}
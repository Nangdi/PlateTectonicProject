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
    public float mouseSpeed= 0.5f;
    public float motionSensitivity = 0.025f;
    public float handHeight= 0.3f;
    public float handStability = 0.5f;
    Sequence clickSequence;
    public GameObject prefab;
    public RectTransform spawnPos;
    public Transform particlePool;
    private bool isTrigger;

    //�չٴ���� �ִϸ��̼� ������
    public UnityEngine.UI.Image cursorImage;
    public Sprite palmSprite; // �չٴ� ��������Ʈ
    public Sprite fistSprite; // �ָ� ��������Ʈ
    private Tween currentTween; // ���� ���� ���� Ʈ��

    private float cumulativeDistance = 0f;   // ������ �Ÿ�
    private float cumulativeYDistance = 0f;  // ������ Y�� �Ÿ�
    private float checkInterval = 0.1f;      // �� 0.1�ʸ��� ������ üũ
    private float lastCheckTime = 0f;        // ���������� üũ�� �ð�
    private float motiondistance;
    void Start()
    {

        cursorRect = GetComponent<RectTransform>();
        cursorImage = GetComponent<UnityEngine.UI.Image>();
        // playecursor ���� �ʱ�ȭ : off���½���
        UpdateCursorState(ActionState.Off);
        gameObject.SetActive(false);
        //clickSequence = DOTween.Sequence();

        //// Append: ���������� ���� (ù ��° �ִϸ��̼��� ������ �� ��°�� �����)
        //clickSequence.Append(cursorObject.transform.DOScale(0.5f, 0.2f));  // �� �Ŀ� 1�� ���� �������� 0.5�� ����
        //clickSequence.Append(cursorObject.transform.DOScale(1, 0.2f));  // �� �Ŀ� 1�� ���� �������� 0.5�� ����


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
            //���̰������� ������ �����ð��� �帥�� state�� Off�� �ٲ㼭 ��� ��Ҹ� �ʱ�ȭ�Ѵ�.
            //�ѳ��� �г� , �÷��̾� ���̵��Ʈ , 
            //�νĵ����� timer �ʱ�ȭ�� Active�� �������� ���� Manager�� ���� ������ �ҰŰ���
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

        //���콺 �ն��������� Lerp ���
        cursorRect.anchoredPosition = Vector2.Lerp(
         cursorRect.anchoredPosition, // ���� ��ġ
         UpdateCursorPosition(mouseX, mouseY), // ��ǥ ��ġ
         Time.deltaTime * (3 - (handStability * 2))   // ���� �ӵ�
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
            if(targetObject.name == "subBtn")
            {
                targetObject = targetObject.transform.parent.gameObject;
            }
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
            // ������ ��ư ��������
            //Ŭ���̺�Ʈ �߻�

            //+��ư�� ���������� �ùķ��̼� ��ư�� ���������� ����� �������ҰŰ���

            //���� �ǳ� Rect AABB�� �����ϱ� , �ǳ��� �켱���� �����ϱ�
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
        // �� ���� ������
        Hand leftHand = frame.Hands[0];
        Hand rightHand = hand;

        // �� �չٴ��� ��ġ
        Vector3 leftHandPosition = leftHand.PalmPosition;
        Vector3 rightHandPosition = rightHand.PalmPosition;

        // �� �� ������ �Ÿ� ���
        float currentDistance = Vector3.Distance(leftHandPosition, rightHandPosition);  // ZoomIn/ZoomOut
        float currentYDistance = Mathf.Abs(leftHandPosition.z - rightHandPosition.z);    // HandsMoveUpDown

        // motionSensitivity�� ���� �Ÿ� �ΰ��� ����
        motiondistance = 0.01f + (0.03f * motionSensitivity);

        // �� ���� �Ÿ� ���� ��� (���� �Ÿ��� ���� �Ÿ��� ����)
        float distanceDelta = currentDistance - previousDistance;
        float yDistanceDelta = currentYDistance - previousYDistance;

        // �Ÿ� ��ȭ���� ����
        //cumulativeDistance += Mathf.Abs(distanceDelta);
        //cumulativeYDistance += Mathf.Abs(yDistanceDelta);
        // ���� �ð��� �������� ���� �ð� ��� �� ������ üũ
        if (Time.time - lastCheckTime >= checkInterval)
        {
            lastCheckTime = Time.time;

            switch (lastbtn.handAction)
            {
                case PlusButton.HandAction.ZoomOut: // ���� ������� �� : ����
                    if (currentDistance < 0.3f * motionSensitivity)
                    {
                        Debug.Log("������ ��� : ���� �������");
                        SimulationPlay();
                        cumulativeDistance = 0f;  // ���� �Ÿ� �ʱ�ȭ
                    }
                    break;

                case PlusButton.HandAction.ZoomIn: // ���� �־��� �� : �߻�
                    if (currentDistance > 0.9f *motionSensitivity )
                    {
                        Debug.Log("�߻��� ��� : ���� �־���");
                        SimulationPlay();
                        cumulativeDistance = 0f;  // ���� �Ÿ� �ʱ�ȭ
                    }
                    break;

                case PlusButton.HandAction.HandsMoveUpDown:  // ���� ���Ʒ��� �־��� �� : ����
                    if (currentYDistance > 0.4f*motionSensitivity )
                    {
                        Debug.Log("������ ��� : ���� ���Ʒ��� �־��� : " + (currentYDistance - previousYDistance));
                        SimulationPlay();
                        cumulativeYDistance = 0f;  // ���� Y�� �Ÿ� �ʱ�ȭ
                    }
                    break;
            }
        }

        // ���� �Ÿ��� ���� �Ÿ��� ������Ʈ
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
        yield return new WaitForSeconds(waitTime);  // 1�� ���

        // �� ���� ù ��° �Ÿ� ��� (���� �ð� ��)
        Vector3 leftHandPosition = leftHand.PalmPosition;
        Vector3 rightHandPosition = rightHand.PalmPosition;
        previousDistance = Vector3.Distance(leftHandPosition, rightHandPosition);
        previousYDistance = Mathf.Abs(leftHandPosition.z - rightHandPosition.z);
        IsHandsInitialized = true;
        AudioManager.instance.Play("enterHand");
        lastbtn.ReadySimulrator(true);
        isTrigger = false;
    }
    //�ùķ��̼� ������ �θ��� �����ǳ� 
    public void SimulationPlay()
    {
        UpdateCursorState(ActionState.playback);
        IsHandsInitialized = false;
        lastbtn.PlaySimulrator();
    }
    public void ChangeToFist()
    {
        // ���� Ʈ���� ���� ���̸� ���
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill(); // ���� ���� Ʈ�� ���
        }

        // �ָ����� ��������Ʈ ����
        cursorImage.sprite = fistSprite;

        // 0.3�� �� �չٴ����� �ٽ� �����ϴ� Ʈ�� ����
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
        lastbtn.unReadyImage.gameObject.SetActive(false); //�غ�Ǹ鲨�ֱ� �ȵǸ� Ű��
        Debug.Log(lastbtn.ReadyImage.activeSelf);
        lastbtn. ReadyImage.SetActive(false); //�غ�Ǹ����ֱ� �ȵǸ� ����
        Debug.Log(lastbtn.ReadyImage.activeSelf);
        spawnPos = lastbtn.particlePos;
        for (int i = 0; i < 6; i++)
        {
            if (lastbtn.ReadyImage.activeSelf)
            {
                lastbtn.ReadyImage.SetActive(false); //�غ�Ǹ����ֱ� �ȵǸ� ����
            }
            Debug.Log("��ƼŬ����");
            // �������� ������ ��ġ�� ����
           GameObject Ob = Instantiate(prefab, spawnPos.position, Quaternion.identity);
            Ob.transform.SetParent(particlePool);

            // 0.5�� ���
            yield return new WaitForSeconds(0.2f);
        }
            yield return new WaitForSeconds(3f);
        SimulationPlay();
    }
}
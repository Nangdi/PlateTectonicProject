using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static ArrowMove;

public class AABBCollisionResolve : MonoBehaviour
{
    public static AABBCollisionResolve Instance;

    public RectTransform rectTransform1; //1P�� ������ �ǳ� �Ҵ�
    public RectTransform rectTransform2; //2P�� ������ �ǳ� �Ҵ�
    public float pushBackDistance = 10f;  // �� ���� UI ��Ҹ� ���� �־����� �� �Ÿ�
    private float overlapX;
    private float overlapY;
    Tween tween;
    //���� �۾��� �ػ� 
    float referenceWidth = 1920f; 
    float referenceHeight = 1080f;
    //���� �������� �ػ�
    float currentWidth = Screen.width;
    float currentHeight = Screen.height;


    private void Awake()
    {
        Instance = this;
    }
    void Update()
    {
        //if (IsAABBCollision(rectTransform1, rectTransform2))
        //{
        //    Debug.Log("AABB �浹 �߻�!");
        //    //ResolveCollision(rectTransform1, rectTransform2);
        //}
    }

    bool IsAABBCollision(RectTransform rect1, RectTransform rect2)
    {
        //���� �ϳ��� ���õ��� �ʾҰų� , ���� �ϳ��� active�� false �� ��ġ�� �����ɷ� ���� 
        if ((rect1 == null || rect2 == null) || (!rect1.gameObject.activeSelf || !rect2.gameObject.activeSelf)) return false;
        Rect rect1World = GetWorldRect(rect1);
        Rect rect2World = GetWorldRect(rect2);
        if (rect1World.max.x < rect2World.min.x || // rect1�� �������� rect2�� ���ʺ��� ���ʿ� ����
          rect1World.min.x > rect2World.max.x || // rect1�� ������ rect2�� �����ʺ��� �����ʿ� ����
          rect1World.max.y < rect2World.min.y || // rect1�� ������ rect2�� �Ʒ��ʺ��� �Ʒ��� ����
          rect1World.min.y > rect2World.max.y)   // rect1�� �Ʒ����� rect2�� ���ʺ��� ���� ����
        {
            //overlapX = 0;
            //overlapY = 0;
            Debug.Log("�浹����");
            return false; // �浹���� ����
        }
       
        overlapX = Mathf.Min(rect1World.max.x, rect2World.max.x) - Mathf.Max(rect1World.min.x, rect2World.min.x); // x ����
        overlapY = Mathf.Min(rect1World.max.y, rect2World.max.y) - Mathf.Max(rect1World.min.y, rect2World.min.y); // y ����
            Debug.Log("�浹��");
        return true; // �浹��
    }

    void ResolveCollision(RectTransform rect1, RectTransform rect2)
    {
        Vector3 direction = rect1.position - rect2.position;
        direction.Normalize();

        // rect1�� rect2�� ���� �ݴ� �������� �о
        rect1.anchoredPosition += (Vector2)direction * pushBackDistance;
        rect2.anchoredPosition -= (Vector2)direction * pushBackDistance;
    }

    Rect GetWorldRect(RectTransform rt)
    {
        Canvas canvas = rt.GetComponentInParent<Canvas>();
        float scaleFactor = canvas.scaleFactor;
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);

        Vector3 topLeft = corners[0] / scaleFactor;
        Vector3 bottomRight = corners[2] / scaleFactor;

        return new Rect(topLeft.x, topLeft.y, bottomRight.x - topLeft.x, bottomRight.y - topLeft.y);
    }
    public void CarculatePriorities()
    {
        ExplanationPanel explanationPanel1 = rectTransform1.gameObject.GetComponent<ExplanationPanel>();
        ExplanationPanel explanationPanel2 = rectTransform2.gameObject.GetComponent<ExplanationPanel>();
        int firstNum = explanationPanel1.priorityIndex;
        int secondNum = explanationPanel2.priorityIndex;

        if(firstNum > secondNum)
        {
            int dir;
            if(rectTransform1.position.x - rectTransform2.position.x > 0)
            {
                //���������� �̵�
                dir = 1;
            }
            else
            {
                //�������� �̵�
                dir = -1;
            }
            MovePanel(rectTransform1 , dir);
            explanationPanel1.priorityIndex = 3;
        }
        else
        {
            int dir;
            if (rectTransform2.position.x - rectTransform1.position.x > 0)
            {
                //���������� �̵�
                dir = 1;
            }
            else
            {
                //�������� �̵�
                dir = -1;
            }
            MovePanel(rectTransform2, dir);
            explanationPanel2.priorityIndex = 3;
        }
    }
    private void MovePanel(RectTransform rectTransform , int dir)
    {

       
        //float widthRatio = currentWidth / referenceWidth;
        //float heightRatio = currentHeight / referenceHeight;
        //float scalingFactor = Mathf.Min(widthRatio, heightRatio);
        //���� ��ġ�� ������
        Vector3 currentPos = rectTransform.localPosition;

        //float adjustedOverlapX = overlapX * scalingFactor;
        // x�� ���� �����ϰ� ������ ���� �״�� ����
        Vector3 targetPos = new Vector3(currentPos.x + dir * overlapX, currentPos.y, currentPos.z);
        tween = rectTransform.DOLocalMove(targetPos, 0.5f).SetEase(Ease.OutQuad);
        //�ι�Ŭ�������� �������� �ʴ¹��� ã�ƾ���

    }
    public void CheckOverLab()
    {
        if (IsAABBCollision(rectTransform1, rectTransform2))
        {
            CarculatePriorities();
        }
    }
    private void OverlabCorrect(float _overlap)
    {
     


    }
}

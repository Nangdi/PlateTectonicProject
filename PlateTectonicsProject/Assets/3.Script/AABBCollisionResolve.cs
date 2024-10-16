using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static ArrowMove;

public class AABBCollisionResolve : MonoBehaviour
{
    public static AABBCollisionResolve Instance;

    public RectTransform rectTransform1; //1P가 선택한 판넬 할당
    public RectTransform rectTransform2; //2P가 선택한 판넬 할당
    public float pushBackDistance = 10f;  // 두 개의 UI 요소를 서로 멀어지게 할 거리
    private float overlapX;
    private float overlapY;
    Tween tween;
    //내가 작업한 해상도 
    float referenceWidth = 1920f; 
    float referenceHeight = 1080f;
    //현재 적용중인 해상도
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
        //    Debug.Log("AABB 충돌 발생!");
        //    //ResolveCollision(rectTransform1, rectTransform2);
        //}
    }

    bool IsAABBCollision(RectTransform rect1, RectTransform rect2)
    {
        //둘중 하나라도 선택되지 않았거나 , 둘중 하나라도 active가 false 면 겹치지 않은걸로 판정 
        if ((rect1 == null || rect2 == null) || (!rect1.gameObject.activeSelf || !rect2.gameObject.activeSelf)) return false;
        Rect rect1World = GetWorldRect(rect1);
        Rect rect2World = GetWorldRect(rect2);
        if (rect1World.max.x < rect2World.min.x || // rect1의 오른쪽이 rect2의 왼쪽보다 왼쪽에 있음
          rect1World.min.x > rect2World.max.x || // rect1의 왼쪽이 rect2의 오른쪽보다 오른쪽에 있음
          rect1World.max.y < rect2World.min.y || // rect1의 위쪽이 rect2의 아래쪽보다 아래에 있음
          rect1World.min.y > rect2World.max.y)   // rect1의 아래쪽이 rect2의 위쪽보다 위에 있음
        {
            //overlapX = 0;
            //overlapY = 0;
            Debug.Log("충돌안함");
            return false; // 충돌하지 않음
        }
       
        overlapX = Mathf.Min(rect1World.max.x, rect2World.max.x) - Mathf.Max(rect1World.min.x, rect2World.min.x); // x 길이
        overlapY = Mathf.Min(rect1World.max.y, rect2World.max.y) - Mathf.Max(rect1World.min.y, rect2World.min.y); // y 길이
            Debug.Log("충돌함");
        return true; // 충돌함
    }

    void ResolveCollision(RectTransform rect1, RectTransform rect2)
    {
        Vector3 direction = rect1.position - rect2.position;
        direction.Normalize();

        // rect1과 rect2를 서로 반대 방향으로 밀어냄
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
                //오른쪽으로 이동
                dir = 1;
            }
            else
            {
                //왼쪽으로 이동
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
                //오른쪽으로 이동
                dir = 1;
            }
            else
            {
                //왼쪽으로 이동
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
        //현재 위치를 가져옴
        Vector3 currentPos = rectTransform.localPosition;

        //float adjustedOverlapX = overlapX * scalingFactor;
        // x축 값을 변경하고 나머지 값을 그대로 유지
        Vector3 targetPos = new Vector3(currentPos.x + dir * overlapX, currentPos.y, currentPos.z);
        tween = rectTransform.DOLocalMove(targetPos, 0.5f).SetEase(Ease.OutQuad);
        //두번클릭했을때 움직이지 않는버그 찾아야함

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

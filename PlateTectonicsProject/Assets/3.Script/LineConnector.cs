using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineConnector : MonoBehaviour
{
    public RectTransform buttonTransform; // 버튼의 Transform
    public RectTransform panelTransform;  // 패널의 Transform
    private LineRenderer lineRenderer;
    private void OnEnable()
    {
        SetLineColor(Color.red, Color.red); 
    }
    void Start()
    {

        // LineRenderer 컴포넌트 가져오기
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2; // 선은 2개의 점으로 구성됨

    }

    void Update()
    {
        if (buttonTransform != null && panelTransform != null)
        {
            // LineRenderer의 시작점과 끝점을 버튼과 패널 위치로 설정
            lineRenderer.SetPosition(0, buttonTransform.position); // 시작점: 버튼
            lineRenderer.SetPosition(1, panelTransform.position); // 끝점: 패널
        }
    }
    public void SetLineColor(Color startColor, Color endColor)
    {
        // 시작과 끝 색상 설정
        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;
    }
}
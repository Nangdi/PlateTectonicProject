using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineConnector : MonoBehaviour
{
    public RectTransform buttonTransform; // ��ư�� Transform
    public RectTransform panelTransform;  // �г��� Transform
    private LineRenderer lineRenderer;
    private void OnEnable()
    {
        SetLineColor(Color.red, Color.red); 
    }
    void Start()
    {

        // LineRenderer ������Ʈ ��������
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2; // ���� 2���� ������ ������

    }

    void Update()
    {
        if (buttonTransform != null && panelTransform != null)
        {
            // LineRenderer�� �������� ������ ��ư�� �г� ��ġ�� ����
            lineRenderer.SetPosition(0, buttonTransform.position); // ������: ��ư
            lineRenderer.SetPosition(1, panelTransform.position); // ����: �г�
        }
    }
    public void SetLineColor(Color startColor, Color endColor)
    {
        // ���۰� �� ���� ����
        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;
    }
}
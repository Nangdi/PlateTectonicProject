using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class ArrowMove : MonoBehaviour
{

    private RectTransform rectTransform;
    private Tween moveTween;
    public float moveDistance = 15f;
    public float moveCycle = 3f;
    public bool isAutoDir = true;
    public Vector3 dir = Vector2.right;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (isAutoDir)
        {
            AutoMoveArrow();
        }
        else
        {
            SetDirMoveArrow(dir);
        }


    }
    private void OnEnable()
    {

        if (moveTween != null)
        {

            moveTween.Play();

        }
    }
  
    public void AutoMoveArrow()
    {
        // Z �� ȸ�� �� �������� (EulerAngles�� ����)
        float zRotation = transform.eulerAngles.z;

        // Z ȸ�� ���� �������� ��ȯ (Trig �Լ��� ������ ���)
        float radians = zRotation * Mathf.Deg2Rad;

        // ȭ��ǥ�� ����Ű�� ���� ��� (���� ����)
        Vector3 direction = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0);

        // DoLocalMove�� ���� �ش� �������� �̵�
        moveTween = transform.DOLocalMove(transform.localPosition + direction * moveDistance, moveCycle).SetLoops(-1, LoopType.Restart);

    }
    public void SetDirMoveArrow(Vector3 dir)
    {
        moveTween = transform.DOLocalMove(transform.localPosition + dir * moveDistance, moveCycle).SetLoops(-1, LoopType.Restart);
    }
    private void OnDisable()
    {
        moveTween.Pause();
    }
}

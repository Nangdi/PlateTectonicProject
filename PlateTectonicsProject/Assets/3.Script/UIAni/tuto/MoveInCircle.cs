using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveInCircle : MonoBehaviour
{
    public Transform Center;
    private void Start()
    {
        playMoveInCircle(Center.position, 100f, 3);
    }
    void playMoveInCircle( Vector3 center, float radius, float duration)
    {
        DOTween.To(() => 0f, angle =>
        {
            // ������ �������� ��ȯ
            float rad = angle * Mathf.Deg2Rad;
            // ���� ��ǥ ���
            float x = Mathf.Cos(rad) * radius;
            float y = Mathf.Sin(rad) * radius;

            // ������Ʈ ��ġ ����
            transform.position = new Vector3(center.x +x/5, center.y + y, transform.position.z);
        }, 360f, duration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart); // ���� ����
    }
}

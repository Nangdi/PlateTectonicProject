using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutoTimer : MonoBehaviour
{
    public TextMeshProUGUI tutoTimer;
    private float timer = 10f;  // Ÿ�̸��� �ʱ� �ð� (��: 10��)

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;  // Time.deltaTime�� ����Ͽ� 1�ʾ� ����
        }
        else
        {
            timer = 0;  // Ÿ�̸Ӱ� 0���� �۾����� �ʵ��� ����
        }

        // �ؽ�Ʈ�� Ÿ�̸� ���� ǥ��
        tutoTimer.text = timer.ToString("F0");  // �Ҽ��� 2�ڸ����� ǥ��
    }
    public void SetTimer(float time)
    {
        timer = time;
        tutoTimer.text = timer.ToString("F0");
    }
    private void OnEnable()
    {
        SetTimer(10);
    }
}
